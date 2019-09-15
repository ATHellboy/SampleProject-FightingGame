using AlirezaTarahomi.FightingGame.Player.Event;
using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Event
{
    public class OnAttackToggled : IEvent, IPlayerIdProperty
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
        public int PlayerId { get; }
        public bool Enable { get; }

        public OnAttackToggled(string entityId, int playerId, bool enable)
        {
            EntityId = entityId;
            PlayerId = playerId;
            Enable = enable;
        }
    }
}