using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class HealthBar : MonoBehaviour
{
    //===========================================================================
    // Notes
    //===========================================================================

    // The healthbar scaling relies on some Unity magic found in the thread:
    // http://answers.unity.com/comments/1605488/view.html.
    // It works by moving the edge of the green bar to the center of an empty GameObject
    // so that the scaling of the empty object will counteract the recursive scaling of
    // the bar.    

    //===========================================================================
    // Instance variables
    //===========================================================================
    private Transform _healthScaler;
    private int _maximumHealth;

    // XXX: Unity cannot display Interface variables within the inspector.
    //      To get around this, we ask that the user assigns the GameObject
    //      which contains a component which implements the interface, then
    //      we extract that component into _health when Start() is called.
    //      This is a stupid hack, but it's the only thing we can do without
    //      writing a script to customize the editor.
    // https://forum.unity.com/threads/c-interface-wont-show-in-inspector.383886/
    //
    // NOTE: We expose this variable as public so that we can test the module
    //       using dependency injection. 
    public GameObject healthSource;  // GameObject with Health Component
    private IHealth _health;

    //===========================================================================
    // Unity Methods
    //===========================================================================
    void Start()
    {
        Init();
    }

    void OnDestroy()
    {
        // Unregister the event to remove reference to the object and allow for
        // the C# garbage collector to do its thing.
        _health.HealthUpdated -= HealthUpdatedEventHandler;
    }

    //===========================================================================
    // Public Methods
    //===========================================================================
    
    /// <summary>
    ///   Initialize the class' instance variables and event handlers.
    /// </summary>
    public void Init()
    {
        _health = healthSource.GetComponent<IHealth>();
        if (_health == null)
        {
            throw new ApplicationException("HealthBar: Input healthSource must "
                                           + "have a Health component.");
        }
        _healthScaler = transform.Find("HealthScaler");
        if (_healthScaler == null)
        {
            // This should never happen since this script is attached to a PreFab
            // that will ALWAYS have HealthScaler as a child.
            throw new ApplicationException("HealthBar could not find HealthScaler"
                                           + " GameObject.");
        }
        _maximumHealth = _health.maximumHealth;
        _health.HealthUpdated += HealthUpdatedEventHandler;

        // Make sure the initial scaling is correct.
        ResizeHealthBar(_health.currentHealth);
    }
    
    //===========================================================================
    // Private Methods
    //===========================================================================
    
    /// <summary>
    ///   Handle a HealthUpdated event triggered by the _health component.
    /// </summary>
    /// <param name="sender">
    ///   The object sending the event. This should always be _health.
    /// </param>
    /// <param name="e">
    ///   The event arguments. Since this event handler is interacting with a
    ///   Health component, these events have type HealthUpdatedEventArgs.
    /// </param>
    private void HealthUpdatedEventHandler(object sender, EventArgs e)
    {
        AHealthUpdatedEventArgs healthArgs = (AHealthUpdatedEventArgs) e;
        ResizeHealthBar(healthArgs.newHealth);
    }

    /// <summary>
    ///   Rescales the health bar.
    /// </summary>
    /// <param name="newHealth">
    ///   The updated HP to be reflected in the health bar.
    ///   Assumes 0 <= newHealth <= maximumHealth
    /// </param>
    private void ResizeHealthBar(int newHealth)
    {
        // Check invariants
        Assert.IsTrue(0 <= newHealth);
        Assert.IsTrue(newHealth <= _maximumHealth);

        _healthScaler.localScale = new Vector3((float)newHealth / _maximumHealth, 1f);
    }
}
