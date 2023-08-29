using AlirezaTarahomi.FightingGame.Character;
using AlirezaTarahomi.FightingGame.Player;
using AlirezaTarahomi.FightingGame.Tool;
using AlirezaTarahomi.FightingGame.UI;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.General
{
    public class RulesManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MessageBusRules>().To<PlayerMessageBusRules>().AsSingle().NonLazy();
            Container.Bind<MessageBusRules>().To<CharacterMessageBusRules>().AsSingle().NonLazy();
            Container.Bind<MessageBusRules>().To<ToolMessageBusRules>().AsSingle().NonLazy();
            Container.Bind<MessageBusRules>().To<UIMessageBusRules>().AsSingle().NonLazy();
        }
    }
}