using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManagerScript : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    public void ChangePriority()
    {
        if(virtualCamera.Priority == 20)
        {
            virtualCamera.Priority = 0;
        }
        else
        {
            virtualCamera.Priority = 20;
        }
    }
}
