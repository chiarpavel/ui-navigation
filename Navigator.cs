using System.Collections.Generic;
using UnityEngine;

namespace UINavigation {
    public class Navigator : MonoBehaviour
    {
        public NavScreen initialScreen;
        public TransitionBase transition;

        public string Path
        {
            get
            {
                return GetPath();
            }
        }

        private List<NavScreen> screens = new List<NavScreen>();
        private bool initialized = false;

        private NavScreen CurrentScreen
        {
            get
            {
                return screens.Count > 0 ? screens[screens.Count - 1] : null;
            }
        }

        void Start()
        {
            if (!initialized)
            {
                Initialize();
            }
        }

        void Update()
        {
            // handle Android back key
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GoBack();
            }
        }

        private void Initialize()
        {
            if (initialScreen != null)
            {
                screens.Add(initialScreen);
            }

            initialized = true;
        }

        public void GoTo(NavScreen targetScreen)
        {
            if (!initialized)
            {
                Initialize();
            }

            if (transition == null)
            {
                if (CurrentScreen != null)
                {
                    CurrentScreen.OnHiding();
                    CurrentScreen.OnHidden();
                }
                targetScreen.OnShowing();
                targetScreen.OnShown();
            }
            else
            {
                StartCoroutine(transition.Play(CurrentScreen, targetScreen));
            }

            screens.Add(targetScreen);
        }

        public void GoBack() {
            if (!initialized)
            {
                Initialize();
            }

            if (screens.Count == 0)
            {
                return;
            }

            NavScreen previousScreen = screens.Count >= 2 ? screens[screens.Count - 2] : null;

            if (transition == null)
            {
                CurrentScreen.OnHiding();
                CurrentScreen.OnHidden();
                if (previousScreen != null)
                {
                    previousScreen.OnShowing();
                    previousScreen.OnShown();
                }
            }
            else
            {
                StartCoroutine(transition.PlayReverse(CurrentScreen, previousScreen));
            }

            screens.RemoveAt(screens.Count - 1);
        }

        private string GetPath() {
            if (screens.Count == 0)
            {
                return "/";
            }

            string path = "";

            foreach (var screen in screens)
            {
                path += "/" + screen.name;
            }

            return path;
        }
    }
}
