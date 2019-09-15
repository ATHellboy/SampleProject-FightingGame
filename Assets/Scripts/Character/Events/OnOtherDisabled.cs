using AlirezaTarahomi.FightingGame.Player.Event;
using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Event
{
    public class OnOtherDisabled : IEvent, IPlayerIdProperty
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
        public int PlayerId { get; }

        public OnOtherDisabled(string entityId, int playerId)
        {
            EntityId = entityId;
            PlayerId = playerId;
        }
    }
}