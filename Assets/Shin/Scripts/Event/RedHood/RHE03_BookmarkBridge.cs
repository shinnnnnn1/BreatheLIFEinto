using UnityEngine;
using DG.Tweening;

public class RHE03_BookmarkBridge : MonoBehaviour
{
    [SerializeField] Collider bookmark;
    [SerializeField] Transform newBridge;
    [SerializeField] float createTime;

    public void CreateBridge()
    {
        Vector3 pos = newBridge.position;
        Vector3 rot = newBridge.localEulerAngles;

        bookmark.transform.DOMove(pos, createTime);
        bookmark.transform.DORotate(rot, createTime).OnComplete(ChangeBridge);
    }
    

    void ChangeBridge()
    {
        bookmark.gameObject.SetActive(false);
        newBridge.gameObject.SetActive(true);
    }

    public void RedhoodCrossTheBridge(bool startMoving)
    {

    }
}
