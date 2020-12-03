using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using System;
using System.Linq;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action that generates a random destination with some constraints.
    /// </summary>
    [Action("MyActions/Wander")]
    [Help("Gets a launch force, finds an angle on the y axis by which instantiating the bullet with said force will reach the enemy tank, and shoots")]
    public class Wander : GOAction
    {
        ///<value>Input Stop Distance.</value>
        [InParam("Stop Distance")]
        [Help("Float Distance to the target at which the object stops moving.")]
        public float stopDistance = 2.0f;

        ///<value>Input Wander Radius.</value>
        [InParam("Wander Radius")]
        [Help("Radius of the circle where the random targets of wander can be.")]
        public float wanderRadius = 2f;

        ///<value>Input Wander Offset.</value>
        [InParam("Wander Offset")]
        [Help("Distance from the object to the center of the circle where the random targets of wander can be.")]
        public float wanderOffset = 3f;

        ///<value>Input Enemy Tank.</value>
        [InParam("Enemy")]
        [Help("Enemy tank")]
        public GameObject enemy;

        ///<value>Input Agent Tank.</value>
        [InParam("Agent")]
        [Help("Agent tank")]
        public GameObject agent;

        ///<value>Output Target Position.</value>
        [OutParam("Target Position")]
        [Help("Position to reach")]
        Vector3 targetPos;

        private LineRenderer circle;
        [Header("Wander Debug Parameters")]
        [Range(0, 50)]
        private int circle_segments = 50;
        
        
        
        private GameObject wanderWaypoint;
        private Collider[] obstacles;
        private Vector3 position;

        /// <summary>Initialization Method of MoveToRandomPosition.</summary>
        /// <remarks>Check if there is a NavMeshAgent to assign it one by default and assign it
        /// to the NavMeshAgent the destination a random position calculated with <see cref="getRandomPosition()"/> </remarks>
        public override void OnStart()
        {
            circle = agent.GetComponent<LineRenderer>();
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

            position = agent.transform.position;
            DoWander();
        }
        /// <summary>Method of Update of MoveToRandomPosition </summary>
        /// <remarks>Check the status of the task, if it has traveled the road or is close to the goal it is completed
        /// and otherwise it will remain in operation.</remarks>
        public override TaskStatus OnUpdate()
        {

            if (Input.GetKeyDown(KeyCode.D))
            {
                circle.enabled = !circle.enabled;
                    wanderWaypoint.GetComponent<Renderer>().enabled = !wanderWaypoint.GetComponent<Renderer>().enabled;
            }

            if (Vector3.Distance(targetPos, agent.transform.position) < stopDistance)
                DoWander();

            return TaskStatus.COMPLETED;

        }

        void DoWander()
        {
            //Select a random point inside circle to set as target and move to it.
            Vector3 localTarget = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0, UnityEngine.Random.Range(-1.0f, 1.0f));
            localTarget.Normalize();
            localTarget *= wanderRadius;
            localTarget += new Vector3(0, 0, wanderOffset);
            Vector3 worldTarget = agent.transform.TransformPoint(localTarget);
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
                    worldTarget = agent.transform.TransformPoint(localTarget);
                    worldTarget.y = 0f;
                };
            }

            DebugCircle(wanderRadius);
            wanderWaypoint.transform.position = worldTarget;

            targetPos = worldTarget;
        }

        //Debug method for wander circle
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
