using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRocket : MonoBehaviour
{
    public GameObject rocket;

    private void FixedUpdate()
    {
        if (rocket != null) transform.position = rocket.transform.position;
    }
}
