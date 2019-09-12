using AlirezaTarahomi.FightingGame.Character.Behavior.Powerup;
using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.Player.Event;
using AlirezaTarahomi.FightingGame.Player.Validator;
using Assets.Infrastructure.Scripts.CQRS;
using Assets.Infrastructure.Scripts.CQRS.Validators;
using ScriptableObjectDropdown;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character.Powerup
{
    [CreateAssetMenu(menuName = "Powerups/FlyOverPowerup")]
    public class FlyOverPowerup : ScriptableObject, IPowerup,
        IEventHandler<OnPushForwardEnded>, IPlayerIdProperty, IScriptableObjectProperty
    {
        [ScriptableObjectDropdown(typeof(FlyOverAttackBehavior))]
        public ScriptableObjectReference _powerupAttackBehavior;
        public ScriptableObject PowerupAttackBehavior { get { return _powerupAttackBehavior.value; } }
        public int PlayerId { get { return _context.PlayerId; } }

        private static MessageRouteRule _rule = MessageRouteRule.Create<OnPushForwardEnded, FlyOverPowerup>(string.Empty, false,
                new AndValidator<OnPushForwardEnded>(new EventPlayerIdValidator<OnPushForwardEnded>(),
                new EventScriptableObjectValidator<OnPushForwardEnded>()));

        private IMessageBus _messageBus;
        private CharacterPowerupContext _context;

        [Inject]
        public void Contruct(IMessageBus messageBus)
        {
            _messageBus = messageBus;

            InitializeEvents();
        }

        public void Inject(CharacterPowerupContext context)
        {
            _context = context;
        }

        //void OnDestroy()
        //{
        //    UnsubscribeEvents();
        //}

        private void InitializeEvents()
        {
            if (_messageBus != null)
            {
                _messageBus.AddRule(_rule);
                _messageBus.Subscribe<FlyOverPowerup, OnPushForwardEnded>(this, new MessageHandlerActionExecutor<OnPushForwardEnded>(Handle));
            }
        }

        //private void UnsubscribeEvents()
        //{
        //    if (_messageBus != null)
        //    {
        //        _messageBus.Unsubscribe<FlyOverPowerup, OnPushForwardEnded>(this);
        //    }
        //}

        public PowerType Active()
        {
            _messageBus.RaiseEvent(new OnPowerupToggled(_context.PlayerId, _context.Stats, true));
            return PowerType.OneTime;
        }

        public void Disable()
        {
            _messageBus.RaiseEvent(new OnPowerupToggled(_context.PlayerId, _context.Stats, false));
        }

        public void Handle(OnPushForwardEnded @event)
        {
            Disable();
        }

        public ScriptableObject GetScriptableObject()
        {
            return _context.Stats;
        }
    }
}