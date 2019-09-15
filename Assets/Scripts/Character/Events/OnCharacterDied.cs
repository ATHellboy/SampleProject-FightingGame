using AlirezaTarahomi.FightingGame.Player.Event;
using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Event
{
    public class OnCharacterDied : IEvent, IPlayerIdProperty
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
        public int PlayerId { get; }

        public OnCharacterDied(string entityId, int playerId)
        {
            EntityId = entityId;
            PlayerId = playerId;
        }
    }
}