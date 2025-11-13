using System.Numerics;
using UnityEngine;

/// <summary>
/// FlipでAnimationを再生するタイプ
/// </summary>
public class AnimObject : BaseObject_V3
{
    public Animator anim;
    public float animDelay;//ㅇㅣ름 이게 맞는지 확인

    public override void Start()
    {
        base.Start();

    }

    /// <summary>
    /// オブジェクトのタイプごとにモーションを実行。
    /// </summary>
    /// <seealso cref="BookController_V3.Flip()"/>
    public override void FlipMotion(BookModel_V3 model, bool isAct)
    {
        //条件はBaseで確認
        base.FlipMotion(model, isAct);

        //AnimObjectの場合、Animationを生成する

        //float delay = model.
        //string animation = isAct ? "Open" : "Close";

        //StartCoroutine(AnimCoroutine());
    }

    /*
    IEnumerator AnimCoroutine(float delay, string animation)
    {
        yield return null;

        anim.SetTrigger(animation);
    }
    */
}
