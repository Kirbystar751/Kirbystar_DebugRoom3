
using UdonSharp;
using UnityEngine;
using UnityEngine.Experimental.AI;
using VRC.SDK3.UdonNetworkCalling;
using VRC.SDKBase;
using VRC.Udon;

public class SGB_ColorSim_IllustButton : UdonSharpBehaviour
{
    // ログ用カラーコード
    const string logColorCode = "#FF0080";

    // ログ用プレフィックス
    const string logPrefix = "<color=" + logColorCode + ">[SGB ColorSim IllustChange]</color>";

    [SerializeField] public Texture[] Illusts;
    [SerializeField] public MeshRenderer[] IllustMats = new MeshRenderer[4];
    AudioSource Sound;
    [SerializeField]public AudioClip IllustChangeSound;
    [SerializeField] public SGB_ColorSim_SyncManager syncManager;
    public int illustIndex = 0;

    void Start()
    {
        Sound = GetComponent<AudioSource>();
        IllustChange();
    }

    public override void Interact()
    {
        NextIllust();
        Sound.Stop();
        Sound.PlayOneShot(IllustChangeSound);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Others, "InteractEvent");
    }
    [NetworkCallable]
    public void InteractEvent()
    {
        Sound.Stop();
        Sound.PlayOneShot(IllustChangeSound);
    }

    public void NextIllust()
    {
        illustIndex = (illustIndex + 1) % Illusts.Length;
        syncManager.syncKind = SGB_ColorSim_SyncManager.SYNC_KIND_ILLUST_CHANGE;
        syncManager.SetIllustIndex(illustIndex);

    }
    /// <summary>
    /// イラストをを指定のインデックスに切り替える（ネットワーク呼び出し用）
    /// </summary>
    public void SetIllust(int index)
    {
        Debug.Log(logPrefix + "SetIllustが呼ばれた illustIndex = " + illustIndex);
        illustIndex = index;
        IllustChange();
        //ColorBinCh
        //Sound.PlayOneShot(BinChangeSound);
    }

    public void IllustChange()
    {
        Debug.Log(logPrefix + "IllustChangeが呼ばれた illustIndex = " + illustIndex);
        for(int i = 0; i < IllustMats.Length; i++)
        {
            IllustMats[i].material.SetTexture("_MainTex", Illusts[illustIndex]);
        }
    }
}
