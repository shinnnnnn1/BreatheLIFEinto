using UnityEngine;

public class HGE02_BookTexture : MonoBehaviour
{
    [SerializeField] BookTexture model;
    [SerializeField] int change;
    [SerializeField] Material beforeMatL, beforeMatR, afterMatL, afterMatR;

    private void OnDestroy()
    {
        model.leftPageMat[change] = beforeMatL;
        model.rightPageMat[change] = beforeMatR;
    }

    public void _ChangeMat()
    {
        model.leftPageMat[change] = afterMatL;
        model.rightPageMat[change] = afterMatR;
    }
}
