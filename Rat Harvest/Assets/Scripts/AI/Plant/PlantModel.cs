using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantModel : MonoBehaviour
{
    private List<Transform> childrens;
    private List<cakeslice.Outline> outlineScripts;

    // Start is called before the first frame update
    void Start()
    {
        FindChildrens();
        //FindOutlineScripts();
    }

    // Method to find all the childrens of the gameObject
    private void FindChildrens()
    {
        foreach (Transform child in transform)
        {
            Debug.Log(child.name);
            childrens.Add(child);
        }
    }

    // Method to store all the outlines scripts
    private void FindOutlineScripts()
    {
        for (int i = 0; i < childrens.Count; i++)      
            outlineScripts.Add(childrens[i].GetComponent<cakeslice.Outline>());
    }

    // Method to enable outlineScripts
    public void EnableOutline()
    {
        for (int i = 0; i < outlineScripts.Count; i++)
            outlineScripts[i].enabled = true;
    }

    // Method to disable outlineScripts
    public void DisableOutline()
    {
        for (int i = 0; i < outlineScripts.Count; i++)
            outlineScripts[i].enabled = false;
    }
}
