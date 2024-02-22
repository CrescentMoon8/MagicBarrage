// ---------------------------------------------------------
// BulletPool.cs
//
// 作成日:2024/02/06
// 作成者:小林慎S
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class BulletPool : MonoBehaviour
{
	#region 変数
	private const int MAX_GENERATE_PLAYER_BULLET = 45;
	private const int MAX_GENERATE_ENEMY_BULLET = 150;
	
	private const int ALL_ENEMY_BULLET = 10;


	// Addressableを用いるときに使う、使うか未定
	/*private const int ALL_BULLET = 10;
	private const int RED_NOMAL_BULLET = 0;
	private const int RED_NEEDLE_BULLET = 1;
	private const int BLUE_NOMAL_BULLET = 2;
	private const int BLUE_NEEDLE_BULLET = 3;
	private const int YERROW_NOMAL_BULLET = 4;
	private const int YERROW_NEEDLE_BULLET = 5;
	private const int GREEN_NOMAL_BULLET = 6;
	private const int GREEN_NEEDLE_BULLET = 7;
	private const int PURPLE_NOMAL_BULLET = 8;
	private const int PURPLE_NEEDLE_BULLET = 9;

	private const string PLAYER_BULLET_PASS = "PlayerBullet";
	private const string RED_NOMAL_BULLET_PASS = "RedNomalBullet";
	private const string RED_NEEDLE_BULLET_PASS = "RedNeedleBullet";
	private const string BLUE_NOMAL_BULLET_PASS = "BlueNomalBullet";
	private const string BLUE_NEEDLE_BULLET_PASS = "BlueNeedleBullet";
	private const string YERROW_NOMAL_BULLET_PASS = "YerrowNomalBullet";
	private const string YERROW_NEEDLE_BULLET_PASS = "YerrowNeedleBullet";
	private const string GREEN_NOMAL_BULLET_PASS = "GreenNomalBullet";
	private const string GREEN_NEEDLE_BULLET_PASS = "GreenNeedleBullet";
	private const string GREEN_NEEDLE_BULLET_PASS = "PurpleNomalBullet";
	private const string GREEN_NEEDLE_BULLET_PASS = "PurpleNeedleBullet";

	private GameObject[] _bulletPrefabs = new GameObject[ALL_BULLET];*/

	[SerializeField]
    private Bullet _playerBulletPrefab = default;
	[SerializeField]
	private Bullet[] _enemyBulletPrefabs = new Bullet[ALL_ENEMY_BULLET];
    /*[SerializeField]
    private Bullet _redNomalBulletPrefab = default;
    [SerializeField]
    private Bullet _redNeedleBulletPrefab = default;
    [SerializeField]
	private Bullet _blueNomalBulletPrefab = default;
	[SerializeField]
	private Bullet _blueNeedleBulletPrefab = default;
	[SerializeField]
	private Bullet _yerrowNomalBulletPrefab = default;
	[SerializeField]
	private Bullet _yerrowNeedleBulletPrefab = default;
	[SerializeField]
	private Bullet _greenNomalBulletPrefab = default;
	[SerializeField]
	private Bullet _greenNeedleBulletPrefab = default;*/

	private Transform _playerBulletParent = default;
	private Transform _enemyBulletParent = default;

    private Queue<Bullet> _playerBulletsPool = new Queue<Bullet>();
	private List<Queue<Bullet>> _enemyBulletsPool = new List<Queue<Bullet>>(ALL_ENEMY_BULLET);
    #endregion

    #region プロパティ
    
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
	{
		_playerBulletsPool.Clear();
		_enemyBulletsPool.Clear();

        _playerBulletParent = GameObject.FindWithTag("PlayerBulletPool").transform;
        _enemyBulletParent = GameObject.FindWithTag("EnemyBulletPool").transform;

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

        for (int i = 0; i < ALL_ENEMY_BULLET; i++)
        {
            Queue<Bullet> bulletPool = new Queue<Bullet>();

            for (int k = 0; k < MAX_GENERATE_ENEMY_BULLET; k++)
            {
				Bullet bullet = Instantiate(_enemyBulletPrefabs[i], _enemyBulletParent);

				bullet.gameObject.SetActive(false);

				bulletPool.Enqueue(bullet);
            }

			_enemyBulletsPool.Add(bulletPool);
        }
    }
	
	private void AddPlayerBulletPool()
    {
		Bullet bullet = Instantiate(_playerBulletPrefab, _playerBulletParent);

		bullet.gameObject.SetActive(false);

		_playerBulletsPool.Enqueue(bullet);
    }

	private void AddEnemyBulletPool(int bulletNumber)
    {
		Bullet bullet = Instantiate(_enemyBulletPrefabs[bulletNumber], _enemyBulletParent);

		bullet.gameObject.SetActive(false);

		_enemyBulletsPool[bulletNumber].Enqueue(bullet);
	}

	/// <summary>
	/// 弾をプレイヤーに貸し出す
	/// </summary>
	/// <returns></returns>
	public Bullet LendPlayerBullet(Vector3 shotPosition, Bullet.MoveType moveType)
	{
		if(_playerBulletsPool.Count <= 0)
		{
			AddPlayerBulletPool();
		}

		Bullet bullet = _playerBulletsPool.Dequeue();

		bullet.transform.position = shotPosition;

		bullet.SettingShooterType = Bullet.ShooterType.Player;

		bullet.SettingMoveType = moveType;

		bullet.gameObject.SetActive(true);

		bullet.Initialize();

		return bullet;
	}

	public Bullet LendEnemyBullet(Vector3 shotPosition, int bulletNumber)
	{
		if (_enemyBulletsPool[bulletNumber].Count <= 0)
		{
			AddEnemyBulletPool(bulletNumber);
		}

		Bullet bullet = _enemyBulletsPool[bulletNumber].Dequeue();

		bullet.transform.position = shotPosition;

		bullet.SettingShooterType = Bullet.ShooterType.Enemy;

        bullet.SettingBulletNumber = bulletNumber;

		bullet.gameObject.SetActive(true);

		bullet.Initialize();

		return bullet;
	}

	public void ReturnBullet(Bullet bullet, int bulletNumber, Bullet.ShooterType type)
	{
        bullet.gameObject.SetActive(false);

        switch (type)
		{
			case Bullet.ShooterType.Player:
                _playerBulletsPool.Enqueue(bullet);
                break;

			case Bullet.ShooterType.Enemy:
                _enemyBulletsPool[bulletNumber].Enqueue(bullet);
				break;

            default:
				break;
		}
    }

	#endregion
}