using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdapterScript : MonoBehaviour
{
    public bool OneTime = false, youCan = true, isConnected = false;
    public Transform objectGrabPointTransform;

    public GameObject Adapter;
    public GameObject Plug;

    public Collider adapterSocket;

    public ChargerScript chargerScript;

    // NearView()
    float distance;
    float angleView;
    Vector3 direction;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (isConnected)
        {
            Adapter.transform.position = adapterSocket.transform.position;
            Adapter.transform.rotation = adapterSocket.transform.rotation;

            // Calculate distance between Adapter and this object (plug)
            float distance = Vector3.Distance(Adapter.transform.position, Plug.transform.position);

            // If distance exceeds 2 units, disconnect both
            if (distance > 3f)
            {
                Disconnect();
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(transform.position, Camera.main.transform.position) <= NearView())
        {
            Disconnect();
        }
    }

    void Disconnect()
    {
        isConnected = false;
        // Additional logic to disconnect both the plug and adapter from the sockets as needed
        // For example:
        // transform.position = someNewPosition; // Move the plug to a new position
        // Adapter.transform.position = someNewPosition; // Move the adapter to a new position
    }


    public void OnTriggerEnter(Collider other)
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
