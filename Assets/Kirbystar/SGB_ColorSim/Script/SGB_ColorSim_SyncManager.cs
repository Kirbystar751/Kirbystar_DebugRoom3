using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class SGB_ColorSim_SyncManager : UdonSharpBehaviour
{
    [UdonSynced] private string syncPassword;

    [SerializeField] private SGB_ColorSim_Core core;
    [SerializeField] private SGB_ColorSim_TestInterFace interfaceUI;

    public void SetPassword(string password)
    {
        if (!Networking.LocalPlayer.IsOwner(gameObject))
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }

        syncPassword = password;
        RequestSerialization();

        // 自分の画面は即反映
        core.SetPassword(syncPassword);
        interfaceUI.colorBoxColorChange();
    }

    public override void OnDeserialization()
    {
        // 他人の画面に反映
        core.SetPassword(syncPassword);
        interfaceUI.colorBoxColorChange();
    }
}
