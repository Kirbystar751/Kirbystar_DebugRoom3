using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class SGB_ColorSim_SyncManager : UdonSharpBehaviour
{
    const string logColorCode = "#FF0080";
    const string logPrefix = "<color=" + logColorCode + ">[SGB ColorSim SyncManager]</color>";

    [SerializeField] private SGB_ColorSim_Core core;
    [SerializeField] private SGB_ColorSim_TestInterFace interfaceUI;
    [SerializeField] private SGB_ColorSim_ColorBinController colorBinController;

    // 同期するパスワード
    [UdonSynced] public string syncPass;
    // 同期する絵の具のビンのページ
    [UdonSynced] public int syncColorBinIndex;

    #region 同期の種類
    /// <summary>
    /// 同期の種類：ダミー（使用禁止）
    /// </summary>
    public const int SYNC_KIND_PALLETE_DAMMY = 0;
    /// <summary>
    /// 同期の種類：絵の具のビン変更
    /// </summary>
    public const int SYNC_KIND_PALLETE_CHANGE = 1;
    /// <summary>
    /// 同期の種類：色つかむ
    /// </summary>
    public const int SYNC_KIND_COLOR_PICKUP = 2;
    /// <summary>
    /// 同期の種類：色離す
    /// </summary>
    public const int SYNC_KIND_COLOR_RELEASE = 3;
    /// <summary>
    /// 同期の種類：色をセット
    /// </summary>
    public const int SYNC_KIND_COLOR_SET = 4;
    /// <summary>
    /// 同期の種類：パスワード操作
    /// </summary>
    public const int SYNC_KIND_PASSWORD = 5;
    /// <summary>
    /// 同期の種類：色を明るくする
    /// </summary>
    public const int SYNC_KIND_COLOR_LIGHT = 6;
    /// <summary>
    /// 同期の種類：色を暗くする
    /// </summary>
    public const int SYNC_KIND_COLOR_DARK = 7;

    /// <summary>
    /// 同期の種類：全部同期する（あとから来た人向け）
    /// </summary>
    public const int SYNC_KIND_ALL = 255;
    #endregion

    // 何を同期したか（操作演出のために必要）
    [UdonSynced] public int syncKind = SYNC_KIND_ALL;


    //もしかしたらこの辺の情報はPickupで同期したほうがいいのかな
    // Pickupをどっちの手で持っているか
    [UdonSynced] public int syncHand;

    // 【色セット/白黒絵の具使用時】どこの色ボックスにセットしたか
    [UdonSynced] public int syncColorBoxIndex;

    // 【パスワード操作時】どの桁を操作したか
    [UdonSynced] public int syncPasswordIndex;

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

    public void SetColorBinIndex(int index)
    {
        if (!Networking.IsOwner(gameObject)) 
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }
        syncColorBinIndex = index;
        RequestSerialization();
        ApplyState();
    }

    /// <summary>
    /// 色を明るくする
    /// </summary>
    /// <param name="index"></param>
    public void ColorLight(int index)
    {
        core.ColorLight(index);
        if (!Networking.IsOwner(gameObject))
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }
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
        if (!Networking.IsOwner(gameObject))
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }
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
        if (!Networking.IsOwner(gameObject))
        {
            Debug.Log(logPrefix + "全部の同期内容を反映");
            core.SetPassword(syncPass);
            interfaceUI.colorBoxColorChange();
            colorBinController.SetColorBin(syncColorBinIndex);
            colorBinController.palAnim.SetInteger("PalNum", syncColorBinIndex);
            return;
        }
        switch (syncKind)
        {
            case SYNC_KIND_PASSWORD:
                Debug.Log(logPrefix + "パスワード操作の同期を反映");
                core.SetPassword(syncPass);
                interfaceUI.colorBoxColorChange();
                break;
            case SYNC_KIND_COLOR_LIGHT:
            case SYNC_KIND_COLOR_DARK:
                Debug.Log(logPrefix + "色の明るさ変更の同期を反映");
                core.SetPassword(syncPass);
                interfaceUI.colorBoxColorChange();
                break;
            case SYNC_KIND_PALLETE_CHANGE:
                Debug.Log(logPrefix + "絵の具のビン変更の同期を反映");
                colorBinController.SetColorBin(syncColorBinIndex);
                colorBinController.palAnim.SetInteger("PalNum", syncColorBinIndex);
                break;
            case SYNC_KIND_ALL:
                Debug.Log(logPrefix + "全部の同期内容を反映");
                core.SetPassword(syncPass);
                interfaceUI.colorBoxColorChange();
                colorBinController.SetColorBin(syncColorBinIndex);
                colorBinController.palAnim.SetInteger("PalNum", syncColorBinIndex);
                break;
            default:
                break;
        }
    }
}
