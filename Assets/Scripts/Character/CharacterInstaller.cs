using AlirezaTarahomi.FightingGame.Character.Behavior;
using AlirezaTarahomi.FightingGame.Character.Context;
using AlirezaTarahomi.FightingGame.Character.Powerup;
using AlirezaTarahomi.FightingGame.Character.State.Combat;
using AlirezaTarahomi.FightingGame.Character.State.Main;
using AlirezaTarahomi.FightingGame.Character.State.SecondaryMovement;
using AlirezaTarahomi.FightingGame.Service;
using Infrastructure.StateMachine;
using System;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class CharacterInstaller : MonoInstaller
    {
        [SerializeField] private bool _debugStateMachine = false;
        [SerializeField] private CharacterStats _stats = default;
        [SerializeField] private float _throwingAngle = 30;

        private string _id;

        public override void InstallBindings()
        {
            SetId();
            BindInstances();
            BindComponents();
            BindContexts();
            InitStartStates();
            BindStates();
        }

        private void SetId()
        {
            _id = Guid.NewGuid().ToString();
            Container.BindInstance(_id).WithId("id").AsSingle();
        }

        private void BindInstances()
        {
            Container.BindInstance(_debugStateMachine).WithId("debug").AsSingle();
            Container.BindInstance(_stats).WithId("stats");
            Container.BindInstance(_throwingAngle).WithId("throwingAngle");
            Container.Bind<GameObject>().FromInstance(gameObject).When(context => string.Equals("go", context.MemberName));
        }

        private void BindComponents()
        {
            Container.Bind<Animator>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Rigidbody2D>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Transform>().FromComponentInHierarchy().AsCached();
            Container.Bind<SpriteRenderer>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GroundCheck>().FromComponentInHierarchy().AsSingle();
            Container.Bind<MovementColliderActivator>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CharacterHurtBoxHandler>().FromComponentInHierarchy().AsSingle();

            Container.Bind<IOwnershipService>().To<HitboxObjectsOwnershipService>().AsSingle().NonLazy();
            Container.Bind<CharacterAnimatorController>().AsSingle().NonLazy();
            Container.Bind<CharacterLocomotionHandler>().AsSingle().NonLazy();
        }

        private void BindContexts()
        {
            Container.Bind<CharacterMainStateMachineContext>().AsSingle().NonLazy();
            Container.Bind<CharacterSecondaryMovementStateMachineContext>().AsSingle().NonLazy();
            Container.Bind<CharacterCombatStateMachineContext>().AsSingle().NonLazy();
            Container.Bind<CharacterBehaviorContext>().AsSingle().NonLazy();
            Container.Bind<CharacterPowerupContext>().AsSingle().NonLazy();
        }

        private void InitStartStates()
        {
            Container.Bind<IState>().To<Idle>().When(context => string.Equals("startState", context.MemberName)
                && context.ObjectType == typeof(CharacterMainStateMachineContext));
            Container.Bind<IState>().To<State.SecondaryMovement.None>().When(context => string.Equals("startState", context.MemberName)
                && context.ObjectType == typeof(CharacterSecondaryMovementStateMachineContext));
            Container.Bind<IState>().To<State.Combat.None>().When(context => string.Equals("startState", context.MemberName)
                && context.ObjectType == typeof(CharacterCombatStateMachineContext));
        }

        private void BindStates()
        {
            Container.Bind<Idle>().AsCached().NonLazy();
            Container.Bind<Walk>().AsSingle().NonLazy();
            Container.Bind<Die>().AsSingle().NonLazy();

            Container.Bind<Jump>().AsSingle().NonLazy();
            Container.Bind<Fall>().AsSingle().NonLazy();
            Container.Bind<Land>().AsSingle().NonLazy();
            Container.Bind<Fly>().AsSingle().NonLazy();
            Container.Bind<State.SecondaryMovement.None>().AsCached().NonLazy();

            Container.Bind<State.Combat.None>().AsCached().NonLazy();
            Container.Bind<Attack>().AsSingle().NonLazy();
        }
    }
}