// ---------------------------------------------------------
// PlayerParticlePool.cs
//
// 作成日:2024/03/15
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーのパーティクル用のオブジェクトプールクラス
/// </summary>
public class PlayerParticlePool : SingletonMonoBehaviour<PlayerParticlePool>
{
    #region 変数
    // プレイヤーの弾が当たった時のパーティクルを生成する数
    private const int BULLET_PARTICLE_AMOUNT = 15;

    // プレイヤーの弾が当たった時のパーティクルの番号
    private const int BULLET_PARTICLE_NUMBER = -1;

    // プレイヤーの色
    private Color32 _pink = new Color32(255, 125, 218, 255);

    private Transform _bulletParticleParent = default;
    private Transform _deadParticleParent = default;

    [SerializeField]
    private ParticleScript _bulletParticlePrefab = default;
    private Queue<ParticleScript> _bulletParticlePool = new Queue<ParticleScript>();

    [SerializeField]
    private ParticleScript _deadParticlePrefab = default;
    private ParticleScript _deadParticleScriptl = default;
    private ParticleSystem _deadParticleSystem = default;
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        _bulletParticleParent = GameObject.FindWithTag("PlayerParticlePool").transform;
        _deadParticleParent = GameObject.FindWithTag("DeadParticlePool").transform;

        GenerateParticlePool();

        _deadParticleScriptl = Instantiate(_deadParticlePrefab, _deadParticleParent);

        _deadParticleSystem = _deadParticleScriptl.GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// パーティクルのオブジェクトプールを生成する
    /// </summary>
    private void GenerateParticlePool()
    {
        // プレイヤーの弾が当たった時のパーティクルを生成する
        for (int i = 0; i < BULLET_PARTICLE_AMOUNT; i++)
        {
            _bulletParticlePool.Enqueue(Instantiate(_bulletParticlePrefab, _bulletParticleParent));
        }
    }

    /// <summary>
    /// プレイヤーのパーティクルを追加で生成する
    /// </summary>
    private void AddPlayerParticle()
    {
        _bulletParticlePool.Enqueue(Instantiate(_bulletParticlePrefab, _bulletParticleParent));
    }

    /// <summary>
    /// プレイヤーの弾用パーティクルを貸し出す
    /// </summary>
    /// <param name="startPos">パーティクルを配置する座標</param>
    /// <returns>パーティクルのクラス</returns>
    public ParticleScript LendPlayerParticle(Vector3 startPos)
    {
        // パーティクルが足りなければ追加する
        if (_bulletParticlePool.Count <= 0)
        {
            AddPlayerParticle();
        }

        ParticleScript particle = _bulletParticlePool.Dequeue();

        particle.transform.position = startPos;

        particle.ReturnParticleCallBack = ReturnPool;

        particle.SettingParticleType = ParticleScript.ParticleType.Player;

        particle.ParticleNumber = BULLET_PARTICLE_NUMBER;

        return particle;
    }

    /// <summary>
    /// プレイヤーが死亡した時のパーティクルを配置する
    /// </summary>
    /// <param name="startPos">パーティクルを配置する位置</param>
    /// <returns>パーティクルのスクリプト</returns>
    public ParticleScript SetPlayerDeadParticle(Vector3 startPos)
    {
        _deadParticleScriptl.transform.position = startPos;

        ParticleSystem.MainModule main = _deadParticleSystem.main;

        // Color32では代入できないのでColorにキャスト
        main.startColor = (Color)_pink;

        return _deadParticleScriptl;
    }

    /// <summary>
    /// パーティクルをプールに返却する
    /// </summary>
    /// <param name="particleScript">返却するパーティクル</param>
    /// <param name="particleNumber">パーティクル判別用番号</param>
    public void ReturnPool(ParticleScript particleScript, int particleNumber = BULLET_PARTICLE_NUMBER)
    {
        _bulletParticlePool.Enqueue(particleScript);

        particleScript.SettingParticleType = ParticleScript.ParticleType.None;
    }
    #endregion
}