
using System;
using System.Net.NetworkInformation;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerManagerClass : UdonSharpBehaviour
{
    #region パブリック変数
        /// <summary>
        /// プレイヤー番号（内部）
        /// VRChatSDK側で返してくる内部的なプレイヤー番号
        /// </summary>
        public int InternalPlayerIndex;
        /// <summary>
        /// プレイヤー番号（通常）
        /// プログラムで割り付ける、独自のプレイヤー番号
        /// 空いてる若い枠から順番に割り当てていく
        /// </summary>
        public int PlayerIndex;
        /// <summary>
        /// プレイヤーの名前
        /// </summary>
        public string PlayerName;
        /// <summary>
        /// VRモードかどうか
        /// </summary>
        public bool IsUseVR;
        /// <summary>
        /// プレイヤーのプラットフォーム
        /// </summary>
        public int Platform;
        /// <summary>
        /// プレイヤーのデバイスタイプ
        /// </summary>
        public int DeviceType;
    #endregion
    #region 定数
        /// <summary>
        /// プレイヤーのプラットフォームを表す値
        /// </summary>
        const int PLAYER_PLATFORM_PC = 0;
        const int PLAYER_PLATFORM_ANDROID = 1;
        const int PLAYER_PLATFORM_UNKNOWN = 255;
        /// <summary>
        /// プレイヤーのデバイス種別を表す値
        /// </summary>
        /// <summary>
        /// PCデスクトップ
        /// </summary>
        const int PLAYER_DEVICE_PC_DESKTOP = 0;
        /// <summary>
        /// PC VR
        /// </summary>
        const int PLAYER_DEVICE_PCVR = 1;
        /// <summary>
        /// Androidスマホ
        /// </summary>
        const int PLAYER_DEVICE_ANDROID_MOBILE = 2;
        /// <summary>
        /// Quest単機
        /// </summary>
        const int PLAYER_DEVICE_QUEST = 3;
        /// <summary>
        /// その他
        /// </summary>
        const int PLAYER_DEVICE_UNKNOWN = 255;
    #endregion

    public DataDictionary playerDataDict = new DataDictionary()
    {
        {"PlayerInternalIndex",0},
        {"PlayerIndex",0 },
        {"PlayerName","" },
        {"IsUseVR",false},
        {"Platform",PLAYER_PLATFORM_UNKNOWN },
        {"DeviceType",PLAYER_DEVICE_UNKNOWN }
    };


    void Start()
    {

    }

    /// <summary>
    /// プレイヤーが入場てきたとき
    /// </summary>
    /// <param name="player"></param>
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        base.OnPlayerJoined(player);
        GetPlayerInfoData(player);
    }
    /// <summary>
    /// プレイヤーの情報を取得する
    /// </summary>
    /// <param name="player"></param>
    void GetPlayerInfoData(VRCPlayerApi playerData)
    {

    }
    
}

