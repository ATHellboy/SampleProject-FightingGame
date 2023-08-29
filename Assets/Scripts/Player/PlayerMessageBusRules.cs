using AlirezaTarahomi.FightingGame.General;
using Assets.Infrastructure.Scripts.CQRS;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class PlayerMessageBusRules : MessageBusRules
    {
        public PlayerMessageBusRules(IMessageBus messageBus) : base(messageBus) { }

        public override void InitRules()
        {
            _rules = new MessageRouteRule[]
            {
            
            };
        }
    }
}