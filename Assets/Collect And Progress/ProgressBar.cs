using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private SlicedFilledImage progressImage;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private float defaultDuration;
    [SerializeField] private EaseType easeType;
    private float currentProgress = 0;
    private float totalCount;

    public Action onProgressCompletion;

    public void Setup(int totalCount, int currentCount)
    {
        this.totalCount = totalCount;
        currentProgress = (float)currentCount / totalCount;
        SetProgressImmediate(currentProgress);
    }

    private void SetProgress(float progress, float duration)
    {
        ClampProgress();
        var targetCount = (int)(totalCount * progress);
        currentProgress = progress;

        if (duration == 0)
        {
            progressImage.fillAmount = currentProgress;
            progressText.text = targetCount + "/" + totalCount;
            return;
        }

        StartCoroutine(DoProgress(currentProgress, duration));

        void ClampProgress()
        {
            if(progress < 0.0f)
                progress = 0f;
            else if (progress > 1.0f)
                progress = 1.0f;
        }
    }

    private IEnumerator DoProgress(float currentProgress, float duration)
    {
        var t = 0f;
        var currentCount = (int)(totalCount * currentProgress);
        int initialAmount = (int)Mathf.Min(Int32.Parse(progressText.text.Split("/")[0]) + 1, totalCount);
        while (t < duration)
        {
            var countString = Mathf.Lerp(initialAmount, currentCount, t / duration);
            progressText.text = countString.ToString("0") + "/" + totalCount;
            progressImage.fillAmount = Mathf.Lerp(progressImage.fillAmount, currentProgress, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        if (currentProgress >= 1)
            onProgressCompletion?.Invoke();
    }
    
    public void SetProgress(float progress)
    {
        StopAllCoroutines();
        SetProgress(progress, defaultDuration);
    }

    public void AddAmountToProgress(int amount)
    { 
        float newProgress = currentProgress + amount / totalCount;
        SetProgress(newProgress);
    }

    public void SetProgressImmediate(float progress)
    {
        SetProgress(progress, 0);            
    }
}