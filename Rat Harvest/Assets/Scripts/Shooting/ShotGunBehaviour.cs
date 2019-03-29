using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunBehaviour : MonoBehaviour
{

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

    List<Quaternion> bullets;


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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            fire();

    }

    void fire()
    {


        for (int i = 0; i < bulletsCount; i++) { 
            bullets[i] = Random.rotation;
            GameObject bullet = Instantiate(bulletPrefab, Barrel.position, Barrel.rotation);
            bullet.transform.rotation = Quaternion.RotateTowards(bullet.transform.rotation, bullets[i], spreadAngele);
            bullet.GetComponent<Rigidbody>().AddForce(-bullet.transform.right * bulletSpeed);
        }
    }
}
