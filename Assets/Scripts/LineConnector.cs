using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnector : MonoBehaviour
{
    public GameObject[] joints;

    private LineRenderer line;

    void Start()
    {
        line = this.gameObject.GetComponent<LineRenderer>();
    }

    void Update()
    {
        for(int i = 0; i < joints.Length; i++) {
            line.SetPosition(i, joints[i].transform.position);
        }

    }
}
