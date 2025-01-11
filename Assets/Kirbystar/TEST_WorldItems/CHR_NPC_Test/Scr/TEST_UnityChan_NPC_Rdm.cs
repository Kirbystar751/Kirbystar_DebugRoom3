
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.AI;
using System;
using System.Net.Configuration;

public class TEST_UnityChan_NPC_Rdm : UdonSharpBehaviour
{
    /// <summary>
    /// 対称のナビメッシュ
    /// </summary>
    [SerializeField] public NavMeshAgent agent;
    /// <summary>
    /// ランダムに対象位置を変える時間
    /// </summary>
    [SerializeField] public int randamPosGenerateFrame = 120;

    [SerializeField]public float randamPos_Range = 8f;
    /// <summary>
    /// カウンター
    /// </summary>
    private int randamPos_Counter=0;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 向かうべき座標
    /// </summary>
    private Vector3 randamPos;
    private void Update()
    {
        if (randamPos_Counter >randamPosGenerateFrame)
        {
            randamPos.x = (agent.transform.position.x) + ((UnityEngine.Random.value * randamPos_Range) - (randamPos_Range/2));
            randamPos.z = (agent.transform.position.z) + ((UnityEngine.Random.value * randamPos_Range) - (randamPos_Range/2));
            randamPos_Counter = 0;
        }
        agent.SetDestination(randamPos);
        
        randamPos_Counter++;
        animator.SetFloat("Move", agent.velocity.magnitude);
    }
}
