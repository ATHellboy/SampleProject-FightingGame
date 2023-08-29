using Assets.Infrastructure.Scripts.CQRS;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.General
{
    public abstract class MessageBusRules
    {
        private IMessageBus _messageBus;

        protected MessageRouteRule[] _rules;

        public MessageBusRules(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public abstract void InitRules();

        public void AddRules()
        {
            for (int i = 0; i < _rules.Length; i++)
            {
                _messageBus.AddRule(_rules[i]);
            }
        }
    }
}