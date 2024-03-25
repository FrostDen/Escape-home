using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightPoint : MonoBehaviour
{
    public GameObject Target;
    public float Speed;
    public PlayerInteractions playerInteractions;

    public bool JustOnStart = false;

    // turn towards target
    void Start()
    {
        Vector3 dir = Target.transform.position - transform.position;
        Quaternion Rotation = Quaternion.LookRotation(dir);

        gameObject.transform.rotation = Rotation;
    }

    // turn towards target
    void FixedUpdate()
    {
        if (JustOnStart == false && Target != null && playerInteractions.currentlyPickedUpObject != null && playerInteractions.currentlyPickedUpObject.CompareTag("Flashlight"))
        {
            Vector3 dir = Target.transform.position - transform.position;
            Quaternion Rotation = Quaternion.LookRotation(dir);

            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Rotation, Speed * Time.deltaTime);
        }
    }
}
