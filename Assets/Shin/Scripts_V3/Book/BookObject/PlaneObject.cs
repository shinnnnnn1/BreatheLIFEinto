using System.Linq;
using UnityEngine;

/// <summary>
/// FlipでStandの回転をするタイプ
/// </summary>
public class PlaneObject : BaseObject_V3
{
    [Space(10)]
    //standはFlip用、planeはTurn用
    public Transform plane;
    public Animator anim;
    public MeshCollider npcCylinder;
    public bool[] isDirectional = new bool[] { true, true, true }; // -1, 0, 1

    [Space(10)]
    public bool isFacingRight;

    public override void Start()
    {
        base.Start();

        //
        stand.localEulerAngles = new Vector3(90, 0, 0);

        //
        plane = stand.GetChild(0);
        anim = plane.GetComponent<Animator>();

        //처음에 안보이게할거는
        npcCylinder = GetComponentsInChildren<MeshCollider>().FirstOrDefault();

        isFacingRight = plane.localEulerAngles.y < 90;
        base.Start();
    }

}
