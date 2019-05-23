using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class ShotGunBehaviour : MonoBehaviour
{

    FirstPersonController firstPersonControllerScript_;
    private PlayerManager playerManagerScript_;
    public GameObject player;

    //Shoting properties variables
    [SerializeField]
    [Range(0.0f, 100.0f)]
    private int weaponRange;

    [SerializeField]
    private int bulletsCount;
    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    public float NotAimingSpreadAngle;
    [SerializeField]
    public float AimingSpreadAngle;

    public float spreadAngle;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform Barrel;

    [SerializeField]
    private bool raycasted;

    List<Quaternion> bullets;

    [SerializeField]
    private float damage;

    RaycastHit hit;
    Ray ray;


    [SerializeField]
    private float weaponRecoilingTime = 0.2f;
    [SerializeField]
    private float weaponRecoilAmount = 0.2f;

    [SerializeField]
    private float weaponRecoilAngle = 20f;

    [SerializeField]
    private float cameraRecoilSpeed;
    [SerializeField]
    private float cameraRecoilingTime;

    private float currentRecoilPosition;
    private float currentRecoilAngle;
    private float currentRecoilPositionSpeed;
    private float currentRecoilAngleSpeed;

    public Transform originalTransform;

    [SerializeField]
    private float angleForCameraRecoil = 15;

    [SerializeField]
    private Transform aimPositionTransform;

    Transform notAimingPosition;

    [SerializeField]
    private float aimingSpeed;


    //Shooting Logic - FireRate
    private bool shooting = false;

    private ObjectPooler objectPooler;

    private bool waitingForNewShot;
    float shootTime = 0;

    [SerializeField]
    private float timeBetweenShoots;

    //Shoting Special Efects
    [SerializeField]
    private ParticleSystem gunFire1;

    [SerializeField]
    private ParticleSystem gunFire2;

    //Hit effects
    [SerializeField]
    private GameObject holes;
    private GameObject holes_;

    [SerializeField]
    private GameObject dirtHolesParticleEffects;

    [SerializeField]
    private GameObject sandHolesParticleEffects;

    [SerializeField]
    private GameObject woodHolesParticleEffects;

    [SerializeField]
    private GameObject metalHolesParticleEffects;

    [SerializeField]
    private GameObject ratHoles;
    private GameObject ratHole_;

    [SerializeField]
    private GameObject ratHolesParticleEffects;

    //Reloading Logic Variables and ammo
    private bool reloading;

    private float reloadTime = 0;

    public int maxAmmo;
    public int Ammo;

    [SerializeField]
    private int shotGunCapacity;


    [SerializeField]
    private float timePerBulletWhenRecharging;

    private int bulletsOnWeapon;



    //UI elements for gunShot amo
    public Text ammoCounterText;
    public Text bulletsOnWeaponCounterText;

    private void Awake()
    {
        bullets = new List<Quaternion>(bulletsCount);

        for (int i = 0; i < bulletsCount; i++)
        {
            bullets.Add(Quaternion.Euler(Vector3.zero));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        firstPersonControllerScript_ = player.GetComponent<FirstPersonController>();
        playerManagerScript_ = player.GetComponent<PlayerManager>();

        notAimingPosition = originalTransform;
        spreadAngle = NotAimingSpreadAngle;

        objectPooler = ObjectPooler.instance;



        maxAmmo = playerManagerScript_.MaxAmmo;


        Ammo = maxAmmo;
        bulletsOnWeapon = shotGunCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            ammoCounterText.text = "| " + Ammo;
            bulletsOnWeaponCounterText.text = bulletsOnWeapon.ToString();


            if (Input.GetKeyDown(KeyCode.R))
            {
                //reloading = true;
                if (bulletsOnWeapon < shotGunCapacity)
                {
                    firstPersonControllerScript_.ShotGunAnimator.SetTrigger("Reload");
                    reloading = true;
                }
            }

            //if (reloading)
            //{
            //    if (Ammo > 0 && bulletsOnWeapon < shotGunCapacity)
            //        ReloadingTimer();
            //}


            ShootingTimer();
            ShootingLogicAndRecoil();
        }
    }


    void fire()
    {

        for (int i = 0; i < bulletsCount; i++)
        {
            if (!raycasted)
            {
                NotRaycastedFire(i);
            }
            else
            {
                RaycastedFire();
            }
        }

        //fire efect for shotgun shot
        gunFire1.Play();

        //particles efect from hitting point
        HittingParticleEffect();


        //boolean for timing between shots
        waitingForNewShot = true;
        //discount bullets on the weapon
        bulletsOnWeapon--;

        //shooting we stop the reloading giving priority to the first one
        //reloading = false;

        if (reloading)
        {
            reloading = false;
        }
        //reloadTime = 0;

        //automatioc reloading
        if (bulletsOnWeapon <= 0)
        {
            //reloading = true;
            firstPersonControllerScript_.ShotGunAnimator.SetTrigger("Reload");
            reloading = true;
        }

        firstPersonControllerScript_.ShotGunAnimator.SetTrigger("Shooting");
    }


    void RaycastedFire()
    {
        //uses raycast, when collides with something it spawns a object that could be a bullet hole
        Vector3 raycastOrigin = Barrel.position;
        Vector3 rayDirection = new Vector3(Barrel.transform.forward.x + Random.Range(-spreadAngle, spreadAngle),
            Barrel.transform.forward.y + Random.Range(-spreadAngle, spreadAngle),
            Barrel.transform.forward.z + Random.Range(-spreadAngle, spreadAngle));
        ray = new Ray(raycastOrigin, rayDirection);

        Debug.DrawRay(raycastOrigin, rayDirection, Color.red, weaponRange);

        if (Physics.Raycast(ray, out hit, weaponRange))
        {
            //call the functions on the object that is colliding with the raycast.


            if (hit.collider.gameObject.GetComponent<Rat_Health_Logic>() != null)
            {
                //holes on the rats

                //objectPooler.spawnFromPool(ratHoles.name, hit.point, ratHoles.transform.rotation, out ratHole_);
                //ratHole_.transform.parent = hit.collider.gameObject.transform;

                ratHit(hit.collider.gameObject.GetComponent<Rat_Health_Logic>()/*, ratHole_*/);

                Instantiate(ratHolesParticleEffects, hit.point, Quaternion.FromToRotation(-Vector3.forward, hit.normal));

            }
            else
            {
                //holes on the surfices

                //objectPooler.spawnFromPool(holes.name, hit.point, holes.transform.rotation, out holes_);

                //Instantiate(holesParticleEffects, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }
        }
    }


    void NotRaycastedFire(int i)
    {
        // instantiate bullets depending on the shoting angle selection
        bullets[i] = Random.rotation;
        GameObject bullet = Instantiate(bulletPrefab, Barrel.position, Barrel.rotation);
        bullet.transform.rotation = Quaternion.RotateTowards(bullet.transform.rotation, bullets[i], spreadAngle);
        bullet.GetComponent<Rigidbody>().AddForce(-bullet.transform.right * bulletSpeed);
    }

    void ShootingLogicAndRecoil()
    {
        if (Input.GetAxis("Fire1") <= 0)
            shooting = false;
        if ((Input.GetButtonDown("Fire1") || (Input.GetAxis("Fire1") > 0 && !shooting)) && !waitingForNewShot && bulletsOnWeapon > 0)
        {
            fire();
            shooting = true;
            currentRecoilPosition -= weaponRecoilAmount;
            currentRecoilAngle = -weaponRecoilAngle;
            firstPersonControllerScript_.cameraRecoil(angleForCameraRecoil, cameraRecoilSpeed, cameraRecoilingTime);
        }

        currentRecoilAngle = Mathf.SmoothDamp(currentRecoilAngle, 0, ref currentRecoilAngleSpeed, weaponRecoilingTime);
        currentRecoilPosition = Mathf.SmoothDamp(currentRecoilPosition, 0, ref currentRecoilPositionSpeed, weaponRecoilingTime);

        if ((Input.GetButton("Fire2") || Input.GetAxis("Fire2") > 0) && !reloading)
        {
            transform.position = Vector3.Lerp(transform.position, aimPositionTransform.position - transform.right * currentRecoilPosition, Time.deltaTime * aimingSpeed);

            //transform.position = originalTransform.position - transform.right * currentRecoilPosition;
            spreadAngle = AimingSpreadAngle;

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, originalTransform.position - transform.right * currentRecoilPosition, Time.deltaTime * aimingSpeed);
            spreadAngle = NotAimingSpreadAngle;
        }

        transform.localRotation = Quaternion.Euler(0.0f, 90.0f, currentRecoilAngle);
    }

    void ratHit(Rat_Health_Logic healthLogic/*, GameObject ratHole*/)
    {
        healthLogic.ratHited(damage/*, ratHole*/);
    }

    void ShootingTimer()
    {

        if (waitingForNewShot)
        {
            shootTime += Time.deltaTime;

            if (shootTime >= timeBetweenShoots)
            {
                shootTime = 0;
                waitingForNewShot = false;
            }
        }
    }


    public void InsertBullet()
    {
        bulletsOnWeapon++;

        Ammo--;

        if (bulletsOnWeapon >= shotGunCapacity)
        {
            firstPersonControllerScript_.ShotGunAnimator.SetTrigger("StopReloading");
            reloading = false;
        }
    }

    void ReloadingTimer()
    {
        //reloadTime += Time.deltaTime;

        //if (reloadTime >= timePerBulletWhenRecharging)
        //{
        //    bulletsOnWeapon++;

        //    //firstPersonControllerScript_.ShotGunAnimator.SetTrigger("Reload");

        //    Ammo--;

        //    if (bulletsOnWeapon >= shotGunCapacity)
        //    {
        //        reloading = false;
        //        firstPersonControllerScript_.ShotGunAnimator.SetTrigger("StopReloading");
        //    }

        //    reloadTime = 0;
        //}
    }

    public void FillBulletsPocket()
    {

        Ammo = maxAmmo;

    }


    void HittingParticleEffect()
    {
        Vector3 raycastOrigin = Barrel.position;
        ray = new Ray(raycastOrigin, Barrel.transform.forward);

        if (Physics.Raycast(ray, out hit, weaponRange))
        {
            if (hit.collider.gameObject.tag == "Wood")
            {
                Instantiate(woodHolesParticleEffects, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }
            else if (hit.collider.gameObject.tag == "Metal")
            {
                Instantiate(metalHolesParticleEffects, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }
            else
            {
                Instantiate(dirtHolesParticleEffects, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }
        }
    }
}
