using UnityEngine;
using UnityEngine.Events;

public class RHE64_SelectItem : MonoBehaviour
{
    [SerializeField] PlayerController_V3 player;

    [SerializeField] float ZoomValue;
    [SerializeField] bool canActivate;
    [SerializeField] bool isActivated;
    [SerializeField] UnityEvent onActivated, onDeactivated;

    private void Update()
    {
        if(canActivate)
        {
            if(player.ZoomEvent(ZoomValue) && !isActivated)
            {
                onActivated.Invoke();
                isActivated = true;
            }
            else if(!player.ZoomEvent(ZoomValue) && isActivated)
            {
                onDeactivated.Invoke();
                isActivated = false;
            }
        }
    }

    public void _SetCanActivate(bool canA)=>canActivate = canA;
}
