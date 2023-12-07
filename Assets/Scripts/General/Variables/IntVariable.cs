using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlirezaTarahomi.FightingGame.General.Variable
{
    [CreateAssetMenu(menuName = "Custom/Variables/Int")]
    public class IntVariable : ShareableVariable
    {
        [SerializeField] private int _defaltValue;

        [NonSerialized] public int value;

        protected override void HandleOnSceneLoaded(Scene current, LoadSceneMode mode)
        {
            value = _defaltValue;
        }
    }
}