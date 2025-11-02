using UnityEngine;

[CreateAssetMenu(fileName = "BookModel_V3", menuName = "Scriptable Objects/BookModel_V3")]
public class BookModel_V3 : ScriptableObject
{
    [Header("Development Settings")]
    [Range(1, 10)] public int setStartPage = 0;

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

    public Vector2[] flipDelay;

}
