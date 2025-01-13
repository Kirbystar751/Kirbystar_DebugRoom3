
using HarmonyLib;
using System;
using System.Linq;
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
    [NonSerialized]
    public int InternalPlayerIndex;
    /// <summary>
    /// プレイヤー番号（通常）
    /// プログラムで割り付ける、独自のプレイヤー番号
    /// 空いてる若い枠から順番に割り当てていく
    /// </summary>
    [NonSerialized]
    public int PlayerIndex;
    /// <summary>
    /// プレイヤーの名前
    /// </summary>
    [NonSerialized]
    public string PlayerName;
    /// <summary>
    /// VRモードかどうか
    /// </summary>
    [NonSerialized]
    public bool IsUseVR;
    /// <summary>
    /// プレイヤーのプラットフォーム
    /// </summary>
    [NonSerialized]
    public int Platform;
    /// <summary>
    /// プレイヤーのデバイスタイプ
    /// </summary>
    [NonSerialized]
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
    const string PLAYERDATA_KEY_PLAYER_DATA_INTERNAL_IDX = "PlayerData_Internal";
    const string PLAYERDATA_KEY_PLAYER_DATA_IDX = "PlayerData_Index";

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
        {PLAYERDATA_KEY_PLAYER_DATA,new DataDictionary()
            {
                {PLAYERDATA_KEY_INTERNAL_INDEX,0},
                {PLAYERDATA_KEY_INDEX,0 },
                {PLAYERDATA_KEY_NAME,"" },
                {PLAYERDATA_KEY_IS_VR,false},
                {PLAYERDATA_KEY_PLATFORM,PLAYER_PLATFORM_UNKNOWN },
                {PLAYERDATA_KEY_DEVICE,PLAYER_DEVICE_UNKNOWN }
            }
        },
        {PLAYERDATA_KEY_PLAYER_DATA_INTERNAL_IDX,0},
        {PLAYERDATA_KEY_PLAYER_DATA_IDX,0}
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
        _playerData.SetValue(PLAYERDATA_KEY_INDEX, 9999);//仮の値は9999


        //空き枠を探す
        DataList keys_pDD = playerDataDict.GetKeys();
        //プレイヤーインデックスの一番若いのを調べるための一時的な配列
        int[] _playerIdxs = new int[256];
        //foreachはDataDictionaryに対しては使えないっぽい
        for (int i = 0; i < keys_pDD.Count; i++)
        {
            DataToken key = keys_pDD[i];
            //型をチェックして、DataDictionary型であれば処理
            if (playerDataDict.TryGetValue(key,out DataToken value))
            {
                if (value.TokenType == TokenType.DataDictionary)
                {
                    DataList keys_pD = _playerData.GetKeys();
                    for (int j = 0; j < keys_pD.Count; j++)
                    {
                        DataToken key2 = keys_pD[j];
                        Debug.Log("[PlayerManager]" + keys_pD[j] + ":" + _playerData[keys_pD[j]]);
                    }
                    _playerIdxs[i] = _playerData[PLAYERDATA_KEY_INDEX].Int;
                }
            }
        }
        //一番若いプレイヤーインデックスを見つける
        //ソートがつかえないので直す
        Array.Sort(_playerIdxs);
        //もし初期値のしかなかったらそいつがPlayer1
        if (_playerIdxs[0] == 9999)
        {
            _playerData.SetValue(PLAYERDATA_KEY_INDEX, 1);
        }else
        {
            _playerData.SetValue(PLAYERDATA_KEY_INDEX,_playerIdxs[0]+1);
        }
        Debug.Log("[PlayerManager]" + _playerData[PLAYERDATA_KEY_NAME] + "(InternalIDX:" + _playerData[PLAYERDATA_KEY_INTERNAL_INDEX] + ")は　Player" + _playerData[PLAYERDATA_KEY_INDEX] + "になった");
        playerDataDict.SetValue(PLAYERDATA_KEY_PLAYER_DATA, _playerData);
    }
    /// <summary>
    /// プレイヤーの情報を取得する
    /// </summary>
    /// <param name="player"></param>
    void GetPlayerInfoData(VRCPlayerApi playerData)
    {

    }
    
}

