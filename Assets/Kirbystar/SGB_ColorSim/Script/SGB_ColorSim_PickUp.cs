
using UdonSharp;
using UnityEngine;
using VRC.SDK3.ClientSim;
using VRC.SDK3.UdonNetworkCalling;
using VRC.SDKBase;
using VRC.Udon;

public class SGB_ColorSim_PickUp : UdonSharpBehaviour
{   
    // ログ用カラーコード
    const string logColorCode = "#FF0080";

    // ログ用プレフィックス
    const string logPrefix = "<color=" + logColorCode + ">[SGB ColorSim ColorBinPickup]</color>";

    [SerializeField]SGB_ColorSim_ColorBin parent ;
    [SerializeField] SGB_ColorSim_ColorBinController controller;
    [SerializeField]SGB_ColorSim_Core core;
    [SerializeField] ParticleSystem particle;
    [SerializeField]SGB_ColorSim_TestInterFace testInterface;
    [SerializeField]SGB_ColorSim_SyncManager syncManager;
    [SerializeField] AudioClip grabSound;
    [SerializeField] AudioClip releaseSound;
    [SerializeField] AudioClip setSound;
    AudioSource sound;

    //初期位置
    Vector3 initialPos;
    Quaternion initialRot;

    //誰が持っているか
    private VRCPlayerApi grabPlayer;
    [UdonSynced] private int grabPlayerId;
    // Pickupをどっちの手で持っているか
    [UdonSynced] public int syncHand;
    const int HAND_LEFT = 0b01;
    const int HAND_RIGHT = 0b10;

    //どの色を持っているか
    [UdonSynced] public int syncColor;
    [UdonSynced] public Color syncColorC;
    //同期相手に見せるつかみ用オブジェクト
    [SerializeField] GameObject syncPickupDmyObj;
    //インスタンス化されたやつ
    GameObject _syncPickupDmyObj;

    int initDelayTimer = 0;
    bool isInitDelay = false;
    bool isGrab = false;
    int colorCode;
    bool isInColorBox = false;

    //つかんだ当初のカラーコードと色（この情報を使って更新する）
    int grab_ColorCode;
    Color grab_Color;

    void Start()
    {
        Color c = parent.binColor;
        colorCode = parent.binColorCode;
        this.GetComponent<MeshRenderer>().material.color = c;
        Debug.Log(logPrefix + this.gameObject.name + "の色は" +  c);
        sound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if ((initDelayTimer < 10) && !isInitDelay) { initDelayTimer++; }
        if(initDelayTimer >= 10 && !isInitDelay) { delay_Start(); isInitDelay = true; }
        if(parent.isColorChanged)
        {
            palleteColorChange();
            parent.isColorChanged = false;
        }
    }

    private void LateUpdate()
    {
        // 他人の画面であり、かつダミーが存在する場合のみ実行
        if (!Networking.IsOwner(gameObject) && _syncPickupDmyObj != null && grabPlayer != null)
        {
            // ターゲットの手を決定
            VRCPlayerApi.TrackingDataType targetHand = (syncHand == HAND_LEFT)
                ? VRCPlayerApi.TrackingDataType.LeftHand
                : VRCPlayerApi.TrackingDataType.RightHand;

            // 持っているプレイヤーの手の情報を取得
            VRCPlayerApi.TrackingData handData = grabPlayer.GetTrackingData(targetHand);

            // ダミーを追従させる
            _syncPickupDmyObj.transform.position = handData.position;
            _syncPickupDmyObj.transform.rotation = handData.rotation;
        }
    }
    void delay_Start()
    {
        Debug.Log(logPrefix + "初期処理の遅延実行をした");
        palleteColorChange();
        initialPos = gameObject.transform.position;
        initialRot = gameObject.transform.rotation;
    }

