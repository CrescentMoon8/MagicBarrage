// ---------------------------------------------------------
// BulletParticle.cs
//
// 作成日:2024/02/25
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;

public class BulletParticle : MonoBehaviour
{
	#region 変数
	private ParticleSystem _particleSystem;

    public delegate void ReturnParticle(BulletParticle bulletParticle);
    private ReturnParticle _returnParticleCallBack;
    #endregion

    #region プロパティ
    public ReturnParticle ReturnParticleCallBack { set { _returnParticleCallBack = value; } }
    #endregion

    #region メソッド
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        _returnParticleCallBack(this);
    }

    public void Play()
	{
        _particleSystem.Play();
	}
	#endregion
}