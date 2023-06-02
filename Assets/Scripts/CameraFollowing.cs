using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollowing : MonoBehaviour
{
    private List<CarScript> cars;
    private CarScript target;
    private CarScript targetNew;

    void Start()
    {
        cars = transform.parent.GetComponent<EvolutionScript>().cars;
        target = cars[0];
        target.GetComponentInChildren<CinemachineVirtualCamera>(true).Priority = 10;
    }

    private void Update()
    {
        float bestFit = -1f;
        foreach (CarScript car in cars)
        {
            if (car.Fitness > bestFit)
            {
                bestFit = car.Fitness;
                targetNew = car;
            }
        }

        if (target != targetNew)
        {
            CinemachineVirtualCamera camera2 = targetNew.GetComponentInChildren<CinemachineVirtualCamera>(true);
            camera2.Priority = 10;
            CinemachineVirtualCamera camera = target.GetComponentInChildren<CinemachineVirtualCamera>(true);
            camera.Priority = 1;
            target = targetNew;
        }
    }

        
}
