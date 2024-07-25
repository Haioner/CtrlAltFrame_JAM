using System.Collections;
using UnityEngine;

public class LightFlower : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [ColorUsage(true, true)][SerializeField] private Color emissionColor;
    [SerializeField] private float transitionDuration = 1.0f; // Duration of the color transition

    private Coroutine colorTransitionCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (colorTransitionCoroutine != null)
            {
                StopCoroutine(colorTransitionCoroutine);
            }
            colorTransitionCoroutine = StartCoroutine(TransitionToColor(emissionColor));
        }
    }

    private IEnumerator TransitionToColor(Color targetColor)
    {
        Color initialColor = spriteRenderer.material.GetColor("_EmissionColor");
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            Color currentColor = Color.Lerp(initialColor, targetColor, elapsedTime / transitionDuration);
            spriteRenderer.material.SetColor("_EmissionColor", currentColor);
            yield return null;
        }

        // Ensure the final color is set
        spriteRenderer.material.SetColor("_EmissionColor", targetColor);
    }
}
