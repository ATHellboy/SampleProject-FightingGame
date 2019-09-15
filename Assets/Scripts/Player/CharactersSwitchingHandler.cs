using AlirezaTarahomi.FightingGame.Character.Event;
using Assets.Infrastructure.Scripts.CQRS;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class CharactersSwitchingHandler
    {
        private IMessageBus _messageBus;
        private int _id;
        private Queue<Character.CharacterController> _characters = new Queue<Character.CharacterController>();
        private Character.CharacterController _currentCharacterController;

        public CharactersSwitchingHandler(IMessageBus messageBus, [Inject(Id = "playerId")] int id)
        {
            _messageBus = messageBus;
            _id = id;
        }

        public Character.CharacterController DequeueCharacter()
        {
            return _characters.Dequeue();
        }

        public void EnqueueCharacter(Character.CharacterController controller)
        {
            _characters.Enqueue(controller);
        }

        public Transform ConfigCharacters()
        {
            _currentCharacterController = DequeueCharacter();
            _currentCharacterController.isJustEntered = true;
            _messageBus.RaiseEvent(new OnOtherDisabled(_currentCharacterController.EntityId, _id));
            return _currentCharacterController.transform;
        }

        public Transform EnterNextCharacter()
        {
            _currentCharacterController = DequeueCharacter();
            _messageBus.RaiseEvent(new OnCharacterArrivalToggled(_currentCharacterController.EntityId, true));
            return _currentCharacterController.transform;
        }

        public void ExitCurrentCharacter()
        {
            EnqueueCharacter(_currentCharacterController);
            _messageBus.RaiseEvent(new OnCharacterArrivalToggled(_currentCharacterController.EntityId, false));
        }
    }
}