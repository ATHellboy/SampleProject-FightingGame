using AlirezaTarahomi.FightingGame.Player.Event;
using AlirezaTarahomi.FightingGame.General;
using Assets.Infrastructure.Scripts.CQRS;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.UI
{
    public class UIMessageBusRules : MessageBusRules
    {
        public UIMessageBusRules(IMessageBus messageBus) : base(messageBus) { }

        public override void InitRules()
        {
            _rules = new MessageRouteRule[]
            {
                MessageRouteRule.Create<OnGameOvered, UIGameOverPanel>(string.Empty, false)
            };
        }
    }
}