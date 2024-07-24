using UnityEngine;
using System.Collections;

public class EyeBlink : MonoBehaviour
{
    [Header("Blink Settings")]
    [SerializeField] private float blinkInterval = 3f; // Time between blinks
    [SerializeField] private float blinkDuration = 0.1f; // Duration of the blink
    [SerializeField] private Sprite[] blinkSprites; // Array of sprites for the blink animation

    [Header("CACHE")]
    [SerializeField] private SpriteRenderer spriteRenderer; // The sprite renderer of the eye

    private Sprite normalSprite; // The normal eye sprite

    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        normalSprite = spriteRenderer.sprite;
        StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(blinkInterval);

            // Start the blink animation
            for (int i = 0; i < blinkSprites.Length; i++)
            {
                spriteRenderer.sprite = blinkSprites[i];
                spriteRenderer.material.SetTexture("_MainTex", blinkSprites[i].texture);
                yield return new WaitForSeconds(blinkDuration / blinkSprites.Length);
            }

            // Return to the normal sprite
            spriteRenderer.sprite = normalSprite;
            spriteRenderer.material.SetTexture("_MainTex", normalSprite.texture);
        }
    }
}
