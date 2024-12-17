using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 10;
    //0 - player, 1 - enemy drone, 2 - friendly drone
    public int laserType = 0;

    private float timeOfStart;
    void Start()
    {
        timeOfStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.forward * speed * Time.deltaTime + transform.position;

        //I use raycasting instead of triggerenter for continuous collision
        RaycastHit hit;
        if(Physics.Linecast(transform.position, newPos, out hit))
        {
            //Enemy drones (from player/friendly drone)
            if(hit.transform.tag == "Drone" && (laserType == 0 || laserType == 2))
            {
                if (!hit.transform.parent.GetComponent<Drone>().isFriendly)
                {
                    hit.transform.parent.GetComponent<Drone>().health--;
                    Destroy(gameObject);
                }
            }

            //Player (from enemy drone)
            if (hit.transform.tag == "Player" && laserType == 1)
            {
                //hit.transform.GetComponent<Drone>().health--;
                Destroy(gameObject);
            }

            //Friendly drone (from enemy drone)
            if (hit.transform.tag == "Drone" && laserType == 1)
            {
                if (hit.transform.parent.GetComponent<Drone>().isFriendly)
                {
                    hit.transform.parent.GetComponent<Drone>().health--;
                    Destroy(gameObject);
                }
            }
        }

        transform.position = newPos;

        //Destroy after 10 seconds
        if(Time.time - timeOfStart > 10)
        {
            Destroy(gameObject);
        }
    }

    //Get rid of laser if it hits a building or terrain
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag != "Drone" && other.transform.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
