using UnityEngine;

public class PlayerHold : MonoBehaviour
{
    [SerializeField] ConfigurableJoint joint;

    Rigidbody playerRigid, targetRigid;

    private void Start()
    {
        joint = GetComponentInChildren<ConfigurableJoint>();
    }

    public void SetJoint(bool isRight, Vector2 anchor, Rigidbody rigid, Rigidbody target)
    {
        playerRigid = rigid;
        targetRigid = target;

        playerRigid.mass = 100;
        targetRigid.mass = 1;

        joint.anchor = new Vector3(isRight ? anchor.x : -anchor.x, anchor.y, 0);
        joint.axis = isRight ? Vector3.right : Vector3.left;
        joint.connectedBody = target;


    }

    public void ResetJoint()
    {
        playerRigid.mass = 1;
        targetRigid.mass = 100f;

        joint.connectedBody = null;
    }
}
