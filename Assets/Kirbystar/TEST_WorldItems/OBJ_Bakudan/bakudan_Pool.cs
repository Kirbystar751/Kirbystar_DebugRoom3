
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class bakudan_Pool : UdonSharpBehaviour
{
    GameObject clashEft;
    Animator anim;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]public GUI_Baku_SpawnMgr spawnMgr;
    /// <summary>
    /// 地形に当たったとき爆発するのに必要な勢い
    /// </summary>
    public float baku_Tuyosa = 1f;
    /// <summary>
    /// １回でもつかまれるとtureになる（スポーン後誤爆防止）
    /// </summary>
    public bool isPickup = false;

    private Transform initialPosition;

    void Start()
    {
        clashEft = transform.Find("Bomb_Hanabi").gameObject;
        anim = GetComponent<Animator>();
        initialPosition = this.gameObject.transform;
    }

    /// <summary>
    /// コリジョン接触（地形衝突）検知時
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb;
        Vector3 velocity;

        rb = this.gameObject.GetComponent<Rigidbody>();
        //指定した勢い以上で地形にぶつけたら爆発処理を呼ぶ
        if (rb.velocity.magnitude > baku_Tuyosa)
        {
            bakuhatu();
        }
    }
    /// <summary>
    /// つかみ時
    /// </summary>
    public override void OnPickup()
    {
        //つかみフラグたてる
        isPickup = true;
    }
    /// <summary>
    /// 爆発処理
    /// </summary>
    void bakuhatu()
    {
        //つかみフラグがtrue = １回でも最低はつかんでいたら爆発させる
        if (isPickup)
        {
            clashEft.GetComponent<ParticleSystem>().Play();

            AudioSource aSource = this.gameObject.GetComponent<AudioSource>();
            aSource.Play();
            anim.SetBool("Boomb", true);
        }
    }

    /// <summary>
    /// 爆弾を消す処理
    /// </summary>
    public void BombClear()
    {
        Debug.Log("ばくだんをもどすよ");

        //if (spawnMgr == null) { Debug.Log("Nullです"); }
        
        //もろもろをリセット
        //見た目とかコライダーはアニメーションで戻してる
        //
        //つかめるフラグとアニメーションフラグ
        isPickup=false;
        anim.SetBool("Boomb", false);
        //勢い
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        //位置と回転
        this.gameObject.transform.position = initialPosition.position;
        this.gameObject.transform.rotation= initialPosition.rotation;
        //消去（といういかオブジェクトプールに返却）
        spawnMgr.Bakudan_Clear(this.gameObject);
    }

}
