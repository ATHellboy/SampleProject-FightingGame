// Copyright (c) ATHellboy (Alireza Tarahomi) Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;

namespace Infrastructure.StateMachine
{
    public abstract class BaseState<TContext> : IState where TContext : IStateMachineContext
    {
        private readonly Type _contextType;

        protected BaseState()
        {
            _contextType = typeof(TContext);
        }

        public void Enter(IStateMachineContext context)
        {
            if (!_contextType.IsAssignableFrom(context.GetType()))
            {
                // Log Or throw exception
                return;
            }

            Enter((TContext)context);
        }

        public abstract void Enter(TContext context);

        public void Update(float deltaTime, StateMachine stateMachine, IStateMachineContext context)
        {
            if (!_contextType.IsAssignableFrom(context.GetType()))
            {
                // Log Or throw exception
                return;
            }

            Update(deltaTime, stateMachine, (TContext)context);
        }

        public abstract void Update(float deltaTime, StateMachine stateMachine, TContext context);

        public void FixedUpdate(float deltaTime, StateMachine stateMachine, IStateMachineContext context)
        {
            if (!_contextType.IsAssignableFrom(context.GetType()))
            {
                // Log Or throw exception
                return;
            }

            FixedUpdate(deltaTime, stateMachine, (TContext)context);
        }

        public abstract void FixedUpdate(float deltaTime, StateMachine stateMachine, TContext context);

        public void LateUpdate(float deltaTime, StateMachine stateMachine, IStateMachineContext context)
        {
            if (!_contextType.IsAssignableFrom(context.GetType()))
            {
                // Log Or throw exception
                return;
            }

            LateUpdate(deltaTime, stateMachine, (TContext)context);
        }

        public abstract void LateUpdate(float deltaTime, StateMachine stateMachine, TContext context);

        public void Exit(IStateMachineContext context)
        {
            if (!_contextType.IsAssignableFrom(context.GetType()))
            {
                // Log Or throw exception
                return;
            }

            Exit((TContext)context);
        }

        public abstract void Exit(TContext context);
    }
}