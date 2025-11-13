using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// FlipでAnimationを再生するタイプ
/// </summary>
public class AnimObject : BaseObject_V3
{
    [Space(10f)]
    public Animator anim;
    public float animDelay;

    public override void Start()
    {
        anim = GetComponentInChildren<Animator>();

        base.Start();
    }

    /// <summary>
    /// オブジェクトのタイプごとにモーションを実行。
    /// </summary>
    /// <seealso cref="BookController_V3.Flip()"/>
    public override void FlipMotion(BookModel_V3 model, bool isAct)
    {
        base.FlipMotion(model, isAct);

        if (isCurrent && isAct == isActivate)
        {
            //AnimObjectの場合、Animationを再生する
            float delay = animDelay;
            string animation = isAct ? "Activate" : "Deactivate";

            StartCoroutine(AnimCoroutine(delay, animation));
        }
    }

    IEnumerator AnimCoroutine(float delay, string animation)
    {
        yield return new WaitForSeconds(delay);

        anim.SetTrigger(animation);
    }
}
