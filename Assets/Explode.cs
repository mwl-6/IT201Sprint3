using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    // Start is called before the first frame update

    bool exploded = false;
    public float timeOfExplosion;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(exploded && Time.time - timeOfExplosion > 15)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    public void ExplodeMesh()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-200, 200), Random.Range(-50, 200), Random.Range(-200, 200)));
        }
        exploded = true;
    }
}
