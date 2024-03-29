﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class AN_DoorScript : MonoBehaviour
{
    [Tooltip("If it is false door can't be used")]
    public bool Locked = false;
    [Tooltip("It is true for remote control only")]
    public bool Remote = false;
    [Space]
    [Tooltip("Door can be opened")]
    public bool CanOpen = true;
    [Tooltip("Door can be closed")]
    public bool CanClose = true;
    [Space]
    [Tooltip("Door locked by red key (use key script to declarate any object as key)")]
    public bool RedLocked = false;
    public bool BlueLocked = false;
    [Tooltip("It is used for key script working")]
    InventoryItemController inventoryItemController;
    [Space]
    public bool isOpened = false;
    [Range(0f, 4f)]
    [Tooltip("Speed for door opening, degrees per sec")]
    public float OpenSpeed = 3f;



    // NearView()
    float distance;
    float angleView;
    Vector3 direction;

    // Hinge
    [HideInInspector]
    public Rigidbody rbDoor;
    HingeJoint hinge;
    JointLimits hingeLim;
    float currentLim;

    void Start()
    {
        rbDoor = GetComponent<Rigidbody>();
        hinge = GetComponent<HingeJoint>();
        inventoryItemController = FindObjectOfType<InventoryItemController>();

        if (CompareTag("OpenedDoor"))
        {
            isOpened = true;
            rbDoor.AddRelativeTorque(new Vector3(0, 0, 20f));
        }
    }

    void Update()
    {
        if ( !Remote && Input.GetKeyDown(KeyCode.E) && NearView())
        {
            Action();
        }
        
    }

    public void Action() // void to open/close door
    {
        if (!Locked)
        {
            // key lock checking
            if (inventoryItemController != null && RedLocked && inventoryItemController.RedKey)
            {
                RedLocked = false;
                inventoryItemController.RedKey = false;
            }
            else if (inventoryItemController != null && BlueLocked && inventoryItemController.BlueKey)
            {
                BlueLocked = false;
                inventoryItemController.BlueKey = false;
            }
            
            // opening/closing
            if (isOpened && CanClose && !RedLocked && !BlueLocked)
            {
                isOpened = false;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.DoorClose, transform.position);

                if (CompareTag("Pantry Door")) 
                {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.DoorPantryClose, transform.position);
                }
            }
            else if (!isOpened && CanOpen && !RedLocked && !BlueLocked)
            {
                isOpened = true;
                rbDoor.AddRelativeTorque(new Vector3(0, 0, 20f));
                AudioManager.instance.PlayOneShot(FMODEvents.instance.DoorOpen, transform.position);

                if (CompareTag("Pantry Door"))
                {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.DoorPantryOpen, transform.position);
                }

                if (CompareTag("Safe Door"))
                {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.KeypadCorrect, transform.position);
                }
            }

        }
    }

    public void AdjustHingeJointLimits(GameObject obj, float minLimit, float maxLimit)
    {
        // Get all the hinge joint components attached to the GameObject
        HingeJoint[] hingeJoints = obj.GetComponentsInChildren<HingeJoint>();

        // Loop through each hinge joint and adjust its limits
        foreach (HingeJoint hingeJoint in hingeJoints)
        {
            JointLimits limits = hingeJoint.limits;
            limits.min = minLimit;
            limits.max = maxLimit;
            hingeJoint.limits = limits;
        }
    }

    bool NearView() // it is true if you near interactive object
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        direction = transform.position - Camera.main.transform.position;
        angleView = Vector3.Angle(Camera.main.transform.forward, direction);
        if (angleView < 30f && distance < 2f) return true; // angleView < 35f && 
        else return false;
    }

    private void FixedUpdate() // door is physical object
    {
        if (isOpened)
        {
            currentLim = 85f;
        }
        else
        {
            // currentLim = hinge.angle; // door will closed from current opened angle
            if (currentLim > 1f)
                currentLim -= .5f * OpenSpeed;
        }

        // using values to door object
        hingeLim.max = currentLim;
        hingeLim.min = -currentLim;
        hinge.limits = hingeLim;
    }
}
