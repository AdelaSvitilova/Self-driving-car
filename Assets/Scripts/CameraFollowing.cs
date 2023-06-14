using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera topCamera;
    [SerializeField] CinemachineVirtualCamera startCamera;
    private CinemachineVirtualCamera activeCamera;
    private bool topCameraEnabled;
    private EvolutionScript evolutionScript;

    void Start()
    {
        evolutionScript = GameObject.FindGameObjectWithTag("EvoManager").GetComponent<EvolutionScript>();
        topCameraEnabled = true;
        activeCamera = startCamera;
    }

    private void Update()
    {
        if (!topCameraEnabled)
        {
            FoundCameraToActivate();
        }
    }
    
    public void ResetGeneration()
    {
        if (!topCameraEnabled)
        {
            ActivateCamera(startCamera);
        }
    }
    
    public void ChangeTopCamera()
    {
        if (topCameraEnabled)
        {            
            FoundCameraToActivate();
            topCamera.Priority = 0;
            topCameraEnabled = false;
        }
        else
        {
            topCamera.Priority = 20;
            activeCamera.Priority = 0;
            topCameraEnabled = true;
            activeCamera = topCamera;
        }
    }

    private void FoundCameraToActivate()
    {
        CarScript bestCar = evolutionScript.BestCar;
        CinemachineVirtualCamera cameraToActivate;
        if(bestCar.Fitness > 15)
        {
            cameraToActivate = bestCar.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        }
        else
        {
            cameraToActivate = startCamera;
        }

        if(cameraToActivate != activeCamera)
        {
            ActivateCamera(cameraToActivate);
        }
    }

    private void ActivateCamera(CinemachineVirtualCamera camera)
    {
        camera.Priority = 10;
        activeCamera.Priority = 0;        
        activeCamera = camera;
    }
}
