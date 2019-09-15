using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class PlayerSideDetector
    {
        private PlayerController _player1;
        private PlayerController _player2;

        public PlayerSideDetector([Inject(Id = "player1")] PlayerController player1,
            [Inject(Id = "player2")] PlayerController player2)
        {
            _player1 = player1;
            _player2 = player2;
        }

        public void UpdateSide()
        {
            if (_player1.currentCharacter.position.x > _player2.currentCharacter.position.x)
            {
                _player1.side = Side.Right;
                _player2.side = Side.Left;
            }
            else
            {
                _player1.side = Side.Left;
                _player2.side = Side.Right;
            }
        }
    }

    public enum Side
    {
        Left = -1,
        Right = 1
    }
}