using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ShotGunBehaviour : MonoBehaviour
{

    FirstPersonController firstPersonControllerScript_;
    public GameObject player;
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
    private GameObject holes;
    private GameObject holes_;

    [SerializeField]
    private GameObject holesParticleEffects;

    [SerializeField]
    private GameObject ratHoles;
    private GameObject ratHole_;

    [SerializeField]
    private GameObject ratHolesParticleEffects;

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

    private bool shooting = false;

    private ObjectPooler objectPooler;

    private bool waitingForNewShot;
    float time = 0;

    [SerializeField]
    private float timeBetweenShoots;

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

        notAimingPosition = originalTransform;
        spreadAngle = NotAimingSpreadAngle;

        objectPooler = ObjectPooler.instance;

    }

    // Update is called once per frame
    void Update()
    {
        ShootingTimer();
        ShootingLogicAndRecoil();
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

        waitingForNewShot = true;
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

                objectPooler.spawnFromPool(ratHoles.name, hit.point, ratHoles.transform.rotation, out ratHole_);
                ratHole_.transform.parent = hit.collider.gameObject.transform;

                ratHit(hit.collider.gameObject.GetComponent<Rat_Health_Logic>(), ratHole_);

                Instantiate(ratHolesParticleEffects, hit.point, Quaternion.FromToRotation(-Vector3.forward, hit.normal));

            }
            else
            {
                objectPooler.spawnFromPool(holes.name, hit.point, holes.transform.rotation, out holes_);

                Instantiate(holesParticleEffects, hit.point, Quaternion.FromToRotation(-Vector3.forward, hit.normal));
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

        if ((Input.GetButtonDown("Fire1") || (Input.GetAxis("Fire1") > 0 && !shooting)) && !waitingForNewShot)
        {
            fire();
            shooting = true;
            currentRecoilPosition -= weaponRecoilAmount;
            currentRecoilAngle = -weaponRecoilAngle;
            firstPersonControllerScript_.cameraRecoil(angleForCameraRecoil, cameraRecoilSpeed, cameraRecoilingTime);
        }

        currentRecoilAngle = Mathf.SmoothDamp(currentRecoilAngle, 0, ref currentRecoilAngleSpeed, weaponRecoilingTime);
        currentRecoilPosition = Mathf.SmoothDamp(currentRecoilPosition, 0, ref currentRecoilPositionSpeed, weaponRecoilingTime);

        if (Input.GetButton("Fire2") || Input.GetAxis("Fire2") > 0)
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

    void ratHit(Rat_Health_Logic healthLogic, GameObject ratHole)
    {
        healthLogic.ratHited(damage, ratHole);
    }

    void ShootingTimer()
    {

        if (waitingForNewShot)
        {
            time += Time.deltaTime;

            if (time >= timeBetweenShoots)
            {
                time = 0;
                waitingForNewShot = false;
            }
        }
    }
}
