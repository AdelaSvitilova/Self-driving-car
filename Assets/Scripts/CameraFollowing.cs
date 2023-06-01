using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    private List<CarScript> cars;
    private CarScript target;

    public float smoothSpeed = 0.5f;
    [SerializeField] Vector3 offset;
    [SerializeField] float behindDistance;
    [SerializeField] float upDistance;


    void Start()
    {
        cars = transform.parent.GetComponent<EvolutionScript>().cars;
    }

    private void Update()
    {
        float bestFit = -1f;
        foreach (CarScript car in cars)
        {
            if (car.Distance > bestFit)
            {
                bestFit = car.Distance;
                target = car;
            }
        }

        Vector3 desiredPosition = target.transform.position - target.transform.forward * behindDistance + target.transform.up * upDistance;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        Quaternion desiredRotation = target.transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothSpeed * Time.deltaTime);
    }
}
