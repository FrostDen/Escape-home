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
        // Set the number of positions in the LineRenderer to match the number of joints
        line.positionCount = joints.Length;
    }

    void Update()
    {
        // Update positions for each joint in the LineRenderer
        for (int i = 0; i < joints.Length; i++)
        {
            line.SetPosition(i, joints[i].transform.position);
        }
    }
}
