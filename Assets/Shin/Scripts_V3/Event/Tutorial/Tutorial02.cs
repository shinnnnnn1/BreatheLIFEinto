using UnityEngine;

public class Tutorial02 : TutorialEvent
{
    [Space(10f)]
    [SerializeField] bool isC;
    [SerializeField] bool[] isCompletedSub = new bool[3];

    public void Completed(int num)
    {
        isCompletedSub[num] = true;

        foreach (bool isCompleted in isCompletedSub)
        {
            if(!isCompleted)
            {
                return;
            }
        }

        isC = true;
    }

    public override bool QuestComplete()
    {
        if(isC)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
