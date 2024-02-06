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
	private const int ALL_ENEMY_BULLET = 6;

	// Addressableを用いるときに使う、使うか未定
	/*private const int ALL_BULLET = 7;
	private const int PLAYER_BULLET = 0;
	private const int BLUE_NOMAL_BULLET = 1;
	private const int BLUE_NEEDLE_BULLET = 2;
	private const int YERROW_NOMAL_BULLET = 3;
	private const int YERROW_NEEDLE_BULLET = 4;
	private const int GREEN_NOMAL_BULLET = 5;
	private const int GREEN_NEEDLE_BULLET = 6;

	private const string PLAYER_BULLET_PASS = "RedNeedleBullet";
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

		_playerBulletParent = GameObject.FindWithTag("PlayerBullets").transform;
		_enemyBulletParent = GameObject.FindWithTag("EnemyBullets").transform;

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

		Queue<Bullet> addPool = new Queue<Bullet>();

        for (int i = 0; i < ALL_ENEMY_BULLET; i++)
        {
			Debug.Log("a");
            for (int k = 0; k < MAX_GENERATE_ENEMY_BULLET; k++)
            {
				Bullet bullet = Instantiate(_enemyBulletPrefabs[i], _enemyBulletParent);

				bullet.gameObject.SetActive(false);

				addPool.Enqueue(bullet);
            }

			_enemyBulletsPool.Add(addPool);
        }
    }

	/// <summary>
	/// 弾をプレイヤーに貸し出す
	/// </summary>
	/// <returns></returns>
	/*public Bullet LendPlayerBullet()
	{

	}*/
	#endregion
}