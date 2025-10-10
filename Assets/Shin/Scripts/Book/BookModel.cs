using Unity.Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "VirtualMouseModel", menuName = "Scriptable Objects/BookModel")]
public class BookModel : ScriptableObject
{
    //배열에 헤더 붙이는법 찾기
    [Header("Curve Settings")]
    [Tooltip("ActTime, DeactTime, ActDelay, DeactDelay")]
    public AnimationCurve[] curveHeight = new AnimationCurve[4];
    public AnimationCurve[] curveShape = new AnimationCurve[4];
    public AnimationCurve[] curvePlane = new AnimationCurve[4];

    //==
    [Header("Page Material Settings")]
    [Space(10f)]
    public Material[] pageMaterialsL;
    public Material[] pageMaterialsR;

    [Header("BookSettings")]
    public float rotValue = 30.0f;
    public float rotTime = 1.0f;

    [Header("Development Settings")]
    [Range(0, 20)] public int setStartPage = 0;
}
