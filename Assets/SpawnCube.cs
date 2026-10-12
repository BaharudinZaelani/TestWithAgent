using UnityEngine;

public class SpawnCube : MonoBehaviour
{
    public Vector3 position = Vector3.zero;
    public Vector3 scale = Vector3.one;
    void Start()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;
        cube.transform.localScale = scale;
    }
}
