using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlirezaTarahomi.FightingGame.General.Variable
{
    public abstract class ShareableVariable : ScriptableObject
    {
        void OnEnable()
        {
            SceneManager.sceneLoaded += HandleOnSceneLoaded;
        }

        protected abstract void HandleOnSceneLoaded(Scene current, LoadSceneMode mode);
    }
}