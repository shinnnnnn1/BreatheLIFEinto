using UnityEngine;

[CreateAssetMenu(fileName = "VirtualMouseModel", menuName = "Scriptable Objects/VirtualMouseModel")]
public class VirtualMouseModel : ScriptableObject
{
    public bool isCursorMode;

    public float cursorSpeed = 1000f;

    public float cursorPadding = 30f;

    public float interactingDistance = 20f;
    public LayerMask interactableLayerMask;
    public LayerMask ignoreLayerMask;
}
