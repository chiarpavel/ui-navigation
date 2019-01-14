using System.Collections.Generic;
using UnityEngine;

namespace UINavigation
{
    public class Navigator : MonoBehaviour
    {
        /// <summary>
        /// This screen is added to the Navigator's stack on startup.
        /// </summary>
        public NavScreen initialScreen;

        /// <summary>
        /// The transition's Play and PlayReverse methods are called when going from one screen to another.
        /// </summary>
        public TransitionBase transition;

        /// <summary>
        /// If set to false calling Navigator.GoBack when there is only one screen left in the stack will do nothing.
        /// </summary>
        [Tooltip("GoBack works when there is only one screen left in Path")]
        public bool emptyPathAllowed = true;

        /// <summary>
        /// If set to true pressing the Android back button, or the keyboard Escape key will call Navigator.GoBack.
        /// </summary>
        [Tooltip("Handle Android back button and keyboard Esc key")]
        public bool escapeKeyHandled = true;

        /// <summary>
        /// A string representation of the list of NavScreens the Navigator has in the current stack.
        /// </summary>
        /// <value></value>
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
            if (escapeKeyHandled && Input.GetKeyDown(KeyCode.Escape))
            {
                GoBack();
            }
        }

        private void Initialize()
        {
            if (initialScreen != null)
            {
                screens.Add(initialScreen);
                initialScreen.OnShowing();
                initialScreen.OnShown();
            }

            initialized = true;
        }

        /// <summary>
        /// Adds _targetScreen_ to the stack and uses the set transition (if there is one, otherwise simply raises the apropriate NavScreen events).
        /// </summary>
        /// <param name="targetScreen"></param>
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

        /// <summary>
        /// Removes the NavScreen at the top of the stack using the set transition.
        /// </summary>
        public void GoBack()
        {
            if (!initialized)
            {
                Initialize();
            }

            if (screens.Count == 0)
            {
                return;
            }

            if (!emptyPathAllowed && screens.Count == 1)
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

        private string GetPath()
        {
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
