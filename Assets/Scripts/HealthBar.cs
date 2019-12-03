using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }
    public Slider healthbar;

    //HealthBar instance
    public static HealthBar instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Resets health to full on game load
        MaxHealth = 20f;
        CurrentHealth = MaxHealth;

        healthbar.value = CalculateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DealDamage(float damageValue)
    {
        //Deduct the damage dealt from the character's health
        CurrentHealth -= damageValue;
        healthbar.value = CalculateHealth();
        //If the character is out of health, die
        if (CurrentHealth <= 0)
            Die();
    }

    float CalculateHealth()
    {
        return CurrentHealth / MaxHealth;
    }

    void Die()
    {
        CurrentHealth = 0;
        Debug.Log("You died.");
    }
}
