using AlirezaTarahomi.FightingGame.Player.Event;
using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Event
{
    public class OnPowerupToggled : IEvent, IPlayerIdProperty, IScriptableObjectProperty
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
        public int PlayerId { get; }
        public CharacterStats Type { get; }
        public bool Active { get; }

        public OnPowerupToggled(int id, CharacterStats type, bool active)
        {
            PlayerId = id;
            Type = type;
            Active = active;
        }

        public ScriptableObject GetScriptableObject()
        {
            return Type;
        }
    }
}