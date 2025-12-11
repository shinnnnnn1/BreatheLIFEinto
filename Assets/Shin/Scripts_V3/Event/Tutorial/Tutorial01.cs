using UnityEngine;

public class Tutorial01 : TutorialEvent
{
    [Space(10f)]
    [SerializeField] Rigidbody rigid;

    public override bool QuestComplete()
    {
        if(rigid.linearVelocity.y > 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
