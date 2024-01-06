using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Behavior.Normal
{
    [CreateAssetMenu(menuName = "Custom/Attacks/Normal Attacks/StraightLineThrowingBehavior")]
    public class StraightLineThrowingAttackBehavior : ThrowingObjectBehavior, INormalAttackBehavior
    {
        public Status BehaviorCondition
        {
            get
            {
                if (_context.SurfaceCheck.onGround || _context.jumpCounter <= 1)
                {
                    return Status.Success;
                }
                return Status.Fail;
            }
        }

        public void Behave()
        {
            UseObject(_context.Pivot.transform.right);
        }

        public void EndBehavior()
        {

        }
    }
}