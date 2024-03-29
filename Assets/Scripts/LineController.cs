using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public HookMove hookMoveScript;
    public GameObject object1;
    public GameObject object2;

    LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();    
    }

    void Update()
    {
        if (lineRenderer != null)
        {
            if (hookMoveScript.GetIsThrown())
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, object1.GetComponent<Transform>().position);
                lineRenderer.SetPosition(1, object2.GetComponent<Transform>().position);
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }
    }
}
