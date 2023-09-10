using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Behavior.Complex
{
    [CreateAssetMenu(menuName = "Attacks/Complex Attacks/ThreeDirectionThrowingBehavior")]
    public class ThreeDirectionThrowingBehavior : ThrowingObjectBehavior, IComplexAttackBehavior
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
            Vector3 angleUnit = (_context.Transform.right - _context.Transform.up) / 90;
            UseObject(-_context.Transform.up + _offsetDegree * angleUnit);
            UseObject(((-_context.Transform.up +
                (_context.Transform.right - _context.Transform.up) / 2) / 2) + _offsetDegree * angleUnit);
            UseObject(((_context.Transform.right - _context.Transform.up) / 2) +
                _offsetDegree * angleUnit);
        }

        public void EndBehavior()
        {
            
        }
    }
}