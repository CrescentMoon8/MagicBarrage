// ---------------------------------------------------------
// SingletonMonoBehaviour.cs
//
// 作成日:2024/03/01
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = default;
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                Type t = typeof(T);

                _instance = (T)FindObjectOfType(t);

                if(_instance == null)
                {
                    Debug.LogError(t + "をアタッチしているGameObjectはありません");
                }
            }

            return _instance;
        }
    }
}