using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Health : MonoBehaviour
{
    public GameObject LoseText;

    public float maxHealth = 500f;
    static float health = 500f;

    public HealthBar healthBar;

    public GameObject player;

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.setHealth(health);

        Debug.Log(health);

        if (health <= 0f)
        {
            Debug.Log("STOP");
            Time.timeScale = 0;
            LoseText.SetActive(true);
        }
    }
    
    void Start()
    {
        LoseText.SetActive(false);
        healthBar.SetMaxHealth(maxHealth);
    }
}