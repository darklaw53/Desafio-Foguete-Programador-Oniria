using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind : MonoBehaviour
{
    public GameObject windPos, flag;
    public RocketMaster rocketMaster;

    private void Start()
    {
        UpdateDir();
    }

    public void AddWindN()
    {
        if (windPos.transform.localPosition.z < 30)
        {
            windPos.transform.position += Vector3.forward;
            UpdateDir();
        }
    }

    public void AddWindS()
    {
        if (windPos.transform.localPosition.z > -30)
        {
            windPos.transform.position -= Vector3.forward;
            UpdateDir();
        }
    }

    public void AddWindW()
    {
        if (windPos.transform.localPosition.x > -30)
        {
            windPos.transform.position -= Vector3.right;
            UpdateDir();
        }
    }

    public void AddWindE()
    {
        if (windPos.transform.localPosition.x < 30)
        {
            windPos.transform.position += Vector3.right;
            UpdateDir();
        }
    }

    void UpdateDir()
    {
        Vector3 targetPostition = new Vector3(windPos.transform.position.x, flag.transform.position.y, windPos.transform.position.z);
        flag.transform.LookAt(targetPostition);

        rocketMaster.windDirectionInVector3 = flag.transform.position - targetPostition;
    }
}
