using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Behavior.Complex
{
    [CreateAssetMenu(menuName = "Attacks/Complex Attacks/ThreeDirectionThrowingBehavior")]
    public class ThreeDirectionThrowingBehavior : ScriptableObject, IComplexAttackBehavior, IThrowingBehavior
    {
        [SerializeField] private float _offsetDegree = 20;

        public ThrowingObjectBehavior ThrowingObjectBehavior { get; private set; }

        private CharacterBehaviorContext _context;

        public void Inject(CharacterBehaviorContext context)
        {
            _context = context;
        }

        public void AssignThrowingObjectBehavior(ThrowingObjectBehavior throwingObjectBehavior)
        {
            ThrowingObjectBehavior = throwingObjectBehavior;
        }

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
            ThrowingObjectBehavior.UseObject(-_context.Transform.up + _offsetDegree * angleUnit);
            ThrowingObjectBehavior.UseObject(((-_context.Transform.up +
                (_context.Transform.right - _context.Transform.up) / 2) / 2) + _offsetDegree * angleUnit);
            ThrowingObjectBehavior.UseObject(((_context.Transform.right - _context.Transform.up) / 2) +
                _offsetDegree * angleUnit);
        }

        public void EndBehavior()
        {
            
        }
    }
}