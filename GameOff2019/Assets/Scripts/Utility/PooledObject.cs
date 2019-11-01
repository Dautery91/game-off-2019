using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// Class that governs the behavior and parameters of an object to be pooled
/// </summary>
[RequireComponent(typeof(Transform))]
public class PooledObject : MonoBehaviour
{
    public int StartingPoolSize = 10;
    public int PoolExpansionFactor = 5;
    public int PoolID = -1;


    private void OnBecameInvisible()
    {
        if (this.gameObject.activeSelf)
        {
            ReturnToPool();
        }
    }

    /// <summary>
    /// Returns object instance back to its pool using the pool manager and stops the object from moving
    /// if it has a rigidbody. The pool manager will deactivate the object
    /// </summary>
    public virtual void ReturnToPool()
    {
        PoolManager.instance.ReturnToPool(this.gameObject);

        Rigidbody2D rb2d = this.GetComponent<Rigidbody2D>();

        if (rb2d == null)
        {
            return;
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }

    }

    /// <summary>
    /// Gets an object from the pool manager and puts it at the location provided with
    /// the specified rotation
    /// </summary>
    /// <param name="position"> New position of the returned game object </param>
    /// <param name="rotation"> New rotation of the returned game object </param>
    /// <returns></returns>
    public virtual GameObject GetPooledObject(Vector3 position, Quaternion rotation)
    {
        GameObject go = PoolManager.instance.GetPooledObject(this.gameObject);
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.SetActive(true);
        return go;
    }

    public virtual void SetParent(Transform parent)
    {
        transform.parent = parent;
    }

}
