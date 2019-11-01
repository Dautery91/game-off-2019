using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class creates and manages object pools.  Improvements to make still:  Handle multiple scenes, make class static (pros/cons vs singleton?)
/// </summary>
public class PoolManager : MonoBehaviour
{
    private Dictionary<int, List<GameObject>> poolDictionary = new Dictionary<int, List<GameObject>>();
    private int dictionaryIndex = 0;
    private GameObject[] prefabsToLoad;
    static PoolManager _instance;

    public bool IsInitialized = false;
    public const string PooledObjectResourcesFolderFilePath = "";
    public static PoolManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }

    /// <summary>
    /// Initializes all prefabs in the Resources folder.  Can use sub folder of resources by passing parameter
    /// </summary>
    public void Initialize(string filePath = PooledObjectResourcesFolderFilePath)
    {
        prefabsToLoad = Resources.LoadAll<GameObject>(PooledObjectResourcesFolderFilePath);

        foreach (GameObject go in prefabsToLoad)
        {
            instance.CreatePool(go);
        }

        IsInitialized = true;
    }

    /// <summary>
    /// Creates pool object given a prefab.  note that the prefab must be a Pool Object 
    /// (or child of PoolObject class)
    /// </summary>
    /// <param name="prefab">Prefab to be pooled</param>
    /// <param name="poolSize"></param>
    public void CreatePool(GameObject prefab)
    {
        PooledObject poolObject = prefab.GetComponent<PooledObject>();
        GameObject poolHolder = new GameObject(prefab.name + " pool");

        // Probably need more error checking in this

        if (poolObject == null)
        {
            Debug.Log("Pool NOT created.  Check if PoolObject script is attached to prefab: " + prefab.name);
            return;
        }
        else
        {
            // Assigns the prefab an identity in the pool dictionary
            // -1 is the default PoolID vaule for pooled objects
            poolObject.PoolID = dictionaryIndex;

            poolDictionary.Add(poolObject.PoolID, new List<GameObject>());

            for (int i = 0; i < poolObject.StartingPoolSize; i++)
            {
                GameObject newObject = Instantiate(prefab) as GameObject;
                newObject.SetActive(false);
                newObject.GetComponent<PooledObject>().SetParent(poolHolder.transform);
                poolDictionary[poolObject.PoolID].Add(newObject);
            }

            // Increment dictionaryIndex for next pool
            dictionaryIndex++;
        }
       

    }


    /// <summary>
    /// Returns a game object instance from its prefab pool if there are available instances 
    /// in that pool.  Expands the pool if necessary.
    /// </summary>
    /// <param name="prefab">Pooled object prefab of which an instance will be returned</param>
    /// <returns></returns>
    public GameObject GetPooledObject(GameObject prefab)
    {
        int poolKey = prefab.GetComponent<PooledObject>().PoolID;

        if (poolDictionary.ContainsKey(poolKey) && poolDictionary[poolKey].Count > 0)
        {
            GameObject poolItem = poolDictionary[poolKey][poolDictionary[poolKey].Count - 1];
            poolDictionary[poolKey].RemoveAt(poolDictionary[poolKey].Count - 1);

            return poolItem;
        }
        else 
        {
            ExpandPool(prefab, poolKey, prefab.GetComponent<PooledObject>().PoolExpansionFactor);
            return GetPooledObject(prefab);
        }

    }

    public void ReturnToPool(GameObject prefab)
    {
        int poolKey = prefab.GetComponent<PooledObject>().PoolID;

        if (poolDictionary.ContainsKey(poolKey))
        {
            prefab.SetActive(false);
            poolDictionary[poolKey].Add(prefab);

        }

    }

    // NEED TO FIX THIS:  The expanded objects dont go into the parent object for organization
    private void ExpandPool(GameObject prefab, int poolKey, int listExpansionFactor)
    {
        for (int i = 0; i < listExpansionFactor; i++)
        {
            GameObject newObject = Instantiate(prefab) as GameObject;
            newObject.SetActive(false);
            poolDictionary[poolKey].Add(newObject);
        }

    }

    public void EmptyAllPools()
    {
        dictionaryIndex = 0;
        poolDictionary.Clear();
    }

}
