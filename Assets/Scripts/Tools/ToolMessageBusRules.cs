using AlirezaTarahomi.FightingGame.General;
using Assets.Infrastructure.Scripts.CQRS;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Tool
{
    public class ToolMessageBusRules : MessageBusRules
    {
        public ToolMessageBusRules(IMessageBus messageBus) : base(messageBus) { }

        public override void InitRules()
        {
            _rules = new MessageRouteRule[]
            {
            
            };
        }
    }
}