using System;
using UnityEngine.UI;

////TODO: have updateBindingUIEvent receive a control path string, too (in addition to the device layout name)

namespace UnityEngine.InputSystem.Samples.RebindUI
{
    /// <summary>
    /// This is an example for how to override the default display behavior of bindings. The component
    /// hooks into <see cref="RebindActionUI.updateBindingUIEvent"/> which is triggered when UI display
    /// of a binding should be refreshed. It then checks whether we have an icon for the current binding
    /// and if so, replaces the default text display with an icon.
    /// </summary>
    public class GamepadIconsExample : MonoBehaviour
    {
        public GamepadIcons xbox;
        public KeyboardIcons keyboard;

        protected void OnEnable()
        {
            // Hook into all updateBindingUIEvents on all RebindActionUI components in our hierarchy.
            var rebindUIComponents = transform.GetComponentsInChildren<RebindActionUI>();
            foreach (var component in rebindUIComponents)
            {
                component.updateBindingUIEvent.AddListener(OnUpdateBindingDisplay);
                component.UpdateBindingDisplay();
            }
        }

        protected void OnUpdateBindingDisplay(RebindActionUI component, string bindingDisplayString, string deviceLayoutName, string controlPath)
        {
            if (string.IsNullOrEmpty(deviceLayoutName) || string.IsNullOrEmpty(controlPath))
                return;

            var icon = default(Sprite);
            if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Keyboard"))
                icon = keyboard.GetSprite(controlPath);
            else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Gamepad"))
                icon = xbox.GetSprite(controlPath);
            var textComponent = component.bindingText;

            // Grab Image component.
            var imageGO = textComponent.transform.parent.Find("ActionBindingIcon");
            var imageComponent = imageGO.GetComponent<Image>();

            if (icon != null)
            {
                textComponent.gameObject.SetActive(false);
                imageComponent.sprite = icon;
                imageComponent.gameObject.SetActive(true);
            }
            else
            {
                textComponent.gameObject.SetActive(true);
                imageComponent.gameObject.SetActive(false);
            }
        }

        [Serializable]
        public struct GamepadIcons
        {
            public Sprite buttonSouth;
            public Sprite buttonNorth;
            public Sprite buttonEast;
            public Sprite buttonWest;
            public Sprite startButton;
            public Sprite selectButton;
            public Sprite leftTrigger;
            public Sprite rightTrigger;
            public Sprite leftShoulder;
            public Sprite rightShoulder;
            public Sprite dpad;
            public Sprite dpadUp;
            public Sprite dpadDown;
            public Sprite dpadLeft;
            public Sprite dpadRight;
            public Sprite leftStick;
            public Sprite rightStick;
            public Sprite leftStickPress;
            public Sprite rightStickPress;

            public Sprite GetSprite(string controlPath)
            {
                // From the input system, we get the path of the control on device. So we can just
                // map from that to the sprites we have for gamepads.
                switch (controlPath)
                {
                    case "buttonSouth": return buttonSouth;
                    case "buttonNorth": return buttonNorth;
                    case "buttonEast": return buttonEast;
                    case "buttonWest": return buttonWest;
                    case "start": return startButton;
                    case "select": return selectButton;
                    case "leftTrigger": return leftTrigger;
                    case "rightTrigger": return rightTrigger;
                    case "leftShoulder": return leftShoulder;
                    case "rightShoulder": return rightShoulder;
                    case "dpad": return dpad;
                    case "dpad/up": return dpadUp;
                    case "dpad/down": return dpadDown;
                    case "dpad/left": return dpadLeft;
                    case "dpad/right": return dpadRight;
                    case "leftStick": return leftStick;
                    case "rightStick": return rightStick;
                    case "leftStickPress": return leftStickPress;
                    case "rightStickPress": return rightStickPress;
                }
                return null;
            }
        }

        [Serializable]
        public struct KeyboardIcons
        {
            public Sprite a;
            public Sprite b;
            public Sprite c;
            public Sprite d;
            public Sprite e;
            public Sprite f;
            public Sprite g;
            public Sprite h;
            public Sprite i;
            public Sprite j;
            public Sprite k;
            public Sprite l;
            public Sprite m;
            public Sprite n;
            public Sprite o;
            public Sprite p;
            public Sprite q;
            public Sprite r;
            public Sprite s;
            public Sprite t;
            public Sprite u;
            public Sprite v;
            public Sprite w;
            public Sprite x;
            public Sprite y;
            public Sprite z;
            public Sprite left;
            public Sprite right;
            public Sprite up;
            public Sprite down;
            public Sprite backspace;
            public Sprite space;
            public Sprite enter;
            public Sprite escape;

            public Sprite GetSprite(string controlPath)
            {
                // From the input system, we get the path of the control on device. So we can just
                // map from that to the sprites we have for gamepads.
                switch (controlPath)
                {
                    case "a": return a;
                    case "b": return b;
                    case "c": return c;
                    case "d": return d;
                    case "e": return e;
                    case "f": return f;
                    case "g": return h;
                    case "h": return h;
                    case "i": return i;
                    case "j": return j;
                    case "k": return k;
                    case "l": return l;
                    case "m": return m;
                    case "n": return n;
                    case "o": return o;
                    case "p": return p;
                    case "q": return q;
                    case "r": return r;
                    case "s": return s;
                    case "t": return t;
                    case "u": return u;
                    case "v": return v;
                    case "w": return w;
                    case "x": return x;
                    case "y": return y;
                    case "z": return z;
                    case "backspace": return backspace;
                    case "space": return space;
                    case "enter": return enter;
                    case "escape": return escape;
                }
                return null;
            }
        }
    }
}
