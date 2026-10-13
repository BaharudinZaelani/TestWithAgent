using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneSetupUtility
{
    [MenuItem("Tools/Setup Test Scene")]
    public static void SetupScene()
    {
        // 1. Create Floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Floor";
        floor.transform.position = Vector3.zero;
        floor.transform.localScale = new Vector3(2, 1, 2);

        // 2. Create Player Cube
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Cube);
        player.name = "PlayerCube";
        player.transform.position = new Vector3(0, 2, 0);

        // Add Rigidbody & Movement
        Rigidbody rb = player.AddComponent<Rigidbody>();
        rb.mass = 1f;
        player.AddComponent<PlayerMovement>();

        // Save Scene
        string scenePath = "Assets/Scenes/TestScene.unity";
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        
        // Mark scene dirty so Unity saves changes directly to disk in editor
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), scenePath);
        
        // Register created objects into the undo stack / scene persistence
        Undo.RegisterCreatedObjectUndo(floor, "Create Floor");
        Undo.RegisterCreatedObjectUndo(player, "Create Player");

        Debug.Log("Scene TestScene created & saved at " + scenePath);
    }
}
