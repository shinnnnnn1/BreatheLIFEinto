using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Book : MonoBehaviour
{
    public Transform[] leftBones, rightBones, currentBones, objectParents;
    public Transform objectParent;
    public Vector2[] adjustmentY, adjustmentMorph;
    public Transform[] morphs;
    [SerializeField] Animator[] animPage;
    [SerializeField] Animation[] animBook;

    [Header("Book")]
    [SerializeField] int currentPage;
    public bool isFlipping;
    [SerializeField] StaticObject[] BookObjects;
    //[SerializeField] NPC[] npcs;

    float flipTime;

    void Awake()
    {
        BookObjects = objectParents[0].GetComponentsInChildren<StaticObject>();

    }

    void Start()
    {
        StartCoroutine(SetStart());
    }

    IEnumerator SetStart()
    {
        yield return new WaitForSeconds(0.3f);
        animPage[1].speed = 20f;
        animPage[1].SetTrigger("Flip");
        yield return new WaitForSeconds(0.4f);
        animPage[1].SetTrigger("FlipReverse");
        yield return new WaitForSeconds(0.3f);
        animPage[1].speed = 1f;
        Debug.Log("Can Start");
    }

    void Update()
    {
        if(isFlipping)
        {
            //Debug.Log(Time.time - flipTime);
        }
    }

    public void Flip(bool isNext)
    {
        if(isFlipping) { return; }

        
        flipTime = Time.time;
        currentPage = isNext ? currentPage + 1 : currentPage - 1;

        for (int i = 0; i < 10; i++)
        {
            morphs[i].transform.position = new Vector3(morphs[i].transform.position.x, 0, morphs[i].transform.position.z);
        }

        StartCoroutine(FlipC(isNext));

        
    }


    void StartMorph()
    {
        for (int i = 0; i < 10; i++)
        {
            morphs[i].DOLocalMoveY(1, adjustmentMorph[i].y).SetDelay(adjustmentMorph[i].x).SetEase(Ease.OutQuad);
        }
    }

    void EndFlip()
    {
        Debug.Log("End" + currentPage);
        isFlipping = false;
    }

    IEnumerator FlipC(bool isNext)
    {
        animPage[0].SetTrigger(isNext ? "StopR" : "StopL");

        yield return null;
        isFlipping = true;

        foreach (StaticObject obj in BookObjects)
        {
            if ((obj.stage == currentPage))
            {
                if ((isNext && obj.isRight) || (!isNext && !obj.isRight))
                {   //SA
                    obj.SetObjext(true, true, true);
                }
                else
                {   //DA
                    obj.SetObjext(false, true, true);
                }
            }
            else if ((isNext && obj.isRight && obj.stage == currentPage - 1)
                || (!isNext && !obj.isRight && obj.stage == currentPage + 1))
            {   //DD
                obj.SetObjext(false, false, true);
            }
            else if ((isNext && !obj.isRight && obj.stage == currentPage - 1)
                || (!isNext && obj.isRight && obj.stage == currentPage + 1))
            {   //SD
                obj.SetObjext(true, false, true);
            }
            else
            {
                obj.SetObjext(false, false, false);
            }
        }
        yield return null;
        animPage[0].SetTrigger(isNext ? "Flip" : "FlipReverse");
        StartMorph();
        yield return new WaitForSeconds(3f);
        EndFlip();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Transform t in leftBones)
        {
            Gizmos.DrawSphere(t.position, 0.1f);
        }
        foreach (Transform t in rightBones)
        {
            Gizmos.DrawSphere(t.position, 0.1f);
        }
        foreach (Transform t in currentBones)
        {
            Gizmos.DrawSphere(t.position, 0.1f);
        }

    }
}
