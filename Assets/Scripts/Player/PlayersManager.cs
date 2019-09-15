using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class PlayersManager : MonoBehaviour
    {
        private PlayerSideDetector _playerSideDetector;

        [Inject]
        public void Constrcut(PlayerSideDetector playerSideDetector)
        {
            _playerSideDetector = playerSideDetector;
        }

        void Update()
        {
            _playerSideDetector.UpdateSide();
        }
    }
}