// ---------------------------------------------------------
// ParticlePool.cs
//
// 作成日:2024/02/24
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections.Generic;

public class ParticlePool : MonoBehaviour
{
	#region 変数
	private const int PLAYER_PARTICLE_AMOUNT = 15;
	private const int ENEMY_PARTICLE_AMOUNT = 2;

    private const int PLAYER_PARTICLE_NUMBER = -1;

    private List<Color> _particleColorList = new List<Color>();
    private Color _red = new Color(0.81f, 0f, 0f);
    private Color _blue = new Color(0f, 0.01f, 0.81f);
    private Color _yellow = new Color(0.93f, 0.96f, 0f);
    private Color _green = new Color(0.17f, 0.81f, 0f);
    private Color _purple = new Color(0.65f, 0f, 0.81f);

    private Transform _playerParticleParent = default;
    private Transform _enemyParticleParent = default;

    [SerializeField]
	private BulletParticle _playerBulletParticlePrefab = default;
	[SerializeField]
	private BulletParticle _enemyBulletParticlePrefab = default;
	private Queue<BulletParticle> _playerBulletParticlePool = new Queue<BulletParticle>();
	private Queue<BulletParticle> _enemyBulletParticlePool = new Queue<BulletParticle>();
    private Dictionary<BulletParticle, ParticleSystem> _enemyParticleSystemDic = new Dictionary<BulletParticle, ParticleSystem>();
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
    {
		_playerParticleParent = GameObject.FindWithTag("PlayerParticlePool").transform;
		_enemyParticleParent = GameObject.FindWithTag("EnemyParticlePool").transform;

        _particleColorList.Add(_red);
        _particleColorList.Add(_blue);
        _particleColorList.Add(_yellow);
        _particleColorList.Add(_green);
        _particleColorList.Add(_purple);

        GenerateParticlePool();
    }

    private void GenerateParticlePool()
    {
        for (int i = 0; i < PLAYER_PARTICLE_AMOUNT; i++)
        {
            _playerBulletParticlePool.Enqueue(Instantiate(_playerBulletParticlePrefab, _playerParticleParent));
        }

        for (int i = 0; i < ENEMY_PARTICLE_AMOUNT; i++)
        {
            BulletParticle bulletParticle = Instantiate(_enemyBulletParticlePrefab, _enemyParticleParent);

            ParticleSystem particleSystem = bulletParticle.GetComponent<ParticleSystem>();

            _enemyParticleSystemDic.Add(bulletParticle, particleSystem);

            _enemyBulletParticlePool.Enqueue(bulletParticle);
        }
    }

    public BulletParticle LendPlayerParticle(Vector3 startPos)
    {
        BulletParticle particle = _playerBulletParticlePool.Dequeue();

        particle.transform.position = startPos;

        particle.ReturnParticleCallBack = ReturnPool;

        particle.SettingParticleType = BulletParticle.ParticleType.Player;

        particle.ParticleNumber = PLAYER_PARTICLE_NUMBER;

        return particle;
    }

    public BulletParticle LendEnemyParicle(Vector3 startPos, int bulletNumber)
    {
        int particleNumber = bulletNumber / 2;

        BulletParticle particle = _enemyBulletParticlePool.Dequeue();

        particle.transform.position = startPos;

        particle.ReturnParticleCallBack = ReturnPool;

        particle.SettingParticleType = BulletParticle.ParticleType.Enemy;

        ParticleSystem.MainModule main = _enemyParticleSystemDic[particle].main;

        main.startColor = _particleColorList[particleNumber];

        particle.ParticleNumber = particleNumber;

        return particle;
    }

    public void ReturnPool(BulletParticle bulletParticle, int particleNumber)
    {
        if(particleNumber == -1)
        {
            _playerBulletParticlePool.Enqueue(bulletParticle);
        }
        else
        {
            _enemyBulletParticlePool.Enqueue(bulletParticle);
        }

        bulletParticle.SettingParticleType = BulletParticle.ParticleType.None;
    }
	#endregion
}