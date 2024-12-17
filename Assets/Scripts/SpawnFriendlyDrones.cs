using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFriendlyDrones : MonoBehaviour
{
    // Start is called before the first frame update

    float timeOfLastSpawn;
    [SerializeField] private float spawnRate = 10;
    [SerializeField] private GameObject drone;
    [SerializeField] private Vector3 spawnLoc;
    [SerializeField] GameObject laser;

    [SerializeField] private Material greenEye;
    [SerializeField] private Material clearEye;
    void Start()
    {
        timeOfLastSpawn = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnDrone()
    {
        if(Time.time - timeOfLastSpawn > spawnRate)
        {
            Debug.Log("Spawned Drone");
            timeOfLastSpawn = Time.time;
            GameObject d = Instantiate(drone, spawnLoc + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)), Quaternion.identity);
            d.transform.GetComponent<Drone>().baseToTarget = -2;
            d.transform.GetComponent<Drone>().isFriendly = true;
            d.transform.GetComponent<Drone>().laser = laser;
            d.transform.Find("Model").Find("Icosphere").Find("Eye").Find("Cylinder.003").GetComponent<Renderer>().material = greenEye;
            d.transform.Find("Model").Find("Icosphere").Find("Eye").Find("eyeback").GetComponent<Renderer>().material = clearEye;

        }
    }
}
