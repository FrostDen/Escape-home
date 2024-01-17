using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceChangeTrigger : MonoBehaviour
{
    [Header("Area")]
    [SerializeField] private AmbienceArea area;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("Player"))
        {
            AudioManager.instance.SetAmbienceArea(area);
        }
    }
}
