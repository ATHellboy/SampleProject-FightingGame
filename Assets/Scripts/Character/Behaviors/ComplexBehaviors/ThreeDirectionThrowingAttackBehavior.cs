using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Behavior.Complex
{
    [CreateAssetMenu(menuName = "Custom/Attacks/Complex Attacks/ThreeDirectionThrowingBehavior")]
    public class ThreeDirectionThrowingAttackBehavior : ThrowingObjectBehavior, IComplexAttackBehavior
    {
        [SerializeField] private float _offsetDegree = 20;

        public Status BehaviorCondition
        {
            get
            {
                if (_context.jumpCounter == 2)
                {
                    return Status.Success;
                }
                return Status.Fail;
            }
        }

        public void Behave()
        {
            Vector3 angleUnit = (_context.Pivot.transform.right - _context.Pivot.transform.up) / 90;
            UseObject(-_context.Pivot.transform.up + _offsetDegree * angleUnit);
            UseObject(((-_context.Pivot.transform.up + (_context.Pivot.transform.right - _context.Pivot.transform.up) / 2) / 2) +
                _offsetDegree * angleUnit);
            UseObject(((_context.Pivot.transform.right - _context.Pivot.transform.up) / 2) + _offsetDegree * angleUnit);
        }

        public void EndBehavior()
        {
            
        }
    }
}