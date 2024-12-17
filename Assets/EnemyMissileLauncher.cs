using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileLauncher : MonoBehaviour
{
    // Start is called before the first frame update

    public int health = 3;
    [SerializeField] private GameObject missile;


    Transform platform;

    private float lerpValue = 0.5f;
    public bool hasTarget = false;
    public Vector3 target;

    float randomV;

    void Start()
    {
        platform = transform.Find("Platform");
        randomV = Random.Range(0.8f, 1.8f);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTarget)
        {
            Vector3 dir = new Vector3(target.x - transform.position.x, 0, target.z - transform.position.z);
            platform.Find("Launcher").rotation = Quaternion.LookRotation(dir, Vector3.up);
            //platform.Find("Launcher").rotation = Quaternion.Euler(platform.Find("Launcher").eulerAngles.x, 0, platform.Find("Launcher").eulerAngles.z);

        }
        
        platform.Find("Launcher").Find("pivot").rotation = Quaternion.Euler(Mathf.Lerp(0, -60, (Mathf.Cos(Time.time*randomV)+1)/2.0f), 0, 0);

        if(health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void fireMissile()
    {
        Transform spawn = platform.Find("Launcher").Find("pivot").Find("Cannon");
        GameObject g = Instantiate(missile, spawn.transform.position, spawn.transform.rotation);
        g.GetComponent<Rigidbody>().AddForce(spawn.transform.forward * 2500);
    }


    
}
