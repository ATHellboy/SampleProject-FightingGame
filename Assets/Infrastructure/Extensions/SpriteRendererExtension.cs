using UnityEngine;

namespace Infrastructure.Extension
{
    public static class SpriteRendererExtension
    {
        public static void SetAlpha(this SpriteRenderer spriteRenderer, float a)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, a);
        }
    }
}