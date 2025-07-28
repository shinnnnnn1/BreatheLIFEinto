using UnityEngine;

public class Test3 : MonoBehaviour
{
    [SerializeField] Animator anim;
    
    public void AnimOn(bool isAnim)
    {
        anim.enabled = true;
        anim.SetBool("IsAnim", isAnim);
    }

    public void DisableTree()
    {
        anim.gameObject.SetActive(false);
    }
}
