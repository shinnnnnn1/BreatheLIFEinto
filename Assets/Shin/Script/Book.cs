using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor.SceneManagement;

public class Book : MonoBehaviour
{
    public Transform objectParent, NPCParent,  morph;
    public Transform[] objectParents, NPCParents,  morphs;

    [Tooltip("Set Manually")]
    public Transform[] leftBones, rightBones, currentBones;
    public SkinnedMeshRenderer[] pages;
    public Material[] pageMaterialsL, pageMaterialsR;

    [Tooltip("ActMorph, DeactMorph , ActY, DeactY")]
    public AnimationCurve[] curvesValue, curvesDelay;

    [Header("Book")]
    public  int currentPage;
    public bool isFlipping;
    [SerializeField] BookObject[] bookObjects;
    //[SerializeField] NPC[] npcs;
    
    [SerializeField] Animator[] animPage;
    [SerializeField] Animation[] animBook;

    public float flipTime;
    float cTime;

    [SerializeField] Image fadeImage;

    void Awake()
    {
        //Set ObjectParents and Morphs
        objectParents = new Transform[objectParent.childCount];
        for (int i = 0; i < objectParent.childCount; i++)
        {
            objectParents[i] = objectParent.GetChild(i);
        }
        NPCParents = new Transform[NPCParent.childCount];
        for (int i = 0; i < NPCParent.childCount; i++)
        {
            NPCParents[i] = NPCParent.GetChild(i);
        }
        morphs = new Transform[morph.childCount];
        for (int i = 0; i < morph.childCount; i++)
        {
            morphs[i] = morph.GetChild(i);
        }

        //Get All BookObjects and NPC
        bookObjects = objectParent.GetComponentsInChildren<BookObject>();
        Array.Resize(ref bookObjects, bookObjects.Length + NPCParents.Length);
        for (int i = 0; i < NPCParents.Length; i++)
        {
            bookObjects[bookObjects.Length - NPCParents.Length + i] = NPCParents[i].GetComponent<BookObject>();
        }

        //float value = curves[0].Evaluate(0.1f);
    }

    IEnumerator Start()
    {
        //Set Transform for Starting at the Center
        //transform.position = Vector3.left * 5;

        //Hide All Pages
        for (int i = 0; i < pages.Length; i++)
        {
            //pages[i].gameObject.SetActive(false);
        }

        //Set Book Objects Start Position
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
            flipTime = Time.time - cTime;
            //Debug.Log(flipTime);
        }
    }

    public void Flip()
    {
        if (isFlipping) { return; }
        Debug.Log("Flip");
        currentPage++;
        isFlipping = true;
        cTime = Time.time;

        StartCoroutine(FlipCoroutine());
    }

    IEnumerator FlipCoroutine()
    {
        //Move Morphs
        StartMorph();

        //Assign the Required Objects
        foreach(var obj in bookObjects)
        {
            if(currentPage == obj.stage || currentPage - 1 == obj.stage)
            {
                obj.SetObject();
            }
        }

        yield return null;

        //Play Flip Animation
        animPage[0].SetTrigger("Flip");

        yield return new WaitForSeconds(3f);

        //Stop Flipping
        Debug.Log($"Currently on Page [ {currentPage} ]");
        isFlipping = false;

        //Reset Parent
        foreach (var obj in bookObjects)
        {
            if (currentPage == obj.stage || currentPage - 1 == obj.stage)
            {
                obj.ResetParent();
                obj.AfterFlip();
            }
        }
        
        //Reset Morph
        for (int i = 0; i < morphs.Length; i++)
        {
            morphs[i].DOPause();
            morphs[i].position = new Vector3(morphs[i].position.x, 0, morphs[i].position.z);
        }

        //Reset Page Animation
        animPage[0].SetTrigger("Reset");

        //Play Timeline After Flip
        if(currentPage == 1)
        {
            GameManager.Instance.PlayCutScene(0);
        }
    }

    void StartMorph()
    {
        for (int i = 0; i < 10; i++)
        {
            float time = curvesValue[0].Evaluate(i);
            float delay = curvesDelay[0].Evaluate(i);
            morphs[i].DOLocalMoveY(1, time).SetDelay(delay).SetEase(Ease.InOutQuad);
        }
        for (int i = 10; i < 20; i++)
        {
            float time = curvesValue[1].Evaluate(i - 10);
            float delay = curvesDelay[1].Evaluate(i - 10);
            morphs[i].DOLocalMoveY(1, time).SetDelay(delay).SetEase(Ease.InOutQuad);
        }
    }

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
