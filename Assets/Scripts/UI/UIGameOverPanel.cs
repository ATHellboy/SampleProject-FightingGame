using AlirezaTarahomi.FightingGame.Player.Event;
using MessagePipe;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace AlirezaTarahomi.FightingGame.UI
{
    public class UIGameOverPanel : MonoBehaviour
    {
        private ISubscriber<OnGameOver> _gameOverSubscriber;

        private UIVisibilityController _visibilityController;
        private IDisposable _disposable;

        [Inject]
        public void Construct(ISubscriber<OnGameOver> gameOverSubscriber)
        {
            _gameOverSubscriber = gameOverSubscriber;
        }

        void Awake()
        {
            _visibilityController = GetComponent<UIVisibilityController>(); 
        }

        void OnEnable()
        {
            DisposableBagBuilder bag = DisposableBag.CreateBuilder();

            _gameOverSubscriber.Subscribe(Handle).AddTo(bag);

            _disposable = bag.Build();
        }

        void OnDisable()
        {
            _disposable.Dispose();
        }

        public void OnClickRestart()
        {
            SceneManager.LoadScene(0);
        }

        private void Handle(OnGameOver @event)
        {
            _visibilityController.ToggleDisplaying(true);
        }
    }
}