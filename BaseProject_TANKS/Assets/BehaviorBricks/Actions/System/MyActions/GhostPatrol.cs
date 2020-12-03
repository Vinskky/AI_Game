using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using System;
using System.Linq;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action that makes destination of the ghost's position.
    /// </summary>
    [Action("MyActions/GhostPatrol")]
    [Help("Gets a launch force, finds an angle on the y axis by which instantiating the bullet with said force will reach the enemy tank, and shoots")]
    public class Patrol : GOAction
    {
        ///<value>Output Target Position.</value>
        [OutParam("Target Position")]
        [Help("Position to reach")]
        Vector3 targetPos;

        private GameObject ghost;

        /// <summary>Initialization Method of MoveToRandomPosition.</summary>
        /// <remarks>Check if there is a NavMeshAgent to assign it one by default and assign it
        /// to the NavMeshAgent the destination a random position calculated with <see cref="getRandomPosition()"/> </remarks>
        public override void OnStart()
        {
            //Assign object target to follow (ghost object)
            ghost = GameObject.Find("Ghost(Clone)");
        }
        /// <summary>Method of Update of MoveToRandomPosition </summary>
        /// <remarks>Check the status of the task, if it has traveled the road or is close to the goal it is completed
        /// and otherwise it will remain in operation.</remarks>
        public override TaskStatus OnUpdate()
        {
            //SteeringSeek to following the ghost and allow a smooth path patrol
            targetPos = ghost.transform.position;

            return TaskStatus.COMPLETED;

        }

    }
}