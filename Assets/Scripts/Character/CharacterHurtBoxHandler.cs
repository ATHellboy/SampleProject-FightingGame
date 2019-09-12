using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using AlirezaTarahomi.FightingGame.Service;
using AlirezaTarahomi.FightingGame.Character.Context;
using AlirezaTarahomi.FightingGame.Character.Behavior;

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
            if (collision.CompareTag("CharactersTrigger"))
            {
                _locomotionHandler.Stop();
            }

            if (collision.CompareTag("Hitbox"))
            {
                ITool tool = collision.gameObject.GetComponent<ITool>();
                if (!_ownershipService.Contains(collision.gameObject) && tool.IsDeadly)
                {
                    _mainStateMachineContext.isInjured = true;
                    return;
                }
            }

            if (_throwingObjectBehavior != null)
            {
                _throwingObjectBehavior.OnTriggerEnter2D(collision);
            }
        }
    }
}