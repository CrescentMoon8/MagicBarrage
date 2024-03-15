// ---------------------------------------------------------
// ParticlePool.cs
//
// 作成日:2024/02/24
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// パーティクル用のオブジェクトプールクラス
/// </summary>
public class EnemyParticlePool : MonoBehaviour
{
	#region 変数
	private const int ENEMY_PARTICLE_AMOUNT = 2;
	private const int DEAD_PARTICLE_AMOUNT = 3;

    private const int DEAD_PARTICLE_NUMBER = -1;

    private List<Color> _particleColorList = new List<Color>();
    private Color32 _red = new Color32(207, 0, 0, 255);
    private Color32 _blue = new Color32(0, 3, 207, 255);
    private Color32 _yellow = new Color32(237, 245, 0, 255);
    private Color32 _green = new Color32(43, 207, 0, 255);
    private Color32 _purple = new Color32(166, 0, 207, 255);

    private Transform _enemyParticleParent = default;
    private Transform _deadParticleParent = default;

    [SerializeField]
	private ParticleScript _enemyBulletParticlePrefab = default;
    private Queue<ParticleScript> _enemyBulletParticlePool = new Queue<ParticleScript>();
    private Dictionary<ParticleScript, ParticleSystem> _enemyParticleSystemDic = new Dictionary<ParticleScript, ParticleSystem>();

    [SerializeField]
    private ParticleScript _deadParticlePrefab = default;
    private Queue<ParticleScript> _deadParticlePool = new Queue<ParticleScript>();
    private Dictionary<ParticleScript, ParticleSystem> _deadParticleSystemDic = new Dictionary<ParticleScript, ParticleSystem>();
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {
		_enemyParticleParent = GameObject.FindWithTag("EnemyParticlePool").transform;
		_deadParticleParent = GameObject.FindWithTag("DeadParticlePool").transform;

        _particleColorList.Add(_red);
        _particleColorList.Add(_blue);
        _particleColorList.Add(_yellow);
        _particleColorList.Add(_green);
        _particleColorList.Add(_purple);

        GenerateParticlePool();
    }

    private void GenerateParticlePool()
    {
        for (int i = 0; i < ENEMY_PARTICLE_AMOUNT; i++)
        {
            ParticleScript bulletParticle = Instantiate(_enemyBulletParticlePrefab, _enemyParticleParent);

            ParticleSystem particleSystem = bulletParticle.GetComponent<ParticleSystem>();

            _enemyParticleSystemDic.Add(bulletParticle, particleSystem);

            _enemyBulletParticlePool.Enqueue(bulletParticle);
        }

        for (int i = 0; i < DEAD_PARTICLE_AMOUNT; i++)
        {
            ParticleScript deadParticle = Instantiate(_deadParticlePrefab, _deadParticleParent);

            ParticleSystem particleSystem = deadParticle.GetComponent<ParticleSystem>();

            _deadParticleSystemDic.Add(deadParticle, particleSystem);

            _deadParticlePool.Enqueue(deadParticle);
        }
    }

    /// <summary>
    /// エネミーのパーティクルを追加で生成する
    /// </summary>
    private void AddEnemyParticle()
    {
        ParticleScript bulletParticle = Instantiate(_enemyBulletParticlePrefab, _enemyParticleParent);

        ParticleSystem particleSystem = bulletParticle.GetComponent<ParticleSystem>();

        _enemyParticleSystemDic.Add(bulletParticle, particleSystem);

        _enemyBulletParticlePool.Enqueue(bulletParticle);
    }

    /// <summary>
    /// プレイヤーの弾用パーティクルを貸し出す
    /// </summary>
    /// <param name="startPos">パーティクルを配置する座標</param>
    /// <param name="bulletNumber">弾の種類（どの敵が撃った弾か）</param>
    /// <returns>パーティクルのクラス</returns>
    public ParticleScript LendEnemyParicle(Vector3 startPos, int bulletNumber)
    {
        // パーティクルが足りなければ追加する
        if (_enemyBulletParticlePool.Count <= 0)
        {
            AddEnemyParticle();
        }

        int particleNumber = bulletNumber / 2;

        ParticleScript particle = _enemyBulletParticlePool.Dequeue();

        particle.transform.position = startPos;

        particle.ReturnParticleCallBack = ReturnPool;

        particle.SettingParticleType = ParticleScript.ParticleType.Enemy;

        ParticleSystem.MainModule main = _enemyParticleSystemDic[particle].main;

        main.startColor = _particleColorList[particleNumber];

        particle.ParticleNumber = particleNumber;

        return particle;
    }

    public ParticleScript LendEnemyDeadParticle(Vector3 startPos, int particleNumber)
    {
        ParticleScript particle = _deadParticlePool.Dequeue();

        particle.transform.position = startPos;

        particle.ReturnParticleCallBack = ReturnPool;

        particle.SettingParticleType = ParticleScript.ParticleType.Enemy;

        ParticleSystem.MainModule main = _deadParticleSystemDic[particle].main;

        main.startColor = _particleColorList[particleNumber];

        particle.ParticleNumber = DEAD_PARTICLE_NUMBER;

        return particle;
    }

    /// <summary>
    /// パーティクルをプールに返却する
    /// </summary>
    /// <param name="particleScript">返却するパーティクル</param>
    /// <param name="particleNumber">パーティクル判別用番号</param>
    public void ReturnPool(ParticleScript particleScript, int particleNumber)
    {
        switch (particleNumber)
        {
            case DEAD_PARTICLE_NUMBER:
                _deadParticlePool.Enqueue(particleScript);
                break;

            default:
                _enemyBulletParticlePool.Enqueue(particleScript);
                break;
        }

        particleScript.SettingParticleType = ParticleScript.ParticleType.None;
    }
	#endregion
}