
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Star_Generator : UdonSharpBehaviour
{
    public GameObject target_Generate;
    public float interval = 30.0f;

    private float interval_Cnt = 0.0f;

    public float fallRange_X = 10.0f;
    public float fallRange_Z = 10.0f;
    public float fallTakasa_Y = 15.0f;

    void Start()
    {
        
    }

    public void Update()
    {
        if (interval_Cnt > interval)
        {
            interval_Cnt = 0;
            GameObject ob = Instantiate(target_Generate,new Vector3(Random.Range(0,fallRange_X),fallTakasa_Y, Random.Range(0, fallRange_Z))+this.gameObject.transform.position,Quaternion.Euler(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360)));
            ob.SetActive(true);
        }
        interval_Cnt += Time.deltaTime;
    }
}
