
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class SGB_ColorSim_TestInterFace : UdonSharpBehaviour
{   
    // ログ用カラーコード
    const string logColorCode = "#FF0080";

    // ログ用プレフィックス
    const string logPrefix = "<color=" + logColorCode + ">[SGB ColorSim Interface]</color>";

    [SerializeField] public InputField passwordField;
    [SerializeField] public SGB_ColorSim_Core core;
    [SerializeField] public GameObject[] colorBoxs = new GameObject[4];
    void Start()
    {
        passwordField.GetComponent<UnityEngine.UI.InputField>();
    }

    public void testButtonPress()
    {
        Debug.Log(logPrefix + "testButtonPress()が呼ばれた");
        core.SGBPassword = passwordField.text;
        core.SendCustomEvent("ev_Pass2Color");
        string colors_Sonomama = core.SGBReturnColors;
        string[] colors = colors_Sonomama.Split(',');

        for (int i = 0; i < 4; i++)
        {
            if(TryParseHexColor(colors[i], out Color color))
            {
                colorBoxs[i].GetComponent<Renderer>().material.color = color;
            }
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