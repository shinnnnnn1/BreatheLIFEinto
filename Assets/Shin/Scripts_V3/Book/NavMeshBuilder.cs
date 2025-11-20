using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshBuilder : MonoBehaviour
{
    NavMeshSurface surface;

    void Start()
    {
        surface = GetComponent<NavMeshSurface>();
    }

    public void _RebuildSurface()
    {
        surface.BuildNavMesh();
    }
}
