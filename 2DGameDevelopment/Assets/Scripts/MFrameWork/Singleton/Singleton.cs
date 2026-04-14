using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
   private static T _instance;
   public static T Instance => _instance;

    private void Awake()
    {
        if(_instance is not null)
        {
            _instance  = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
