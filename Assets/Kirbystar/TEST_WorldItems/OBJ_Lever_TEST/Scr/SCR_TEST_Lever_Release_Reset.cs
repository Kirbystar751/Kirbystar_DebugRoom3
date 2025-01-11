
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SCR_TEST_Lever_Release_Reset : UdonSharpBehaviour
{
    // レバーを手放したときに、元の位置にリセットがかかるようにする

    [SerializeField] VRC_Pickup pickup;
    [SerializeField] GameObject LeverObject;
    [SerializeField] float FollowPower = 10;
    /// <summary>
    /// 手を離した時点のレバーつかみ判定の位置
    /// </summary>
    private Vector3 _lever_Rel_Position;
    /// <summary>
    /// レバーのリセット中かフラグ
    /// </summary>
    private bool _lever_Rel_Flag;

    /// <summary>
    /// レバーアニメタイマー
    /// </summary>
    private float _timer = 0;
    /// <summary>
    /// 動く速さ
    /// </summary>
    float _moveTime = 0.1f;

    /// <summary>
    /// PickUpを手放したときに発動
    /// </summary>
    public override void OnDrop()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "LeverReset");
    }

    public void Update()
    {
        LeverAddforce();

        if (_lever_Rel_Flag)
        {
            //持てなくさせる
            pickup.Drop();
            pickup.pickupable = false;

            _timer += Time.deltaTime;
            float t = _timer / _moveTime;
            pickup.transform.localPosition = Vector3.Lerp(_lever_Rel_Position, Vector3.zero, t);
            if (t> 1.0f)
            {
                _timer = 0.0f;
                _lever_Rel_Flag = false;
                pickup.transform.localRotation = Quaternion.identity;
                pickup.pickupable = true;
            }
        }
    }

    /// <summary>
    /// レバーを当たり判定についていかせる
    /// </summary>
    public void LeverAddforce()
    {

    }

    /// <summary>
    /// レバーの位置を初期状態にする
    /// </summary>
    public void LeverReset()
    {
        _lever_Rel_Flag = true;
        _lever_Rel_Position = pickup.transform.localPosition;
        Debug.Log("Lever_Reseted");
    }

}
