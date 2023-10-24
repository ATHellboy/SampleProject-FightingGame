using UnityEngine;
using Cinemachine;
using Assets.Infrastructure.Scripts.CQRS;
using Assets.Infrastructure.MessageBusImplementations.UniRx;
using AlirezaTarahomi.FightingGame.CameraSystem;
using AlirezaTarahomi.FightingGame.InputSystem;
using Infrastructure.Factory;
using Infrastructure.ObjectPooling;
using Infrastructure.StateMachine;
using VContainer;
using VContainer.Unity;
using AlirezaTarahomi.FightingGame.UI;

namespace AlirezaTarahomi.FightingGame.General
{
    public class ServicesLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            //builder.RegisterComponentInHierarchy<Canvas>();
            builder.RegisterComponentInHierarchy<CameraManager>();
            builder.RegisterComponentInHierarchy<MainCameraController>();
            builder.RegisterComponentInHierarchy<CinemachineTargetGroup>();
            builder.RegisterComponentInHierarchy<PoolingSystem>();
            builder.Register<MessageBusRules, UIMessageBusRules>(Lifetime.Singleton);
            builder.Register<TargetGroupController>(Lifetime.Singleton);
            builder.Register<InputManager, UnityInputManager>(Lifetime.Singleton);
            builder.Register<IMessageBus, UniRxMessageBus>(Lifetime.Singleton);
            builder.Register<StateMachine>(Lifetime.Singleton);
            builder.Register<IResourceFactory, VContainerResourceFactory>(Lifetime.Singleton);

            //For Injection
            builder.RegisterComponentInHierarchy<RulesManager>();
        }
    }
}