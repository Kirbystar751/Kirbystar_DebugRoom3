using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class SGB_ColorSim_SyncManager : UdonSharpBehaviour
{
    const string logColorCode = "#FF0080";
    const string logPrefix = "<color=" + logColorCode + ">[SGB ColorSim SyncManager]</color>";

    [SerializeField] private SGB_ColorSim_Core core;
    [SerializeField] private SGB_ColorSim_TestInterFace interfaceUI;

    // 同期するパスワード
    [UdonSynced] public string syncPass;
    // 何を同期したか（操作演出のために必要）
    [UdonSynced] public int syncKind;
    // Pickupをどっちの手で持っているか
    [UdonSynced] public int syncHand;


    /// <summary>
    /// パスワードを反映する
    /// </summary>
    /// <param name="newPass"></param>
    public void SetPassword(string newPass)
    {
        if (!Networking.IsOwner(gameObject))
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }

        syncPass = newPass;

        RequestSerialization();

        ApplyState(); // ローカル即時反映
    }

    /// <summary>
    /// 色を明るくする
    /// </summary>
    /// <param name="index"></param>
    public void ColorLight(int index)
    {
        core.ColorLight(index);
        syncPass = core.SGBPassword;
        RequestSerialization();
        ApplyState();
    }

    /// <summary>
    /// 色を暗くする
    /// </summary>
    /// <param name="index"></param>
    public void ColorDark(int index)
    {
        core.ColorDark(index);
        syncPass = core.SGBPassword;
        RequestSerialization();
        ApplyState();
    }

    /// <summary>
    /// 同期受信時に呼ばれる
    /// </summary>
    public override void OnDeserialization()
    {
        ApplyState(); // 同期受信時反映
    }

    /// <summary>
    /// 状態を反映する
    /// </summary>
    private void ApplyState()
    {
        Debug.Log(logPrefix + "ApplyState() syncPass = " + syncPass);

        core.SetPassword(syncPass);
        interfaceUI.colorBoxColorChange();
    }
}
