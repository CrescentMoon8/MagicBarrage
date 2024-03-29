// ---------------------------------------------------------
// BulletParticle.cs
//
// 作成日:2024/02/25
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

public class ParticleScript : MonoBehaviour
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

    public delegate void ReturnParticle(ParticleScript particleScript, int particleNumber);
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
        if(!IsPlaying() && _particleType != ParticleType.None)
        {
            _returnParticleCallBack(this, _particleNumber);
        }
    }

    public void Play()
	{
        _particleSystem.Play();
	}

    public bool IsPlaying()
    {
        if(_particleSystem.isPlaying)
        {
            return true;
        }

        return false;
    }
	#endregion
}