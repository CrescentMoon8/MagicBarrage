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
	private const int MAX_GENERATE_PLAYER_BULLET = 15;
	private const int MAX_GENERATE_ENEMY_BULLET = 150;
	private const int ALL_ENEMY_BULLET = 10;

    

    // Addressableを用いるときに使う、使うか未定
    /*private const int ALL_BULLET = 9;
	private const int PLAYER_BULLET = 0;
	private const int RED_NOMAL_BULLET = 1;
	private const int RED_NEEDLE_BULLET = 2;
	private const int BLUE_NOMAL_BULLET = 3;
	private const int BLUE_NEEDLE_BULLET = 4;
	private const int YERROW_NOMAL_BULLET = 5;
	private const int YERROW_NEEDLE_BULLET = 6;
	private const int GREEN_NOMAL_BULLET = 7;
	private const int GREEN_NEEDLE_BULLET = 8;

	private const string PLAYER_BULLET_PASS = "PlayerBullet";
	private const string RED_NOMAL_BULLET_PASS = "RedNomalBullet";
	private const string RED_NEEDLE_BULLET_PASS = "RedNeedleBullet";
	private const string BLUE_NOMAL_BULLET_PASS = "BlueNomalBullet";
	private const string BLUE_NEEDLE_BULLET_PASS = "BlueNeedleBullet";
	private const string YERROW_NOMAL_BULLET_PASS = "YerrowNomalBullet";
	private const string YERROW_NEEDLE_BULLET_PASS = "YerrowNeedleBullet";
	private const string GREEN_NOMAL_BULLET_PASS = "GreenNomalBullet";
	private const string GREEN_NEEDLE_BULLET_PASS = "GreenNeedleNomalBullet";

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

		GenerateObjectPool();
	}

    private void GenerateObjectPool()
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

	/// <summary>
	/// 弾をプレイヤーに貸し出す
	/// </summary>
	/// <returns></returns>
	public Bullet LendPlayerBullet(Vector3 shotPosition)
	{
		if(_playerBulletsPool.Count <= 0)
		{
			return null;
		}

		Bullet bullet = _playerBulletsPool.Dequeue();

		bullet.gameObject.SetActive(true);

		bullet.transform.position = shotPosition;

		bullet.SettingShooterType = Bullet.ShooterType.Player;

		return bullet;
	}

	public Bullet LendEnemyBullet(Vector3 shotPosition, int bulletNumber)
	{
		Debug.LogError(bulletNumber);
		if (_enemyBulletsPool[bulletNumber].Count <= 0)
		{
			return null;
		}

		Bullet bullet = _enemyBulletsPool[bulletNumber].Dequeue();

		bullet.gameObject.SetActive(true);

		bullet.transform.position = shotPosition;

		bullet.SettingShooterType = Bullet.ShooterType.Enemy;

        bullet.SettingBulletNumber = bulletNumber;

        return bullet;
	}

	public void ReturnBullet(Bullet bullet, int bulletNumber, Bullet.ShooterType type)
	{
        bullet.gameObject.SetActive(false);

        switch (type)
		{
			case Bullet.ShooterType.Player:
				Debug.Log("Playerの弾を回収");
                _playerBulletsPool.Enqueue(bullet);
                Debug.Log("Playerの弾を回収");
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