using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class trafficSound : MonoBehaviour
{
    private StudioEventEmitter emitter;

    // Start is called before the first frame update
    void Start()
    {
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.Traffic, this.gameObject);
        emitter.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
