using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HealthUpdatedEventArgs : AHealthUpdatedEventArgs
{
    public override int previousHealth { get; }
    public override int newHealth { get; }

    public HealthUpdatedEventArgs(int previousHealth, int newHealth)
    {
        this.previousHealth = previousHealth;
        this.newHealth = newHealth;
    }
}

public class DeathEventArgs : ADeathEventArgs
{
    // Empty
}

public class Health : MonoBehaviour, IHealth
{
    //============================================================================
    // Events
    //============================================================================
    public event EventHandler<AHealthUpdatedEventArgs> HealthUpdated;
    public event EventHandler<ADeathEventArgs> Death;

    //============================================================================
    // Instance variables
    //============================================================================

    // DEFAULT_INITIALIZED_HEALTH is used to set the default values of
    // currentHealth and maximumHealth. Usually these may be set with the Unity
    // inspector, but that is not possible for unit testing, so a default value
    // is created for instances of Health that are created programatically.
    [NonSerialized] public const int DEFAULT_INITIALIZED_HEALTH = 100;

    [SerializeField] private int _currentHealth = DEFAULT_INITIALIZED_HEALTH;
    [SerializeField] private int _maximumHealth = DEFAULT_INITIALIZED_HEALTH;

    //============================================================================
    // Properties
    //============================================================================
    public int maximumHealth
    {
        get { return  _maximumHealth; }
    }
    public int currentHealth
    {
        get { return _currentHealth; }
        private set
        {
            int newHealth = value;

            // Invariant: 0 <= currentHealth <= maximumHealth
            if (newHealth > this.maximumHealth) {
                newHealth = this.maximumHealth;
            }
            else if (newHealth < 0){
                newHealth = 0;
            }

            // Update health if the value changes and notify registered events
            if (newHealth != _currentHealth)
            {
                var healthEventArgs = new HealthUpdatedEventArgs(_currentHealth,
                                                                 newHealth);

                // Update internal state before sending event
                _currentHealth = newHealth;
                
                // Notify subscribers of event
                OnHealthUpdated(healthEventArgs);
                if (IsDead())
                {
                    var deathEventArgs = new DeathEventArgs();
                    OnDeath(deathEventArgs);
                }
            }
        }
    }
    //============================================================================
    // Event Dispatch Methods
    //============================================================================

    /// <summary>
    ///   Invoke all registered event handlers for the HealthUpdated Event
    /// </summary>
    /// <param name="e">
    ///   Arguments which are passed to registered event handlers
    /// </param>
    private void OnHealthUpdated(HealthUpdatedEventArgs e)
    {
        EventHandler<AHealthUpdatedEventArgs> handler = HealthUpdated;
        handler?.Invoke(this, e);
    }
    
    /// <summary>
    ///   Invoke all registered event handlers for the Death Event
    /// </summary>
    /// <param name="e">
    ///   Arguments which are passed to registered event handlers
    /// </param>
    private void OnDeath(DeathEventArgs e)
    {
        EventHandler<ADeathEventArgs> handler = Death;
        handler?.Invoke(this, e);
    }
    
    //============================================================================
    // Unity Component Methods
    //============================================================================
    private void Start()
    {
        // Invariant Check
        Assert.IsTrue(0 <= currentHealth);
        Assert.IsTrue(currentHealth <= maximumHealth);
    }
    
    //============================================================================
    // Public Methods
    //============================================================================
    public void Hurt(int HP)
    {
        // Decrease HP.
        // Setter is responsible for ensuring currentHealth >= 0
        currentHealth -= HP;

        // Invariant Check
        Assert.IsTrue(0 <= currentHealth);
    }

    public void Heal(int HP)
    {
        // Increase HP
        // Setter is responsible for ensuring currentHealth <= maximumHealth
        currentHealth += HP;

        // Invariant Check
        Assert.IsTrue(currentHealth <= maximumHealth);
    }

    public bool IsDead()
    {
        return currentHealth == 0;
    }
}
