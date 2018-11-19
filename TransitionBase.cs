using System;
using System.Collections;
using UnityEngine;

namespace UINavigation
{
    public abstract class TransitionBase : ScriptableObject
    {
        public abstract IEnumerator Play(NavScreen currentScreen, NavScreen nextScreen);
        public abstract IEnumerator PlayReverse(NavScreen currentScreen, NavScreen previousScreen);
    }
}
