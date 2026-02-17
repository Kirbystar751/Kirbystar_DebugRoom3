
using UdonSharp;
using UnityEngine;
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
    
    [SerializeField] AudioClip grabSound;
    [SerializeField] AudioClip releaseSound;
    [SerializeField] AudioClip setSound;
    AudioSource sound;

    //初期位置
    Vector3 initialPos;
    Quaternion initialRot;


    int initDelayTimer = 0;
    bool isInitDelay = false;
    bool isGrab = false;
    int colorCode;
    bool isInColorBox = false;

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
        Color c = parent.binColor;
        colorCode = parent.binColorCode;
        GetComponent<MeshRenderer>().material.color = c;
        Debug.Log(logPrefix + this.gameObject.name + "の色は" + c + "、Code:" + colorCode);

        ParticleSystem.MainModule main = particle.main;
        main.startColor = c;
    }
    /// <summary>
    /// つかまれた時に発生
    /// </summary>
    public override void OnPickup()
    {
        Debug.Log(logPrefix + "つかみました");
        this.GetComponent<MeshRenderer>().enabled = true;
        sound.PlayOneShot(grabSound);
    }
    /// <summary>
    /// 離したときに発生
    /// </summary>
    public override void OnDrop()
    {
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
        }
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
                            core.SetPassword(newpass);
                            break;
                        case 2:
                            //最初に、ハイフンを取る
                            rawpass = pass.Replace("-", "");
                            //４文字～６文字を置き換え
                            rawpass = rawpass.Remove(3, 3).Insert(3, colorCode.ToString("D3"));
                            //またハイフンを入れる
                            newpass = rawpass.Substring(0, 4) + "-" + rawpass.Substring(4, 4) + "-" + rawpass.Substring(8, 4);
                            //core.SGBPassword = newpass;
                            core.SetPassword(newpass);
                            break;
                        case 3:
                            //最初に、ハイフンを取る
                            rawpass = pass.Replace("-", "");
                            //７文字～９文字を置き換え
                            rawpass = rawpass.Remove(6, 3).Insert(6, colorCode.ToString("D3"));
                            //またハイフンを入れる
                            newpass = rawpass.Substring(0, 4) + "-" + rawpass.Substring(4, 4) + "-" + rawpass.Substring(8, 4);
                            //core.SGBPassword = newpass;
                            core.SetPassword(newpass);
                            break;
                        case 4:
                            //最初に、ハイフンを取る
                            rawpass = pass.Replace("-", "");
                            //10文字～12文字を置き換え
                            rawpass = rawpass.Remove(9, 3).Insert(9, colorCode.ToString("D3"));
                            //またハイフンを入れる
                            newpass = rawpass.Substring(0, 4) + "-" + rawpass.Substring(4, 4) + "-" + rawpass.Substring(8, 4);
                            //core.SGBPassword = newpass;
                            core.SetPassword(newpass);
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
                    this.GetComponent<MeshRenderer>().enabled = false;
                    this.gameObject.transform.position = initialPos;
                    this.gameObject.transform.rotation = initialRot;
                    isInColorBox = false; 
                }
            }
        }
    }
}
