using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class CreateGame {
    [MenuItem("CELL/Create Main Scene")]
    public static void Generate() {
        Directory.CreateDirectory("Assets/Scenes");
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var camera = new GameObject("Main Camera").AddComponent<Camera>();
        camera.tag = "MainCamera";
        camera.orthographic = true;
        camera.orthographicSize = 9f;
        camera.backgroundColor = new Color(0.06f,0.09f,0.14f);
        camera.transform.position = new Vector3(0,0,-10);
        var root = new GameObject("CELL Game");
        root.AddComponent<CellGame>();
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/Main.unity");
        AssetDatabase.SaveAssets();
        Debug.Log("CELL main scene generated.");
    }
}
