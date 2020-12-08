using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using System;
using UnityEngine.AI;
using System.Linq;
using System.Collections.Specialized;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action that makes a gameObject move towards a destination.
    /// </summary>
    [Action("MyActions/SteeringSeek")]
    [Help("Gets a launch force, finds an angle on the y axis by which instantiating the bullet with said force will reach the enemy tank, and shoots")]
    public class SteeringSeek : GOAction
    {
        ///<value>Input Target Position.</value>
        [InParam("Target Position")]
        [Help("Position to reach")]
        Vector3 targetPos;

        ///<value>Input Stop Distance.</value>
        [InParam("Stop Distance")]
        [Help("Float Distance to the target at which the object stops moving.")]
        public float stopDistance = 2.0f;

        ///<value>Input Slow Distance.</value>
        [InParam("Slow Distance")]
        [Help("Float Distance to the target at which the object starts slowing down.")]
        public float slowDistance = 4.0f;

        ///<value>Input Maximum Turn Speed.</value>
        [InParam("Max Turn Speed")]
        [Help("Maximum Angular Speed the object can reach.")]
        public float maxTurnSpeed = 5.0f;

        ///<value>Input Maximum Speed.</value>
        [InParam("Max Speed")]
        [Help("Maximum Linear Speed the object can reach.")]
        public float maxSpeed = 5.0f;

        ///<value>Input Maximum Acceleration.</value>
        [InParam("Max Acceleration")]
        [Help("Maximum Linear Acceleration the object can reach.")]
        public float acceleration = 2.0f;

        ///<value>Input Maximum Turn Acceleration.</value>
        [InParam("Max Turn Acceleration")]
        [Help("Maximum Angular Acceleration the object can reach.")]
        public float turnAcceleration = 2.0f;

        ///<value>Local Turn Speed.</value>
        [OutParam("Turn Speed")]
        [Help("Angular Speed of the object.")]
        public float outTurnSpeed;

        ///<value>Local Speed.</value>
        [OutParam("Speed")]
        [Help("Linear Speed of the object.")]
        public float outMovSpeed;

        ///<value>Local Position.</value>
        [OutParam("Position")]
        [Help("Position of the object.")]
        public Vector3 outPosition;

        ///<value>Local Rot.</value>
        [OutParam("Rot")]
        [Help("Calculation of the future Rotation of the object.")]
        public Quaternion outRot;

        ///<value>Input Turn Speed.</value>
        [InParam("Turn Speed")]
        [Help("Angular Speed of the object.")]
        public float turnSpeed;

        ///<value>Input Speed.</value>
        [InParam("Speed")]
        [Help("Linear Speed of the object.")]
        public float movSpeed;

        ///<value>Input Position.</value>
        [InParam("Position")]
        [Help("Position of the object.")]
        public Vector3 position;

        ///<value>Input Rot.</value>
        [InParam("Rot")]
        [Help("Calculation of the future Rotation of the object.")]
        public Quaternion rot;

        ///<value>Input Wander Radius.</value>
        [InParam("Wander Radius")]
        [Help("Radius of the circle where the random targets of wander can be.")]
        public float wanderRadius = 2f;

        ///<value>Input Wander Offset.</value>
        [InParam("Wander Offset")]
        [Help("Distance from the object to the center of the circle where the random targets of wander can be.")]
        public float wanderOffset = 3f;

        private LineRenderer circle;
        private int circle_segments = 50;
        private GameObject wanderWaypoint;
        private Collider[] obstacles;
        private Vector3 movement;
        private Quaternion rotation;

        /// <summary>Initialization Method of MoveToRandomPosition.</summary>
        /// <remarks>Check if there is a NavMeshAgent to assign it one by default and assign it
        /// to the NavMeshAgent the destination a random position calculated with <see cref="getRandomPosition()"/> </remarks>
        public override void OnStart()
        {
            if (gameObject.CompareTag("Player 1"))
                targetPos = GameObject.Find("Ghost(Clone)").transform.position;

            if (gameObject.CompareTag("Player 2"))
            {
                circle = gameObject.GetComponent<LineRenderer>();
                circle.enabled = false;

                //code for movement using wander
                GameObject[] gos = GameObject.FindGameObjectsWithTag("Obstacles").ToArray();

                obstacles = new Collider[gos.Length];

                for (int i = 0; i < gos.Length; i++)
                {
                    obstacles[i] = gos[i].GetComponent<Collider>();
                }

                circle.positionCount = circle_segments + 1;
                circle.useWorldSpace = false;
                circle.startWidth = 0.1f;
                circle.endWidth = 0.1f;

                wanderWaypoint = GameObject.Find("WanderWaypoint");

                position = gameObject.transform.position;
                Wander();
            }
        }
        /// <summary>Method of Update of MoveToRandomPosition </summary>
        /// <remarks>Check the status of the task, if it has traveled the road or is close to the goal it is completed
        /// and otherwise it will remain in operation.</remarks>
        public override TaskStatus OnUpdate()
        {
            if (gameObject.CompareTag("Player 1"))
                targetPos = GameObject.Find("Ghost(Clone)").transform.position;

            if(gameObject.CompareTag("Player 2") && Input.GetKeyDown(KeyCode.D))
            {
                circle.enabled = !circle.enabled;
                wanderWaypoint.GetComponent<Renderer>().enabled = !wanderWaypoint.GetComponent<Renderer>().enabled;
            }

            if (Vector3.Distance(targetPos, position) < stopDistance)
                return TaskStatus.COMPLETED;

            Seek(targetPos);


            turnSpeed += turnAcceleration * Time.deltaTime;
            turnSpeed = Mathf.Min(turnSpeed, maxTurnSpeed);

            if (Vector3.Distance(targetPos, position) < slowDistance)
            {
                //slows movement when arriving destination
                movSpeed = (maxSpeed * Vector3.Distance(targetPos, position)) / slowDistance;
            }
            else
            {
                movSpeed += acceleration * Time.deltaTime;
                movSpeed = Mathf.Min(movSpeed, maxSpeed);
            }

            rot = Quaternion.Slerp(rot, rotation, Time.deltaTime * turnSpeed);
            position += gameObject.transform.forward.normalized * movSpeed * Time.deltaTime;
            gameObject.GetComponent<NavMeshAgent>().destination = position;
            gameObject.transform.rotation = rot;

            outTurnSpeed = turnSpeed;
            outMovSpeed = movSpeed;
            outPosition = position;
            outRot = rot;
            

            return TaskStatus.RUNNING;

    }

        private void Seek(Vector3 targetPos)
        {
            Vector3 direction = targetPos - position;
            direction.y = 0f;
            movement = direction.normalized * acceleration;
            float angle = Mathf.Rad2Deg * Mathf.Atan2(movement.x, movement.z);
            rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }

        void Wander()
        {
            //Select a random point inside circle to set as target and move to it.
            Vector3 localTarget = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0, UnityEngine.Random.Range(-1.0f, 1.0f));
            localTarget.Normalize();
            localTarget *= wanderRadius;
            localTarget += new Vector3(0, 0, wanderOffset);
            Vector3 worldTarget = gameObject.transform.TransformPoint(localTarget);
            worldTarget.y = 0f;

            //check if the point its inside the obstacles to set a better point to move.
            for (int i = 0; i < obstacles.Length; i++)
            {
                while (obstacles[i].bounds.Contains(worldTarget))
                {
                    localTarget = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0, UnityEngine.Random.Range(-1.0f, 1.0f));
                    localTarget.Normalize();
                    localTarget *= wanderRadius;
                    localTarget += new Vector3(0, 0, wanderOffset);
                    worldTarget = gameObject.transform.TransformPoint(localTarget);
                    worldTarget.y = 0f;
                };
            }
            DebugCircle(wanderRadius);
            wanderWaypoint.transform.position = worldTarget;

            targetPos = worldTarget;
        }

        void DebugCircle(float radius)
        {
            float x;
            float z;

            float angle = 20.0f;

            for (int i = 0; i < (circle_segments + 1); i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
                z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

                circle.SetPosition(i, new Vector3(x, 0, z + wanderOffset));

                angle += (360.0f / circle_segments);
            }
        }
    }
}
