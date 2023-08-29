using AlirezaTarahomi.FightingGame.CameraSystem;
using AlirezaTarahomi.FightingGame.Character;
using Assets.Infrastructure.Scripts.CQRS;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class CharactersSwitchingHandler
    {
        public Side side;

        private IMessageBus _messageBus;
        private TargetGroupController _targetGroupController;
        private int _id;
        private Queue<Character.CharacterController> _characters = new();
        private Character.CharacterController _preCharacterController;
        private Character.CharacterController _currentCharacterController;

        public CharactersSwitchingHandler(IMessageBus messageBus, TargetGroupController targetGroupController, [Inject(Id = "playerId")] int id)
        {
            _messageBus = messageBus;
            _targetGroupController = targetGroupController;
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

        public Character.CharacterController ConfigCharacters()
        {
            _currentCharacterController = DequeueCharacter();
            _preCharacterController = _currentCharacterController;
            _currentCharacterController.isJustEntered = true;
            _currentCharacterController.Activate();
            return _currentCharacterController;
        }

        public Character.CharacterController EnterNextCharacter()
        {
            _currentCharacterController = DequeueCharacter();
            _currentCharacterController.EnterStage(side, _preCharacterController.transform.position);
            CameraValues cameraValues = _currentCharacterController.Stats.cameraValues;
            _targetGroupController.AssignTarget(_id - 1, _currentCharacterController.transform, cameraValues.cameraRadius, cameraValues.cameraWeight);
            _preCharacterController = _currentCharacterController;
            return _currentCharacterController;
        }

        public void ExitCurrentCharacter()
        {
            EnqueueCharacter(_currentCharacterController);
            _currentCharacterController.ExitStage(side);
        }
    }
}