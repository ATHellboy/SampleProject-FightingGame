using UnityEngine;
using Zenject;
using Assets.Infrastructure.Scripts.CQRS;
using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.InputSystem;
using AlirezaTarahomi.FightingGame.Player.Event;
using AlirezaTarahomi.FightingGame.Player.Validator;
using System.Collections;
using UniRx;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class PlayerController : MonoBehaviour, IEventHandler<OnCharacterDied>, IEventHandler<OnAttackToggled>,
        IPlayerIdProperty
    {
        [HideInInspector] public Transform currentCharacter;
        [HideInInspector] public Side side;

        public int PlayerId { get; private set; }

        private IMessageBus _messageBus;
        private InputManager _inputManager;
        private CharactersSwitchingHandler _charactersSwitchingHandler;
        private PlayerSideDetector _playerSideDetector;
        private float _switchingCoolDown;
        private bool _canSwitch = true;
        private int _numOfChars;
        private int _numOfRemainedChars;
        private static MessageRouteRule[] _rules = new MessageRouteRule[]
        {
        MessageRouteRule.Create<OnCharacterDied, PlayerController>(string.Empty, false,
            new EventPlayerIdValidator<OnCharacterDied>()),
        MessageRouteRule.Create<OnAttackToggled, PlayerController>(string.Empty, false,
            new EventPlayerIdValidator<OnAttackToggled>())
        };

        [Inject]
        public void Construct(InputManager inputManager, CharactersSwitchingHandler charactersSwitchingHandler,
            PlayerSideDetector playerSideDetector, IMessageBus messageBus, [Inject(Id = "playerId")] int playerId,
            [Inject(Id = "switchingCoolDown")] float switchingCoolDown)
        {
            _inputManager = inputManager;
            _messageBus = messageBus;
            _charactersSwitchingHandler = charactersSwitchingHandler;
            _playerSideDetector = playerSideDetector;
            PlayerId = playerId;
            _switchingCoolDown = switchingCoolDown;
        }

        void OnEnable()
        {
            InitializeEvents();
        }

        void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void InitializeEvents()
        {
            _messageBus.AddRule(_rules[0]);
            _messageBus.Subscribe<PlayerController, OnCharacterDied>(this, new MessageHandlerActionExecutor<OnCharacterDied>(Handle));

            _messageBus.AddRule(_rules[1]);
            _messageBus.Subscribe<PlayerController, OnAttackToggled>(this, new MessageHandlerActionExecutor<OnAttackToggled>(Handle));
        }

        private void UnsubscribeEvents()
        {
            _messageBus.Unsubscribe<PlayerController, OnCharacterDied>(this);
            _messageBus.Unsubscribe<PlayerController, OnAttackToggled>(this);
        }

        void Awake()
        {
            GetCharacters();
            _numOfRemainedChars = _numOfChars;
            currentCharacter = _charactersSwitchingHandler.ConfigCharacters();
        }

        // Update is called once per frame
        void Update()
        {
            if (_inputManager.IsDown("Switch_P" + PlayerId) && _numOfRemainedChars > 1 && _canSwitch)
            {
                Switch();
            }
        }

        private void GetCharacters()
        {
            foreach (Transform child in transform)
            {
                _numOfChars++;
                _charactersSwitchingHandler.EnqueueCharacter(child.GetComponent<Character.CharacterController>());
            }
        }

        private void Switch()
        {
            _canSwitch = false;
            Observable.FromCoroutine(_ => SwitchingTimer()).Subscribe();
            _charactersSwitchingHandler.ExitCurrentCharacter();
            currentCharacter = _charactersSwitchingHandler.EnterNextCharacter();
        }

        IEnumerator SwitchingTimer()
        {
            yield return new WaitForSeconds(_switchingCoolDown);
            _canSwitch = true;
        }

        /// <summary>
        /// Handles the event when each character dies
        /// </summary>
        public void Handle(OnCharacterDied @event)
        {
            _numOfRemainedChars--;

            if (_numOfRemainedChars == 0)
            {
                _messageBus.RaiseEvent(new OnGameOvered());
            }
            else
            {
                currentCharacter = _charactersSwitchingHandler.EnterNextCharacter();
            }
        }

        /// <summary>
        /// Handles the event when character attack is toggled (attack is started or ended)
        /// </summary>
        public void Handle(OnAttackToggled @event)
        {
            if (@event.Enable)
            {
                _canSwitch = false;
            }
            else
            {
                _canSwitch = true;
            }
        }
    }
}