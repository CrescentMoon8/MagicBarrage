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
    public enum ParticleType
    {
        None,
        Player,
        Enemy
    }

    [SerializeField]
    private ParticleType _particleType = ParticleType.None;
	
    private ParticleSystem _particleSystem = default;

    private int _particleNumber = -1;

    public delegate void ReturnParticle(BulletParticle bulletParticle, int particleNumber, ParticleType particleType);
    private ReturnParticle _returnParticleCallBack;
    #endregion

    #region プロパティ
    public ReturnParticle ReturnParticleCallBack { set { _returnParticleCallBack = value; } }
    public int ParticleNumber { set { _particleNumber = value; } }
    public ParticleType SettingParticleType { set { _particleType = value; } }
    #endregion

    #region メソッド
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(!_particleSystem.isPlaying && _particleType != ParticleType.None)
        {
            _returnParticleCallBack(this, _particleNumber, _particleType);
        }
    }

    public void Play()
	{
        _particleSystem.Play();
	}
	#endregion
}