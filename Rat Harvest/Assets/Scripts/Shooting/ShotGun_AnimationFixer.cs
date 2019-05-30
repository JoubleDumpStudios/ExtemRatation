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
     public void AllowShotingAgain()
    {
        shotGunScript.ShootingAgainAllowed();
    }
    //These are functions to use in amimation events to play Wwise audio.
    public void WwiseShotgunShoot()
    {
        //Wwise play shooting sfx
        AkSoundEngine.PostEvent("Shoot", gameObject);
    }
    public void WWiseShotgunPump()
    {
        //Wwise play pumping sfx
        AkSoundEngine.PostEvent("Pump", gameObject);
    }
    public void WWiseShotgunReload()
    {
        //Wwise play reloading sfx
        AkSoundEngine.PostEvent("Reload", gameObject);
    }
}
