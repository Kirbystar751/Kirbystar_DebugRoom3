
using UdonSharp;
using Unity.Mathematics;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SCR_TEST_Lever_Limit : UdonSharpBehaviour
{
    public Vector3 Lever_Angle;

    void Start()
    {
        
    }

    public void Update()
    {
        Quaternion lever_Rot = this.gameObject.transform.rotation;
        Vector3 Lever_Rot_Euler = lever_Rot.eulerAngles;

        Lever_Angle = new Vector3((Lever_Rot_Euler.x - 270), Lever_Rot_Euler.y, Lever_Rot_Euler.z);
    }
}
