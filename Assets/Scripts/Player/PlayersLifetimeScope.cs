using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class PlayersLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<PlayersContext>();
        }
    }
}