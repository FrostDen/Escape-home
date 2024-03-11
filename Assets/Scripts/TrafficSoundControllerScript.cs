using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class TrafficSoundControllerScript : MonoBehaviour
{
    [SerializeField] private string parameterName = "toggle_Traffic";
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set the traffic parameter to 1 when player enters the collider
            audioManager.SetParameter(parameterName, 1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set the traffic parameter to 0 when player exits the collider
            audioManager.SetParameter(parameterName, 0);
        }
    }
}