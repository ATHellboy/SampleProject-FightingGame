using UnityEngine;
using AlirezaTarahomi.FightingGame.Character.Context;

namespace AlirezaTarahomi.FightingGame.Character.State.Main
{
    public interface IMainState
    {
        void ChangeMoveSpeed(CharacterMainStateMachineContext context);
    }
}