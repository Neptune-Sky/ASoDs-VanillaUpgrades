using System;
using System.Globalization;
using SFS.UI.ModGUI;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Type = SFS.UI.ModGUI.Type;

namespace VanillaUpgrades
{
    public static class GUI_Utility
    {
        public static TextInput CreateNumberInput(GameObject parent, int width, int height, float changeStep,
            string title, float titleSizeMultiplier, float currentValue, Action applyChanges,
            float titleHeightMultiplier = 1)
        {
            int labelWidth = Mathf.RoundToInt((width - 20) * titleSizeMultiplier);
            int buttonWidth = Mathf.RoundToInt(height * 0.7f);
            int buttonHeight = Mathf.RoundToInt(height * 0.6f);
            int inputWidth = width - labelWidth - buttonWidth * 2 - 50;

            GameObject tempHolder = new GameObject();

            TextInput input = Builder.CreateTextInput(tempHolder, inputWidth, height, 0, 0, currentValue.ToString("G",
                CultureInfo.InvariantCulture));

            Container container = Builder.CreateContainer(parent, 0, 0);
            container.CreateLayoutGroup(Type.Horizontal).spacing = 5f;
            container.CreateLayoutGroup(Type.Horizontal).DisableChildControl();
            container.CreateLayoutGroup(Type.Horizontal).childAlignment = TextAnchor.MiddleCenter;

            Builder.CreateLabel(container.gameObject, labelWidth,
                Mathf.RoundToInt(height * 0.8f * titleHeightMultiplier), 0, 0, title);

            void OnClick(float m)
            {
                input.ChangeAsNumber(changeStep * m);
                applyChanges.Invoke();
            }

            Builder.CreateButton(container.gameObject, buttonWidth, buttonHeight, 0, 0,
                () => OnClick(-1), "<");
            input.rectTransform.SetParent(container.rectTransform);
            Builder.CreateButton(container.gameObject, buttonWidth, buttonHeight, 0, 0,
                () => OnClick(1), ">");
            Object.Destroy(tempHolder);

            return input;
        }

        public static TextInput CreateStringInput(GameObject parent, int width, int height,
            string title, float titleSizeMultiplier, string currentValue, float titleHeightMultiplier = 1)
        {
            int labelWidth = Mathf.RoundToInt((width - 20) * titleSizeMultiplier);
            int inputWidth = width - labelWidth - 30;
            Container container = Builder.CreateContainer(parent, 0, 0);
            container.CreateLayoutGroup(Type.Horizontal).spacing = 5f;
            container.CreateLayoutGroup(Type.Horizontal).DisableChildControl();
            container.CreateLayoutGroup(Type.Horizontal).childAlignment = TextAnchor.MiddleCenter;

            Builder.CreateLabel(container.gameObject, labelWidth,
                Mathf.RoundToInt(height * 0.8f * titleHeightMultiplier), 0, 0, title);
            TextInput input = Builder.CreateTextInput(container.gameObject, inputWidth, height, 0, 0, currentValue);

            return input;
        }

        public static void CreateBoolInput(GameObject parent, int width, int height,
            string title, float titleSizeMultiplier, Func<bool> get, Action onChange, float titleHeightMultiplier = 1)
        {
            const int toggleWidth = 80;
            int labelWidth = Mathf.RoundToInt((width - 20) * titleSizeMultiplier);
            int space = width - 20 - toggleWidth - labelWidth;

            Container container = Builder.CreateContainer(parent, 0, 0);
            container.CreateLayoutGroup(Type.Horizontal).spacing = space;
            container.CreateLayoutGroup(Type.Horizontal).DisableChildControl();
            container.CreateLayoutGroup(Type.Horizontal).childAlignment = TextAnchor.MiddleCenter;

            Builder.CreateLabel(container.gameObject, labelWidth,
                Mathf.RoundToInt(height * 0.8f * titleHeightMultiplier), 0, 0, title);

            Builder.CreateToggle(container.gameObject, 0, 0, get, onChange);
        }

        static void ChangeAsNumber(this TextInput input, float change)
        {
            input.Text =
                (float.Parse(input.Text, NumberStyles.Any, CultureInfo.InvariantCulture) + change).ToString("G",
                    CultureInfo.InvariantCulture);
        }

        public static Box CustomBox(int width, string label, GameObject parent)
        {
            Box box = Builder.CreateBox(parent, width, 10);
            
            // Auto resize for box
            box.gameObject.AddComponent<ContentSizeFitter>().verticalFit =
                ContentSizeFitter.FitMode.PreferredSize;

            box.CreateLayoutGroup(Type.Vertical).spacing = 10f;
            box.CreateLayoutGroup(Type.Vertical).DisableChildControl();
            box.CreateLayoutGroup(Type.Vertical).childAlignment = TextAnchor.MiddleCenter;
            box.CreateLayoutGroup(Type.Vertical).padding = new RectOffset(0, 0, 5, 5);
            Builder.CreateLabel(box.gameObject, width, 35, 0, 0, label);

            return box;
        }
    }
}