using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class SGB_ColorSim_SyncManager : UdonSharpBehaviour
{
    // ログ用カラーコード
    const string logColorCode = "#FF0080";

    // ログ用プレフィックス
    const string logPrefix = "<color=" + logColorCode + ">[SGB ColorSim SyncManager]</color>";

    [SerializeField] private SGB_ColorSim_Core core;
    [SerializeField] private SGB_ColorSim_TestInterFace interfaceUI;
    //[SerializeField] private string bPass;
    [UdonSynced, FieldChangeCallback(nameof(syncPassword))] private string syncPass;

    //Todo:今のままだとあとから来た人の動作が暴走する
    //この頃同期がドーキドキ


    public string syncPassword
    {
        set { syncPass = value; SyncPassChange(); Debug.Log(logPrefix + "受けたパスワードは" + value); }
        get => syncPass;
    }

    public void SetPassword()
    {
        Debug.Log(logPrefix + "SyncPassword()");
        //オブジェクトオーナー取得
        if (!Networking.LocalPlayer.IsOwner(this.gameObject))
        {
            Debug.Log(logPrefix + "オブジェクトのオーナーではないため、オーナーに変更します");
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        }
        syncPassword = core.SGBPassword;
        Debug.Log(logPrefix + "syncPasswordを" +  syncPassword + "に書き換えた");
        RequestSerialization();
        Debug.Log(logPrefix + "同期要求を飛ばしました") ; 
    }

    /// <summary>
    /// 同期用のパスワードが変更されたときの処理
    /// </summary>
    private void SyncPassChange()
    {
        Debug.Log(logPrefix + "SyncPassChange()");
        interfaceUI.colorBoxColorChange();
    }
}