
using UdonSharp;
using UnityEngine;
using UnityEngine.Experimental.AI;
using VRC.SDKBase;
using VRC.Udon;

public class SGB_ColorSim_IllustButton : UdonSharpBehaviour
{
    [SerializeField] public Texture[] Illusts;
    [SerializeField] public MeshRenderer[] IllustMats = new MeshRenderer[4];
    AudioSource Sound;
    [SerializeField]public AudioClip IllustChangeSound;

    void Start()
    {
        Sound = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        Sound.Stop();
        Sound.PlayOneShot(IllustChangeSound);
    }
}
