using System;
using System.Collections;
using UINavigation;
using UnityEngine;

[CreateAssetMenu(fileName = "SlideTransition", menuName = "UINavigation/Slide Transition")]
public class SlideTransition : TransitionBase {
    [Range(0.01f, 0.2f)]
    public float speed = 0.08f;

    public override IEnumerator Play(NavScreen currentScreen, NavScreen nextScreen)
    {
        nextScreen.RectTransform.SetAsLastSibling();
        nextScreen.OnShowing();
        if (currentScreen != null)
        {
            currentScreen.OnHiding();
        }

        float progress = 0f;
        while (progress < 1)
        {
            progress += speed;
            progress = Mathf.Clamp01(progress);

            UpdateScreenPosition(nextScreen, 1f, 0f, progress);
            if (currentScreen != null)
            {
                UpdateScreenPosition(currentScreen, 0f, -0.3f, progress);
            }

            yield return null;
        }

        nextScreen.OnShown();
        if (currentScreen != null)
        {
            currentScreen.OnHidden();
        }
    }

    public override IEnumerator PlayReverse(NavScreen currentScreen, NavScreen previousScreen) {
        currentScreen.OnHiding();
        if (previousScreen != null)
        {
            var currentScreenIndex = previousScreen.RectTransform.GetSiblingIndex();
            previousScreen.RectTransform.SetSiblingIndex(currentScreenIndex);
            previousScreen.OnShowing();
        }

        float progress = 0f;
        while (progress < 1)
        {
            progress += speed;
            progress = Mathf.Clamp01(progress);

            UpdateScreenPosition(currentScreen, 0f, 1f, progress);
            if (previousScreen != null)
            {
                UpdateScreenPosition(previousScreen, -0.3f, 0f, progress);
            }

            yield return null;
        }

        currentScreen.OnHidden();
        if (previousScreen != null)
        {
            previousScreen.OnShown();
        }
    }

    private void UpdateScreenPosition(NavScreen screen, float from, float to, float progress) {
        float easedProgress = Mathf.SmoothStep(from, to, progress);
        Vector2 newAnchorMin = new Vector2(easedProgress, 0f);
        Vector2 newAnchorMax = new Vector2(1f + easedProgress, 1f);
        screen.RectTransform.anchorMin = newAnchorMin;
        screen.RectTransform.anchorMax = newAnchorMax;
    }

    private Vector2 GetAnchoredPosition(float startValue, float endValue, float progress)
    {
        float x = Mathf.SmoothStep(startValue, endValue, progress);
        return new Vector2(x, 0f);
    }
}
