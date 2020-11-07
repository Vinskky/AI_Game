using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Patrol : MonoBehaviour
{
    private Transform[] waypoints;
    public int destPoint = 0;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //create a new array with name tagg in order, so we can do the path as we want.

        GameObject[] gos = GameObject.FindGameObjectsWithTag("WayPoint").OrderBy(go => go.name).ToArray();
        
        //Set transform vector = length of the sorted array

        waypoints = new Transform[gos.Length];

         for(int i = 0; i < gos.Length; i++)
         {
            //Assign transform of the vector of gameobjects sets as waypoints to the vector of transform.
             waypoints[i] = gos[i].transform;
         }

        agent.autoBraking = false;

        GoNextWayPoint();
    }

    void GoNextWayPoint()
    {
        if (waypoints.Length == 0)
        {
            return;
        }

        agent.destination = waypoints[destPoint].position;

        destPoint = (destPoint + 1) % waypoints.Length;

    }
    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoNextWayPoint();

        if (Input.GetKeyDown(KeyCode.D))
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                //Assign transform of the vector of gameobjects sets as waypoints to the vector of transform.
                waypoints[i].GetComponent<Renderer>().enabled = !waypoints[i].GetComponent<Renderer>().enabled;
            }

            this.GetComponent<Renderer>().enabled = !this.GetComponent<Renderer>().enabled;
        }
    }

  
}
