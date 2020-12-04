using UnityEngine;

using Pada1.BBCore.Framework;
using Pada1.BBCore;

namespace BBUnity.Conditions
{
    [Condition("MyConditions/Is Enemy Near?")]
    [Help("Checks whether another tank is near this one.")]

    public class IsEnemyNear : GOCondition
    {
        ///<value>Input Gravity.</value>
        [InParam("Gravity")]
        [Help("Gravity of the world")]
        public float gravity;

        ///<value>Input Launch Force.</value>
        [InParam("Launch Force")]
        [Help("Second value to be compared")]
        public float launchForce;


        private GameObject enemy;

        public override bool Check()
        {
            GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; //will return an array of all GameObjects in the scene
            foreach (GameObject go in gos)
            {
                if (go.layer == 9 && go != gameObject && go.name != "FireTransform")
                {
                    enemy = go.gameObject;
                }
            }

            return Mathf.Abs(Vector3.Distance(enemy.transform.position, gameObject.transform.position)) < ((launchForce * launchForce) / gravity);
        }
    }
}
