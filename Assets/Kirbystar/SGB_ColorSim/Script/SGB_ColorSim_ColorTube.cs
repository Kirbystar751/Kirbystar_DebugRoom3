
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SGB_ColorSim_ColorTube : UdonSharpBehaviour
{
    [SerializeField] public ParticleSystem Paintparticle;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnPickupUseDown()
    {
        Paintparticle.Play();
        animator.Play("PaintTube_Sibori");
    }
}
