using UnityEngine;

using Pada1.BBCore.Framework;
using Pada1.BBCore;

[Condition("MyConditions/Is Enemy Near?")]
[Help("Checks whether another tank is near this one.")]
public class IsEnemyNear : ConditionBase
{
    ///<value>Input Gravity.</value>
    [InParam("Gravity")]
    [Help("Gravity of the world")]
    public float gravity;

    ///<value>Input Launch Force.</value>
    [InParam("Launch Force")]
    [Help("Second value to be compared")]
    public float launchForce;

    ///<value>Input Player Number.</value>
    [InParam("Player")]
    [Help("Number of the player")]
    public int player;

    ///<value>Output Enemy Tank.</value>
    [OutParam("Enemy")]
    [Help("Enemy tank")]
    public GameObject enemy;

    ///<value>Output Agent Tank.</value>
    [OutParam("Agent")]
    [Help("Agent tank")]
    public GameObject agent;

    public override bool Check()
    {
        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; //will return an array of all GameObjects in the scene
        foreach (GameObject go in gos)
        {
            if (go.layer == 9 && player.ToString() != go.tag && go.name != "FireTransform")
            {
                enemy = go.gameObject;
            }
            else if (go.layer == 9 && player.ToString() == go.tag && go.name != "FireTransform")
            {
                agent = go.gameObject;
            }
        }

        return Mathf.Abs(Vector3.Distance(enemy.transform.position, agent.transform.position)) < ((launchForce * launchForce) / gravity);
    }
}