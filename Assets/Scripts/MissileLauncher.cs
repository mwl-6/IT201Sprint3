using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    // Start is called before the first frame update

    public int health = 1;
    //Could either be the player or a player drone
    public Transform activeController;
    [SerializeField] private GameObject missile;

    //For drones to move to
    public Vector3 commandPanelPos;

    private float timeSinceFire;
    public float fireRate = 5;

    private Transform player;
    private Camera mainCamera;

    Transform platform;

    private float lerpValue = 0.5f;

    
    public HashSet<int> enemiesOccupying;
    public HashSet<int> friendsOccupying;
    public bool inEnemyHands = true;

    //For debugging
    [SerializeField] private int occupyNumber = 0;

    void Start()
    {
        timeSinceFire = Time.time;
        player = GameObject.Find("Player").transform;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform.GetComponent<Camera>();
        platform = transform.Find("Platform");

        friendsOccupying = new HashSet<int>();
    }

    // Update is called once per frame
    void Update()
    {
        occupyNumber = enemiesOccupying.Count;
        platform.Find("Launcher").Find("pivot").localEulerAngles = new Vector3(0, Mathf.Lerp(0, 90, lerpValue), 0);
        if(activeController == player)
        {
            
            
            platform.Rotate(new Vector3(0, 0, Input.GetAxis("Horizontal") * 10f * Time.fixedDeltaTime));
            if (Input.GetKey(KeyCode.W) && lerpValue >= 0)
            {
                lerpValue -= 0.25f * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S) && lerpValue <= 1)
            {
                lerpValue += 0.25f * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                activeController = null;
                platform.Find("Camera").gameObject.SetActive(false);
                mainCamera.transform.gameObject.SetActive(true);
                player.GetComponent<FPSController>().canMove = true;
            }
            if(Input.GetKey(KeyCode.Space) && Time.time - timeSinceFire > fireRate)
            {
                Debug.Log("spawned missile");
                timeSinceFire = Time.time;
                Transform spawn = platform.Find("Launcher").Find("pivot").Find("Cannon");
                GameObject g = Instantiate(missile, spawn.transform.position, spawn.transform.rotation);
                g.GetComponent<Rigidbody>().AddForce(spawn.transform.forward * 2500);
            }
        }
        else if(Vector3.Distance(player.transform.position, transform.position) < 20 )
        {
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (activeController != player.transform && !inEnemyHands)
                {
                    activeController = player.transform;
                    platform.Find("Camera").gameObject.SetActive(true);
                    mainCamera.gameObject.SetActive(false);
                    player.GetComponent<FPSController>().canMove = false;
                }
            }
        }

        if((friendsOccupying.Count > enemiesOccupying.Count || enemiesOccupying.Count == 0))
        {
            inEnemyHands = false;
        }
        else
        {
            inEnemyHands = true;
        }
    }

    public void RemoveID(int id)
    {
        if (friendsOccupying.Contains(id))
        {
            friendsOccupying.Remove(id);
        }
        if (enemiesOccupying.Contains(id))
        {
            enemiesOccupying.Remove(id);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Drone")
        {
            if (other.GetComponent<Drone>().isFriendly && !friendsOccupying.Contains(other.transform.GetInstanceID()))
            {
                friendsOccupying.Add(other.transform.GetInstanceID());
            }
            else if(!other.GetComponent<Drone>().isFriendly && !enemiesOccupying.Contains(other.transform.GetInstanceID()))
            {
                enemiesOccupying.Add(other.transform.GetInstanceID());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Drone")
        {
            if (other.GetComponent<Drone>().isFriendly && friendsOccupying.Contains(other.transform.GetInstanceID()))
            {
                friendsOccupying.Remove(other.transform.GetInstanceID());
            }
            /*
            else if (!other.GetComponent<Drone>().isFriendly && enemiesOccupying.Contains(other.transform.GetInstanceID()))
            {
                enemiesOccupying.Remove(other.transform.GetInstanceID());
            }
            */
        }
    }

}
