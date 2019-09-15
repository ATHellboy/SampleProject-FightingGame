using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Behavior
{
    public interface IBehavior
    {
        Status BehaviorCondition { get; }

        void Behave();

        void EndBehavior();

        void Inject(CharacterBehaviorContext context);
    }

    public enum Status
    {
        Success,
        Fail
    }
}