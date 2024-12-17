using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public int health = 20;
    public int scraps = 0;
    public float firingRate = 0.1f;
    public float firingSpeed = 25;

    private float timeOfFire;
    [SerializeField] GameObject laser;
    void Start()
    {
        timeOfFire = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && Time.time - timeOfFire > firingRate)
        {
            timeOfFire = Time.time;
            GameObject g = Instantiate(laser, Camera.main.transform.position, Camera.main.transform.rotation);
            g.GetComponent<Laser>().laserType = 0;
            g.GetComponent<Laser>().speed = firingSpeed;
        }
    }
}
