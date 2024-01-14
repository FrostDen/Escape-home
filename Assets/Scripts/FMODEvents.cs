using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("SFX")]
    [field: SerializeField] public EventReference PickupSound { get; private set; }
    [field: SerializeField] public EventReference UseSound { get; private set; }
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }
    [field: SerializeField] public EventReference Radio { get; private set; }
    [field: SerializeField] public EventReference traffic { get; private set; }

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
