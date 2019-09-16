// Copyright (c) ATHellboy (Alireza Tarahomi) Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

namespace Infrastructure.StateMachine
{
    public interface IState
    {
        void Enter(IStateMachineContext context);

        void Update(float deltaTime, StateMachine stateMachine, IStateMachineContext context);

        void FixedUpdate(float deltaTime, StateMachine stateMachine, IStateMachineContext context);

        void LateUpdate(float deltaTime, StateMachine stateMachine, IStateMachineContext context);

        void Exit(IStateMachineContext context);
    }
}