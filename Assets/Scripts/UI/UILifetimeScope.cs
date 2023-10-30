using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace AlirezaTarahomi.FightingGame.UI
{
    public class UILifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            //For Injection
            builder.RegisterComponentInHierarchy<UIGameOverPanel>();
        }
    }
}