using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag != "MissileLauncher")
        {
            Debug.Log(collision.transform.tag);
            Debug.Log(collision.transform.name);
            Destroy(gameObject);
        }

    }
}
