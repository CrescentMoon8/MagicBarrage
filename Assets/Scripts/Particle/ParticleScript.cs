// ---------------------------------------------------------
// BulletParticle.cs
//
// 作成日:2024/02/25
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

/// <summary>
/// パーティクルを制御するクラス
/// </summary>
public class ParticleScript : MonoBehaviour
{
	#region 変数
    // 誰のパーティクルか
    public enum ParticleType
    {
        None,
        Player,
        Enemy
    }

    [SerializeField]
    private ParticleType _particleType = ParticleType.None;
	
    private ParticleSystem _particleSystem = default;

    // パーティクルの番号
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
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        if(!IsPlaying() && _particleType != ParticleType.None)
        {
            _returnParticleCallBack(this, _particleNumber);
        }
    }

    /// <summary>
    /// パーティクルの再生を行う
    /// </summary>
    public void Play()
	{
        _particleSystem.Play();
	}

    /// <summary>
    /// パーティクルが再生されているかどうか
    /// </summary>
    /// <returns>パーティクルの再生の有無</returns>
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