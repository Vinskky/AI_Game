using UnityEngine;

using Pada1.BBCore.Framework;
using Pada1.BBCore;

[Condition("MyConditions/Is Enemy Near?")]
[Help("Checks whether another tank is near this one.")]
public class IsEnemyNear : ConditionBase
{
    ///<value>Input First Boolean Parameter.</value>
    [InParam("Gravity")]
    [Help("Gravity of the world")]
    public float gravity;

    ///<value>Input Second Boolean Parameter.</value>
    [InParam("Launch Force")]
    [Help("Second value to be compared")]
    public float launchForce;

    public override bool Check()
    {
        GameObject enemy = GameObject.Find("Enemy");
        GameObject agent = GameObject.Find("Agent");
        return Mathf.Abs(Vector3.Distance(enemy.transform.position, agent.transform.position)) < ((launchForce * launchForce) / gravity);
    }
}