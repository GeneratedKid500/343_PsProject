using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Sight))]
public class NavMeshMovement : MonoBehaviour, iPooledObject
{
    [Header("Movement Attributes")]
    public GameObject[] waypoints;
    [SerializeField] int moveSpeed = 3;
    [SerializeField] bool random = false;

    private Sight sight;
    private int originalAngle;

    public NavMeshAgent agent;
    private Animator animator;
    private RandomEnemySpawning res;

    private GameObject currentWaypoint;
    private Rigidbody rb;
    private Transform player;

    private int pointer = -1;
    private float timer = 0;
    private bool travelling;
    
    public void OnObjectSpawn()
    {
        agent.enabled = true;
        agent.speed = moveSpeed;

        sight = GetComponent<Sight>();
        originalAngle = sight.angle;

        animator = GetComponentInChildren<Animator>();

        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        res = GameObject.FindGameObjectWithTag("Floor").GetComponent<RandomEnemySpawning>();

        waypoints = new GameObject[0];
        if (waypoints.Length == 0)
        {
            //create x amount of waypoints within specified radius
            CreateWaypoints();

            //waypoints = GameObject.FindGameObjectsWithTag("AI_Waypoint");
            random = true;
            RandomWaypoint();
        }

        NextWaypoint();
    }

    void Update()
    {
        if (sight.targeting.enabled)
        {
            if (travelling || timer > 0)
            {
                travelling = false;
                timer = 0;
            }

            sight.angle = 360;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.gameObject.SetActive(true);

            sight.angle = originalAngle;

            pointer--;
            NextWaypoint();
        }

        if (travelling && agent.remainingDistance < 1f) //if enemy has reached destination and isnt chasing player
        {
            if (random) RandomWaypoint();
            else NextWaypoint();
        }

        AnimationMoveBlend();
    }

    #region WAYPOINTS
    void CreateWaypoints()
    {
        waypoints = new GameObject[res.numberOfWaypoints];
        for (int i = 0; i < res.numberOfWaypoints; i++)
        {
                float RandomX = Random.Range(transform.position.x - res.maxWaypointSpawnDistance, transform.position.x + res.maxWaypointSpawnDistance);
                float RandomZ = Random.Range(transform.position.z - res.maxWaypointSpawnDistance, transform.position.z + res.maxWaypointSpawnDistance);
                Vector3 waypointSpawnLoc = new Vector3(RandomX, transform.position.y, RandomZ);

                if (Physics.Raycast(waypointSpawnLoc, -Vector3.up, Mathf.Abs(transform.position.y) + 2, res.groundLayer))
                {
                    //waypoints[i] = Instantiate(res.waypoint, waypointSpawnLoc, transform.rotation);
                    waypoints[i] = ObjectPooler.Instance.SpawnFromPool(res.waypoint.name, waypointSpawnLoc, transform.rotation);
                }
                else
                {
                    i--;
                }
        }
    }

    void SetWaypoint()
    {
        currentWaypoint = waypoints[pointer];
        agent.SetDestination(currentWaypoint.transform.position);
        travelling = true;
    }

    void NextWaypoint()
    {
        if (pointer != waypoints.Length - 1) pointer++;
        else pointer = 0;

        SetWaypoint();
    }

    void RandomWaypoint()
    {
        pointer = Random.Range(0, waypoints.Length-1);

        SetWaypoint();
    }
    #endregion

    void AnimationMoveBlend()
    {
        // gets velocity on scale of 0 - 1
        float velocity = agent.velocity.magnitude / agent.speed;

        if (sight.targeting.enabled)
        {
            animator.SetFloat("moveSpeed", velocity+1);
        }
        else
        {
            animator.SetFloat("moveSpeed", velocity);
        }
    }
}
