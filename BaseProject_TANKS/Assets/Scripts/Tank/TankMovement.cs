using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI; // navmesh
using System.Linq; // sort array for waypoints

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;         
    public float m_Speed = 12f;            
    public float m_TurnSpeed = 180f;       
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;      
    public float m_PitchRange = 0.2f;

    
    private string m_MovementAxisName;     
    private string m_TurnAxisName;         
    private Rigidbody m_Rigidbody;         
    private float m_MovementInputValue;    
    private float m_TurnInputValue;        
    private float m_OriginalPitch;


    //Variables for using path using waypoints.
    public Transform[] waypoints;
    private int destPoint = 0;
    private NavMeshAgent agent;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }


    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        
         if (this.m_PlayerNumber == 1)
         {
            //code for movement using navmesh patrol

            //create a new array with name tagg in order, so we can do the path as we want.

            GameObject[] gos = GameObject.FindGameObjectsWithTag("WayPoint").OrderBy(go => go.name).ToArray();

            //Set transform vector = length of the sorted array

            waypoints = new Transform[gos.Length];

            for (int i = 0; i < gos.Length; i++)
            {
                //Assign transform of the vector of gameobjects sets as waypoints to the vector of transform.
                waypoints[i] = gos[i].transform;
            }

            agent.autoBraking = false;

            GoNextWayPoit();
        }
        else if (this.m_PlayerNumber == 2)
        {
            //code for movement using wander
        }

        /*m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;*/

        m_OriginalPitch = m_MovementAudio.pitch;
    }
    

    private void Update()
    {
        

        // Store the player's input and make sure the audio for the engine is playing.
        /*m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);*/

        EngineAudio();
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.

        if(Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            if(m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = UnityEngine.Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = UnityEngine.Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }

    }


    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        //Turn();
    }


    private void Move()
    {
        if (this.m_PlayerNumber == 1)
        {
            //code for movement using navmesh patrol
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GoNextWayPoit();
        }
        else if(this.m_PlayerNumber == 2)
        {
            //code for movement using wander
        }
         

        // Adjust the position of the tank based on the player's input.

        /*Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);*/

    }


    /*private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }*/

    void GoNextWayPoit() // code for going to next Waypoint
    {
        if (waypoints.Length == 0)
        {
            return;
        }

        agent.destination = waypoints[destPoint].position;

        destPoint = (destPoint + 1) % waypoints.Length;

    }
}