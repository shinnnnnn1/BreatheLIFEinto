using UnityEngine;
using UnityEngine.Events;

public class BookAfterFlip_V3 : MonoBehaviour, IBookAfterFlip
{
    [SerializeField] UnityEvent[] afterFlipEvent;

    public void OnAfterFlip(int currentPage)
    {
        afterFlipEvent[currentPage].Invoke();
    }
}
