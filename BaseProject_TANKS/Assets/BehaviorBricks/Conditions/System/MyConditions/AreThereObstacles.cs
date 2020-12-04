using UnityEngine;
using System;

using Pada1.BBCore.Framework;
using Pada1.BBCore;

namespace BBUnity.Conditions
{
    [Condition("MyConditions/Are There Obstacles ?")]
    [Help("Checks whether there are obstacles between this tank and the enemy through a raycast.")]
    public class AreThereObstacles : GOCondition
    {
        private GameObject enemy;

        public override bool Check()
        {
            RaycastHit ray;

            Vector3 origin = gameObject.transform.position;
            origin.y = 2;

            GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; //will return an array of all GameObjects in the scene
            foreach (GameObject go in gos)
            {
                if (go.layer == 9 && go != gameObject && go.name != "FireTransform")
                {
                    enemy = go.gameObject;
                }
            }

            Vector3 final = enemy.transform.position;
            final.y = 2;

            Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
            direction = final - origin;
            direction.Normalize();

            Physics.Raycast(origin, direction, out ray, Mathf.Abs(Vector3.Distance(origin, final)));
            Debug.DrawLine(origin, final, Color.red);

            return ray.collider == null;
        }
    }
}

