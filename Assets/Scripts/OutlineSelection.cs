using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;
    public float maxDistance = 1f; // Maximum distance for outline visibility

    PhysicsObject physicsObject;

    void Start()
    {
        // Find the PhysicsObject in the scene and assign it to physicsObject
        physicsObject = FindObjectOfType<PhysicsObject>();
        if (physicsObject == null)
        {
            Debug.LogError("PhysicsObject not found in the scene!");
        }
    }


    void Update()
    {
        // Highlight
        if (highlight != null)
        {
            Outline outline = highlight.gameObject.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            highlight = raycastHit.transform;
            float distance = Vector3.Distance(transform.position, highlight.position); // Calculate distance between player and object
            if ((highlight.CompareTag("Phone") 
                || highlight.CompareTag("Charger") 
                || highlight.CompareTag("Object") 
                || highlight.CompareTag("Inspect retrievable") 
                || highlight.CompareTag("Radio") 
                || highlight.CompareTag("Charger") 
                || highlight.CompareTag("CovidTest") 
                || highlight.CompareTag("Facemask") 
                || highlight.CompareTag("Safe Door")
                || highlight.CompareTag("Flashlight")) && highlight != selection)
            {
                if (highlight.gameObject.GetComponent<Outline>() != null && distance > maxDistance)
                {
                    // Check if the object is grabbed, if yes, don't enable the outline
                    if (highlight.gameObject == physicsObject.gameObject && physicsObject.isGrabbed)
                    {
                        return; // Exit the Update method early
                    }

                    highlight.gameObject.GetComponent<Outline>().enabled = true;
                }
                else if (distance <= maxDistance || Input.GetMouseButtonDown(1))
                {
                    // If the player is closer than maxDistance, turn off outline
                    Outline outline = highlight.gameObject.GetComponent<Outline>();
                    if (outline != null)
                        outline.enabled = false;
                }
                else
                {
                    // If the object doesn't have an outline, add it
                    Outline outline = highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = false;
                }
            }
            else
            {
                highlight = null;
            }
        }
    }



}
