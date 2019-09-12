using AlirezaTarahomi.FightingGame.Service;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        public int id;

        public override void InstallBindings()
        {
            Container.BindInstance(id).WithId("id");
            Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();

            Container.Bind<CharactersSwitchingHandler>().AsSingle().NonLazy();
        }
    }
}