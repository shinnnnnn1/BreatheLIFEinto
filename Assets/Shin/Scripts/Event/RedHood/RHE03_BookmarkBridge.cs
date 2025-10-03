using UnityEngine;
using DG.Tweening;

public class RHE03_BookmarkBridge : MonoBehaviour
{
    [SerializeField] Transform newBookmark, newBridge;
    [SerializeField] float createTime;


    //dhkswjsgl durldp ensmsrjsrk dkslaus wjscjfja rkRkdlaks goeh ehlsk
    public void CreateBridge()
    {
        Vector3 pos = newBridge.position;
        Vector3 rot = newBridge.localEulerAngles;

        //newBookmark.DORotate(rot, createTime);
    }
    

    void ChangeBridge()
    {
        newBookmark.gameObject.SetActive(false);
        newBridge.gameObject.SetActive(true);
    }

    public void RedhoodCrossTheBridge(bool startMoving)
    {

    }
}
