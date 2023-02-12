using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health, maxHealth =3f;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if(health <=0 )
        {
            Destroy(gameObject);
        }
    }
}

