using System;

/// <summary>
///   Abstract class which provides information about HealthUpdated events
/// </summary>
/// <remarks>
///   This is an abstract class rather than an interface since C# doesn't
///   provide an interface for EventArgs, so the only way to derive the
///   functionality of EventArgs in an "interface-like" manner is to use
///   an abstract class.
/// </remarks>
public abstract class AHealthUpdatedEventArgs : EventArgs
{
    public abstract int previousHealth { get; }
    public abstract int newHealth { get; }
}

/// <summary>
///   Abstract class which provides information about Death events
/// </summary>
/// <remarks>
///   This is an abstract class rather than an interface since C# doesn't
///   provide an interface for EventArgs, so the only way to derive the
///   functionality of EventArgs in an "interface-like" manner is to use
///   an abstract class.
/// </remarks>
public abstract class ADeathEventArgs : EventArgs
{
    // Intentionally Empty:
    // The caller already gets a reference to the dead object.
    // Nothing else is obviously needed.
}

/// <summary>
///   Component which enables HP functionality on GameObject
/// </summary>
/// <remarks>
///   This class is responsible for maintaining the HP system of a GameObject.
///   Other classes may subscribe to the HealthUpdated and Death events to learn
///   when this instance has an update to it's health or is no longer alive.
/// </remarks>
/// <invariants>
///   This class should always ensure 0 <= currentHealth <= maximumHealth
/// </invariants>
public interface IHealth
{
    //============================================================================
    // Events
    //============================================================================
    event EventHandler<AHealthUpdatedEventArgs> HealthUpdated;
    event EventHandler<ADeathEventArgs> Death;

    //============================================================================
    // Properties
    //============================================================================
    int maximumHealth { get; }
    int currentHealth { get; }

    //============================================================================
    // Public Methods
    //============================================================================

    /// <summary>
    ///   Deal damage to the object which holds this component.
    /// </summary>
    /// <param name="HP">
    ///   Units of damage to be dealt.
    /// </remarks>
    void Hurt(int HP);

    /// <summary>
    ///   Heal the object which holds this component.
    /// </summary>
    /// <param name="HP">
    ///   Units of health to be restored
    /// </remarks>
    void Heal(int HP);

    /// <summary>
    ///   Returns if the player has enough HP to be alive.
    /// </summary>
    bool IsDead();
}
