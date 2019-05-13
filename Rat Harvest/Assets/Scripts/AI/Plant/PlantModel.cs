using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantModel : MonoBehaviour
{
    private List<GameObject> childrens = new List<GameObject>();
    private List<cakeslice.Outline> outlineScripts = new List<cakeslice.Outline>();

    // Start is called before the first frame update
    void Start()
    {
        FindChildrens();
        FindOutlineScripts();
        DisableOutline();
    }

    // Method to find all the childrens of the gameObject
    private void FindChildrens()
    {
        foreach (Transform child in transform)
        {
            childrens.Add(child.gameObject);
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
