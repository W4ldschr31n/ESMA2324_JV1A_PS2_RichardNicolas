using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StartSceneWindow : EditorWindow // This window is used to force the play mode to start from a chosen scene
{
    private SceneAsset selectedStartScene;
    private int selectedStartButton = 0;
    void OnGUI()
    {
        string[] opts = { "Starting scene", "Current scene" };
        // Use the Object Picker to select the start SceneAsset
        selectedStartScene = (SceneAsset)EditorGUILayout.ObjectField(new GUIContent("Start Scene"), selectedStartScene, typeof(SceneAsset), false);
        selectedStartButton = GUILayout.SelectionGrid(selectedStartButton, opts, 2, EditorStyles.radioButton);
        if (selectedStartButton == 0)
        {
            EditorSceneManager.playModeStartScene = selectedStartScene;
        }
        else
        {
            EditorSceneManager.playModeStartScene = null;
        }


    }

    [MenuItem("Window/StartScene")]
    static void Open()
    {
        GetWindow<StartSceneWindow>();
    }
}