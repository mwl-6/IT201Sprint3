using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFacility : MonoBehaviour
{
    // Start is called before the first frame update
    public int health = 3;
    [SerializeField] private GameObject drone;
    bool dead = false;
    private void Update()
    {
        if(health <= 0 && !dead)
        {
            dead = true;
            if(transform.tag == "Production")
            {
                transform.GetComponent<Renderer>().enabled = false;
                transform.Find("shardsdroneprod").gameObject.SetActive(true);
                transform.Find("shardsdroneprod").GetComponent<Explode>().timeOfExplosion = Time.time;
                transform.Find("shardsdroneprod").GetComponent<Explode>().ExplodeMesh();
            }
        }
    }

    public void SpawnEnemy(int target)
    {
        GameObject d = Instantiate(drone, new Vector3(transform.position.x + Random.Range(0, 10), 29f, transform.position.z + Random.Range(0, 10)), Quaternion.identity);
        d.transform.GetComponent<Drone>().baseToTarget = target;
    }
}
