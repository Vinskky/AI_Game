using UnityEngine;

using Pada1.BBCore.Framework;
using Pada1.BBCore;

[Condition("MyConditions/Are There Obstacles ?")]
[Help("Checks whether there are obstacles between this tank and the enemy through a raycast.")]
public class AreThereObstacles : ConditionBase
{
    public override bool Check()
    {

        GameObject enemy = GameObject.Find("Enemy");
        GameObject agent = GameObject.Find("Agent");

        RaycastHit ray;

        Vector3 origin = agent.transform.position;
        origin.y = 2;

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