using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character.Behavior.Complex
{
    [CreateAssetMenu(menuName = "Attacks/Complex Attacks/ThreeDirectionThrowingBehavior")]
    public class ThreeDirectionThrowingBehavior : ScriptableObject, IComplexAttackBehavior, IThrowingBehavior
    {
        public float offsetDegree = 20;

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

        public Status Behave()
        {
            if (_context.jumpCounter == 3)
            {
                Vector3 downToFrontCurve = (_context.Transform.right - _context.Transform.up) / 90;
                ThrowingObjectBehavior.UseObject(-_context.Transform.up + offsetDegree * downToFrontCurve);
                ThrowingObjectBehavior.UseObject(((-_context.Transform.up +
                    (_context.Transform.right - _context.Transform.up) / 2) / 2) + offsetDegree * downToFrontCurve);
                ThrowingObjectBehavior.UseObject(((_context.Transform.right - _context.Transform.up) / 2) +
                    offsetDegree * downToFrontCurve);
                return Status.Success;
            }
            return Status.Fail;
        }

        public Status EndBehavior()
        {
            return Status.Success;
        }
    }
}