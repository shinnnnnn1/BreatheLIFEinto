using UnityEngine;

public class Tutorial00 : TutorialEvent
{
    [Space(10f)]
    [SerializeField] Rigidbody rigid;

    public override bool QuestComplete()
    {
        if(rigid.linearVelocity.magnitude > 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
