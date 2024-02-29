// ---------------------------------------------------------
// ParticlePool.cs
//
// 作成日:2024/02/24
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections.Generic;

public class ParticlePool : MonoBehaviour, IObjectPool<BulletParticle>
{
	#region 変数
	private const int PLAYER_PARTICLE_AMOUNT = 15;
	private const int ENEMY_PARTICLE_AMOUNT = 4;

    private Transform _playerParticleParent = default;
    private Transform _enemyParticleParent = default;

    [SerializeField]
	private BulletParticle _playerBulletParticlePrefab = default;
	[SerializeField]
	private List<BulletParticle> _enemyBulletParticlePrefabList = new List<BulletParticle>();
	private Queue<BulletParticle> _playerBulletParticlePool = new Queue<BulletParticle>();
	private List<Queue<BulletParticle>> _enemyBulletParticlePool = new List<Queue<BulletParticle>>();
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

        GenerateParticlePool();
    }

    private void GenerateParticlePool()
    {
        for (int i = 0; i < PLAYER_PARTICLE_AMOUNT; i++)
        {
            _playerBulletParticlePool.Enqueue(Instantiate(_playerBulletParticlePrefab, _playerParticleParent));
        }

        for (int i = 0; i < _enemyBulletParticlePrefabList.Count; i++)
        {
            Queue<BulletParticle> particlePool = new Queue<BulletParticle>();

            for (int j = 0; j < ENEMY_PARTICLE_AMOUNT; j++)
            {
                particlePool.Enqueue(Instantiate(_enemyBulletParticlePrefabList[i], _enemyParticleParent));
            }

            _enemyBulletParticlePool.Add(particlePool);
        }
    }

    public BulletParticle LendPlayer(Vector3 startPos, int particleNumber)
    {
        BulletParticle particle = _playerBulletParticlePool.Dequeue();

        particle.transform.position = startPos;

        particle.ReturnParticleCallBack = ReturnPool;

        particle.SettingParticleType = BulletParticle.ParticleType.Player;

        particle.ParticleNumber = particleNumber;

        return particle;
    }

    public BulletParticle LendEnemy(Vector3 startPos, int bulletNumber)
    {
        int particleNumber = bulletNumber / 2;

        BulletParticle particle = _enemyBulletParticlePool[particleNumber].Dequeue();

        particle.transform.position = startPos;

        particle.ReturnParticleCallBack = ReturnPool;

        particle.SettingParticleType = BulletParticle.ParticleType.Enemy;

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
            _enemyBulletParticlePool[particleNumber].Enqueue(bulletParticle);
        }

        bulletParticle.SettingParticleType = BulletParticle.ParticleType.None;
    }
	#endregion
}