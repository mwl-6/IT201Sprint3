using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public int health = 20;
    public int scraps = 0;
    public float firingRate = 0.1f;
    public float firingSpeed = 25;

    private float timeOfFire;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject spawner;

    public TMP_Text healthTxt;
    public TMP_Text wealthTxt;
    public TMP_Text strengthTxt;

    [SerializeField] private GameObject TaskManager;

    void Start()
    {
        timeOfFire = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        healthTxt.text = "Player Health: " + health.ToString();
        if(health <= 0)
        {
            healthTxt.text = "Game Over!";
            transform.GetComponent<FPSController>().canMove = false;
        }
        wealthTxt.text = "Metal Scraps: " + scraps.ToString();
        strengthTxt.text = "Firing Rate: " + firingRate.ToString() + "/s";


        if(Input.GetMouseButton(0) && Time.time - timeOfFire > firingRate && GetComponent<FPSController>().canMove)
        {
            timeOfFire = Time.time;
            GameObject g = Instantiate(laser, Camera.main.transform.position, Camera.main.transform.rotation);
            g.GetComponent<Laser>().laserType = 0;
            g.GetComponent<Laser>().speed = firingSpeed;
        }

        RaycastHit hit;
        
        if(GetComponent<FPSController>().canMove && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if(hit.transform.tag == "Lever" && scraps >= 4)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    spawner.GetComponent<SpawnFriendlyDrones>().SpawnDrone();
                    scraps -= 4;
                }
            }
            if(hit.transform.tag == "Drone" && Input.GetMouseButtonDown(1) && hit.transform.parent.GetComponent<Drone>().isFriendly)
            {
                TaskManager.GetComponent<TaskManager>().ToggleTaskManager(hit.transform.parent);
            }
        }
    }
}
