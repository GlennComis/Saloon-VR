using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(NetworkObject))]
public abstract class NetworkSingletonMonoBehaviour<T> : NetworkBehaviour where T : NetworkBehaviour
{
    private static T instance;

    public static T Instance => instance;

    /// <summary>
    /// We use a bool instead of a null check because a nullcheck is expensive
    /// </summary>
    public static bool instance_exists = false;

    protected virtual void Awake()
    {
        if (instance == null)
            SetSingletonInstance();
    }

    /// <summary>
    /// Sets the instance, gets called in awake unless we want to call it at another moment
    /// </summary>
    protected virtual void SetSingletonInstance(bool dontDestroyOnLoad = false)
    {
        if(dontDestroyOnLoad)
            DontDestroyOnLoad(this.gameObject);
            
        instance = this as T;
        instance_exists = instance != null;
    }


    public override void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
        
        instance_exists = instance != null;
    }
}