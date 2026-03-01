
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.UdonNetworkCalling;
using VRC.SDKBase;
using VRC.Udon;

public class SGB_ColorSim_PasswordInputter : UdonSharpBehaviour
{
    // ログ用カラーコード
    const string logColorCode = "#FF0080";

    // ログ用プレフィックス
    const string logPrefix = "<color=" + logColorCode + ">[SGB ColorSim PassInputter]</color>";

    [SerializeField] public GameObject CharDisplayObject;
    [SerializeField] public SGB_ColorSim_Core core;
    [SerializeField] public SGB_ColorSim_TestInterFace interFace;
    [SerializeField] public AudioClip interactSound;
    [SerializeField]SGB_ColorSim_SyncManager syncManager;
    AudioSource Sound;
    Text charDisp;

    void Start()
    {
        charDisp = CharDisplayObject.GetComponent<Text>();
        Sound = GetComponent<AudioSource>();
        charDisp.text = core.SGBPassword.Substring(int.Parse(gameObject.name), 1);
    }

    [NetworkCallable]
    public void InteractEvent()
    {
        //charDisp.text = core.SGBPassword.Substring(int.Parse(gameObject.name), 1);
        Sound.PlayOneShot(interactSound);
    }

    public override void Interact()
    {
        Debug.Log(logPrefix + "Interactが呼ばれました");
        string currentPass = core.SGBPassword;
        int interactedObj = int.Parse(gameObject.name);

        if (currentPass.Substring(interactedObj,1) == "-")
        {
            Debug.Log(logPrefix + interactedObj + "はハイフンなので処理しません");
            return;
        }
        else
        {
            Debug.Log(logPrefix + interactedObj + "文字目が押された");
            Sound.PlayOneShot(interactSound);
            //押された数字を１進める、９だったら０にする
            int currentNum = int.Parse(currentPass.Substring(interactedObj, 1));
            int nextNum = (currentNum + 1) % 10;
            currentPass = currentPass.Remove(interactedObj, 1).Insert(interactedObj, nextNum.ToString());
            //core.SGBPassword = currentPass;
            //core.SetPassword(currentPass);
            syncManager.syncKind = SGB_ColorSim_SyncManager.SYNC_KIND_PASSWORD;
            syncManager.SetPassword(currentPass);

            interFace.colorBoxColorChange();
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Others, "InteractEvent");
        }
    }
}
