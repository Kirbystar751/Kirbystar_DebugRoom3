
using System.Security.Permissions;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class DebugPanel : UdonSharpBehaviour
{
    float defaultRunSpeed;
    float defaultWalkSpeed;
    float defaultStrafeSpeed;
    float defaultJumpPower;

    float RunSpeed;
    float WalkSpeed;
    float StrafeSpeed;
    float JumpPower = 3;
    [SerializeField] GameObject runSpeedDisplay;
    [SerializeField] GameObject wlkSpeedDisplay;
    [SerializeField] GameObject strSpeedDisplay;
    [SerializeField] GameObject jmpPowerDisplay;

    void Start()
    {
        //ワールドデフォルトの移動速度/ジャンプ力を取得する
        defaultRunSpeed = 4; Networking.LocalPlayer.GetRunSpeed();
        defaultWalkSpeed = 2; Networking.LocalPlayer.GetWalkSpeed();
        defaultStrafeSpeed = 2; Networking.LocalPlayer.GetStrafeSpeed();
        defaultJumpPower = 3; Networking.LocalPlayer.GetJumpImpulse();
        Debug.Log("デフォルト設定値\n★はしり：" + defaultRunSpeed.ToString()
            + "\n★あるき：" + defaultWalkSpeed.ToString()
            + "\n★よこ：" + defaultStrafeSpeed.ToString()
            + "\n★ジャンプ：" + defaultJumpPower.ToString());
        //設定値に反映
        RunSpeed = defaultRunSpeed;
        WalkSpeed = defaultWalkSpeed;
        StrafeSpeed = defaultStrafeSpeed;
        JumpPower = defaultJumpPower;
        GUIUpdate();
    }

    private void Update()
    {
        
    }

    public void RunSpeedDec()
    {
        RunSpeed -= 1.0f;
        Networking.LocalPlayer.SetRunSpeed(RunSpeed);
        GUIUpdate();
    }
    public void RunSpeedInc()
    {
        RunSpeed += 1.0f;
        Networking.LocalPlayer.SetRunSpeed(RunSpeed);
        GUIUpdate();
    }
    public void WalkSpeedDec()
    {
        WalkSpeed -= 1.0f;
        Networking.LocalPlayer.SetWalkSpeed(WalkSpeed);
        GUIUpdate();
    }
    public void WalkSpeedInc()
    {
        WalkSpeed += 1.0f;
        Networking.LocalPlayer.SetWalkSpeed(WalkSpeed);
        GUIUpdate();
    }
    public void StrafeSpeedDec()
    {
        StrafeSpeed -= 1.0f;
        Networking.LocalPlayer.SetStrafeSpeed(StrafeSpeed);
        GUIUpdate();
    }
    public void StrafeSpeedInc()
    {
        StrafeSpeed += 1.0f;
        Networking.LocalPlayer.SetStrafeSpeed(StrafeSpeed);
        GUIUpdate();
    }
    public void JumpPowerDec()
    {
        JumpPower -= 1.0f;
        Networking.LocalPlayer.SetJumpImpulse(JumpPower);
        GUIUpdate();
    }
    public void JumpPowerInc()
    {
        JumpPower += 1.0f;
        Networking.LocalPlayer.SetJumpImpulse(JumpPower);
        GUIUpdate();
    }
    public void SpeedReset()
    {
        RunSpeed = defaultRunSpeed;
        WalkSpeed = defaultWalkSpeed; 
        StrafeSpeed = defaultStrafeSpeed;
        JumpPower = defaultJumpPower;

        Networking.LocalPlayer.SetRunSpeed(RunSpeed);
        Networking.LocalPlayer.SetWalkSpeed(WalkSpeed);
        Networking.LocalPlayer.SetStrafeSpeed(StrafeSpeed);
        Networking.LocalPlayer.SetJumpImpulse(JumpPower);

        GUIUpdate();
    }

    private void GUIUpdate()
    {
        Text rsText = runSpeedDisplay.GetComponent<Text>();
        rsText.text = RunSpeed.ToString();
        Text wsText = wlkSpeedDisplay.GetComponent<Text>();
        wsText.text = WalkSpeed.ToString();
        Text ssText = strSpeedDisplay.GetComponent<Text>();
        ssText.text = StrafeSpeed.ToString();
        Text jpText = jmpPowerDisplay.GetComponent<Text>();
        jpText.text = JumpPower.ToString();
    }
}
