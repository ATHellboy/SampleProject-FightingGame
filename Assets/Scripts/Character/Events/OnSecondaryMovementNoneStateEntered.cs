using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Event
{
    public class OnSecondaryMovementNoneStateEntered : IEvent
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }

        public OnSecondaryMovementNoneStateEntered(string entityId)
        {
            EntityId = entityId;
        }
    }
}