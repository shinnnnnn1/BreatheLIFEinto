using UnityEngine;
using DG.Tweening;

public class RHE03_BookmarkBridge : MonoBehaviour
{
    [SerializeField] Collider bookmark;
    [SerializeField] Transform newBridge;
    [SerializeField] Transform coll;
    [SerializeField] Vector3 rot;
    [SerializeField] float createTime;

    public void CreateBridge()
    {
        Vector3 pos = newBridge.position;

        bookmark.transform.DOMove(pos, createTime);
        bookmark.transform.DOLocalRotate(rot, createTime).OnComplete(ChangeBridge);
    }

    void ChangeBridge()
    {
        bookmark.gameObject.SetActive(false);
        coll.gameObject.SetActive(false);
        newBridge.gameObject.SetActive(true);
    }

    public void RedhoodCrossTheBridge(bool startMoving)
    {

    }
}
