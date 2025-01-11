
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Bakudan_Generator : UdonSharpBehaviour
{
    public GameObject target_Generate;

    void Start()
    {
        
    }

    public override void Interact()
    {
        //Object_Generate();
        //場にいる全員にむけて生成処理発動
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Object_Generate");
    }

    public void Object_Generate()
   {
        //インスタンス化して生成
        GameObject ob = Instantiate(target_Generate,this.gameObject.transform.position,Quaternion.identity);
        //生成されたオブジェクトを有効化
        ob.SetActive(true);
    }
}
