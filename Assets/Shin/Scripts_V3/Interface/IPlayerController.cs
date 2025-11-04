using UnityEngine;

public interface IPlayerController
{
    /// <summary>
    /// 本の初期設定ができてゲームをスタートできる状態にする。ついでに本のデーターも渡す
    /// </summary>
    public void SetCanGameStart(Transform[] pL, Transform[] pR, Transform[] pLC, Transform[] pRC);
    /// <summary>
    /// 進行ができる状態でFlipTriggerに触れたら実行。Flipができる状態にする
    /// </summary>
    public void PlayerFlipTrigger();
    /// <summary>
    /// Flip中にキャラクターを閉じ、位置を設定し、開く動作
    /// </summary>
    public void PlayerFlip(bool isOpen, int currentPage);

    public void StopFlip();

}
