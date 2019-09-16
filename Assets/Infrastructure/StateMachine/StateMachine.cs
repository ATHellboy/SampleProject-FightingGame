// Copyright (c) ATHellboy (Alireza Tarahomi) Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class StateMachine
    {
        public void Start(IStateMachineContext context)
        {
            ChangeState(null, context.StartState, context);
        }

        /// <summary>
        /// Calls in Update loop
        /// </summary>
        /// <param name="deltaTime"><see cref="Time.deltaTime"/></param>
        public void Update(float deltaTime, IStateMachineContext context)
        {
            if (!context.IsActive)
                return;

            if (context.CurrentState != null) context.CurrentState.Update(deltaTime, this, context);
        }

        /// <summary>
        /// Calls in FixedUpdate loop
        /// </summary>
        /// <param name="deltaTime"><see cref="Time.deltaTime"/></param>
        public void FixedUpdate(float deltaTime, IStateMachineContext context)
        {
            if (!context.IsActive)
                return;

            if (context.CurrentState != null) context.CurrentState.FixedUpdate(deltaTime, this, context);
        }

        /// <summary>
        /// Calls in LateUpdate loop
        /// </summary>
        /// <param name="deltaTime"><see cref="Time.deltaTime"/></param>
        public void LateUpdate(float deltaTime, IStateMachineContext context)
        {
            if (!context.IsActive)
                return;

            if (context.CurrentState != null) context.CurrentState.LateUpdate(deltaTime, this, context);
        }

        private void ChangeState(IState newState, IStateMachineContext context)
        {
            if (context.DebugStateMachine)
            {
                Debug.Log(context.GO.name + " " + context.GetType().Name + " " + newState.GetType().Name);
            }

            if (context.CurrentState != null) context.CurrentState.Exit(context);
            context.CurrentState = newState;
            if (context.CurrentState != null) context.CurrentState.Enter(context);
        }

        public void ChangeState(object sender, IState nextState, IStateMachineContext context)
        {
            if (!context.IsActive && sender != null)
                return;

            if (sender == context.CurrentState)
            {
                ChangeState(nextState, context);
            }
        }

        public void Reset(IStateMachineContext context)
        {
            ChangeState(context.StartState, context);
        }
    }
}