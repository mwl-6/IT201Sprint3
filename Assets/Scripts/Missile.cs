using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody rb;
    public int type = 0;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag != "MissileLauncher" && type == 0)
        {
            
            if (other.transform.tag == "Test")
            {
                other.gameObject.GetComponent<EnemyMissileLauncher>().health--;
            }
            else if(other.transform.tag == "Production")
            {
                other.gameObject.GetComponent<EnemyFacility>().health--;
            }
            Destroy(gameObject);
        }

        if (other.transform.tag != "Test" && type == 1)
        {
            Debug.Log(other.transform.tag);
            Debug.Log(other.transform.name);

            if (other.transform.tag == "MissileLauncher")
            {
                other.gameObject.SetActive(false);
            }
            else if (other.transform.tag == "Production")
            {
                other.gameObject.GetComponent<EnemyFacility>().health--;
            }

            Destroy(gameObject);
        }

        


    }
}
