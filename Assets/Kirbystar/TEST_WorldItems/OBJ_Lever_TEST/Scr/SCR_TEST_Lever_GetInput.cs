
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class SCR_TEST_Lever_GetInput : UdonSharpBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        SCR_TEST_Lever_Limit sCR_TEST_Lever_Limit;
        GameObject obj = GameObject.Find("Lever_Ball");
        sCR_TEST_Lever_Limit = obj.GetComponent<SCR_TEST_Lever_Limit>();

        Vector3 leverVec;
        leverVec = sCR_TEST_Lever_Limit.Lever_Angle;

        GameObject tObj = GameObject.Find("Text_Honbun_LeverDebug");
        Text tObjT = tObj.GetComponent<Text>();
        tObjT.text = "X:" + leverVec.x.ToString() + "\nY:" + leverVec.y.ToString() + "\nZ:" + leverVec.z.ToString();
    }
}
