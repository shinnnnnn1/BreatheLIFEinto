using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

public class FlipTriggerController : MonoBehaviour
{
    [SerializeField] bool canProceed = false;
    [SerializeField] bool isBookHorizontal = true;

    [SerializeField] int currentTrigger = 0;
    [SerializeField] FlipTrigger[] triggers;

    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public BookController bookController;

    void Start()
    {
        //FlipTriggerを参照
        triggers = GetComponentsInChildren<FlipTrigger>();

        //0番のTriggerだけ表示
        ResetTrigger(currentTrigger);

        //Controllerを参照
        playerController = FindFirstObjectByType<PlayerController>();
        bookController = FindFirstObjectByType<BookController>();
    }

    public void ResetTrigger(int currentTrigger)
    {
        Debug.Log("Reset to " + currentTrigger);

        //表示したいTriggerだけ表示
        for (int i = 0; i < triggers.Length; i++)
        {
            triggers[i].gameObject.SetActive(i == currentTrigger);
        }
    }

    public void SetCanProceed(bool can)
    {
        Debug.Log("SetCanProceed " + can);

        //進行できる状態の設定
        canProceed = can;

        //本が水平なのか確認
        CheckBookIsHorizontal(bookController.bookDir);
    }

    public void CheckBookIsHorizontal(int bookDir)
    {
        isBookHorizontal = bookDir == 0;
    }

    public bool CanTrigger()
    {
        if(canProceed && isBookHorizontal) { return true; }
        else {  return false; }
    }

    void OnDrawGizmos()
    {
        if (triggers.Length > 0)
        {
            Gizmos.color = (canProceed && isBookHorizontal) ? Color.green : Color.red;
            Gizmos.DrawSphere(triggers[0].transform.position, 0.5f);
        }
    }
}
