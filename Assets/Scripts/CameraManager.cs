using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera cam1, cam2, cam3;
    public MeshRenderer skySphere;
    GameObject mainCam;

    private void Start()
    {
        mainCam = cam1.gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (cam1.Priority == 11)
            {
                cam1.Priority = 10;
                cam2.Priority = 11;
                mainCam = cam2.gameObject;
            }
            else if (cam2.Priority == 11)
            {
                cam2.Priority = 10;
                cam3.Priority = 11;
                mainCam = cam3.gameObject;
            }
            else if (cam3.Priority == 11)
            {
                cam3.Priority = 10;
                cam1.Priority = 11;
                mainCam = cam1.gameObject;
            }
        }

        skySphere.gameObject.transform.position = mainCam.transform.position;
        float opacity = 1 - (mainCam.transform.position.y - 100) / 1000;
        if (opacity <= 0) opacity = 0;
        if (opacity >= 1) opacity = 1;
    
        if (mainCam.transform.position.y > 100)
        {
            skySphere.material.color = new Color(1, 1, 1, opacity);
        }
    }
}
