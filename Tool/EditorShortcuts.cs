#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class EditorShortcuts : Editor
{
    [MenuItem("Shortcuts/LogoScene #F1")]
    public static void LogoScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/LogoScene.unity");
    }

    [MenuItem("Shortcuts/LoginScene #F2")]
    public static void LoginScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/LoginScene.unity");
    }

    [MenuItem("Shortcuts/MainScene #F3")]
    public static void MainScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainScene.unity");
    }

    [MenuItem("Shortcuts/LoadingScene #F4")]
    public static void LoadingScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/LoadingScene.unity");
    }

    [MenuItem("Shortcuts/GameScene #F5")]
    public static void GameScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/GameScene.unity");
    }

    [MenuItem("Shortcuts/Active Object %t")]
    public static void ActiveObject()
    {
        foreach(var go in Selection.gameObjects)
        {
            go.SetActive(!go.activeSelf);
            EditorUtility.SetDirty(go);
        }
    }
}
#endif