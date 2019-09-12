using UnityEngine;
using Zenject;
using Assets.Infrastructure.Scripts.CQRS;
using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.InputSystem;
using AlirezaTarahomi.FightingGame.Player.Event;
using AlirezaTarahomi.FightingGame.Player.Validator;
using AlirezaTarahomi.FightingGame.Game.Event;
using System.Collections;
using UniRx;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class PlayerController : MonoBehaviour, IEventHandler<OnCharacterDied>, IEventHandler<OnAttackToggled>,
        IEventHandler<OnGrounded>, IPlayerIdProperty
    {
        [SerializeField] private float _switchingCoolDown = 2.0f;

        [HideInInspector] public Transform currentCharacter;
        [HideInInspector] public Side side;

        public int PlayerId { get; private set; }

        private IMessageBus _messageBus;
        private InputManager _inputManager;
        private CharactersSwitchingHandler _charactersSwitchingHandler;
        private PlayerSideDetector _playerSideDetector;
        private bool _canSwitch = true;
        private bool _isOnGround = true;
        private int _numOfChars;
        private int _numOfRemainedChars;
        private static MessageRouteRule[] _rules = new MessageRouteRule[]
        {
        MessageRouteRule.Create<OnCharacterDied, PlayerController>(string.Empty, false,
            new EventPlayerIdValidator<OnCharacterDied>()),
        MessageRouteRule.Create<OnAttackToggled, PlayerController>(string.Empty, false,
            new EventPlayerIdValidator<OnAttackToggled>()),
        MessageRouteRule.Create<OnGrounded, PlayerController>(string.Empty, false,
            new EventPlayerIdValidator<OnGrounded>())
        };

        [Inject]
        public void Construct(InputManager inputManager, CharactersSwitchingHandler charactersSwitchingHandler,
            PlayerSideDetector playerSideDetector, IMessageBus messageBus, [Inject(Id = "id")] int id)
        {
            _inputManager = inputManager;
            _messageBus = messageBus;
            _charactersSwitchingHandler = charactersSwitchingHandler;
            _playerSideDetector = playerSideDetector;
            PlayerId = id;
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

            _messageBus.AddRule(_rules[2]);
            _messageBus.Subscribe<PlayerController, OnGrounded>(this, new MessageHandlerActionExecutor<OnGrounded>(Handle));
        }

        private void UnsubscribeEvents()
        {
            _messageBus.Unsubscribe<PlayerController, OnCharacterDied>(this);
            _messageBus.Unsubscribe<PlayerController, OnAttackToggled>(this);
            _messageBus.Unsubscribe<PlayerController, OnGrounded>(this);
        }

        void Awake()
        {
            foreach (Transform child in transform)
            {
                _numOfChars++;
                _charactersSwitchingHandler.EnqueueCharacter(child.GetComponent<Character.CharacterController>());
            }

            _numOfRemainedChars = _numOfChars;

            currentCharacter = _charactersSwitchingHandler.ConfigCharacters();
        }

        // Update is called once per frame
        void Update()
        {
            if (_inputManager.IsDown("Switch_P" + PlayerId) && _numOfRemainedChars > 1 && _canSwitch && _isOnGround)
            {
                _canSwitch = false;
                _isOnGround = false;
                Observable.FromCoroutine(_ => SwitchingTimer()).Subscribe();
                _charactersSwitchingHandler.ExitCurrentCharacter();
                currentCharacter = _charactersSwitchingHandler.EnterNextCharacter();
            }
        }

        private void CreateCharacters()
        {
            
        }

        IEnumerator SwitchingTimer()
        {
            yield return new WaitForSeconds(_switchingCoolDown);
            _canSwitch = true;
        }

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

        public void Handle(OnAttackToggled @event)
        {
            if (@event.Enter)
            {
                _canSwitch = false;
            }
            else
            {
                _canSwitch = true;
            }
        }

        public void Handle(OnGrounded @event)
        {
            _isOnGround = true;
        }
    }

    public enum Side
    {
        Left = -1,
        Right = 1
    }
}