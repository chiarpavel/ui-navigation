using System;
using System.Collections;
using UINavigation;
using UnityEngine;

[CreateAssetMenu(fileName = "FadeTransition", menuName = "UINavigation/Fade Transition")]
public class FadeTransition : TransitionBase
{
    [Range(0.01f, 0.2f)]
    public float speed = 0.08f;

    public override IEnumerator Play(NavScreen currentScreen, NavScreen nextScreen)
    {
        nextScreen.RectTransform.SetAsLastSibling();
        nextScreen.OnShowing();

        if (currentScreen != null)
        {
            currentScreen.OnHiding();
            currentScreen.CanvasGroup.interactable = false;
            currentScreen.CanvasGroup.blocksRaycasts = false;
        }

        float progress = 0f;
        do
        {
            progress += speed;
            progress = Mathf.Clamp01(progress);

            UpdateScreenFade(nextScreen.CanvasGroup, 0f, 1f, progress);

            yield return null;
        } while (progress < 1f);

        nextScreen.OnShown();
        nextScreen.CanvasGroup.interactable = true;
        nextScreen.CanvasGroup.blocksRaycasts = true;
        if (currentScreen != null)
        {
            currentScreen.OnHidden();
            currentScreen.CanvasGroup.alpha = 0f;
        }
    }

    public override IEnumerator PlayReverse(NavScreen currentScreen, NavScreen previousScreen)
    {
        currentScreen.OnHiding();
        currentScreen.CanvasGroup.interactable = false;
        currentScreen.CanvasGroup.blocksRaycasts = false;

        if (previousScreen != null)
        {
            var currentScreenIndex = currentScreen.RectTransform.GetSiblingIndex();
            previousScreen.RectTransform.SetSiblingIndex(currentScreenIndex);
            previousScreen.OnShowing();
            previousScreen.CanvasGroup.alpha = 1f;
        }

        float progress = 0f;
        do
        {
            progress += speed;
            progress = Mathf.Clamp01(progress);

            UpdateScreenFade(currentScreen.CanvasGroup, 1f, 0f, progress);

            yield return null;
        } while (progress < 1f);

        currentScreen.OnHidden();
        if (previousScreen != null)
        {
            previousScreen.OnShown();
            previousScreen.CanvasGroup.interactable = true;
            previousScreen.CanvasGroup.blocksRaycasts = true;
        }
    }

    private void UpdateScreenFade(CanvasGroup canvasGroup, float from, float to, float progress)
    {
        float easedProgress = Mathf.SmoothStep(from, to, progress);
        canvasGroup.alpha = easedProgress;
    }
}
