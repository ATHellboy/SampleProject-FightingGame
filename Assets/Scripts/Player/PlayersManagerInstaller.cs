using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class PlayersManagerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController _player1 = default;
        [SerializeField] private PlayerController _player2 = default;
        [SerializeField] private float _switchingCoolDown = 3.0f;

        public override void InstallBindings()
        {
            BindInstances();

            Container.Bind<PlayersManager>().FromComponentInHierarchy().AsSingle();

            Container.Bind<PlayerSideDetector>().AsSingle().NonLazy();
        }

        private void BindInstances()
        {
            Container.BindInstance(_player1).WithId("player1");
            Container.BindInstance(_player2).WithId("player2");
            Container.BindInstance(_switchingCoolDown).WithId("switchingCoolDown");
        }
    }
}