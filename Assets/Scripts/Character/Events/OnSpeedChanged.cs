using AlirezaTarahomi.FightingGame.Player.Event;
using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Event
{
    // TODO: Rename
    public class OnSpeedChanged : IEvent, IPlayerIdProperty, IScriptableObjectProperty
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
        public int PlayerId { get; }
        public CharacterStats Type { get; }

        public OnSpeedChanged(int playerId, CharacterStats type)
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