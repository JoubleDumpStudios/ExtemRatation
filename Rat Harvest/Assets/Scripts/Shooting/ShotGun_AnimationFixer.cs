using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun_AnimationFixer : MonoBehaviour
{
    [SerializeField]
    private ShotGunBehaviour shotGunScript;
    // Start is called before the first frame update
    void Start()
    {
        shotGunScript = GetComponentInParent<ShotGunBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InsertBullet()
    {
        shotGunScript.InsertBullet();
    }
}
