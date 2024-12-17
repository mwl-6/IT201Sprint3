using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int numberOfMissiles;
    private int numberOfFactories;

    public float timeBetweenDecisions;
    private float lastDecision = 0;

    public int[] layout;
    public int[] destroyed;

    [SerializeField] private GameObject factory;
    [SerializeField] private GameObject missileSite;
    [SerializeField] private Vector3[] spawnLocations;

    [SerializeField] private GameObject playerMissileSiteSpawner;

    public Transform[] assets;

    private int countM;
    private int countF;

    public TMP_Text base1;
    public TMP_Text base2;
    public TMP_Text base3;
    public TMP_Text base4;

    public bool everythingDestroyed = false;
    [SerializeField] GameObject door;
    float timeOfOpen = 0;
    

    void Start()
    {
        lastDecision = Time.time;
        numberOfMissiles = Random.Range(1, 3);
        numberOfFactories = 4 - numberOfMissiles;
        countM = numberOfMissiles;
        countF = numberOfFactories;
        int i = 0;
        while(countF > 0 || countM > 0)
        {
            if(countF > countM)
            {
                GameObject g = Instantiate(factory, new Vector3(spawnLocations[i].x,31.6f, spawnLocations[i].z), Quaternion.Euler(-90, 0, 90));
                assets[i] = g.transform;
                layout[i] = 1;
                countF--;
                Debug.Log("Added factory");
            }
            else if(countM >= countF)
            {
                GameObject g = Instantiate(missileSite, new Vector3(spawnLocations[i].x, 29.994f, spawnLocations[i].z), Quaternion.Euler(-90, 0, -90));
                assets[i] = g.transform;
                layout[i] = 0;
                countM--;
                Debug.Log("Added missile");
            }
            destroyed[i] = 0;
            i++;
        }
    }
    private void Awake()
    {
        layout = new int[4];
        assets = new Transform[4];
        destroyed = new int[4];
    }

    // Update is called once per frame
    void Update()
    {

        if(Time.time - lastDecision > timeBetweenDecisions)
        {
            MakeDecision();
            lastDecision = Time.time;
        }

        int broken = 0;
        if(layout[0] == 0)
        {
            if (assets[0].GetComponent<EnemyMissileLauncher>().health > 0)
            {
                base1.text = "Missile Site A: " + assets[0].GetComponent<EnemyMissileLauncher>().health;
            }
            else
            {
                base1.text = "Missile Site A: Destroyed";
                broken++;
                destroyed[0] = 1;
            }
        }
        else
        {
            if (assets[0].GetComponent<EnemyFacility>().health > 0)
            {
                base1.text = "Factory A: " + assets[0].GetComponent<EnemyFacility>().health;
            }
            else
            {
                base1.text = "Factory A: Destroyed";
                broken++;
                destroyed[0] = 1;
            }
        }

        if (layout[1] == 0)
        {
            if (assets[1].GetComponent<EnemyMissileLauncher>().health > 0)
            {
                base1.text = "Missile Site B: " + assets[1].GetComponent<EnemyMissileLauncher>().health;
            }
            else
            {
                base1.text = "Missile Site B: Destroyed";
                broken++;
                destroyed[1] = 1;
            }
        }
        else
        {
            if (assets[1].GetComponent<EnemyFacility>().health > 0)
            {
                base2.text = "Factory B: " + assets[1].GetComponent<EnemyFacility>().health;
            }
            else
            {
                base2.text = "Factory B: Destroyed";
                broken++;
                destroyed[1] = 1;
            }
        }

        if (layout[2] == 0)
        {
            if (assets[2].GetComponent<EnemyMissileLauncher>().health > 0)
            {
                base3.text = "Missile Site C: " + assets[2].GetComponent<EnemyMissileLauncher>().health;
            }
            else
            {
                base3.text = "Missile Site C: Destroyed";
                broken++;
                destroyed[2] = 1;
            }
        }
        else
        {
            if (assets[2].GetComponent<EnemyFacility>().health > 0)
            {
                base3.text = "Factory C: " + assets[2].GetComponent<EnemyFacility>().health;
            }
            else
            {
                base3.text = "Factory C: Destroyed";
                broken++;
                destroyed[2] = 1;
            }
        }


        if (layout[3] == 0)
        {
            if (assets[3].GetComponent<EnemyMissileLauncher>().health > 0)
            {
                base4.text = "Missile Site D: " + assets[3].GetComponent<EnemyMissileLauncher>().health;
            }
            else
            {
                base4.text = "Missile Site D: Destroyed";
                broken++;
                destroyed[3] = 1;
            }
        }
        else
        {
            if (assets[3].GetComponent<EnemyFacility>().health > 0)
            {
                base4.text = "Factory D: " + assets[3].GetComponent<EnemyFacility>().health;
            }
            else
            {
                base4.text = "Factory D: Destroyed";
                broken++;
                destroyed[3] = 1;
                
            }
        }


        if(broken >= 4 && !everythingDestroyed)
        {
            everythingDestroyed = true;
            timeOfOpen = Time.time;
        }

        if (everythingDestroyed)
        {
            door.SetActive(false);
        }
    }

    void MakeDecision()
    {
        int facility = Random.Range(0, 4);
        while(destroyed[facility] == 1)
        {
            facility = Random.Range(0, 4);
        }
        //Missile
        if(layout[facility] == 0)
        {
            int target = Random.Range(0, 4);
            assets[facility].GetComponent<EnemyMissileLauncher>().target = playerMissileSiteSpawner.GetComponent<SpawnMissileSites>().bases[target].Find("SecretAgent").position;
            assets[facility].GetComponent<EnemyMissileLauncher>().hasTarget = true;
            assets[facility].GetComponent<EnemyMissileLauncher>().fireMissile();
        }//Factory
        else if(layout[facility] == 1)
        {
            int target = Random.Range(0, 4);
            for(int i = 0; i < 5; i++)
            {
                assets[facility].GetComponent<EnemyFacility>().SpawnEnemy(target);
            }
        }
    }
}
