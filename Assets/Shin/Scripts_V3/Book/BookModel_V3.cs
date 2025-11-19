using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "BookModel_V3", menuName = "Scriptable Objects/BookModel_V3")]
public class BookModel_V3 : ScriptableObject
{
    [Header("Development Settings")]
    [Range(1, 10)] public int setStartPage = 0;

    [Header("Curve Settings")]
    [Tooltip("ActTime, DeactTime, ActDelay, DeactDelay")]
    public AnimationCurve[] curveHeight = new AnimationCurve[4];
    public Ease[] easeHeight = new Ease[4];
    public AnimationCurve[] curveShape = new AnimationCurve[4];
    public Ease[] easeShape = new Ease[4];
    public AnimationCurve[] curvePlane = new AnimationCurve[4];
    public Ease[] easePlane = new Ease[4];

    public float distortionValue;
    public float distortionTime;
    public Ease easeDistortion;

    [Header("Page Material Settings")]
    public Material[] pageMaterialsL;
    public Material[] pageMaterialsR;

    [Header("BookSettings")]
    public float rotValue = 45.0f;
    public float rotTime = 1.0f;
}
