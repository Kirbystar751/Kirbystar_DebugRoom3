
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Star_Falling : UdonSharpBehaviour
{

    GameObject clashEft;
    GameObject _star;
    Animator anim;

    void Start()
    {
        clashEft = transform.Find("Eft_Clash").gameObject;
        _star = this.gameObject;
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        clashEft.GetComponent<ParticleSystem>().Play();

        anim.SetBool("Break_Star", true);
    }

    public void StarClear()
    {
        foreach (Transform s in gameObject.transform)
        {
            Destroy(s.gameObject);
        }
        Destroy(gameObject);
    }
}
