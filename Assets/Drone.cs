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
    [SerializeField] GameObject laser;
    [SerializeField] GameObject drop;

    public int health = 1;
    public bool isFriendly = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();

        stopDist = Random.Range(12, 21);
        navMeshAgent.stoppingDistance = stopDist;
        model = transform.Find("Model");
        gun = model.Find("Gun");

        detectionRange = 50;
        ignoreRange = detectionRange*1.5f;

        timeOfLastFire = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        model.transform.localPosition = new Vector3(0, Mathf.Lerp(0, 1, (Mathf.Cos(Time.time) + 1) / 2.0f), 0);
        model.Find("Icosphere").Find("Spin").Rotate(new Vector3(0, 0, 2 * Time.deltaTime));
        float playerDist = Vector3.Distance(player.transform.position, transform.position);
        if (playerDist < detectionRange)
        {
            playerInRange = true;
        }
        else if(playerDist > ignoreRange)
        {
            playerInRange = false;
        }

        if (playerInRange)
        {
            navMeshAgent.SetDestination(player.transform.position);
            gun.transform.LookAt(player.transform);
            if(playerDist <= stopDist)
            {
                Vector3 dir = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
                transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            }

            if(Time.time - timeOfLastFire > firingRate)
            {
                timeOfLastFire = Time.time;
                GameObject g = Instantiate(laser, gun.transform.position, gun.transform.rotation);
                g.GetComponent<Laser>().laserType = 1;
            }
        }

        if(health <= 0)
        {
            Instantiate(drop, model.transform.position + new Vector3(0, 3, 0), Quaternion.identity);
            Instantiate(drop, model.transform.position + new Vector3(0, 3, 0), Quaternion.identity);
            Destroy(gameObject);
        }


    }
}
