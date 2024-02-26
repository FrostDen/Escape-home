using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class KeypadKey : MonoBehaviour
{
    public string key;
    public Transform keypadTransform;

    public void SendKey()
    {
        this.transform.GetComponentInParent<KeypadController>().PasswordEntry(key);
    }

    public void PlayKeypodBtnSound()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.KeypadBtn, keypadTransform.position);
    }

}
