using AlirezaTarahomi.FightingGame.General;
using Assets.Infrastructure.Scripts.CQRS;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class CharacterMessageBusRules : MessageBusRules
    {
        public CharacterMessageBusRules(IMessageBus messageBus) : base(messageBus) { }

        public override void InitRules()
        {
            _rules = new MessageRouteRule[]
            {
            
            };
        }
    }
}