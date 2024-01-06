using AlirezaTarahomi.FightingGame.CameraSystem;
using AlirezaTarahomi.FightingGame.Character;
using System.Collections.Generic;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class CharactersSwitchingHandler
    {
        public Side side;

        private readonly PlayerContext _playerContext;
        private readonly TargetGroupController _targetGroupController;

        private Queue<Character.CharacterController> _characters = new();
        private Character.CharacterController _preCharacterController;
        private Character.CharacterController _currentCharacterController;

        public CharactersSwitchingHandler(PlayerContext playerContext, TargetGroupController targetGroupController)
        {
            _playerContext = playerContext;
            _targetGroupController = targetGroupController;
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
            _currentCharacterController.Activate();
            return _currentCharacterController;
        }

        public Character.CharacterController EnterNextCharacter()
        {
            _currentCharacterController = DequeueCharacter();
            _currentCharacterController.EnterStage(side, _preCharacterController.transform.position);
            CameraValues cameraValues = _currentCharacterController.Context.stats.cameraValues;
            _targetGroupController.AssignTarget(
                _playerContext.index - 1, _currentCharacterController.transform, cameraValues.cameraRadius, cameraValues.cameraWeight);
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