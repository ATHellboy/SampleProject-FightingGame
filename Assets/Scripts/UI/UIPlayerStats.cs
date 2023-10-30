using AlirezaTarahomi.FightingGame.Character.Behavior.Complex;
using AlirezaTarahomi.FightingGame.Character.Behavior.Normal;
using AlirezaTarahomi.FightingGame.Player.Event;
using MessagePipe;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace AlirezaTarahomi.FightingGame.UI
{
    public class UIPlayerStats : MonoBehaviour
    {
        [SerializeField] private int index = 0;
        [SerializeField] private RawImage _avatarRawImage;
        [SerializeField] private RenderTexture _avatarRenderTexture;
        [SerializeField] private Image _normalAttackCooldownImage;
        [SerializeField] private Image _complexAttackCooldownImage;
        [SerializeField] private Image _powerupCooldownImage;
        [SerializeField] private Image _powerupDurationImage;

        private ISubscriber<int, OnAttackCooldownStarted> _onAttackCooldownSubscriber;
        private ISubscriber<int, OnPowerupTimerStarted> _onPowerupCooldownSubscriber;

        private IDisposable _disposable;

        [Inject]
        public void Construct(ISubscriber<int, OnAttackCooldownStarted> onAttackCooldownSubscriber,
            ISubscriber<int, OnPowerupTimerStarted> onPowerupTimerSubscriber)
        {
            _onAttackCooldownSubscriber = onAttackCooldownSubscriber;
            _onPowerupCooldownSubscriber = onPowerupTimerSubscriber;
        }

        void OnEnable()
        {
            DisposableBagBuilder bag = DisposableBag.CreateBuilder();

            _onAttackCooldownSubscriber.Subscribe(index, HandleAttackCooldown).AddTo(bag);
            _onPowerupCooldownSubscriber.Subscribe(index, HandlePowerupCooldown).AddTo(bag);

            _disposable = bag.Build();
        }

        void OnDisable()
        {
            _disposable.Dispose();
        }

        void Awake()
        {
            _avatarRawImage.texture = _avatarRenderTexture;
        }

        IEnumerator FillTimer(bool active, Image image, float time, Action callback = null)
        {
            image.fillAmount = active ? 0 : 1;

            float currentTime = 0;
            float perc = 0;
            float currentFillAmount = image.fillAmount;

            float finalFillAmount = active ? 1 : 0;

            if (time == 0)
            {
                image.fillAmount = finalFillAmount;
                callback?.Invoke();
                yield break;
            }

            while (perc != 1)
            {
                currentTime += Time.deltaTime;
                if (currentTime > time)
                {
                    currentTime = time;
                }
                perc = currentTime / time;
                image.fillAmount = Mathf.Lerp(currentFillAmount, finalFillAmount, perc);
                yield return null;
            }

            callback?.Invoke();
        }

        private void HandleAttackCooldown(OnAttackCooldownStarted @event)
        {
            switch (@event.attackType.Name)
            {
                case nameof(INormalAttackBehavior):
                    StartCoroutine(FillTimer(true, _normalAttackCooldownImage, @event.cooldown));
                    break;
                case nameof(IComplexAttackBehavior):
                    StartCoroutine(FillTimer(true, _complexAttackCooldownImage, @event.cooldown));
                    break;
            }
        }

        private void HandlePowerupCooldown(OnPowerupTimerStarted @event)
        {
            StartCoroutine(FillTimer(false, _powerupDurationImage, @event.duration, 
                () => StartCoroutine(FillTimer(true, _powerupCooldownImage, @event.cooldown - @event.duration))));
        }
    }
}