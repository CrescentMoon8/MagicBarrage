// ---------------------------------------------------------
// PlayerParticlePool.cs
//
// 作成日:2024/03/15
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

public class PlayerParticlePool : MonoBehaviour
{
    #region 変数
    private const int PLAYER_PARTICLE_AMOUNT = 15;

    private const int PLAYER_PARTICLE_NUMBER = -1;

    private Color32 _pink = new Color32(255, 125, 218, 255);

    private Transform _playerParticleParent = default;
    private Transform _deadParticleParent = default;

    [SerializeField]
    private ParticleScript _playerBulletParticlePrefab = default;
    private Queue<ParticleScript> _playerBulletParticlePool = new Queue<ParticleScript>();

    [SerializeField]
    private ParticleScript _deadParticlePrefab = default;
    private ParticleScript _deadParticleScriptl = default;
    private ParticleSystem _deadParticleSystem = default;
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {
        _playerParticleParent = GameObject.FindWithTag("PlayerParticlePool").transform;
        _deadParticleParent = GameObject.FindWithTag("DeadParticlePool").transform;

        GenerateParticlePool();

        ParticleScript deadParticle = Instantiate(_deadParticlePrefab, _deadParticleParent);

        ParticleSystem particleSystem = deadParticle.GetComponent<ParticleSystem>();
    }

    private void GenerateParticlePool()
    {
        for (int i = 0; i < PLAYER_PARTICLE_AMOUNT; i++)
        {
            _playerBulletParticlePool.Enqueue(Instantiate(_playerBulletParticlePrefab, _playerParticleParent));
        }
    }

    /// <summary>
    /// プレイヤーのパーティクルを追加で生成する
    /// </summary>
    private void AddPlayerParticle()
    {
        _playerBulletParticlePool.Enqueue(Instantiate(_playerBulletParticlePrefab, _playerParticleParent));
    }

    /// <summary>
    /// プレイヤーの弾用パーティクルを貸し出す
    /// </summary>
    /// <param name="startPos">パーティクルを配置する座標</param>
    /// <returns>パーティクルのクラス</returns>
    public ParticleScript LendPlayerParticle(Vector3 startPos)
    {
        // パーティクルが足りなければ追加する
        if (_playerBulletParticlePool.Count <= 0)
        {
            AddPlayerParticle();
        }

        ParticleScript particle = _playerBulletParticlePool.Dequeue();

        particle.transform.position = startPos;

        particle.ReturnParticleCallBack = ReturnPool;

        particle.SettingParticleType = ParticleScript.ParticleType.Player;

        particle.ParticleNumber = PLAYER_PARTICLE_NUMBER;

        return particle;
    }

    public ParticleScript LendPlayerDeadParticle(Vector3 startPos)
    {

        _deadParticleScriptl.transform.position = startPos;

        _deadParticleScriptl.SettingParticleType = ParticleScript.ParticleType.Player;

        ParticleSystem.MainModule main = _deadParticleSystem.main;

        // Color32では代入できないのでColorにキャスト
        main.startColor = (Color)_pink;

        _deadParticleScriptl.ParticleNumber = 0;

        return _deadParticleScriptl;
    }

    /// <summary>
    /// パーティクルをプールに返却する
    /// </summary>
    /// <param name="particleScript">返却するパーティクル</param>
    /// <param name="particleNumber">パーティクル判別用番号</param>
    public void ReturnPool(ParticleScript particleScript, int particleNumber = PLAYER_PARTICLE_NUMBER)
    {
        _playerBulletParticlePool.Enqueue(particleScript);

        particleScript.SettingParticleType = ParticleScript.ParticleType.None;
    }
    #endregion
}