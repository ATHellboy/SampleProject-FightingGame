using AlirezaTarahomi.FightingGame.Character.Behavior.Powerup;
using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.Character.Validator;
using Assets.Infrastructure.Scripts.CQRS;
using ScriptableObjectDropdown;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character.Powerup
{
    [CreateAssetMenu(menuName = "Powerups/FlyOverPowerup")]
    public class FlyOverPowerup : ScriptableObject, IPowerup,
        IEventHandler<OnFlyOverEnded>, ICharacterIdProperty
    {
        [ScriptableObjectDropdown(typeof(FlyOverAttackBehavior))]
        public ScriptableObjectReference _powerupAttackBehavior;
        public ScriptableObject PowerupAttackBehavior { get { return _powerupAttackBehavior.value; } }
        public string CharacterId { get { return _context.CharacterId; } }

        private static MessageRouteRule _rule = MessageRouteRule.Create<OnFlyOverEnded, FlyOverPowerup>(string.Empty, false,
                new EventCharacterIdValidator<OnFlyOverEnded>());

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
                _messageBus.Subscribe<FlyOverPowerup, OnFlyOverEnded>(this, new MessageHandlerActionExecutor<OnFlyOverEnded>(Handle));
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
            _messageBus.RaiseEvent(new OnPowerupToggled(CharacterId, true));
            return PowerType.OneTime;
        }

        public void Disable()
        {
            _messageBus.RaiseEvent(new OnPowerupToggled(CharacterId, false));
        }

        /// <summary>
        /// Handles the event when flying over attack behaivor is ended
        /// </summary>
        public void Handle(OnFlyOverEnded @event)
        {
            Disable();
        }
    }
}