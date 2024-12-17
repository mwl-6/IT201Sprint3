using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnMissileSites : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject missileSite;
    public int siteNumber = 4;

    [SerializeField] private List<Vector3> spawnOptions;
    [SerializeField] private GameObject enemy;

    //Stores every base in the scene
    public List<Transform> bases;

    int bonusDrones = 20;
    void Awake()
    {
        bases = new List<Transform>();
        for(int i = 0; i < siteNumber; i++)
        {
            /*
             * First pick a random option for a base location and assign a new missile site
             * */
            int location = Random.Range(0, spawnOptions.Count);
            GameObject g = Instantiate(missileSite, spawnOptions[location] + new Vector3(0, 0.15f, 0), Quaternion.Euler(-90, 0, 90));
            spawnOptions.RemoveAt(location);

            //Store reference to new base
            bases.Add(g.transform);

            //Add a random number of extra drones to each base
            //Must be enough bonus drones remaining and must be under 7
            int bonus = 0;
            if (bonusDrones > 0) {
                bonus = Random.Range(1, Mathf.Min(bonusDrones, 6));
                bonusDrones -= bonus;
            }

            g.transform.GetComponent<MissileLauncher>().enemiesOccupying = new HashSet<int>();

            //Create the new drones
            int dronesToAdd = 4 + bonus;
            for(int j = 0; j < dronesToAdd; j++)
            {
                Vector3 spawnLoc = g.transform.position + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
                Bounds b = g.transform.Find("Collider").GetComponent<BoxCollider>().bounds;

                //Spawn within the range of site but make sure it is not inside the missile launcher
                while (b.Contains(spawnLoc))
                {
                    spawnLoc = g.transform.position + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
                }

                //Find a location that places the drone on the navmesh
                //RaycastHit r;
                //Physics.Raycast(spawnLoc, Vector3.down, out r);
                //spawnLoc = new Vector3(spawnLoc.x, r.point.y + 2.0f, spawnLoc.z);

                NavMeshHit hit;
                if (NavMesh.SamplePosition(spawnLoc, out hit, 20.0f, NavMesh.AllAreas))
                {
                    spawnLoc = hit.position;
                }

                //Make the drone and add it to the occupying list
                GameObject d = Instantiate(enemy, spawnLoc, Quaternion.identity);
                d.transform.GetComponent<Drone>().baseToTarget = i;

                g.transform.GetComponent<MissileLauncher>().enemiesOccupying.Add(d.GetInstanceID());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveID(int id)
    {
        for(int i = 0; i < bases.Count; i++)
        {
            bases[i].GetComponent<MissileLauncher>().RemoveID(id);
        }
    }
}
