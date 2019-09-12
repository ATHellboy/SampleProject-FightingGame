using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class CharacterAnimatorController
    {
        private Animator _animator;

        public CharacterAnimatorController(Animator animator)
        {
            _animator = animator;
        }

        public void ToggleStopping(bool active)
        {
            _animator.SetBool("isStopped", active);
        }

        public void ToggleWalking(bool active)
        {
            _animator.SetBool("isWalked", active);
        }

        public void ToggleJumping(bool active)
        {
            _animator.SetBool("isJumped", active);
        }

        public void ToggleAttacking(bool active)
        {
            _animator.SetBool("isAttacked", active);
        }

        public void Die()
        {
            _animator.SetBool("isDied", true);
        }
    }
}