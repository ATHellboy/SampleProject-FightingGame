using System;
using System.Collections;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.UI
{
    public class UIVisibilityController : MonoBehaviour
    {
        [SerializeField] private float _time = 0;

        public bool IsVisible { get; private set; }

        private CanvasGroup _canvasGroup;
        private IEnumerator _displaying;

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void ToggleDisplaying(bool active, Action callback = null)
        {
            IsVisible = active;

            if (_displaying != null)
            {
                StopCoroutine(_displaying);
            }
            _displaying = Displaying(active, callback);
            StartCoroutine(_displaying);
        }

        IEnumerator Displaying(bool active, Action callback)
        {
            float currentTime = 0;
            float perc = 0;
            float currentAlpha = _canvasGroup.alpha;

            float finalAlpha;
            if (active)
            {
                finalAlpha = 1;
            }
            else
            {
                finalAlpha = 0;
            }

            if (_time == 0)
            {
                _canvasGroup.blocksRaycasts = active;
                _canvasGroup.alpha = finalAlpha;

                if (callback != null)
                {
                    callback();
                }

                yield break;
            }

            if (!active)
            {
                _canvasGroup.blocksRaycasts = false;
            }

            while (perc != 1)
            {
                currentTime += Time.deltaTime;
                if (currentTime > _time)
                {
                    currentTime = _time;
                }
                perc = currentTime / _time;
                _canvasGroup.alpha = Mathf.Lerp(currentAlpha, finalAlpha, perc);
                yield return null;
            }

            if (active)
            {
                _canvasGroup.blocksRaycasts = true;
            }

            if (callback != null)
            {
                callback();
            }
        }
    }
}