using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("SFX")]
    [field: SerializeField] public EventReference PickupSound { get; private set; }
    [field: SerializeField] public EventReference UseSound { get; private set; }
    [field: SerializeField] public EventReference Dog { get; private set; }
    [field: SerializeField] public EventReference FlashlightON { get; private set; }
    [field: SerializeField] public EventReference FlashlightOFF { get; private set; }
    [field: SerializeField] public EventReference DoorOpen { get; private set; }
    [field: SerializeField] public EventReference DoorClose { get; private set; }
    [field: SerializeField] public EventReference DoorPantryOpen { get; private set; }
    [field: SerializeField] public EventReference DoorPantryClose { get; private set; }
    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }
    [field: SerializeField] public EventReference playerVoice { get; private set; }
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference Radio { get; private set; }
    [field: SerializeField] public EventReference Traffic { get; private set; }
    [field: SerializeField] public EventReference ambience { get; private set; }


    public static FMODEvents instance {  get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events in the scene.");
        }

        instance = this;
    }
}
