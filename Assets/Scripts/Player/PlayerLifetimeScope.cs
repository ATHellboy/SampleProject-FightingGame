using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class PlayerLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<PlayerContext>().UnderTransform(transform);
            builder.Register<CharactersSwitchingHandler>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<Camera>().UnderTransform(transform);

            //For Injection
            builder.RegisterComponentInHierarchy<PlayerController>().UnderTransform(transform);
        }
    }
}