using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using System;
using UnityEngine.AI;

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

        ///<value>Input Steering Seek Timer.</value>
        [InParam("Seek Timer")]
        [Help("Timer that controls the frequency at which the seek variables are applied to the object.")]
        public float timer = 0.6f;


        private float turnSpeed;
        private float movSpeed;
        private Quaternion rotation;
        private Vector3 movement;
        private Vector3 position;
        private Quaternion rot;

        /// <summary>Initialization Method of MoveToRandomPosition.</summary>
        /// <remarks>Check if there is a NavMeshAgent to assign it one by default and assign it
        /// to the NavMeshAgent the destination a random position calculated with <see cref="getRandomPosition()"/> </remarks>
        public override void OnStart()
        {

        }
        /// <summary>Method of Update of MoveToRandomPosition </summary>
        /// <remarks>Check the status of the task, if it has traveled the road or is close to the goal it is completed
        /// and otherwise it will remain in operation.</remarks>
        public override TaskStatus OnUpdate()
        {

            if (Vector3.Distance(targetPos, position) < stopDistance)
                return TaskStatus.FAILED;

            //seek delay so it does not iterate every frame
            if (timer > 0.5)
            {
                Seek(targetPos);
            }


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

            if (timer > 0.5)
            {
                gameObject.GetComponent<NavMeshAgent>().destination = position;
                timer = 0;
            }
            gameObject.transform.rotation = rot;

            timer += Time.smoothDeltaTime;

            return TaskStatus.COMPLETED;

    }

        private void Seek(Vector3 targetPos)
        {
            Vector3 direction = targetPos - position;
            direction.y = 0f;
            movement = direction.normalized * acceleration;
            float angle = Mathf.Rad2Deg * Mathf.Atan2(movement.x, movement.z);
            rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
    }
}
