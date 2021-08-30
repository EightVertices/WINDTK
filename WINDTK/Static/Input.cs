using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WINDXN
{
    public static class Input
    {
        public static Engine Engine;

        // Input variables
        private static Keys[] PreviousKeys = new Keys[0];
        private static Keys[] CurrentKeys = new Keys[0];

        private static Dictionary<string, bool> PreviousMouseButtons = new Dictionary<string, bool>()
        {
            { "Left", false },
            { "Middle", false },
            { "Right", false }
        };
        private static Dictionary<string, bool> CurrentMouseButtons = new Dictionary<string, bool>()
        {
            { "Left", false },
            { "Middle", false },
            { "Right", false }
        };


        public static void Initialize(Engine engine)
        {
            Engine = engine;
        }

        public static void Update()
        {
            // Keyboard
            if (PreviousKeys != CurrentKeys) { PreviousKeys = CurrentKeys; }

            CurrentKeys = Keyboard.GetState().GetPressedKeys();

            // Mouse
            if (PreviousMouseButtons["Left"] != CurrentMouseButtons["Left"]) { PreviousMouseButtons["Left"] = CurrentMouseButtons["Left"]; }
            if (PreviousMouseButtons["Middle"] != CurrentMouseButtons["Middle"]) { PreviousMouseButtons["Middle"] = CurrentMouseButtons["Middle"]; }
            if (PreviousMouseButtons["Right"] != CurrentMouseButtons["Right"]) { PreviousMouseButtons["Right"] = CurrentMouseButtons["Right"]; }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed) { CurrentMouseButtons["Left"] = true; }
            else { CurrentMouseButtons["Left"] = false; }
            if (Mouse.GetState().MiddleButton == ButtonState.Pressed) { CurrentMouseButtons["Middle"] = true; }
            else { CurrentMouseButtons["Middle"] = false; }
            if (Mouse.GetState().RightButton == ButtonState.Pressed) { CurrentMouseButtons["Right"] = true; }
            else { CurrentMouseButtons["Right"] = false; }
        }

        // Utility functions

        // Keyboard
        public static bool IsKeyPressed(string key)
        {
            return Keyboard.GetState().IsKeyDown(Enum.Parse<Keys>(key));
        }

        public static bool IsKeyJustPressed(string key)
        {
            bool ReturnValue = false;

            // Checking for keys
            bool HasKeyInPreviousKeys = false;
            for (int i = 0; i < PreviousKeys.Length; i++)
            {
                if (PreviousKeys[i] == Enum.Parse<Keys>(key)) { HasKeyInPreviousKeys = true; }
            }

            bool HasKeyInCurrentKeys = false;
            for (int i = 0; i < CurrentKeys.Length; i++)
            {
                if (CurrentKeys[i] == Enum.Parse<Keys>(key)) { HasKeyInCurrentKeys = true; }
            }

            if (HasKeyInCurrentKeys & !HasKeyInPreviousKeys) { ReturnValue = true; }

            return ReturnValue;
        }

        // Mouse
        public static bool IsMouseButtonPressed(string button)
        {
            return CurrentMouseButtons[button];
        }

        public static bool IsMouseButtonJustPressed(string button)
        {
            return (CurrentMouseButtons[button] & !PreviousMouseButtons[button]);
        }
    }
}
