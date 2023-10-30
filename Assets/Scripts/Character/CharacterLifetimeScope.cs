using AlirezaTarahomi.FightingGame.Character.Behavior;
using AlirezaTarahomi.FightingGame.Character.Context;
using AlirezaTarahomi.FightingGame.Character.Powerup;
using AlirezaTarahomi.FightingGame.Character.State.Combat;
using AlirezaTarahomi.FightingGame.Character.State.Main;
using AlirezaTarahomi.FightingGame.Character.State.SecondaryMovement;
using AlirezaTarahomi.FightingGame.Service;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class CharacterLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            BindComponents(builder);
            BindContexts(builder);
            BindStates(builder);
        }

        private void BindComponents(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<Animator>().UnderTransform(transform);
            builder.RegisterComponentInHierarchy<Rigidbody2D>().UnderTransform(transform);
            builder.RegisterComponentInHierarchy<Transform>().UnderTransform(transform);
            builder.RegisterComponentInHierarchy<SpriteRenderer>().UnderTransform(transform);
            builder.RegisterComponentInHierarchy<GroundCheck>().UnderTransform(transform);
            builder.RegisterComponentInHierarchy<MovementColliderActivator>().UnderTransform(transform);
            builder.Register<IOwnershipService, HitboxObjectsOwnershipService>(Lifetime.Singleton);
            builder.Register<CharacterAnimatorController>(Lifetime.Singleton);
            builder.Register<CharacterLocomotionHandler>(Lifetime.Singleton);

            //For Injection
            builder.RegisterComponentInHierarchy<CharacterController>().UnderTransform(transform);
            builder.RegisterComponentInHierarchy<CharacterHurtBoxHandler>().UnderTransform(transform);
        }

        private void BindContexts(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<CharacterContext>().UnderTransform(transform);
            builder.Register<CharacterMainStateMachineContext>(Lifetime.Singleton);
            builder.Register<CharacterSecondaryMovementStateMachineContext>(Lifetime.Singleton);
            builder.Register<CharacterCombatStateMachineContext>(Lifetime.Singleton);
            builder.Register<CharacterBehaviorContext>(Lifetime.Singleton);
            builder.Register<CharacterPowerupContext>(Lifetime.Singleton);
        }

        private void BindStates(IContainerBuilder builder)
        {
            //Main
            builder.Register<Idle>(Lifetime.Singleton);
            builder.Register<Walk>(Lifetime.Singleton);
            builder.Register<Die>(Lifetime.Singleton);

            //Secondary
            builder.Register<Jump>(Lifetime.Singleton);
            builder.Register<Fall>(Lifetime.Singleton);
            builder.Register<Land>(Lifetime.Singleton);
            builder.Register<Fly>(Lifetime.Singleton);
            builder.Register<State.SecondaryMovement.None>(Lifetime.Singleton);

            //Combat
            builder.Register<State.Combat.None>(Lifetime.Singleton);
            builder.Register<Attack>(Lifetime.Singleton);
        }
    }
}