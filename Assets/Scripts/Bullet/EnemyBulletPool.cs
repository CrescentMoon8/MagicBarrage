// ---------------------------------------------------------
// BulletPool.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class EnemyBulletPool : SingletonMonoBehaviour<EnemyBulletPool>
{
	#region 変数
	private const int MAX_GENERATE_ENEMY_BULLET = 200;
	
	private const int ALL_ENEMY_BULLET = 10;
	private const int ENEMY_BULLET_KINDS = 2;


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
	private List<Bullet> _enemyBulletPrefabs = default;
	[SerializeField]
	private List<Sprite> _enemyBulletSprite = default;
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

	private Transform _enemyBulletParent = default;

	private List<Queue<Bullet>> _enemyBulletsPool = new List<Queue<Bullet>>();

	private List<Dictionary<Bullet, SpriteRenderer>> _spriteRendererList = new List<Dictionary<Bullet, SpriteRenderer>>();
    #endregion

    #region プロパティ
    
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
	{
		_enemyBulletsPool.Clear();

        _enemyBulletParent = GameObject.FindWithTag("EnemyBulletPool").transform;

		GenerateBulletPool();
	}

    private void GenerateBulletPool()
	{
        for (int i = 0; i < ENEMY_BULLET_KINDS; i++)
        {
			Dictionary<Bullet, SpriteRenderer> spriteRendererDic = new Dictionary<Bullet, SpriteRenderer>();
			Queue<Bullet> bulletPool = new Queue<Bullet>();

			for (int k = 0; k < MAX_GENERATE_ENEMY_BULLET; k++)
            {
                Bullet bullet = Instantiate(_enemyBulletPrefabs[i], _enemyBulletParent);

				SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();

				spriteRendererDic.Add(bullet, spriteRenderer);

				bullet.gameObject.SetActive(false);

				bulletPool.Enqueue(bullet);
            }

			_spriteRendererList.Add(spriteRendererDic);
			_enemyBulletsPool.Add(bulletPool);
		}
    }

	private void AddEnemyBulletPool(int bulletNumber)
    {
		Bullet bullet = Instantiate(_enemyBulletPrefabs[bulletNumber], _enemyBulletParent);

		SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();

		_spriteRendererList[bulletNumber].Add(bullet, spriteRenderer);

		bullet.gameObject.SetActive(false);

		_enemyBulletsPool[bulletNumber].Enqueue(bullet);
	}

	public Bullet LendEnemyBullet(Vector3 shotPos, int bulletNumber)
	{
		int bulletKindsNumber = bulletNumber % 2;

		if (_enemyBulletsPool[bulletKindsNumber].Count <= 0)
		{
			AddEnemyBulletPool(bulletKindsNumber);
		}

		Bullet bullet = _enemyBulletsPool[bulletKindsNumber].Dequeue();

		_spriteRendererList[bulletKindsNumber][bullet].sprite = _enemyBulletSprite[bulletNumber];

		bullet.transform.position = shotPos;

		bullet.SettingShooterType = Bullet.ShooterType.Enemy;

		bullet.SettingBulletNumber = bulletNumber;

		bullet.gameObject.SetActive(true);

		return bullet;
	}

	public void ReturnBullet(Bullet bullet, int bulletNumber)
	{
        bullet.gameObject.SetActive(false);

		_enemyBulletsPool[bulletNumber % 2].Enqueue(bullet);
	}

	#endregion
}