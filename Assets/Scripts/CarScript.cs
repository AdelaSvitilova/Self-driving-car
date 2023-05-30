using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    private float turn, speed;

    [SerializeField]
    float speedMultiplier, turnMultiplier;

    private Rigidbody rb;

    private float distance = 0f;
    private float duration = 0f;

    private Vector3 firstPosition;
    private Vector3 firstRotation;

    private Vector3 lastPosition;

    [SerializeField]
    float distanceMultiplier, durationMultiplier;

    [SerializeField]
    float maxRayDistance;

    public float Fitness { get; private set; }
    public bool EndRide { get; private set; } = false;

    public NeuralNetwork NeuralNet { get; set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
        NeuralNet = new NeuralNetwork();
        firstPosition = transform.position;
        firstRotation = transform.eulerAngles;
        // uložit všechny potøebné parametry na znovu spuštìní - pozice + rotace
    }

    private void FixedUpdate()
    {
        if (!EndRide)
        {
            UpdateFitnessInformation();
            float forwardSensor = SensorScanning(transform.forward);
            float leftSensor = SensorScanning(transform.forward - transform.right);
            float rightSensor = SensorScanning(transform.forward + transform.right);
            //Debug.Log("L:" + leftSensor + " F:" + forwardSensor + " R:" + rightSensor);

            NeuralNet.Predict(new float[] { leftSensor, forwardSensor, rightSensor }, out turn, out speed);
            //Debug.Log(turn + "   " + speed);
            Move();
        }        
    }

    private void Move()
    {
        //rb.AddRelativeForce(Vector3.forward * speed * speedMultiplier * Time.fixedDeltaTime * 100000f);
        Vector3 currentPosition = transform.position;
        transform.position = currentPosition + transform.forward * speed * speedMultiplier * Time.fixedDeltaTime;

        transform.eulerAngles += new Vector3(0f, turn * turnMultiplier, 0f);

        //Debug.Log((Vector3.forward * speed * speedMultiplier * Time.fixedDeltaTime * 100000f) + "   " + new Vector3(0f, turn * turnMultiplier, 0f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            EndRide = true;
            CalculateFitness();
            rb.isKinematic = true;
            Debug.Log("Auto nabouralo - konec!");            
        }
    }

    private void CalculateFitness()
    {
        float avgSpeed = distance / duration;
        Fitness = distance * distanceMultiplier + avgSpeed * durationMultiplier;
    }

    private void UpdateFitnessInformation()
    {
        Vector3 currentPosition = transform.position;
        float distanceOfPositions = Vector3.Distance(currentPosition, lastPosition);
        distance += distanceOfPositions;
        lastPosition = currentPosition;
        duration += Time.fixedDeltaTime;

        if(duration > 10 && duration < 15 && Vector3.Distance(currentPosition, firstPosition) < 20)
        {
            distance = 0f;
            EndRide = true;
            CalculateFitness();
            rb.isKinematic = true;
        }

        if(distance > 500)
        {
            EndRide = true;
            CalculateFitness();
            rb.isKinematic = true;
        }
    }

    private float SensorScanning(Vector3 direction)
    {
        //nastaveni, aby pocatecni bod snimani nebyl uprostred auta, ale byl v predni casti auta
        Vector3 from = transform.position + (transform.forward * 2f);

        Ray ray = new Ray(from, direction);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, maxRayDistance))
        {
            Debug.DrawRay(from, direction * hit.distance, Color.red);
        }
        else
        {
            Debug.DrawRay(from, direction * maxRayDistance, Color.green);
        }
        return hit.distance / maxRayDistance;
    }

    public void ResetCar()
    {
        transform.position = firstPosition;
        transform.eulerAngles = firstRotation;
        rb.isKinematic = false;

        Fitness = 0f;
        duration = 0f;
        distance = 0f;
        EndRide = false;
    }

}
