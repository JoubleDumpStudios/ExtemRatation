using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_Health_Logic : MonoBehaviour
{

    public RectTransform topHeadHealthBar;

    private Rat_Movement_Logic ratMovementScript;

    [SerializeField]
    private List<GameObject> wounds_ = new List<GameObject>();

    [SerializeField]
    private float maxHealth = 100;
    public float MaxHealth { get { return this.maxHealth; } }
    [SerializeField]
    private float health;
    public float Health { get { return this.health; } }

    [SerializeField]
    private List<GameObject> bloodObject;
    // Start is called before the first frame update

    public void OnEnable()
    {
        ResetRatHealth();
        topHeadHealthBar.sizeDelta = new Vector2(health * 2, topHeadHealthBar.sizeDelta.y);
    }


    public void Start()
    {
        ResetRatHealth();
        topHeadHealthBar.sizeDelta = new Vector2(health * 2, topHeadHealthBar.sizeDelta.y);
        ratMovementScript = GetComponent<Rat_Movement_Logic>();        
    }

    public void ratHited(float damage, float hitDistance/*, GameObject wound*/)
    {
        if (health > 0)
        { 
            health -= damage;
        }

        if(health <= 0)
        {
            AkSoundEngine.PostEvent("Rat_Death", gameObject);
            health = 0;
            ratMovementScript.killRat(hitDistance);

            ObjectPooler.instance.spawnFromPool(randomBloodEfectName(), new Vector3(this.transform.position.x, 0.001f, this.transform.position.z), new Quaternion(0,this.transform.rotation.y,0,0));
        }

        //in the case you want wonds on the rat
        /*wounds_.Add(wound);*/ // each time the rat recieve damage this method recieve the object that allow us to visualice the hole and add it to a list to manage it

        topHeadHealthBar.sizeDelta = new Vector2(health * 2, topHeadHealthBar.sizeDelta.y);
    }

    string randomBloodEfectName()
    {
        int i;
        string bloodName;
        i = Random.Range(0, bloodObject.Count);
        bloodName = bloodObject[i].name;
        return bloodName;
    }

    void cleanWounds(List<GameObject> wounds)// allow us to remove the wounds from the rat and send them again to the pooler to be used again when needed
    {
        for (int i = 0; i < wounds.Count; i++)
        {
            ObjectPooler.instance.killGameObject(wounds[i]);
            wounds[i].transform.parent = null;
        }

        wounds.Clear();
    }

    public void ResetRatHealth()
    {
        health = maxHealth;
    }


}
