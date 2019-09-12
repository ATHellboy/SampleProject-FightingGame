using UnityEngine;
using System;
using Assets.Infrastructure.Scripts.CQRS;

namespace AlirezaTarahomi.FightingGame.Tool.Event
{
    public class OnThrowableObjectPicked : IEvent, IGameObjectProperty
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
        public GameObject GameObject { get; }

        public OnThrowableObjectPicked(GameObject gameObject)
        {
            GameObject = gameObject;
        }
    }
}