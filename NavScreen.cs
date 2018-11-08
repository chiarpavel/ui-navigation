using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UINavigation
{
    public class NavScreen : MonoBehaviour
    {
        private RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = GetComponent<RectTransform>();
                }
                return rectTransform;
            }
        }

        /// <summary>
        /// Event invoked when the screen is coming into view and the transition is starting.
        /// </summary>
        public UnityEvent Showing;

        /// <summary>
        /// Event invoked when the screen has come into view and the transition has finished.
        /// </summary>
        public UnityEvent Shown;

        /// <summary>
        /// Event invoked when the screen is leaving view and the transition is starting.
        /// </summary>
        public UnityEvent Hiding;

        /// <summary>
        /// Event invoked when the screen has left view and the transition has finished.
        /// </summary>
        public UnityEvent Hidden;

        /// <summary>
        /// Called at the end of a transition when the screen has come into view
        /// </summary>
        public void OnShown()
        {
            Shown.Invoke();
        }

        /// <summary>
        /// Called at the end of a transition when the screen has left view
        /// </summary>
        public void OnHidden()
        {
            Hidden.Invoke();
        }

        /// <summary>
        /// Called at the start of a transition when the screen is coming into view
        /// </summary>
        public void OnShowing()
        {
            Showing.Invoke();
        }

        /// <summary>
        /// Called at the start of a transition when the screen is leaving view
        /// </summary>
        public void OnHiding()
        {
            Hiding.Invoke();
        }
    }
}
