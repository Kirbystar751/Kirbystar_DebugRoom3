
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using static VRC.SDK3.Dynamics.PhysBone.PhysBoneMigration.DynamicBoneColliderData;

public class SGB_ColorSim_ColorBox : UdonSharpBehaviour
{
    // ログ用カラーコード
    const string logColorCode = "#FF0080";
    // ログ用プレフィックス
    const string logPrefix = "<color=" + logColorCode + ">[SGB ColorSim ColorBox]</color>";

    [SerializeField]public SGB_ColorSim_Core core;
    [SerializeField] public AudioClip errorSound;
    [SerializeField] public AudioClip LightSound;
    [SerializeField] public AudioClip DarkSound;
    [SerializeField] public SGB_ColorSim_TestInterFace testInterface;
    [SerializeField] public GameObject lightParticle;
    [SerializeField] public GameObject darkParticle;

    int const_colorLight = 1;
    int const_colorDark = 2;

    AudioSource sound;
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    /// <summary>
    /// パーティクルが当たったときの処理
    /// </summary>
    /// <param name="other"></param>
    public void OnParticleCollision(GameObject other)
    {
        //アバターパーティクルなどだった場合
        if(!Utilities.IsValid(other)) 
        { 
            Debug.Log(logPrefix + "アバターパーティクルが当たった");
            sound.PlayOneShot(errorSound);
            //何もさせずに終了
            return; 
        }
        string hitParticleName = other.name;
        if (hitParticleName == lightParticle.name)
        {
            Debug.Log(logPrefix + "白絵の具が当たった");
            sound.PlayOneShot(LightSound);
            int thisBox = int.Parse(this.gameObject.transform.parent.gameObject.name.Substring(9)) - 1;
            core.ColorLight(thisBox);
            testInterface.colorBoxColorChange();
            return;
        }
        if(hitParticleName == darkParticle.name)
        {
            Debug.Log(logPrefix + "黒絵の具が当たった");
            sound.PlayOneShot(DarkSound);
            int thisBox = int.Parse(this.gameObject.transform.parent.gameObject.name.Substring(9)) - 1;
            core.ColorDark(thisBox);
            testInterface.colorBoxColorChange();
            return;
        }
    }
    
}
