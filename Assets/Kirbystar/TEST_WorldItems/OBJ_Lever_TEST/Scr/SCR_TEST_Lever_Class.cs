
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SCR_TEST_Lever_Class : UdonSharpBehaviour
{
    [Header("レバーオブジェクト")]
    [SerializeField] public GameObject Lever;

    [HideInInspector]
    /// <summary>
    /// レバークラス
    /// </summary>
    public Vector2 Lever_Input;

    GameObject _lever_Stick;

    void Start()
    {
        _lever_Stick = transform.Find("Lever_Ball/Lever").gameObject;
    }

    void Update()
    {
       
    }
}
