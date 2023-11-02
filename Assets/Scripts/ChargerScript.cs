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
    //public AN_DoorScript DoorObject;

    private Collider connectedSocket;

    public float pickUpDistance = 2f;

    // NearView()
    float distance;
    float angleView;
    Vector3 direction;

    bool follow = false, isConnected = false, followFlag = false, youCan = true;
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
                gameObject.transform.position = connectedSocket.transform.position; // Position based on each socket
                gameObject.transform.rotation = connectedSocket.transform.rotation;
            }

            if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(transform.position, Camera.main.transform.position) <= pickUpDistance)
            {
                isConnected = false;
                connectedSocket = null;
            }
            //else
            //{
            //    DoorObject.isOpened = false;
            //}
        }
    }

   

    bool NearView() // it is true if you near interactive object
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        direction = transform.position - Camera.main.transform.position;
        angleView = Vector3.Angle(Camera.main.transform.forward, direction);
        if (distance < 3f && angleView < 35f)
            return true;
        else
            return false;
    }

    // OnTriggerEnter method outside of Update
    private void OnTriggerEnter(Collider other)
    {
        foreach (Collider socket in Sockets)
        {
            if (other == socket)
            {
                isConnected = true;
                follow = false;
                connectedSocket = other;
            }
        }
        if (OneTime)
            youCan = false;
    }
}
