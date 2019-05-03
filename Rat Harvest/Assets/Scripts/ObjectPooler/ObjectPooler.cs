using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
 
    // A class to create pools of different objects
    [System.Serializable]
    public class objectPoolItem
    {
        private string tag;
        public string Tag { get { return this.tag; } set { this.tag = value; } }
        public GameObject objectToPool;
        public int amountToPool;
        public bool shouldExpand;
    }


    // A list to store all of our different types of items
    public List<objectPoolItem> itemsToPool;

    // New dictionary set to be able to find a specific pool of objects
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // We make the pool a singleton to get access in an easy way
    public static ObjectPooler instance;

    private void Awake()
    {
        instance = this;

        // We create a new dictionary
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (objectPoolItem item in itemsToPool)
        {
            // We create a gameObject queue for each key of the dictionary
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // We add the objects to the pools
            for (int i = 0; i < item.amountToPool; i++)
            {
                item.Tag = item.objectToPool.name;

                // We instantiate the gameObject
                GameObject go = Instantiate(item.objectToPool);
                go.SetActive(false);
                objectPool.Enqueue(go);
            }

            // We add the pool to the dictionary
            poolDictionary.Add(item.Tag, objectPool);
        }      
    }

    // Method to get an item from one of the pools
    private GameObject getItemFromPool(string tag)
    {
        GameObject objectToSpawn = null;

        // To prevent unexpected errors
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("GameObject with tag '" + tag + "' doesn't exist.");
            return null;
        }

        // We grab the first element of the queue
        objectToSpawn = poolDictionary[tag].Dequeue();
        poolDictionary[tag].Enqueue(objectToSpawn);

        // We search in the queue if there's some inactive element
        //for (int i = 0; i < poolDictionary[tag].Count; i++)
        //{
        //    objectToSpawn = poolDictionary[tag].Dequeue();
        //    poolDictionary[tag].Enqueue(objectToSpawn);

        //    if (!objectToSpawn.activeInHierarchy)
        //        return objectToSpawn;
        //    else
        //        objectToSpawn = null;
        //}

        // If all the elements are inactive we create another one if the ShouldExpand attribute is true
        foreach (objectPoolItem item in itemsToPool)
        {
            if (item.Tag == tag)
            {
                if (item.shouldExpand)
                {
                    objectToSpawn = Instantiate(item.objectToPool);
                    objectToSpawn.SetActive(false);
                    poolDictionary[tag].Enqueue(objectToSpawn);
                    return objectToSpawn;
                }
            }
        }

        return objectToSpawn;
    }

    // Method to spawn a gameObject from one of the pools
    public void spawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        // We search the pool and give life to one of the objects
        GameObject obj = getItemFromPool(tag);

        if (obj == null)
            Debug.LogError("There's no item to pool or they are all in use!");

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        // We call an specific method of an interface to make sure the start method of the reused objects works 
        IPooledObject pooledObj = obj.GetComponent<IPooledObject>();

        if (pooledObj != null)
            pooledObj.OnObjectSpawn();
    }


    // Method to spawn a gameObject from one of the pools and return a reference for that gameObject
    public void spawnFromPool(string tag, Vector3 position, Quaternion rotation, out GameObject spawnedObject)
    {
        // We search the pool and give life to one of the objects
        spawnedObject = getItemFromPool(tag);

        if (spawnedObject == null)
            Debug.LogError("There's no item to pool or they are all in use!");

        spawnedObject.transform.position = position;
        spawnedObject.transform.rotation = rotation;
        spawnedObject.SetActive(true);

        // We call an specific method of an interface to make sure the start method of the reused objects works 
        IPooledObject pooledObj = spawnedObject.GetComponent<IPooledObject>();

        if (pooledObj != null)
            pooledObj.OnObjectSpawn();
    }

    // Method to spawn an specific object of the queue
    public void spawnSpecificFromPool(GameObject go, Vector3 position, Quaternion rotation)
    {
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.SetActive(true);

        // We call an specific method of an interface to make sure the start method of the reused objects works 
        IPooledObject pooledObj = go.GetComponent<IPooledObject>();

        if (pooledObj != null)
            pooledObj.OnObjectSpawn();
    }

    // Method to disable a gameObject form one of the pools
    public void killGameObject(GameObject obj)
    {
        if(obj != null)
            obj.SetActive(false);
    }

    // Checks if there's a pool with an specific tag
    public bool itemExists(string tag)
    {
        bool b;

        if (poolDictionary.ContainsKey(tag))
            b = true;
        else
            b = false;

        return b;
    }
}
