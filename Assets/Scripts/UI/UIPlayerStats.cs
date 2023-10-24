using UnityEngine;
using UnityEngine.UI;

namespace AlirezaTarahomi.FightingGame.UI
{
    public class UIPlayerStats : MonoBehaviour
    {
        [SerializeField] private RawImage _avatarRawImage;
        [SerializeField] private RenderTexture _avatarRenderTexture;

        void Awake()
        {
            _avatarRawImage.texture = _avatarRenderTexture;
        }
    }
}