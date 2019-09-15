using UnityEngine;
using System;
using Assets.Infrastructure.Scripts.CQRS;

namespace AlirezaTarahomi.FightingGame.Tool.Event
{
    public class OnThrowableObjectPickedUp : IEvent, IGameObjectProperty
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
        public GameObject GameObject { get; }

        public OnThrowableObjectPickedUp(GameObject gameObject)
        {
            GameObject = gameObject;
        }
    }
}