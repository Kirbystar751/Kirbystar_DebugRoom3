
using Cysharp.Threading.Tasks.Triggers;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class bakudan : UdonSharpBehaviour
{
    GameObject clashEft;
    Animator anim;
    /// <summary>
    /// 地形に当たったとき爆発するのに必要な勢い
    /// </summary>
    public float baku_Tuyosa = 1f;
    /// <summary>
    /// １回でもつかまれるとtureになる（スポーン後誤爆防止）
    /// </summary>
    private bool isPickup = false;
    void Start()
    {
        clashEft = transform.Find("Bomb_Hanabi").gameObject;
        anim = GetComponent<Animator>();

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
        if (rb.velocity.magnitude> baku_Tuyosa) 
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
        foreach (Transform s in gameObject.transform)
        {
            Destroy(s.gameObject);
        }
        Destroy(gameObject);
    }

}
