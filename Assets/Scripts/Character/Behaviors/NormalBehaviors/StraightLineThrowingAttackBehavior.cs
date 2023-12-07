﻿using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Behavior.Normal
{
    [CreateAssetMenu(menuName = "Custom/Attacks/Normal Attacks/StraightLineThrowingBehavior")]
    public class StraightLineThrowingAttackBehavior : ThrowingObjectBehavior, INormalAttackBehavior
    {
        public Status BehaviorCondition
        {
            get
            {
                if (_context.GroundCheck.OnGround || _context.jumpCounter <= 1)
                {
                    return Status.Success;
                }
                return Status.Fail;
            }
        }

        public void Behave()
        {
            UseObject(_context.Transform.right);
        }

        public void EndBehavior()
        {

        }
    }
}