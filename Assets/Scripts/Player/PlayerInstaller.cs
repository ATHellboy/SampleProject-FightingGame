using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private int _id = 0;

        public override void InstallBindings()
        {
            Container.BindInstance(_id).WithId("playerId");

            Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();

            Container.Bind<CharactersSwitchingHandler>().AsSingle().NonLazy();
        }
    }
}