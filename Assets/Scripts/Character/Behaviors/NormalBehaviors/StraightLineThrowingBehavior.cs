using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character.Behavior.Normal
{
    [CreateAssetMenu(menuName = "Attacks/Normal Attacks/StraightLineThrowingBehavior")]
    public class StraightLineThrowingBehavior : ScriptableObject, INormalAttackBehavior, IThrowingBehavior
    {
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
            if (_context.isGrounded || _context.jumpCounter <= 2)
            {
                ThrowingObjectBehavior.UseObject(_context.Transform.right);
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