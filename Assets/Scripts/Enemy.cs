using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 target;
    public GameObject player;
    Animator animator;

    //fov
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public bool seesPlayer;
    public bool seenPlayer;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public float health;
    public int damageFromBullet;

    public Vector3 location;

    public float angleX;
    public float angleY;
    public float angleZ;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public int attackDamage;

    public AudioSource InjuredSFX;

    public Light lt;

    void Start()
    {
        lt.color = Color.yellow;
        agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
        animator = GetComponent<Animator>();
    }

    //Detect Collision with Bullet
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            InjuredSFX.Play ();
            //Debug.Log("shot!");
            Destroy(collision.gameObject);
            TakeDamage(damageFromBullet);
        }
    }

    void Update()
    {
        if (seesPlayer == true)
        {
            lt.color = Color.red;
        }

        if (seenPlayer == true && seesPlayer == false)
        {
            lt.color = Color.yellow;
        }

        if (seesPlayer == false && seenPlayer == false)
        {
            lt.color = Color.yellow;
        }

        //look for player
        FieldOfViewCheck();
        if (seesPlayer)
        {
            //follow player
            Vector3 dirToPlayer = transform.position - player.transform.position;
            Vector3 newPos = transform.position - dirToPlayer;
            agent.SetDestination(newPos);

            float distance = Vector3.Distance (transform.position, player.transform.position);
            if (distance <= 5) AttackPlayer();
            
        }
        else
        {
            //patrol points
            if (Vector3.Distance(transform.position, target) < 2)
            {
                waypointIndex = Random.Range(0, waypoints.Length);
                UpdateDestination();
            }
        }
        //Get enemy position
        location = transform.position;
    }
    void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        //Distance check
        if(rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            //FOV check
            if(Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                //Obstruction check
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    //Debug.Log("sees player");
                    seesPlayer = true;
                    seenPlayer = true;
                }
                else
                {
                    //Debug.Log("Obstructed");
                    seesPlayer = false;
                    if (seenPlayer)
                    {
                        seenPlayer = false;
                        StartCoroutine(ForgetPlayer());
                    }
                }
            }
            else
            {
                //Debug.Log("out of fov");
                seesPlayer = false;
                if (seenPlayer)
                {
                    seenPlayer = false;
                    StartCoroutine(ForgetPlayer());
                }
            }
        }
        else if (seesPlayer)
        {
            seesPlayer = false;
            if (seenPlayer)
            {
                seenPlayer = false;
                StartCoroutine(ForgetPlayer());
            }
        }
    }

    private IEnumerator ForgetPlayer()
    {
        yield return new WaitForSeconds(10);
        //Debug.Log("Player Forgotten");
        UpdateDestination();
    }

    private void AttackPlayer()
    {

        if (!alreadyAttacked)
        {
            //Debug.Log("attack!");
            player.GetComponent<PlayerMovement>().health -= attackDamage;
            if (player.GetComponent<PlayerMovement>().health == 0)player.GetComponent<PlayerMovement>().Death();
            animator.SetTrigger("Attack");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        animator.SetTrigger("Death");
        agent.speed = 0f;
    }
}
