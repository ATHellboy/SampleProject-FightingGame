using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class PlayersManager : MonoBehaviour
    {
        [SerializeField] private PlayerController _player1 = default;
        [SerializeField] private PlayerController _player2 = default;

        private bool _isAllPlayersConfigured = false;

        void OnEnable()
        {
            _player1.OnCharactersConfigured.AddListener(HandleOnCharactersConfigured);
            _player2.OnCharactersConfigured.AddListener(HandleOnCharactersConfigured);
        }

        void OnDisable()
        {
            _player1.OnCharactersConfigured.RemoveListener(HandleOnCharactersConfigured);
            _player2.OnCharactersConfigured.RemoveListener(HandleOnCharactersConfigured);
        }

        void Update()
        {
            UpdateSide();
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