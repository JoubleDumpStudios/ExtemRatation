using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ShotGunBehaviour : MonoBehaviour
{

    FirstPersonController fpc;
    public GameObject player;
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private int weaponRange;

    [SerializeField]
    private int bulletsCount;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float spreadAngele;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform Barrel;

    [SerializeField]
    private bool raycasted;

    List<Quaternion> bullets;

    [SerializeField]
    private GameObject holes;

    RaycastHit hit;
    Ray ray;

    [SerializeField]
    private Transform gunPositionA;
    


    public float smoothLevel = 5;

    [SerializeField]
    private float recoilingTime = 0.2f;
    [SerializeField]
    private float recoilAmount = 0.2f;

    [SerializeField]
    private float recoilAngle = 20f;

    private float currentRecoilPosition;
    private float currentRecoilAngle;
    private float currentRecoilPositionSpeed;
    private float currentRecoilAngleSpeed;

    public Transform originalTransform;

    [SerializeField]
    private float angleForCameraRecoil = 15;

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
        fpc = player.GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            fire();
            currentRecoilPosition -= recoilAmount;
            currentRecoilAngle = -recoilAngle;
            fpc.cameraRecoil(angleForCameraRecoil);
        }

        currentRecoilAngle = Mathf.SmoothDamp(currentRecoilAngle, 0, ref currentRecoilAngleSpeed, recoilingTime);

        currentRecoilPosition = Mathf.SmoothDamp(currentRecoilPosition, 0, ref currentRecoilPositionSpeed, recoilingTime);
        transform.position = originalTransform.position - transform.right * currentRecoilPosition;

        transform.localRotation = Quaternion.Euler(0.0f,90.0f, currentRecoilAngle);


    }


    void fire()
    {

        for (int i = 0; i < bulletsCount; i++)
        {
            if (!raycasted)
            { // instantiate bullets depending on the shoting angle selection
                bullets[i] = Random.rotation;
                GameObject bullet = Instantiate(bulletPrefab, Barrel.position, Barrel.rotation);
                bullet.transform.rotation = Quaternion.RotateTowards(bullet.transform.rotation, bullets[i], spreadAngele);
                bullet.GetComponent<Rigidbody>().AddForce(-bullet.transform.right * bulletSpeed);
            }
            else
            {
                //uses raycast, when collides with something it spawns a object that could be a bullet hole
                Vector3 raycastOrigin = Barrel.position;
                Vector3 rayDirection = new Vector3(Barrel.transform.forward.x + Random.Range(-spreadAngele, spreadAngele),
                    Barrel.transform.forward.y + Random.Range(-spreadAngele, spreadAngele),
                    Barrel.transform.forward.z + Random.Range(-spreadAngele, spreadAngele));
                ray = new Ray(raycastOrigin, rayDirection);

                Debug.DrawRay(raycastOrigin, rayDirection, Color.red, weaponRange);
                
                if(Physics.Raycast(ray, out hit, weaponRange))
                {
                    //call the functions on the object that is colliding with the raycast.
                    Instantiate(holes, hit.point, holes.transform.rotation);
                }
            }
        }



    }
}
