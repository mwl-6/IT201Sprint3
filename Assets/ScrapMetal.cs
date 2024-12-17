using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapMetal : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-3, 3), Random.Range(3, 5), Random.Range(-3, 3)));
        GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            other.transform.GetComponent<Player>().scraps++;
            Destroy(gameObject);
        }
    }
}