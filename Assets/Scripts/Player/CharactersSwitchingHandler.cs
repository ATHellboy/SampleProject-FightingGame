using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.Player.Event;
using Assets.Infrastructure.Scripts.CQRS;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Player
{
    // TODO: Switch state on character
    public class CharactersSwitchingHandler
    {
        private IMessageBus _messageBus;
        private int _id;
        private Queue<Character.CharacterController> _characters = new Queue<Character.CharacterController>();
        private Character.CharacterController _currentCharacterController;

        public CharactersSwitchingHandler(IMessageBus messageBus, [Inject(Id = "id")] int id)
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
            _messageBus.RaiseEvent(new OnControlToggled(_id, _currentCharacterController.Stats, true));
            _messageBus.RaiseEvent(new OnOtherDisabled(_id, _currentCharacterController.Stats));
            return _currentCharacterController.transform;
        }

        public Transform EnterNextCharacter()
        {
            _currentCharacterController = DequeueCharacter();
            _messageBus.RaiseEvent(new OnCharacterArrivalToggled(_id, _currentCharacterController.Stats, true));
            return _currentCharacterController.transform;
        }

        public void ExitCurrentCharacter()
        {
            EnqueueCharacter(_currentCharacterController);
            _messageBus.RaiseEvent(new OnCharacterArrivalToggled(_id, _currentCharacterController.Stats, false));
        }
    }
}