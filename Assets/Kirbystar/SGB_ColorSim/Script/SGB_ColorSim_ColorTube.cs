
using UdonSharp;
using UnityEngine;
using VRC.SDK3.UdonNetworkCalling;
using VRC.SDKBase;
using VRC.Udon;

public class SGB_ColorSim_ColorTube : UdonSharpBehaviour
{
    [SerializeField] public ParticleSystem Paintparticle;
    [SerializeField] public ParticleSystem DummyParticle;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnPickupUseDown()
    {
        Paintparticle.Play();
        animator.Play("PaintTube_Sibori");
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Others, "PickupUseEvent");
    }

    [NetworkCallable]
    public void PickupUseEvent()
    {
         DummyParticle.Play();
         animator.Play("PaintTube_Sibori");
    }
}
