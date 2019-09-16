// Copyright (c) ATHellboy (Alireza Tarahomi) Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using UnityEngine;

namespace Infrastructure.StateMachine
{
    public interface IStateMachineContext
    {
        GameObject GO { get; }
        IState StartState { get; }
        IState CurrentState { get; set; }
        bool IsActive { get; }
        // Uses before getting input from player
        bool CanControl { get; }
        bool DebugStateMachine { get; }
    }
}