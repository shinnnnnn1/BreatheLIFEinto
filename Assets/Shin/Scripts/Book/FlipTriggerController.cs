using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

public class FlipTriggerController : MonoBehaviour
{
    public bool canFlip = true;
    public int currentTrigger = 0;

    [SerializeField] Transform[] triggers;

    public PlayerController playerController;

    void Start()
    {
        //triggers = GetComponentsInChildren<Transform>();
        ResetTrigger(currentTrigger);

        playerController = FindFirstObjectByType<PlayerController>();
    }

    public void ResetTrigger(int currentTrigger)
    {
        for (int i = 0; i < triggers.Length; i++)
        {
            bool isActivate = i == currentTrigger;
            triggers[i].gameObject.SetActive(isActivate);
        }
    }
}
