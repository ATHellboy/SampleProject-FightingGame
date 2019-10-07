using UnityEngine;
using Zenject;
using AlirezaTarahomi.FightingGame.Service;
using AlirezaTarahomi.FightingGame.Character.Context;
using AlirezaTarahomi.FightingGame.Character.Behavior;
using AlirezaTarahomi.FightingGame.Tool;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class CharacterHurtBoxHandler : MonoBehaviour
    {
        private IOwnershipService _ownershipService;
        private CharacterMainStateMachineContext _mainStateMachineContext;
        private CharacterLocomotionHandler _locomotionHandler;
        private ThrowingObjectBehavior _throwingObjectBehavior;

        [Inject]
        public void Construct(IOwnershipService ownershipService,
            CharacterMainStateMachineContext mainStateMachineContext,
            CharacterLocomotionHandler locomotionHandler)
        {
            _ownershipService = ownershipService;
            _mainStateMachineContext = mainStateMachineContext;
            _locomotionHandler = locomotionHandler;
        }

        public void Inject(ThrowingObjectBehavior throwingObjectBehavior)
        {
            _throwingObjectBehavior = throwingObjectBehavior;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.CharactersTrigger))
            {
                _locomotionHandler.Stop();
            }

            if (collision.CompareTag(Tags.Hitbox))
            {
                if (GetInjured(collision))
                {
                    return;
                }
            }

            if (_throwingObjectBehavior != null)
            {
                _throwingObjectBehavior.OnTriggerEnter2D(collision);
            }
        }

        private bool GetInjured(Collider2D collision)
        {
            ITool tool = collision.gameObject.GetComponent<ITool>();
            if (!_ownershipService.Contains(collision.gameObject) && tool.IsDeadly)
            {
                _mainStateMachineContext.isInjured = true;
                return true;
            }
            return false;
        }
    }
}