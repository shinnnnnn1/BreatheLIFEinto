using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Book : MonoBehaviour
{
    public Transform[] leftBones, rightBones, currentBones;
    public Transform objectParent;
    [SerializeField] Animator[] animPage;
    [SerializeField] Animation[] animBook;

    [Header("Book")]
    [SerializeField] int currentPage;
    [SerializeField] bool isFlipping;
    [SerializeField] StaticObject[] BookObjects;
    //[SerializeField] NPC[] npcs;

    void Awake()
    {
        BookObjects = objectParent.GetComponentsInChildren<StaticObject>();

    }

    void Start()
    {
        StartCoroutine(SetStart());
    }

    IEnumerator SetStart()
    {
        yield return new WaitForSeconds(0.001f);
        animPage[1].speed = 100f;
        animPage[1].SetTrigger("Flip");
        yield return new WaitForSeconds(0.1f);
        animPage[1].SetTrigger("FlipReverse");
        yield return new WaitForSeconds(0.1f);
        animPage[1].speed = 1f;
    }

    void Update()
    {
        
    }

    public void Flip(bool isNext)
    {
        currentPage = isNext ? currentPage + 1 : currentPage - 1;
        animPage[0].SetTrigger(isNext ? "StopR" : "StopL");

        foreach (StaticObject obj in BookObjects)
        {
            if ((obj.stage == currentPage))
            {
                if((isNext && obj.isRight) || (!isNext && !obj.isRight))
                {   //SA
                    obj.SetObjext(true, true);
                }
                else
                {   //DA
                    obj.SetObjext(false, true);
                }
            }
            else if ((isNext && obj.isRight && obj.stage == currentPage - 1)
                || (!isNext && !obj.isRight && obj.stage == currentPage + 1))
            {   //DD
                obj.SetObjext(false, false);
            }
            else if ((isNext && !obj.isRight && obj.stage == currentPage - 1)
                || (!isNext && obj.isRight && obj.stage == currentPage + 1))
            {   //SD
                obj.SetObjext(false, false);
            }
        }

        animPage[0].SetTrigger(isNext ? "Flip" : "FlipReverse");
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

    }
}
