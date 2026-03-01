
using UdonSharp;
using UdonSharp.Examples.Utilities;
using UnityEngine;
using VRC.SDK3.UdonNetworkCalling;
using VRC.SDKBase;
using VRC.Udon;

public class SGB_ColorSim_ColorBinController : UdonSharpBehaviour
{
    // ログ用カラーコード
    const string logColorCode = "#FF0080";

    // ログ用プレフィックス
    const string logPrefix = "<color=" + logColorCode + ">[SGB ColorSim ColorBinController]</color>";

    //絵の具のビンのページ（横に置いてあるパレットをUseで切り替え、1～5ページまである）
    public int ColorBinIndex = 0;

    public SGB_ColorSim_ColorBin[] ColorBins = new SGB_ColorSim_ColorBin[12]; //絵の具のビンのGameObjectを格納する配列
    [SerializeField] public AudioClip BinChangeSound;
    AudioSource Sound;

    //SGBマイカラー画面でのパレット配列
    //５２色あります　白と黒はどのパレットでも共通
    int[] colorBinPallete1 = new int[12] { 242,192,193,194,195,196,
                                            243,197,198,199,200,201};
    int[] colorBinPallete2 = new int[12] { 242,202,203,204,205,206,
                                            243,207,208,209,210,211};
    int[] colorBinPallete3 = new int[12] { 242,212,213,214,215,216,
                                            243,217,218,219,220,221};
    int[] colorBinPallete4 = new int[12] { 242,222,223,224,225,226,
                                            243,227,228,229,230,231};
    int[] colorBinPallete5 = new int[12] {242,232,233,234,235,236,
                                            243,237,238,239,240,241};

    [SerializeField] public SGB_ColorSim_Core sgbCore;
    [SerializeField] public SGB_ColorSim_SyncManager syncManager;

    public Animator palAnim;

    void Start()
    {
        Sound = GetComponent<AudioSource>();
        palAnim = GetComponent<Animator>();

        //変数が変わったことが通知されると勝手に同期されるからStart()内で同期変数はいじらない
        ColorBinChange();
        //syncManager.syncKind = SGB_ColorSim_SyncManager.SYNC_KIND_PALLETE_CHANGE;
        //syncManager.SetColorBinIndex(ColorBinIndex);
    }

    public override void Interact()
    {
        NextColorBin();
        //syncManager.SetColorBinIndex(ColorBinIndex);
        Sound.PlayOneShot(BinChangeSound);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Others, "InteractEvent");
    }

    [NetworkCallable]
    public void InteractEvent()
    {
        Sound.PlayOneShot(BinChangeSound);
    }

    public void NextColorBin()
    {
        ColorBinIndex++;
        if (ColorBinIndex > 4)
        {
            ColorBinIndex = 0;
        }
        syncManager.syncKind = SGB_ColorSim_SyncManager.SYNC_KIND_PALLETE_CHANGE;
        syncManager.SetColorBinIndex(ColorBinIndex);
        ColorBinChange();
    }

    /// <summary>
    /// 絵の具のビンの色を指定のインデックスに切り替える（ネットワーク呼び出し用）
    /// </summary>
    public void SetColorBin(int index)
    {
        Debug.Log(logPrefix + "SetColorBinが呼ばれた ColorBinIndex = " + ColorBinIndex);
        ColorBinIndex = index;
        ColorBinChange();
        //Sound.PlayOneShot(BinChangeSound);
    }

    public void ColorBinChange()
    {
        switch (ColorBinIndex)
        {
            case 0:
                for (int i = 0; i <= 11; i++)
                {
                    GameObject colorObj = ColorBins[i].transform.Find("Color").gameObject;
                    TryParseHexColor(sgbCore.SGBNormalColorTable[colorBinPallete1[i]].String, out Color color);
                    colorObj.GetComponent<Renderer>().material.color = color;
                    ColorBins[i].binColor = color;
                    ColorBins[i].binColorCode = colorBinPallete1[i];
                    ColorBins[i].isColorChanged = true;
                }
                break;
            case 1:
                for (int i = 0; i <= 11; i++)
                {
                    GameObject colorObj = ColorBins[i].transform.Find("Color").gameObject;
                    TryParseHexColor(sgbCore.SGBNormalColorTable[colorBinPallete2[i]].String, out Color color);
                    colorObj.GetComponent<Renderer>().material.color = color;
                    ColorBins[i].binColor = color;
                    ColorBins[i].binColorCode = colorBinPallete2[i];
                    ColorBins[i].isColorChanged = true;
                }
                break;
            case 2:
                for (int i = 0; i <= 11; i++)
                {
                    GameObject colorObj = ColorBins[i].transform.Find("Color").gameObject;
                    TryParseHexColor(sgbCore.SGBNormalColorTable[colorBinPallete3[i]].String, out Color color);
                    colorObj.GetComponent<Renderer>().material.color = color;
                    ColorBins[i].binColor = color;
                    ColorBins[i].binColorCode = colorBinPallete3[i];
                    ColorBins[i].isColorChanged = true;
                }
                break;
            case 3:
                for (int i = 0; i <= 11; i++)
                {
                    GameObject colorObj = ColorBins[i].transform.Find("Color").gameObject;
                    TryParseHexColor(sgbCore.SGBNormalColorTable[colorBinPallete4[i]].String, out Color color);
                    colorObj.GetComponent<Renderer>().material.color = color;
                    ColorBins[i].binColor = color;
                    ColorBins[i].binColorCode = colorBinPallete4[i];
                    ColorBins[i].isColorChanged = true;
                }
                break;
            case 4:
                for (int i = 0; i <= 11; i++)
                {
                    GameObject colorObj = ColorBins[i].transform.Find("Color").gameObject;
                    TryParseHexColor(sgbCore.SGBNormalColorTable[colorBinPallete5[i]].String, out Color color);
                    colorObj.GetComponent<Renderer>().material.color = color;
                    ColorBins[i].binColor = color;
                    ColorBins[i].binColorCode = colorBinPallete5[i];
                    ColorBins[i].isColorChanged = true;
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// TryParseHTMLStringの自前実装版
    /// </summary>
    /// <param name="hex"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static bool TryParseHexColor(string hex, out Color color)
    {
        color = Color.black;

        if (string.IsNullOrEmpty(hex))
            return false;

        // 先頭の # を除去
        if (hex[0] == '#')
            hex = hex.Substring(1);

        int len = hex.Length;
        if (len != 6 && len != 8)
            return false;

        // 16進数としてパース
        uint value;
        if (!uint.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out value))
            return false;

        if (len == 6)
        {
            color = new Color(
                ((value >> 16) & 0xFF) / 255f,
                ((value >> 8) & 0xFF) / 255f,
                (value & 0xFF) / 255f,
                1f
            );
        }
        else // 8
        {
            color = new Color(
                ((value >> 24) & 0xFF) / 255f,
                ((value >> 16) & 0xFF) / 255f,
                ((value >> 8) & 0xFF) / 255f,
                (value & 0xFF) / 255f
            );
        }

        return true;
    }

}
