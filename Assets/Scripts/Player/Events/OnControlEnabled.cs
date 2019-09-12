using AlirezaTarahomi.FightingGame.Character;
using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Player.Event
{
    public class OnControlToggled : IEvent, IPlayerIdProperty, IScriptableObjectProperty
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
        public int PlayerId { get; }
        public CharacterStats Type { get; }
        public bool Enable { get; }

        public OnControlToggled(int playerId, CharacterStats type, bool enable)
        {
            PlayerId = playerId;
            Type = type;
            Enable = enable;
        }

        public ScriptableObject GetScriptableObject()
        {
            return Type;
        }
    }
}