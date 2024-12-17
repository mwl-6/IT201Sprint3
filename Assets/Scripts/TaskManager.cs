using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject spawnMissileSites;
    [SerializeField] private TMP_Text currentTask;

    private Transform activeDrone;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            TaskOption(1);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            TaskOption(2);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            TaskOption(3);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            TaskOption(4);
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            TaskOption(5);
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            TaskOption(6);
        }
    }

    public void ToggleTaskManager(Transform drone)
    {
        if (transform.gameObject.activeSelf)
        {
            activeDrone = null;
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            activeDrone = drone;

            int target = activeDrone.GetComponent<Drone>().baseToTarget + 1;
            if(target <= 4 && target >= 1)
            {
                currentTask.text = "Assign Task to Drone [Current Task: Defend Base " + target.ToString() + "]";
            }
            else if(target == 0)
            {
                currentTask.text = "Assign Task to Drone [Current Task: Protect Player]";
            }
            else if(target == -1)
            {
                currentTask.text = "Assign Task to Drone [Current Task: None]";
            }
        }
    }

    public void TaskOption(int option)
    {
        //The options correspond to the bases
        if(option < 5)
        {
            activeDrone.GetComponent<Drone>().baseToTarget = option-1;
        }
        else if(option == 5)
        {
            //follow player
            activeDrone.GetComponent<Drone>().baseToTarget = -1;
        }
        else if(option == 6)
        {
            //do nothing
            activeDrone.GetComponent<Drone>().baseToTarget = -2;
        }
        //close everything
        ToggleTaskManager(null);
    }
}
