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

    //fov
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public bool seesPlayer;
    public bool seenPlayer;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
    }

    void Update()
    {
        //look for player
        FieldOfViewCheck();
        if (seesPlayer)
        {
            //follow player
            Vector3 dirToPlayer = transform.position - player.transform.position;
            Vector3 newPos = transform.position - dirToPlayer;
            agent.SetDestination(newPos);
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
                    //Debug.Log("Sees player");
                    seesPlayer = true;
                    seenPlayer = true;
                }
                else
                {
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
        Debug.Log("Player Forgotten");
        UpdateDestination();
    }
}