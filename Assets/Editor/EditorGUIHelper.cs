using UnityEngine;

public class EditorGUIHelper
{
    /// <summary>
    /// Draws a button in the Unity Editor and returns whether the user has clicked it.
    /// </summary>
    /// <param name="label">The text displayed on the button.</param>
    /// <param name="height">Optional. The height of the button (default is 25).</param>
    /// <param name="width">Optional. The width of the button (default is 100).</param>
    /// <param name="isActive">
    /// Optional. Highlights the button (e.g., cyan background) when true, typically used to show toggle state.
    /// </param>
    /// <returns>True if the button was clicked; otherwise, false.</returns>
    public static bool ClickButton(string label, int height = 25, int width = 100, bool isActive = false)
    {
        Color originalColor = GUI.backgroundColor;

        if (isActive)
            GUI.backgroundColor = Color.cyan;

        bool clicked = GUILayout.Button(label, GUILayout.Height(height), GUILayout.Width(width));

        GUI.backgroundColor = originalColor;
        return clicked;
    }

    /// <summary>
    /// Draws a remove button and and returns whether the user has clicked it.
    /// </summary>
    /// <returns>True if the button was clicked; otherwise, false.</returns>
    public static bool ClickRemoveButton()
    {
        GUIStyle removeStyle = new GUIStyle(GUI.skin.button);
        removeStyle.normal.textColor = Color.red;

        bool clicked = GUILayout.Button("X", removeStyle, GUILayout.Width(20));

        return clicked;
    }

    /// <summary>
    /// Calculates the needed button width from a label
    /// </summary>
    /// <param name="label">The text displayed on the button.</param>
    /// <returns>True if the button was clicked; otherwise, false.</returns>
    public static int GetButtonWidthFromString(string label)
    {
        int buttonWidth = (int)GUI.skin.button.CalcSize(new GUIContent(label)).x;

        return buttonWidth;
    }

    /// <summary>
    /// Draws a toggle-style button in the Unity Editor that updates a referenced enum value when clicked.
    /// </summary>
    /// <typeparam name="T">An enum type used to determine and update toggle state.</typeparam>
    /// <param name="label">The text displayed on the button.</param>
    /// <param name="type">The enum value associated with this button.</param>
    /// <param name="currentlySelected">
    /// A reference to the currently selected enum value. 
    /// Will be set to <c>default</c> if this button is clicked while active; 
    /// otherwise, will be set to <paramref name="type"/>.
    /// </param>
    /// <param name="height">Optional. Height of the button (default is 25).</param>
    /// <param name="width">Optional. Width of the button (default is 100).</param>
    public static void DrawToggleButton<T>(string label, T type, ref T currentSelected, int height = 25, int width = 100) where T : struct, System.Enum
    {
        bool isActive = currentSelected.Equals(type);

        if (ClickButton(label, height, width, isActive))
        {
            if (isActive)
            {
                currentSelected = default;
            }
            else
            {
                currentSelected = type;
            }
        }
    }
}