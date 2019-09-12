using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Behavior
{
    public interface IBehavior
    {
        Status Behave();

        Status EndBehavior();

        void Inject(CharacterBehaviorContext context);
    }

    public enum Status
    {
        Success,
        Fail
    }
}