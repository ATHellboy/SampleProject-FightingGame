using UnityEngine;
using Cinemachine;
using AlirezaTarahomi.FightingGame.CameraSystem;
using AlirezaTarahomi.FightingGame.InputSystem;
using Infrastructure.Factory;
using Infrastructure.ObjectPooling;
using Infrastructure.StateMachine;
using VContainer;
using VContainer.Unity;
using MessagePipe;
using AlirezaTarahomi.FightingGame.Player.Event;

namespace AlirezaTarahomi.FightingGame.General
{
    public class ServicesLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
            builder.RegisterMessageBroker<OnGameOver>(options);
            builder.RegisterMessageBroker<int, OnAttackCooldownStarted>(options);
            builder.RegisterMessageBroker<int, OnPowerupTimerStarted>(options);

            builder.RegisterComponentInHierarchy<CameraManager>();
            builder.RegisterComponentInHierarchy<MainCameraController>();
            builder.RegisterComponentInHierarchy<CinemachineTargetGroup>();
            builder.RegisterComponentInHierarchy<PoolingSystem>();
            builder.Register<TargetGroupController>(Lifetime.Singleton);
            builder.Register<InputManager, UnityInputManager>(Lifetime.Singleton);
            builder.Register<StateMachine>(Lifetime.Singleton);
            builder.Register<IResourceFactory, VContainerResourceFactory>(Lifetime.Singleton);
        }
    }
}