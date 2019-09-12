using AlirezaTarahomi.FightingGame.Player.Event;
using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Event
{
    public class OnAttackToggled : IEvent, IPlayerIdProperty, IScriptableObjectProperty
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
        public int PlayerId { get; }
        public CharacterStats Type { get; }
        public bool Enter { get; }

        public OnAttackToggled(int playerId, CharacterStats type, bool enter)
        {
            PlayerId = playerId;
            Type = type;
            Enter = enter;
        }

        public ScriptableObject GetScriptableObject()
        {
            return Type;
        }
    }
}