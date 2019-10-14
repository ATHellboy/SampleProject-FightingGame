using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.Character.Powerup;
using AlirezaTarahomi.FightingGame.Character.Validator;
using AlirezaTarahomi.FightingGame.Player.Event;
using AlirezaTarahomi.FightingGame.Player.Validator;
using AlirezaTarahomi.FightingGame.Service;
using Assets.Infrastructure.Scripts.CQRS;
using Assets.Infrastructure.Scripts.CQRS.Validators;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character
{
    public class CharacterRulesManager : RulesManager
    {
        protected override void InitRules()
        {
            _rules = new MessageRouteRule[]
            {
            MessageRouteRule.Create<OnPowerupToggled, CharacterController>(string.Empty, false,
                new EventEntityIdValidator<OnPowerupToggled>()),
            MessageRouteRule.Create<OnControlToggled, CharacterController>(string.Empty, false,
                new EventEntityIdValidator<OnControlToggled>()),
            MessageRouteRule.Create<OnCharacterArrivalToggled, CharacterController>(string.Empty, false,
                new EventEntityIdValidator<OnCharacterArrivalToggled>()),
            MessageRouteRule.Create<OnOtherDisabled, CharacterController>(string.Empty, false,
                new AndValidator<OnOtherDisabled>(new EventPlayerIdValidator<OnOtherDisabled>(),
                new NotValidator<OnOtherDisabled>((new EventEntityIdValidator<OnOtherDisabled>())))),
            MessageRouteRule.Create<OnGrounded, CharacterController>(string.Empty, false,
                new EventEntityIdValidator<OnGrounded>()),
            MessageRouteRule.Create<OnCharacterDied, CharacterController>(string.Empty, false,
                new EventEntityIdValidator<OnCharacterDied>()),
            MessageRouteRule.Create<OnAttackToggled, CharacterController>(string.Empty, false,
                new AndValidator<OnAttackToggled>(new EventEntityIdValidator<OnAttackToggled>(),
                                                  new EventPlayerIdValidator<OnAttackToggled>())),
            MessageRouteRule.Create<OnSecondaryMovementNoneStateEntered, CharacterController>(string.Empty, false,
                new EventEntityIdValidator<OnSecondaryMovementNoneStateEntered>()),
            MessageRouteRule.Create<OnCharacterFlownToggled, CharacterController>(string.Empty, false,
                new EventEntityIdValidator<OnCharacterFlownToggled>()),
            MessageRouteRule.Create<OnFlyOverEnded, FlyOverPowerup>(string.Empty, false,
                new EventCharacterIdValidator<OnFlyOverEnded>())
            };
        }
    }
}