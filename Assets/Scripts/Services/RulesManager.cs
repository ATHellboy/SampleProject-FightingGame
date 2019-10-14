using Assets.Infrastructure.Scripts.CQRS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Service
{
    public abstract class RulesManager : MonoBehaviour
    {
        private IMessageBus _messageBus;

        protected MessageRouteRule[] _rules;

        [Inject]
        public void Construct(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        void Awake()
        {
            InitRules();
            AddRules();
        }

        protected abstract void InitRules();

        private void AddRules()
        {
            for (int i = 0; i < _rules.Length; i++)
            {
                _messageBus.AddRule(_rules[i]);
            }
        }
    }
}