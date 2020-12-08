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
    [Action("MyActions/GetNearestRecharge")]
    [Help("Finds the nearest recharge spot and finds its position.")]
    public class GetNearestRecharge : GOAction
    {

        ///<value>Output Target Position.</value>
        [OutParam("Target Position")]
        [Help("Position to reach")]
        Vector3 targetPos;

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
            GameObject recharge = GameObject.Find("Recharge");
            targetPos = recharge.transform.position;

            return TaskStatus.COMPLETED;

        }
    }
}
