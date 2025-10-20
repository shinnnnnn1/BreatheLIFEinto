using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

public class FlipTriggerController : MonoBehaviour
{
    public bool canProceed = false;
    public bool isBookHorizontal = true;

    [SerializeField] int currentTrigger = 0;
    [SerializeField] FlipTrigger[] triggers;

    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public BookController bookController;

    void Start()
    {
        triggers = GetComponentsInChildren<FlipTrigger>();
        ResetTrigger(currentTrigger);

        playerController = FindFirstObjectByType<PlayerController>();
        bookController = FindFirstObjectByType<BookController>();
    }

    public void ResetTrigger(int currentTrigger)
    {
        Debug.Log("Reset to " + currentTrigger);
        for (int i = 0; i < triggers.Length; i++)
        {
            triggers[i].gameObject.SetActive(i == currentTrigger);
        }
    }

    public void SetCanProceed(bool can)
    {
        Debug.Log("SetCanProceed " + can);
        canProceed = can;
        CheckBookIsHorizontal(bookController.bookDir);
    }
    public void CheckBookIsHorizontal(int bookDir)
    {
        isBookHorizontal = bookDir == 0;
    }

    private void OnDrawGizmos()
    {
        if (triggers.Length > 0)
        {
            Gizmos.color = (canProceed && isBookHorizontal) ? Color.green : Color.red;
            Gizmos.DrawSphere(triggers[0].transform.position, 0.5f);
        }
    }
}
