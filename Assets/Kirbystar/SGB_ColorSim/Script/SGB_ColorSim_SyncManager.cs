using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class SGB_ColorSim_SyncManager : UdonSharpBehaviour
{
    const string logColorCode = "#FF0080";
    const string logPrefix = "<color=" + logColorCode + ">[SGB ColorSim SyncManager]</color>";

    [SerializeField] private SGB_ColorSim_Core core;
    [SerializeField] private SGB_ColorSim_TestInterFace interfaceUI;

    [UdonSynced] public string syncPass;

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

    public void ColorLight(int index)
    {
        core.ColorLight(index);
        syncPass = core.SGBPassword;
        RequestSerialization();
        ApplyState();
    }

    public void ColorDark(int index)
    {
        core.ColorLight(index);
        syncPass = core.SGBPassword;
        RequestSerialization();
        ApplyState();
    }

    public override void OnDeserialization()
    {
        ApplyState(); // 同期受信時反映
    }

    private void ApplyState()
    {
        Debug.Log(logPrefix + "ApplyState() syncPass = " + syncPass);

        core.SetPassword(syncPass);
        interfaceUI.colorBoxColorChange();
    }
}
