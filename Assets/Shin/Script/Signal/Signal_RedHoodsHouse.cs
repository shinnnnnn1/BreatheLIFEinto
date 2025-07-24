using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Signal_RedHoodsHouse : MonoBehaviour
{
    [SerializeField] Animator dialogueAnim;
    [SerializeField] TMP_Text text;
    Rigidbody rigid;
    bool canMove;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!canMove) { return; }

        if (transform.position.x < 5.65f)
        {
            rigid.linearVelocity = new Vector3(2, rigid.linearVelocity.y, 0);
        }
        else if (transform.position.x > 5.65f)
        {
            rigid.isKinematic = true;
            canMove = false;
        }
    }

    public void StartRedHood()
    {
        GameManager.Instance.player.canMove = false;
        rigid.isKinematic = false;
        canMove = true;
        dialogueAnim.SetTrigger("Start");
        StartCoroutine(Dialogue());
    }

    IEnumerator Dialogue()
    {
        string t = text.text;
        text.text = "";
        foreach (char c in t)
        {
            text.text += c;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void PlayerCanMove()
    {
        GameManager.Instance.player.canMove = true;
        GameManager.Instance.player.rigid.isKinematic = false;
    }

    public void DeleteRedHood()
    {
        gameObject.SetActive(false);
    }
}
