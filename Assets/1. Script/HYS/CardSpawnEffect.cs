
using System.Collections;
using UnityEngine;

public class CardSpawnEffect : MonoBehaviour
{
    [Header("Spawn Animation")]
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private ParticleSystem spawnParticles;

    [SerializeField]
    private AnimationCurve scaleCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 originalScale;
    private Coroutine effectRoutine;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void PlaySpawnEffect()
    {
        if (effectRoutine != null)
            StopCoroutine(effectRoutine);

        effectRoutine = StartCoroutine(SpawnAnimation());
    }

    private IEnumerator SpawnAnimation()
    {
        float elapsed = 0f;
        float animationDuration = Mathf.Max(0.01f, duration);

        transform.localScale = Vector3.zero;

        if (spawnParticles != null)
            spawnParticles.Play();

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsed / animationDuration);

            float scaleValue = scaleCurve.Evaluate(progress);

            transform.localScale = originalScale * scaleValue;

            yield return null;
        }

        transform.localScale = originalScale;
        effectRoutine = null;
    }

    public void ResetEffect()
    {
        if (effectRoutine != null)
        {
            StopCoroutine(effectRoutine);
            effectRoutine = null;
        }

        transform.localScale = originalScale;

        if (spawnParticles != null)
            spawnParticles.Stop();
    }
}
