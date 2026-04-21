using _01_Script._00_Core.ETC;
using UnityEngine;

public class BlinkFeedback : Feedback
{
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private float blinkDuration = 0.15f;
    [SerializeField] private float blinkIntensity = 0.2f;

    private readonly int _blinkHash = Shader.PropertyToID("_BlinkValue");

    public override async void CreateFeedback()
    {
        meshRenderer.material.SetFloat(_blinkHash, blinkIntensity);
        await Awaitable.WaitForSecondsAsync(blinkDuration);
        StopFeedback();
    }

    public override void StopFeedback()
    {
        if (meshRenderer != null)
            meshRenderer.material.SetFloat(_blinkHash, 0);
    }
}