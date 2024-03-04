// ---------------------------------------------------------
// PlayerBulletPool.cs
//
// 作成日:2024/03/04
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerBulletPool : SingletonMonoBehaviour<PlayerBulletPool>
{
	#region 変数
	private const int MAX_GENERATE_PLAYER_BULLET = 45;

	private const int PLAYER_BULLET_NUMBER = -1;

	[SerializeField]
	private Bullet _playerBulletPrefab = default;

	private Transform _playerBulletParent = default;

	private Queue<Bullet> _playerBulletsPool = new Queue<Bullet>();
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
		_playerBulletsPool.Clear();

		_playerBulletParent = GameObject.FindWithTag("PlayerBulletPool").transform;

		GenerateBulletPool();
	}

	private void GenerateBulletPool()
	{
		for (int i = 0; i < MAX_GENERATE_PLAYER_BULLET; i++)
		{
			Bullet bullet = Instantiate(_playerBulletPrefab, _playerBulletParent);

			bullet.gameObject.SetActive(false);

			_playerBulletsPool.Enqueue(bullet);
		}
	}

	private void AddPlayerBulletPool()
	{
		Bullet bullet = Instantiate(_playerBulletPrefab, _playerBulletParent);

		bullet.gameObject.SetActive(false);

		_playerBulletsPool.Enqueue(bullet);
	}

	/// <summary>
	/// 弾をプレイヤーに貸し出す
	/// </summary>
	/// <returns></returns>
	public Bullet LendPlayerBullet(Vector3 shotPos)
	{
		if (_playerBulletsPool.Count <= 0)
		{
			AddPlayerBulletPool();
		}

		Bullet bullet = _playerBulletsPool.Dequeue();

		bullet.transform.position = shotPos;

		bullet.SettingShooterType = Bullet.ShooterType.Player;

		bullet.SettingBulletNumber = PLAYER_BULLET_NUMBER;

		bullet.gameObject.SetActive(true);

		return bullet;
	}

	public void ReturnBullet(Bullet bullet)
	{
		bullet.gameObject.SetActive(false);

		_playerBulletsPool.Enqueue(bullet);
	}
	#endregion
}