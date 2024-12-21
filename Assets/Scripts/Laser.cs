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

    Vector3 previousPosition;
    public LayerMask layersToIgnore;

    void Start()
    {
        timeOfStart = Time.time;
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        previousPosition = transform.position;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //I use raycasting instead of triggerenter for continuous collision

        RaycastHit hit;
        //Debug.DrawLine(oldPos - transform.forward * transform.localScale.z / 2.0f, newPos + transform.forward * transform.localScale.z / 2.0f);
        //Debug.DrawLine(oldPos - transform.forward * transform.localScale.z / 2.0f, oldPos + transform.up * speed * Time.deltaTime);
        //Debug.DrawRay(oldPos, transform.forward, Color.red);
        if(Physics.Linecast(previousPosition, transform.position, out hit, ~layersToIgnore))
        {
            if (hit.collider != null)
            {
                Collider col = hit.collider;
                ProcessCollision(col);
            }
        }

        //If a laser WILL hit the drone in the future, warn the drone to give them the chance to dodge
        Debug.DrawRay(previousPosition, transform.position - previousPosition);
        RaycastHit futureHit;
        if(Physics.Raycast(transform.position, transform.position - previousPosition, out futureHit))
        {
            if(futureHit.transform.tag == "Drone" && !futureHit.transform.parent.GetComponent<Drone>().dodging)
            {
                if ((futureHit.transform.parent.GetComponent<Drone>().isFriendly && laserType == 1) || (!futureHit.transform.parent.GetComponent<Drone>().isFriendly && laserType != 1))
                {
                    futureHit.transform.parent.GetComponent<Drone>().dodging = true;
                    futureHit.transform.parent.GetComponent<Drone>().timeOfDodge = Time.time;
                }
            }
        }


        //Destroy after 10 seconds
        if(Time.time - timeOfStart > 10)
        {
            Destroy(gameObject);
        }
    }

    /*
     * Handle collisions detected through either triggerenter or raycasting
     * */
    private void ProcessCollision(Collider hit)
    {
        Debug.Log(hit.name);
        //Player or friendly drone hits drone
        if (hit.transform.tag == "Drone" && (laserType == 0 || laserType == 2))
        {
            Debug.Log("Hit");
            //Hurt enemy
            if (!hit.transform.parent.GetComponent<Drone>().isFriendly)
            {
                hit.transform.parent.GetComponent<Drone>().health--;
                hit.transform.parent.gameObject.GetComponent<Animator>().SetTrigger("shot");
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
                hit.transform.parent.gameObject.GetComponent<Animator>().SetTrigger("shot");
                Destroy(gameObject);
            }
            else
            {
                //Dont hurt allies
                Destroy(gameObject);
            }
        }

        if(hit.gameObject.tag != "Player" && laserType == 0 && !(hit.gameObject.layer == 7))
            Destroy(gameObject);
    }

    //Get rid of laser if it hits a building or terrain
    private void OnTriggerEnter(Collider other)
    {
        //ProcessCollision(other);
        if (other.transform.tag != "scrap" && other.transform.tag != "Drone" && other.transform.tag != "Player" && other.gameObject.GetComponent<MissileLauncher>() == null && other.gameObject.layer != 7)
        {
            Destroy(gameObject);
        }
    }
}
