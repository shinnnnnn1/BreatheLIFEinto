using UnityEngine;

[CreateAssetMenu(fileName = "BookTexture_", menuName = "Scriptable Objects/BookTexture")]
public class BookTexture : ScriptableObject
{
    public Material[] leftPageMat;
    public Material[] rightPageMat;
}
