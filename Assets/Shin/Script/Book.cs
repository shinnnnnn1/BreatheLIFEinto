using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Book : MonoBehaviour
{
    public Transform objectParent, morph;
    public Transform[] objectParents, morphs;

    [Tooltip("Set Manually")]
    public Transform[] leftBones, rightBones, currentBones;
    public SkinnedMeshRenderer[] pages;
    public Material[] pageMaterialsL, pageMaterialsR;

    [Tooltip("ActMorph, ActY, DeactMorph, DeactY")]
    public AnimationCurve[] curvesValue, curvesDelay;

    [Header("Book")]
    [SerializeField] int currentPage;
    public bool isFlipping;
    [SerializeField] BookObject[] bookObjects;
    //[SerializeField] NPC[] npcs;
    
    [SerializeField] Animator[] animPage;
    [SerializeField] Animation[] animBook;

    float flipTime;
    float a;

    [SerializeField] Image fadeImage;

    void Awake()
    {
        //Set ObjectParents and Morphs
        objectParents = new Transform[objectParent.childCount];
        for (int i = 0; i < objectParent.childCount; i++)
        {
            objectParents[i] = objectParent.GetChild(i);
        }
        morphs = new Transform[morph.childCount];
        for (int i = 0; i < morph.childCount; i++)
        {
            morphs[i] = morph.GetChild(i);
        }

        //Get All BookObjects
        bookObjects = objectParents[0].GetComponentsInChildren<BookObject>();

        //float value = curves[0].Evaluate(0.1f);
    }

    void Start()
    {
        //Set Transform for Starting at the Center
        //transform.position = Vector3.left * 5;

        //Set Book Objects Start Position
        StartCoroutine(SetStart());

        //Hide All Book Objects
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

    public void Flip()
    {
        Debug.Log("Flippp");
        if (isFlipping) { return; }
        flipTime = Time.time;
        currentPage++;

        for (int i = 0; i < morphs.Length; i++)
        {
            morphs[i].position = new Vector3(morphs[i].position.x, 0, morphs[i].position.z);
        }

        StartCoroutine(FlipCoroutine());
    }

    IEnumerator FlipCoroutine()
    {
        yield return null;
        isFlipping = true;




        yield return new WaitForSeconds(3f);

        Debug.Log($"Currently on Page [ {currentPage} ]");
        isFlipping = false;

        //animPage[0].SetTrigger("Reset");
    }


    /*
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
    */

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Transform t in leftBones)
        {
            Gizmos.DrawSphere(t.position, 0.05f);
        }
        foreach (Transform t in rightBones)
        {
            Gizmos.DrawSphere(t.position, 0.05f);
        }
        foreach (Transform t in currentBones)
        {
            Gizmos.DrawSphere(t.position, 0.05f);
        }

    }
}
