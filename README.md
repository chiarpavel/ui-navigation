# ui-navigation

UI navigation for Unity

version 0.1.0

[Download unitypackage](https://github.com/chiarpavel/ui-navigation/raw/master/ui-navigation.unitypackage)

## How to use

### Setup

1. Add a __Navigator__ component to a GameObject. It doesn't matter where, just make sure it's enabled.
2. Add __NavScreen__ components to all the menu screens you want to be able to navigate through.
3. (_optional_) Add handlers to the NavScreen's events to set up or clean up the screens, useful if a screen's content depends on external state, such as a search results page.
4. (_optional_) Add one of the NavScreens to your Navigator's _Initial Screen_ field if you have a start-up screen
5. (_optional_) Add a transition to your Navigator's _Transition_ field. There is a SlideTransition included, or you can create your own by extending TransitionBase.

### Navigation

Simply call _Navigator.GoTo_. This will navigate to the NavScreen passed as a parameter and add it to the navigation history. Call _Navigator.GoBack_ from your Back buttons to return to the last NavScreen and remove the current one from the navigation history.

## How to create a transition

You should extend TransitionBase and implement the _Play_ and _PlayReverse_ coroutines.

_Play_ is called when moving "forward" or "deeper" into a menu, and _PlayReverse_ when going "up" or "back" to a previously visited menu.

Make sure to call NavScreen's _OnEvent_ methods (_OnShowing_, _OnShown_, _OnHiding_, _OnHidden_) at the appropriate time if you plan on using NavScreen's events.

Because TransitionBase is a ScriptableObject you need to create an asset. Add a CreateAssetMenuAttribute to be able to create one using the editor.

See SlideTransition.cs for an example.
