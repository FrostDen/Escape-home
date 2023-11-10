using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerScript : MonoBehaviour
{
    [Tooltip("Feature for one using only")]
    public bool OneTime = false;
    [Tooltip("Plug follow this local EmptyObject")]
    public Transform objectGrabPointTransform;
    [Tooltip("SocketObject with collider(shpere, box etc.) (is trigger = true)")]
    public Collider[] Sockets;

    private Collider connectedSocket;

    public GameObject Plug;

   

    // NearView()
    float distance;
    float angleView;
    Vector3 direction;

    bool isConnected = false, youCan = true;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //if (youCan) Interaction();

            // frozen if it is connected to PowerOut
            if (isConnected && connectedSocket != null) // Check if there are any sockets
        {
            foreach (Collider socket in Sockets)
            {
                Plug.transform.position = connectedSocket.transform.position; // Position based on each socket
                Plug.transform.rotation = connectedSocket.transform.rotation;
            }

            if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(transform.position, Camera.main.transform.position) <= NearView())
            {
                isConnected = false;
                connectedSocket = null;
            }
        }
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

    // OnTriggerEnter method outside of Update
    private void OnTriggerEnter(Collider other)
    {
        foreach (Collider socket in Sockets)
        {
            if (other == socket)
            {
                isConnected = true;
                connectedSocket = other;
            }
        }
        if (OneTime)
            youCan = false;
    }
    
}
