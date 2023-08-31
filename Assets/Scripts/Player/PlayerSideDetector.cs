using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class PlayerSideDetector
    {
        private PlayerController _player1;
        private PlayerController _player2;
        private bool _isAllPlayersConfigured = false;

        public PlayerSideDetector([Inject(Id = "player1")] PlayerController player1,
            [Inject(Id = "player2")] PlayerController player2)
        {
            _player1 = player1;
            _player2 = player2;

            _player1.OnCharactersConfigured.AddListener(HandleOnCharactersConfigured);
            _player2.OnCharactersConfigured.AddListener(HandleOnCharactersConfigured);
        }

        public void UpdateSide()
        {
            if (_player1.currentCharacterController.transform.position.x > _player2.currentCharacterController.transform.position.x)
            {
                _player1.Side = Side.Right;
                _player2.Side = Side.Left;
            }
            else
            {
                _player1.Side = Side.Left;
                _player2.Side = Side.Right;
            }
        }

        public void HandleOnCharactersConfigured()
        {
            if (_isAllPlayersConfigured)
            {
                UpdateSide();
                _player1.InitCharacterFace();
                _player2.InitCharacterFace();
            }

            _isAllPlayersConfigured = true;
        }
    }

    public enum Side
    {
        Left = -1,
        Right = 1
    }
}