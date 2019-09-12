using AlirezaTarahomi.FightingGame.Player.Event;
using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Event
{
    public class OnCollideObject : IEvent, IPlayerIdProperty, IScriptableObjectProperty
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
        public int PlayerId { get; }
        public CharacterStats Type { get; }

        public OnCollideObject(int playerId, CharacterStats type)
        {
            PlayerId = playerId;
            Type = type;
        }

        public ScriptableObject GetScriptableObject()
        {
            return Type;
        }
    }
}