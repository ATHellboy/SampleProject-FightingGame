using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Behavior
{
    public interface IThrowingBehavior
    {
        ThrowingObjectBehavior ThrowingObjectBehavior { get; }

        void AssignThrowingObjectBehavior(ThrowingObjectBehavior throwingObjectBehavior);
    }
}