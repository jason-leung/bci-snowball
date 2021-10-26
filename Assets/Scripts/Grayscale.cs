using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grayscale : MonoBehaviour
{
    public void Start()
    {
        EnableGrayscale();
    }

    private float duration = 1f;

    public SpriteRenderer spriteRenderer;

    public void SetGrayscale(float amount)
    {
        spriteRenderer.material.SetFloat("_GrayscaleAmount", amount);
    }

    public void EnableGrayscale()
    {
        SetGrayscale(1f);
    }

    public void DisableGrayscale()
    {
        SetGrayscale(0f);
    }

    public IEnumerator GrayscaleRoutine(float duration, bool isGrayscale)
    {
        float time = 0;
        while (duration > time)
        {
            float durationFrame = Time.deltaTime;
            float ratio = time / duration;
            float grayAmount = isGrayscale ? ratio : 1 - ratio;
            SetGrayscale(grayAmount);
            time += durationFrame;
            yield return null;
        }
        SetGrayscale(isGrayscale ? 1f : 0f);
    }

    public void StartGrayscaleRoutine()
    {
        StartCoroutine(GrayscaleRoutine(duration, true));
    }

    public void Reset()
    {
        StartCoroutine(GrayscaleRoutine(duration, false));
    }
}
