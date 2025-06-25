using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Book : MonoBehaviour
{
    public Transform objectParent;
    public Transform[] objectParents, leftBones, rightBones, currentBones, morphs, pages;

    [Tooltip("AM, AY, DM, DY / Dealy")]
    public AnimationCurve[] curves;

    [Header("Book")]
    [SerializeField] int currentPage;
    public bool isFlipping;
    [SerializeField] StaticObject[] bookObjects;
    //[SerializeField] NPC[] npcs;
    
    [SerializeField] Animator[] animPage;
    [SerializeField] Animation[] animBook;

    float flipTime;
    float a;

    [SerializeField] Image fadeImage;

    void Awake()
    {
        bookObjects = objectParents[0].GetComponentsInChildren<StaticObject>();
        float value = curves[0].Evaluate(0.1f);
    }

    void Start()
    {
        //transform.position = Vector3.left * 5;
        StartCoroutine(SetStart());
        for (int i = 0; i < pages.Length; i++)
        {
            //pages[i].gameObject.SetActive(false);
        }
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
        for (int i = 10; i < 20; i++)
        {
            morphs[i].transform.position = new Vector3(morphs[i].transform.position.x, 0, morphs[i].transform.position.z);
        }

        StartCoroutine(FlipC(isNext));

        
    }

    void StartMorph()
    {
        for (int i = 0; i < 10; i++)
        {
            float time = curves[0].Evaluate(i);
            float delay = curves[4].Evaluate(i);
            morphs[i].DOLocalMoveY(1, time).SetDelay(delay).SetEase(Ease.OutQuad);
        }
        for (int i = 10; i < 20; i++)
        {
            float time = curves[2].Evaluate(i);
            float delay = curves[6].Evaluate(i);
            morphs[i].DOLocalMoveY(1, time).SetDelay(delay).SetEase(Ease.OutQuad);
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

        foreach (StaticObject obj in bookObjects)
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
