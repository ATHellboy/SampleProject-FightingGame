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
        private IDisposable disposable;

        [Inject]
        public void Construct(ISubscriber<OnGameOver> gameOverSubscriber)
        {
            _gameOverSubscriber = gameOverSubscriber;
            _visibilityController = GetComponent<UIVisibilityController>(); 
        }

        void OnEnable()
        {
            DisposableBagBuilder bag = DisposableBag.CreateBuilder();

            _gameOverSubscriber.Subscribe(Handle).AddTo(bag);

            disposable = bag.Build();
        }

        void OnDisable()
        {
            disposable.Dispose();
        }

        public void OnClickRestart()
        {
            SceneManager.LoadScene(0);
        }

        public void Handle(OnGameOver @event)
        {
            _visibilityController.ToggleDisplaying(true);
        }
    }
}