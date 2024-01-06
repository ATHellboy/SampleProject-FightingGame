using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class CharacterAnimatorController
    {
        private const string StopParameter = "isStopped";
        private const string MoveParameter = "isMoved";
        private const string JumpParameter = "isJumped";
        private const string FallParameter = "isFallen";
        private const string LandParameter = "isLanded";
        private const string AttackParameter = "isAttacked";
        private const string DieParameter = "isDied";

        private readonly Animator _animator;

        public CharacterAnimatorController(Animator animator)
        {
            _animator = animator;
        }

        public void ToggleStopping(bool active)
        {
            _animator.SetBool(StopParameter, active);
        }

        public void ToggleMoving(bool active)
        {
            _animator.SetBool(MoveParameter, active);
        }

        public void ToggleJumping(bool active)
        {
            _animator.SetBool(JumpParameter, active);
        }

        public void ToggleFalling(bool active)
        {
            _animator.SetBool(FallParameter, active);
        }

        public void ToggleLanding(bool active)
        {
            _animator.SetBool(LandParameter, active);
        }

        public void ToggleAttacking(bool active)
        {
            _animator.SetBool(AttackParameter, active);
        }

        public void Die()
        {
            _animator.SetBool(DieParameter, true);
        }

        public void Reset()
        {
            _animator.Rebind();
        }
    }
}