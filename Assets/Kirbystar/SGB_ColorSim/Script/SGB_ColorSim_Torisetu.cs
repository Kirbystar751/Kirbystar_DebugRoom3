
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SGB_ColorSim_Torisetu : UdonSharpBehaviour
{

    int pageIndex = 0;
    Animator torisetuAnim;
    void Start()
    {
        torisetuAnim = GetComponent<Animator>();
    }

    public override void Interact()
    {
        pageIndex++;
        if (pageIndex > 3) pageIndex = 0;
        torisetuAnim.SetInteger("Page",pageIndex);
    }
}
