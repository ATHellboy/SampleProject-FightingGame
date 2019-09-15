using AlirezaTarahomi.FightingGame.Player.Event;
using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Event
{
    public class OnPowerupToggled : IEvent
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
        public bool Active { get; }

        public OnPowerupToggled(string entityId, bool active)
        {
            EntityId = entityId;
            Active = active;
        }
    }
}