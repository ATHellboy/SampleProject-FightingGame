using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Player
{
    // TODO: Change it to non-monobehaviour class
    public class PlayerSideDetector : MonoBehaviour
    {
        [SerializeField] private PlayerController _player1 = default;
        [SerializeField] private PlayerController _player2 = default;

        void Update()
        {
            UpdateSide();
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
}