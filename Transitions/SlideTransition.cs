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

            nextScreen.RectTransform.anchoredPosition = GetAnchoredPosition(Screen.width, 0f, progress);
            if (currentScreen != null)
            {
                currentScreen.RectTransform.anchoredPosition = GetAnchoredPosition(0f, -Screen.width / 3, progress);
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

            currentScreen.RectTransform.anchoredPosition = GetAnchoredPosition(0f, Screen.width, progress);
            if (previousScreen != null)
            {
                previousScreen.RectTransform.anchoredPosition = GetAnchoredPosition(-Screen.width / 3, 0f, progress);
            }

            yield return null;
        }

        currentScreen.OnHidden();
        if (previousScreen != null)
        {
            previousScreen.OnShown();
        }
    }

    private Vector2 GetAnchoredPosition(float startValue, float endValue, float progress)
    {
        float x = Mathf.SmoothStep(startValue, endValue, progress);
        return new Vector2(x, 0f);
    }
}
