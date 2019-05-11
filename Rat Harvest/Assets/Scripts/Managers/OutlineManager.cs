using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    // A variable to access to the outline script
    private cakeslice.Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<cakeslice.Outline>();
        DisableOutline();
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }
}
