using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Bowling : MonoBehaviour 
{
    private Dictionary<GameObject, Vector3> pinStartPosDic;
    private Dictionary<GameObject, Quaternion> pinStartRotDic;
    private Dictionary<GameObject, Vector3> ballStartPosDic;

    [SerializeField] private GameObject[] balls;
    [SerializeField] private GameObject[] pins;

    void Start()
    {
        ballStartPosDic = new Dictionary<GameObject, Vector3>();

        foreach(GameObject obj in balls)
        {
            ballStartPosDic.Add(obj, obj.transform.position);
        }

        pinStartPosDic = new Dictionary<GameObject, Vector3>();
        pinStartRotDic = new Dictionary<GameObject, Quaternion>();

        foreach (GameObject obj in pins)
        {
            pinStartPosDic.Add(obj, obj.transform.position);
            pinStartRotDic.Add(obj, obj.transform.rotation);
        }
    }

    public void ResetBalls()
    {
        foreach(GameObject obj in ballStartPosDic.Keys)
        {
            obj.transform.position = ballStartPosDic[obj];
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void ResetPins()
    {
        foreach (GameObject obj in pinStartPosDic.Keys)
        {
            obj.transform.position = pinStartPosDic[obj];
            obj.transform.rotation = pinStartRotDic[obj];
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
