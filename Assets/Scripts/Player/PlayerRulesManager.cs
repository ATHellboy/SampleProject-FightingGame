using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.Player.Validator;
using AlirezaTarahomi.FightingGame.Service;
using Assets.Infrastructure.Scripts.CQRS;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class PlayerRulesManager : RulesManager
    {
        protected override void InitRules()
        {
            _rules = new MessageRouteRule[]
            {
            MessageRouteRule.Create<OnCharacterDied, PlayerController>(string.Empty, false,
                new EventPlayerIdValidator<OnCharacterDied>()),
            MessageRouteRule.Create<OnAttackToggled, PlayerController>(string.Empty, false,
                new EventPlayerIdValidator<OnAttackToggled>())
            };
        }
    }
}