using UnityEngine;

using Pada1.BBCore.Framework;
using Pada1.BBCore;

[Condition("MyConditions/Is Player 1?")]
[Help("Checks whether this tank is player 1.")]
public class IsPlayer1 : ConditionBase
{
    ///<value>Input Player Number.</value>
    [InParam("Player")]
    [Help("Number of the player")]
    public int player;

    public override bool Check()
    {
        return player == 1;
    }
}