    public void palleteColorChange()
    {
        Debug.Log(logPrefix + "パレットの色変更がかかった");
    }
    /// <summary>
    /// つかまれた時に発生
    /// </summary>
    public override void OnPickup()
    {
        Debug.Log(logPrefix + "つかみました");
        this.GetComponent<MeshRenderer>().enabled = true;
        sound.PlayOneShot(grabSound);
        //もう片手でつかめないようにする
        this.GetComponent<VRC_Pickup>().pickupable = false;

        //つかんだ時の色を反映
        Color c = parent.binColor;
        colorCode = parent.binColorCode;
        syncColorC = c;
        GetComponent<MeshRenderer>().material.color = c;
        Debug.Log(logPrefix + this.gameObject.name + "の色は" + c + "、Code:" + colorCode);
        ParticleSystem.MainModule main = particle.main;
        main.startColor = c;

        //相手にもつかんだことを知らせる処理をいれる
        grabPlayer = Networking.LocalPlayer;
        grabPlayerId = grabPlayer.playerId;
        VRC_Pickup pickup = (VRC_Pickup)this.GetComponent(typeof(VRC_Pickup));
        syncColor = colorCode;
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Others, "GrabEvent");

        //持ち手を判定する
        if (grabPlayer != null)
        {
            Vector3 lp = grabPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand).position;
            Vector3 rp = grabPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).position;
            if(Vector3.Distance(transform.position, lp) < Vector3.Distance(transform.position, rp))
            {
                syncHand = HAND_LEFT;
                Debug.Log(logPrefix + "左手で持ちました");
            }
            else
            {
                syncHand = HAND_RIGHT;
                Debug.Log(logPrefix + "右手で持ちました");
            }
            RequestSerialization();
        }
    }

    public override void OnDeserialization()
    {
        //自分が操作したなら何もしない
        if (Networking.IsOwner(gameObject)) { return; }
        //IDからプレイヤーを特定
        if (grabPlayerId != -1)
        {
            grabPlayer = VRCPlayerApi.GetPlayerById(grabPlayerId);
        }
        else
        {
            grabPlayer = null;
        }
        //誰かが持っているか
        bool isGrabOther = (syncHand !=0 && grabPlayer != null);
        if (isGrabOther)
        {
            //ダミーがないなら生成
            if(_syncPickupDmyObj == null && syncPickupDmyObj != null)
            {
                _syncPickupDmyObj = Instantiate(syncPickupDmyObj,this.gameObject.transform.parent.transform,false);
                //色などを反映
                MeshRenderer dmyRender = _syncPickupDmyObj.GetComponent<MeshRenderer>();
                if (dmyRender != null)
                {
                    dmyRender.enabled = true;
                    dmyRender.material.color = syncColorC;
                }
            }
        }else
        {
            //手放されたのでダミーがあれば消す
            if (_syncPickupDmyObj != null)
            {
                Destroy(_syncPickupDmyObj);
                _syncPickupDmyObj = null;
            }
        }
    }

    /// <summary>
    /// 離したときに発生
    /// </summary>
    public override void OnDrop()
    {
        grabPlayer = null;
        syncHand = 0;
        grabPlayerId = -1;
        RequestSerialization();
        //色ボックスに触れずに手放したときだけ
        if (!isInColorBox)
        {
            Debug.Log(logPrefix + "離しました");
            Vector3 particlePos = gameObject.transform.position;
            particle.transform.position = particlePos;
            particle.Play();
            sound.PlayOneShot(releaseSound);
            this.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.transform.position = initialPos;
            this.gameObject.transform.rotation = initialRot;
            //つかめる状態を戻す
            this.GetComponent<VRC_Pickup>().pickupable = true;
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Others, "DropEvent");
        }
    }

    /// <summary>
    /// 相手が色をセットしたときに発生するイベント
    /// </summary>
    [NetworkCallable]
    public void ColorSetEvent()
    {
        sound.PlayOneShot(setSound);
    }
    //相手が色をつかんだ時に起きるイベント
    [NetworkCallable]
    public void GrabEvent()
    {
        sound.PlayOneShot(grabSound);
    }
    [NetworkCallable]
    public void DropEvent()
    {
        sound.PlayOneShot(releaseSound);
    }
    /// <summary>
    /// コライダーと当たったときに発生
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {

        if (other != null)
        {

            //色ボックスと当たったときだけ処理
            if (other.gameObject.name == "SGB_ColorHitBox")
            {
                //衝突相手の取得
                string hitObjName = other.gameObject.transform.parent.gameObject.name;
                Debug.Log(logPrefix + hitObjName + "と衝突が起きた");

                if (hitObjName.Contains("ColorBox_"))
                {
                    int colorBoxPosition = int.Parse(hitObjName.Substring(9));
                    Debug.Log(logPrefix + "色ボックス" + colorBoxPosition + "番と当たったよ");
                    isInColorBox = true;

                    //パスワード生成
                    string pass = core.SGBPassword;
                    string rawpass = "";
                    string newpass = "";

                    Debug.Log(logPrefix + "core is null? " + (core == null));
                    Debug.Log(logPrefix + "password = " + (core != null ? core.SGBPassword : "core null"));
                    Debug.Log(logPrefix + "現在のパスワードは" + pass);

                    switch (colorBoxPosition)
                    {
                        case 1:
                            //最初に、ハイフンを取る
                            rawpass = pass.Replace("-", "");
                            //１文字～３文字を置き換え
                            rawpass = rawpass.Remove(0, 3).Insert(0, colorCode.ToString("D3"));
                            //またハイフンを入れる
                            newpass = rawpass.Substring(0, 4) + "-" + rawpass.Substring(4, 4) + "-" + rawpass.Substring(8, 4);
                            //core.SGBPassword = newpass;
                            //core.SetPassword(newpass);
                            syncManager.syncKind = SGB_ColorSim_SyncManager.SYNC_KIND_PASSWORD;
                            syncManager.SetPassword(newpass);
                            break;
                        case 2:
                            //最初に、ハイフンを取る
                            rawpass = pass.Replace("-", "");
                            //４文字～６文字を置き換え
                            rawpass = rawpass.Remove(3, 3).Insert(3, colorCode.ToString("D3"));
                            //またハイフンを入れる
                            newpass = rawpass.Substring(0, 4) + "-" + rawpass.Substring(4, 4) + "-" + rawpass.Substring(8, 4);
                            //core.SGBPassword = newpass;
                            //core.SetPassword(newpass);
                            syncManager.syncKind = SGB_ColorSim_SyncManager.SYNC_KIND_PASSWORD;
                            syncManager.SetPassword(newpass);
                            break;
                        case 3:
                            //最初に、ハイフンを取る
                            rawpass = pass.Replace("-", "");
                            //７文字～９文字を置き換え
                            rawpass = rawpass.Remove(6, 3).Insert(6, colorCode.ToString("D3"));
                            //またハイフンを入れる
                            newpass = rawpass.Substring(0, 4) + "-" + rawpass.Substring(4, 4) + "-" + rawpass.Substring(8, 4);
                            //core.SGBPassword = newpass;
                            //core.SetPassword(newpass);
                            syncManager.syncKind = SGB_ColorSim_SyncManager.SYNC_KIND_PASSWORD;
                            syncManager.SetPassword(newpass);
                            break;
                        case 4:
                            //最初に、ハイフンを取る
                            rawpass = pass.Replace("-", "");
                            //10文字～12文字を置き換え
                            rawpass = rawpass.Remove(9, 3).Insert(9, colorCode.ToString("D3"));
                            //またハイフンを入れる
                            newpass = rawpass.Substring(0, 4) + "-" + rawpass.Substring(4, 4) + "-" + rawpass.Substring(8, 4);
                            //core.SGBPassword = newpass;
                            //core.SetPassword(newpass);
                            syncManager.syncKind = SGB_ColorSim_SyncManager.SYNC_KIND_PASSWORD;
                            syncManager.SetPassword(newpass);
                            break;
                        default:
                            break;
                    }
                    testInterface.colorBoxColorChange();

                    Debug.Log(logPrefix + "パスワードを" + newpass + "に変更した");

                    //色の塊を元の位置に戻す
                    VRC_Pickup pickup = (VRC_Pickup)this.GetComponent(typeof(VRC_Pickup));
                    pickup.Drop(); 
                    sound.PlayOneShot(setSound);
                    //相手にもセットサウンドを鳴らす処理をいれる
                    SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Others, "ColorSetEvent");
                    this.GetComponent<MeshRenderer>().enabled = false;
                    this.gameObject.transform.position = initialPos;
                    this.gameObject.transform.rotation = initialRot;
                    isInColorBox = false;
                    //つかめる状態を戻す
                    this.GetComponent<VRC_Pickup>().pickupable = true;
                }
            }
        }
    }
}
