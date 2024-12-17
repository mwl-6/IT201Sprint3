using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drone : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject player;

    private NavMeshAgent navMeshAgent;
    private Transform model;
    private Transform gun;

    [SerializeField] private bool playerInRange = false;
    private float detectionRange;
    private float ignoreRange;

    [SerializeField] private float stopDist;

    public float accuracy = 0;
    public float firingRate = 3;

    private float timeOfLastFire;
    public GameObject laser;
    [SerializeField] GameObject drop;

    public int health = 1;
    public bool isFriendly = false;

    //-2 = do nothing, -1 - stick with player, anything else is a base
    //This applies to enemy drones too
    public int baseToTarget = -1;

    private GameObject spawnMissileSites;

    private bool enemyToTarget = false;
    Collider[] hits;
    private Transform enemyTarget;

    //Probability that an enemy will pick a drone to attack over the player
    //Usually the player is prioritized
    float playerPriorityThreshold = 0.37f;
    float priorityVal = 0f;
    void Start()
    {
        spawnMissileSites = GameObject.Find("MissileSiteManager");
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();

        stopDist = Random.Range(12, 21);
        navMeshAgent.stoppingDistance = stopDist;
        model = transform.Find("Model");
        gun = model.Find("Gun");

        detectionRange = 50;
        ignoreRange = detectionRange*1.5f;

        timeOfLastFire = Time.time;

        priorityVal = Random.Range(0.0f, 1.0f);


    }

    // Update is called once per frame
    void Update()
    {
        //Random bug where drones spawn too high up so they don't register as a navmeshagent
        if(GetComponent<NavMeshAgent>() == null)
        {
            Debug.Log("Bad Drone");
            spawnMissileSites.GetComponent<SpawnMissileSites>().RemoveID(transform.gameObject.GetInstanceID());
            Destroy(gameObject);
        }

        model.transform.localPosition = new Vector3(0, Mathf.Lerp(0, 1, (Mathf.Cos(Time.time) + 1) / 2.0f), 0);
        model.Find("Icosphere").Find("Spin").Rotate(new Vector3(0, 0, 720 * Time.deltaTime));
        float playerDist = Vector3.Distance(player.transform.position, transform.position);
        if (playerDist < detectionRange)
        {
            playerInRange = true;
        }
        else if(playerDist > ignoreRange)
        {
            playerInRange = false;
        }
        //If drones encounter enemy drones they will prioritize fighting before returning to their base
        enemyToTarget = FindEnemiesInRange();


        //Attack enemy drones
        /*
         * A friendly drone will always attack an enemy drone
         * An enemy drone will only attack a friendly drone if there is no player in range or its priority threshold is below a certain value
         * */
        if (enemyToTarget && ((!playerInRange || priorityVal < playerPriorityThreshold) || isFriendly))
        {
            navMeshAgent.SetDestination(enemyTarget.position);
            gun.transform.LookAt(enemyTarget.transform.Find("Model").Find("Icosphere"));
            //Always look at enemy
            if (Vector3.Distance(transform.position, enemyTarget.position) <= stopDist)
            {
                Vector3 dir = new Vector3(enemyTarget.transform.position.x - transform.position.x, 0, enemyTarget.transform.position.z - transform.position.z);
                transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            }

            if (Time.time - timeOfLastFire > firingRate)
            {
                timeOfLastFire = Time.time;
                Quaternion rot = gun.transform.rotation;
                rot = Quaternion.Euler(rot.eulerAngles.x + Random.Range(-4, 4), rot.eulerAngles.y + Random.Range(-4, 4), rot.eulerAngles.z);
                GameObject g = Instantiate(laser, gun.transform.position, rot);

            }


        }

        //Go to assigned base if there's no danger
        if(baseToTarget >= 0 && ( (isFriendly && !enemyToTarget) || (!isFriendly && !playerInRange && !enemyToTarget) ))
        {

            navMeshAgent.SetDestination(spawnMissileSites.GetComponent<SpawnMissileSites>().bases[baseToTarget].Find("SecretAgent").transform.position);
        }
        else if(baseToTarget == -1 && isFriendly && !enemyToTarget)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }

        //A player in range of the enemy is prioritized above everything else
        if (playerInRange && !isFriendly && (!enemyToTarget || priorityVal >= playerPriorityThreshold))
        {
            if (baseToTarget != -2)
            {
                navMeshAgent.SetDestination(player.transform.position);
                gun.transform.LookAt(player.transform);
            }
            
            //Make sure drone always faces player
            if(playerDist <= stopDist)
            {
                Vector3 dir = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
                transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            }

            if(Time.time - timeOfLastFire > firingRate)
            {
                timeOfLastFire = Time.time;
                Quaternion rot = gun.transform.rotation;
                rot = Quaternion.Euler(rot.eulerAngles.x + Random.Range(-2, 2), rot.eulerAngles.y + Random.Range(-2, 2), rot.eulerAngles.z);
                GameObject g = Instantiate(laser, gun.transform.position, rot);

                
            }
        }

        if(health <= 0)
        {
            Instantiate(drop, model.transform.position + new Vector3(0, 3, 0), Quaternion.identity);
            Instantiate(drop, model.transform.position + new Vector3(0, 3, 0), Quaternion.identity);
            spawnMissileSites.GetComponent<SpawnMissileSites>().RemoveID(transform.gameObject.GetInstanceID());
            Destroy(gameObject);
        }


    }

    bool FindEnemiesInRange()
    {
        //Check colliders in vicinity for drones
        hits = Physics.OverlapSphere(transform.position, detectionRange);
        bool returnVal = false;
        float maxDist = float.MaxValue;
        for(int i = 0; i < hits.Length; i++)
        {
            //Check if enemy
            if(hits[i].tag == "Drone" && hits[i].transform.parent.GetComponent<Drone>().isFriendly == !isFriendly)
            {
                returnVal = true;
                //Prioritize closest enemy to fight
                float d = Vector3.Distance(transform.position, hits[i].transform.parent.position);
                if (d < maxDist && enemyTarget == null)
                {
                    maxDist = d;
                    enemyTarget = hits[i].transform.parent;
                }
            }
        }

        return returnVal;
    }
}
