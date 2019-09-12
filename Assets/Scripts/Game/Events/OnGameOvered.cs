using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Game.Event
{
    public class OnGameOvered : IEvent
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
    }
}