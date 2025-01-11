
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class GUI_Baku_SpawnMgr : UdonSharpBehaviour
{

    [SerializeField] public VRCObjectPool _pool;
    [SerializeField] AudioClip _spawnSound;
    [SerializeField] AudioClip _errorSound;

    private AudioSource aSource;

    void Start()
    {
        Debug.Log(_pool.Pool.ToString());
        aSource = this.gameObject.GetComponent<AudioSource>();
    }

    public void Baku_SpawnButton_Clik()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "Bakudan_Spawn");
    }

    /// <summary>
    /// バクダンをスポーンさせる
    /// </summary>
    public void Bakudan_Spawn()
    {
        bool p = _pool.TryToSpawn();
        if (p == null)
        {
            Debug.Log("オブジェクトプールがnull");
            aSource.PlayOneShot(_errorSound);

        }
        else
        {
            aSource.PlayOneShot(_spawnSound);
        }
    }

    public void Bakudan_Clear(GameObject bomb)
    {
        Debug.Log("ばくだんをもどしてるよ");
        _pool.Return(bomb);
        Debug.Log("ばくだんもどしたよ");
    }
}
