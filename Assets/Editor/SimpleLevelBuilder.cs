using UnityEditor;
using UnityEngine;

public class SimpleLevelBuilder
{
    [MenuItem("Tools/Build Simple Level")]
    static void BuildLevel()
    {
        // 1. Ground: Cube tipis dan lebar
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        ground.transform.position = new Vector3(0, -0.5f, 0);
        ground.transform.localScale = new Vector3(10f, 1f, 10f);
        Undo.RegisterCreatedObjectUndo(ground, "Create Ground");

        // 2. Player: Capsule
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.transform.position = new Vector3(0, 1.5f, 0);
        
        // Tambah Rigidbody agar jatuh ke ground
        player.AddComponent<Rigidbody>();
        Undo.RegisterCreatedObjectUndo(player, "Create Player");

        Selection.activeGameObject = player;
        Debug.Log("Ground (Cube) & Player (Capsule) spawned.");
    }
}
