using UnityEditor;
using UnityEngine;

public class QuickCubePlacer
{
    [MenuItem("Tools/Spawn Test Cube")]
    static void SpawnCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "MyTestCube";
        cube.transform.position = Vector3.zero;
        Undo.RegisterCreatedObjectUndo(cube, "Spawn Test Cube");
        Selection.activeGameObject = cube;
        Debug.Log("Cube spawned in scene!");
    }
}
