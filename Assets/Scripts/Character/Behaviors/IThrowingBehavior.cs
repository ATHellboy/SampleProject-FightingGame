using AlirezaTarahomi.FightingGame.Character.Behavior;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Behavior
{
    public interface IThrowingBehavior
    {
        ThrowingObjectBehavior ThrowingObjectBehavior { get; }

        void AssignThrowingObjectBehavior(ThrowingObjectBehavior throwingObjectBehavior);
    }
}