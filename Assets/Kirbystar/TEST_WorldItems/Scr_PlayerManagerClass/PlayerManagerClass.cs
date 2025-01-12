
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

    const string PLAYERDATA_KEY_PLAYER_DATA = "PlayerData";
    const string PLAYERDATA_KEY_INTERNAL_INDEX = "PlayerInternalIndex";
    const string PLAYERDATA_KEY_INDEX = "PlayerIndex";
    const string PLAYERDATA_KEY_NAME = "PlayerName";
    const string PLAYERDATA_KEY_IS_VR = "IsUseVR";
    const string PLAYERDATA_KEY_PLATFORM = "Platform";
    const string PLAYERDATA_KEY_DEVICE = "DeviceType";
    #endregion

    [NonSerialized]
    public DataDictionary playerDataDict = new DataDictionary()
    {
        {"PlayerData",new DataDictionary()
            {
                {PLAYERDATA_KEY_INTERNAL_INDEX,0},
                {PLAYERDATA_KEY_INDEX,0 },
                {PLAYERDATA_KEY_NAME,"" },
                {PLAYERDATA_KEY_IS_VR,false},
                {PLAYERDATA_KEY_PLATFORM,PLAYER_PLATFORM_UNKNOWN },
                {PLAYERDATA_KEY_DEVICE,PLAYER_DEVICE_UNKNOWN }
            }
        }
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

        DataDictionary _playerData = new DataDictionary();
        int _internalIdx = player.playerId;
        int _playerIdx=0;
        string _playerName = player.displayName;
        bool _isVR = player.IsUserInVR();
        _playerData.SetValue(PLAYERDATA_KEY_INTERNAL_INDEX, _internalIdx);
        _playerData.SetValue(PLAYERDATA_KEY_NAME,_playerName);
        _playerData.SetValue(PLAYERDATA_KEY_IS_VR, _isVR);


        //空き枠を探す
        DataList keys_pDD = playerDataDict.GetKeys();

        for (int i = 0; i < keys_pDD.Count; i++)
        {
            DataToken key = keys_pDD[i];

            DataList keys_pD = _playerData.GetKeys();
            for (int j = 0; j < keys_pD.Count; j++)
            {
                DataToken key2 = keys_pD[j];
                Debug.Log(keys_pD[j]);
            }
        }
    }
    /// <summary>
    /// プレイヤーの情報を取得する
    /// </summary>
    /// <param name="player"></param>
    void GetPlayerInfoData(VRCPlayerApi playerData)
    {

    }
    
}

