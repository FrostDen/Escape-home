using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdapterScript : MonoBehaviour
{
    public bool OneTime = false, youCan = true, isConnected = false;
    public Transform objectGrabPointTransform;

    public GameObject Adapter;

    public Collider adapterSocket;

    // NearView()
    float distance;
    float angleView;
    Vector3 direction;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isConnected)
        {
            Adapter.transform.position = adapterSocket.transform.position;
            Adapter.transform.rotation = adapterSocket.transform.rotation;
        }

        if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(transform.position, Camera.main.transform.position) <= NearView())
        {
            
            isConnected = false;
        }

    }
    

private void OnTriggerEnter(Collider other)
    {
        if (other == adapterSocket)
        {
            isConnected = true;
        }
        if (OneTime)
            youCan = false;
    }

    float NearView() // it is true if you are near an interactive object
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        direction = transform.position - Camera.main.transform.position;
        angleView = Vector3.Angle(Camera.main.transform.forward, direction);
        float maxDistance = 2f; // Define the maximum distance for the ray
        if (angleView < 5f && distance < maxDistance)
            return maxDistance; // Returning the maximum distance for the ray
        else
            return 0f; // Return 0 if the conditions are not met
    }
}
