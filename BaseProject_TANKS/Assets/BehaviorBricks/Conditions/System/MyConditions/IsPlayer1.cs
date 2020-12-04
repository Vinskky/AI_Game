using UnityEngine;

using Pada1.BBCore.Framework;
using Pada1.BBCore;

namespace BBUnity.Conditions
{
    [Condition("MyConditions/Is Player 1?")]
    [Help("Checks whether this tank is player 1.")]
    public class IsPlayer1 : GOCondition
    {
        public override bool Check()
        {
            return gameObject.CompareTag("Player 1");
        }
    }
}
