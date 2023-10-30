using AlirezaTarahomi.FightingGame.Character.Context;
using Infrastructure.Extension;
using Infrastructure.StateMachine;
using System.Collections;
using UniRx;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.State.Main
{
    public class Die : BaseState<CharacterMainStateMachineContext>
    {
        private SpriteRenderer _spriteRenderer;

        public Die(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
        }

        public override void Enter(CharacterMainStateMachineContext context)
        {
            context.OnDied?.Invoke();
            context.LocomotionHandler.Stop();
            context.AnimatorController.Die();
            Observable.FromCoroutine(_ => FadeOut(context)).Subscribe().AddTo(context.CharacterContext.Disposables);
        }

        public override void Update(float deltaTime, StateMachine stateMachine, CharacterMainStateMachineContext context)
        {

        }

        public override void FixedUpdate(float deltaTime, StateMachine stateMachine, CharacterMainStateMachineContext context)
        {

        }

        public override void LateUpdate(float deltaTime, StateMachine stateMachine, CharacterMainStateMachineContext context)
        {

        }

        public override void Exit(CharacterMainStateMachineContext context)
        {

        }

        IEnumerator FadeOut(CharacterMainStateMachineContext context)
        {
            yield return new WaitForSeconds(context.CharacterContext.dieFadeOutDelay);

            float currentTime = 0;
            float perc = 0;
            float currentAlpha = _spriteRenderer.color.a;

            if (context.CharacterContext.dieFadeOutDuration == 0)
            {
                _spriteRenderer.SetAlpha(0);
                yield break;
            }

            while (perc != 1)
            {
                currentTime += Time.deltaTime;
                if (currentTime > context.CharacterContext.dieFadeOutDuration)
                {
                    currentTime = context.CharacterContext.dieFadeOutDuration;
                }
                perc = currentTime / context.CharacterContext.dieFadeOutDuration;
                _spriteRenderer.SetAlpha(Mathf.Lerp(currentAlpha, 0, perc));
                yield return null;
            }
        }
    }
}