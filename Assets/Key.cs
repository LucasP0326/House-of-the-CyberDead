using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Component doorcolliderhere;
    public GameObject keygone;
    public GameObject enemyPerson;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerStay ()
    {
        if(Input.GetKey(KeyCode.E))
        doorcolliderhere.GetComponent<BoxCollider> ().enabled = true;

        if(Input.GetKey(KeyCode.E))
        keygone.SetActive(false);
    }

    void Update()
    {
        Enemy enemy = enemyPerson.gameObject.GetComponent<Enemy>();
        if (enemy.health <= 0)
        {
            transform.position = enemy.location;
        }
    }
}