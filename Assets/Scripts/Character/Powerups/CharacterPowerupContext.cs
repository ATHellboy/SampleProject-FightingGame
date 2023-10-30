using AlirezaTarahomi.FightingGame.Character.Event;
using UniRx;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Powerup
{
    public class CharacterPowerupContext
    {
        public OnPowerupStarted OnPowerupStarted { get; private set; } = new();
        public OnPowerupEnded OnPowerupEnded { get; private set; } = new();
        public CompositeDisposable Disposables { get; private set; } = new();
    }
}