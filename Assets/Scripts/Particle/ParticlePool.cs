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
	private const int PLAYER_PARTICLE_AMOUNT = 6;
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

    public void LendPlayerParticle(Vector3 startPos)
    {
        BulletParticle particle = _playerBulletParticlePool.Dequeue();

        particle.transform.position = startPos;

        particle.ReturnParticleCallBack = ReturnParticle;

        particle.Play();
    }

    public void LendEnemyParticle(Vector3 startPos, int bulletNumber)
    {
        BulletParticle particle = _enemyBulletParticlePool[bulletNumber / 2].Dequeue();

        particle.transform.position = startPos;

        particle.Play();
    }

    public void ReturnParticle(BulletParticle bulletParticle)
    {

    }
	#endregion
}