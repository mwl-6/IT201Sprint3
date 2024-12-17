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
        Vector3 oldPos = transform.position;
        //I use raycasting instead of triggerenter for continuous collision
        RaycastHit hit;
        //Debug.DrawLine(oldPos - transform.forward * transform.localScale.z / 2.0f, newPos + transform.forward * transform.localScale.z / 2.0f);
        //Debug.DrawLine(oldPos - transform.forward * transform.localScale.z / 2.0f, oldPos + transform.up * speed * Time.deltaTime);
        //Debug.DrawRay(oldPos, transform.forward, Color.red);
        if(Physics.Linecast(transform.position - transform.forward * transform.localScale.z / 2.0f, newPos + transform.forward * transform.localScale.z / 2.0f, out hit))
        {
            //Player or friendly drone hits drone
            if(hit.transform.tag == "Drone" && (laserType == 0 || laserType == 2))
            {
                Debug.Log("Hit");
                //Hurt enemy
                if (!hit.transform.parent.GetComponent<Drone>().isFriendly)
                {
                    hit.transform.parent.GetComponent<Drone>().health--;
                    Destroy(gameObject);
                }
                else
                {
                    //Don't hurt allies
                    Destroy(gameObject);
                }
            }

            //Drone Hits Player
            if (hit.transform.tag == "Player" && (laserType == 1 || laserType == 2))
            {
                //Enemy Drone takes health away
                if (laserType == 1)
                {
                    hit.transform.GetComponent<Player>().health--;
                }
                Destroy(gameObject);
            }

            //Enemy drone hits drone
            if (hit.transform.tag == "Drone" && laserType == 1)
            {
                //Friendly drones get hurt
                if (hit.transform.parent.GetComponent<Drone>().isFriendly)
                {
                    hit.transform.parent.GetComponent<Drone>().health--;
                    Destroy(gameObject);
                }
                else
                {
                    //Dont hurt allies
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
        if(other.transform.tag != "Drone" && other.transform.tag != "Player" && other.gameObject.GetComponent<MissileLauncher>() == null)
        {
            Destroy(gameObject);
        }
    }
}
