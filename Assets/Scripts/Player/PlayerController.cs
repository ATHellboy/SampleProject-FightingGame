using UnityEngine;
using Zenject;
using Assets.Infrastructure.Scripts.CQRS;
using AlirezaTarahomi.FightingGame.InputSystem;
using AlirezaTarahomi.FightingGame.Player.Event;
using System.Collections;
using UniRx;

namespace AlirezaTarahomi.FightingGame.Player
{
    public class PlayerController : MonoBehaviour
    {
        [HideInInspector] public Character.CharacterController currentCharacterController;
        [HideInInspector]
        public Side side
        {
            get => _charactersSwitchingHandler.side;
            set => _charactersSwitchingHandler.side = value;
        }

        private int _playerId;

        private IMessageBus _messageBus;
        private InputManager _inputManager;
        private CharactersSwitchingHandler _charactersSwitchingHandler;
        private float _switchingCoolDown;
        private bool _canSwitch = true;
        private int _numOfChars;
        private int _numOfRemainedChars;

        [Inject]
        public void Construct(InputManager inputManager, CharactersSwitchingHandler charactersSwitchingHandler,
            IMessageBus messageBus, [Inject(Id = "playerId")] int playerId, [Inject(Id = "switchingCoolDown")] float switchingCoolDown)
        {
            _inputManager = inputManager;
            _messageBus = messageBus;
            _charactersSwitchingHandler = charactersSwitchingHandler;
            _playerId = playerId;
            _switchingCoolDown = switchingCoolDown;
        }

        void OnDisable()
        {

        }

        void Awake()
        {
            GetCharacters();
            _numOfRemainedChars = _numOfChars;
            currentCharacterController = _charactersSwitchingHandler.ConfigCharacters();
        }

        // Update is called once per frame
        void Update()
        {
            if (_inputManager.IsPressed("Switch_P" + _playerId) && _numOfRemainedChars > 1 && _canSwitch)
            {
                Switch();
            }

            currentCharacterController.HandleInputPressed("Jump", _inputManager.IsPressed("Jump_P" + _playerId));
            currentCharacterController.HandleInputPressed("Attack", _inputManager.IsPressed("Attack_P" + _playerId));
            currentCharacterController.HandleInputPressed("PowerupAttack", _inputManager.IsPressed("PowerupAttack_P" + _playerId));

            currentCharacterController.moveAxes = 
                new Vector2(_inputManager.GetAxis("MoveHorizontal_P" + _playerId), _inputManager.GetAxis("MoveVertical_P" + _playerId));
        }

        private void GetCharacters()
        {
            foreach (Transform child in transform)
            {
                _numOfChars++;
                Character.CharacterController characterController = child.GetComponent<Character.CharacterController>();
                _charactersSwitchingHandler.EnqueueCharacter(characterController);
                characterController.OnAttackStarted.AddListener(HandleOnAttackStarted);
                characterController.OnAttackEnded.AddListener(HandleOnAttackEnded);
                characterController.OnDied.AddListener(HandleOnCharacterDied);
            }
        }

        private void Switch()
        {
            _canSwitch = false;
            Observable.FromCoroutine(_ => SwitchingTimer()).Subscribe();
            _charactersSwitchingHandler.ExitCurrentCharacter();
            currentCharacterController = _charactersSwitchingHandler.EnterNextCharacter();
        }

        IEnumerator SwitchingTimer()
        {
            yield return new WaitForSeconds(_switchingCoolDown);
            _canSwitch = true;
        }

        /// <summary>
        /// Handles the event when each character dies
        /// </summary>
        public void HandleOnCharacterDied()
        {
            _numOfRemainedChars--;

            if (_numOfRemainedChars == 0)
            {
                _messageBus.RaiseEvent(new OnGameOvered());
            }
            else
            {
                currentCharacterController = _charactersSwitchingHandler.EnterNextCharacter();
            }
        }

        /// <summary>
        /// Handles the event when character attack is started
        /// </summary>
        public void HandleOnAttackStarted()
        {
            _canSwitch = false;
        }

        /// <summary>
        /// Handles the event when character attack is ended
        /// </summary>
        public void HandleOnAttackEnded()
        {
            _canSwitch = true;
        }
    }
}