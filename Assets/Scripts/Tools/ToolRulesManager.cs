using AlirezaTarahomi.FightingGame.Service;
using AlirezaTarahomi.FightingGame.Tool.Event;
using Assets.Infrastructure.Scripts.CQRS;
using Assets.Infrastructure.Scripts.CQRS.Validators;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Tool
{
    public class ToolRulesManager : RulesManager
    {
        protected override void InitRules()
        {
            _rules = new MessageRouteRule[]
            {
            MessageRouteRule.Create<OnThrowableObjectPickedUp,
                ShurikenController>(string.Empty, false, new EventGameObjectValidator<OnThrowableObjectPickedUp>())
            };
        }
    }
}