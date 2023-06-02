using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollowing : MonoBehaviour
{
    private List<CarScript> cars;
    private CarScript target;
    private CarScript newTarget;

    void Start()
    {
        cars = transform.parent.GetComponent<EvolutionScript>().cars;
        target = cars[0];
        newTarget = null;
    }

    private void Update()
    {
        float bestFit = -1f;
        foreach (CarScript car in cars)
        {
            if (car.Fitness > bestFit && car.Fitness > 3)
            {
                bestFit = car.Fitness;
                newTarget = car;
            }
        }

        if (newTarget && target != newTarget)
        {
            CinemachineVirtualCamera camera2 = newTarget.GetComponentInChildren<CinemachineVirtualCamera>(true);
            camera2.Priority = 10;
            CinemachineVirtualCamera camera = target.GetComponentInChildren<CinemachineVirtualCamera>(true);
            camera.Priority = 1;
            target = newTarget;
        }

    }

    public void MoveToStart()
    {
        CinemachineVirtualCamera camera = target.GetComponentInChildren<CinemachineVirtualCamera>(true);
        camera.Priority = 1;
        newTarget = null;
    }
        
}
