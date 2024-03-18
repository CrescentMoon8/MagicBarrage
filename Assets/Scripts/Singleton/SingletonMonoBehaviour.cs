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
    // 継承先で「破棄はできないか？」を指定
    protected bool _dontDestroyOnLoad = false;

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

    protected virtual void Awake()
    {
        // インスタンスが複数存在する場合は自身を破棄
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        // 継承先で破棄不可能が指定された場合はシーン遷移時も破棄しない
        if (_dontDestroyOnLoad)
        {
            transform.parent = null;

            DontDestroyOnLoad(this.gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        // 破棄された場合は実体の削除を行う
        if (this == Instance)
        {
            _instance = null;
        }
    }
}