using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

public class TutorialEvent : MonoBehaviour
{   
    public bool canPlay;
    [SerializeField] bool[] isCompleted = new bool[1];
    [SerializeField] UnityEvent onComplete, onCompleteP, onCompleteStart, onCompleteStart2;

    Transform trans;
    Material mat;
    Animator anim;

    private void Start()
    {
        trans = transform.GetChild(0);
        mat = trans.GetComponent<MeshRenderer>().material;
        mat.SetFloat("_Dithering", 2);
        anim = trans.GetComponent<Animator>();
    }

    public void _SetTutorialCanPlay()
    {
        canPlay = true;
    }
    public void TutorialComplete()
    {
        transform.DOLocalMoveY(1, 0).SetRelative();
        transform.DOLocalMoveY(-1, 1).SetRelative();
        anim.SetTrigger("Complete");

        _CompleteAnother(0);
    }

    public void _CompleteAnother(int num)
    {
        isCompleted[num] = true;
        if (num == 0)
        {
            onCompleteStart2.Invoke();
            Invoke("InvokeEvent2", 1.0f);
        }

        foreach (bool isC in isCompleted)
        {
            if (!isC)
            {
                return;
            }
        }

        onCompleteStart.Invoke();

        Invoke("InvokeEvent", 1.0f);
    }
    void InvokeEvent()
    {
        onComplete.Invoke();
    }
    void InvokeEvent2()
    {
        onCompleteP.Invoke();
    }

    private void Update()
    {
        if (canPlay)
        {
            if(QuestComplete())
            {
                isCompleted[0] = true;
                canPlay = false;
                TutorialComplete();
            }
        }
    }

    public virtual bool QuestComplete()
    {
        return false;
    }
}
