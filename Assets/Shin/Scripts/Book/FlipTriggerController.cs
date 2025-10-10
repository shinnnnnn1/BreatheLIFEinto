using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

public class FlipTriggerController : MonoBehaviour
{
    public bool canProceed = false;
    public int currentTrigger = 0;

    [SerializeField] Transform[] triggers;

    [HideInInspector] public PlayerController playerController;

    void Start()
    {
        triggers = GetComponentsInChildren<Transform>().Where(w => w != this.transform).ToArray();
        ResetTrigger(currentTrigger);

        playerController = FindFirstObjectByType<PlayerController>();
    }

    public void ResetTrigger(int currentTrigger)
    {
        for (int i = 0; i < triggers.Length; i++)
        {
            triggers[i].gameObject.SetActive(i == currentTrigger);
        }
    }

    public void SetCanProceed(bool can)
    {
        canProceed = can;
    }
}
