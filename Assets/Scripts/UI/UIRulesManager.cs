using AlirezaTarahomi.FightingGame.Player.Event;
using AlirezaTarahomi.FightingGame.Service;
using Assets.Infrastructure.Scripts.CQRS;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.UI
{
    public class UIRulesManager : RulesManager
    {
        protected override void InitRules()
        {
            _rules = new MessageRouteRule[]
            {
                MessageRouteRule.Create<OnGameOvered, UIGameOverPanel>(string.Empty, false)
            };
        }
    }
}