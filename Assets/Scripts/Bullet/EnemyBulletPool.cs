// ---------------------------------------------------------
// BulletPool.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// エネミーが撃った弾用のオブジェクトプールクラス
/// </summary>
public class EnemyBulletPool : SingletonMonoBehaviour<EnemyBulletPool>
{
	#region 変数
	// 生成する弾の数
	private const int MAX_GENERATE_ENEMY_BULLET = 250;
	
	//private const int ALL_ENEMY_BULLET = 10;
	// 生成する弾の種類
	private const int ENEMY_BULLET_KINDS = 2;

	[SerializeField]
	private List<Bullet> _enemyBulletPrefabs = default;
	[SerializeField]
	private List<Sprite> _enemyBulletSprite = default;

	// 生成する弾の親オブジェクト
	private Transform _enemyBulletParent = default;

	private List<Queue<Bullet>> _enemyBulletsPool = new List<Queue<Bullet>>();

	// 貸し出した弾のSpriteを変えるためのSpriteRendererを保管するDictionaryのList
	private List<Dictionary<Bullet, SpriteRenderer>> _spriteRendererList = new List<Dictionary<Bullet, SpriteRenderer>>();
    #endregion

    #region プロパティ
    
    #endregion

    #region メソッド
    /// <summary>
    /// 弾のオブジェクトプールの初期化処理
    /// </summary>
    public void BulletAwake()
	{
		_enemyBulletsPool.Clear();

        _enemyBulletParent = GameObject.FindWithTag("EnemyBulletPool").transform;

		GenerateBulletPool();
	}

	/// <summary>
	/// 弾のオブジェクトプールを生成する
	/// </summary>
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

				// 弾のスクリプトをKeyとしてSpriteRendererを追加する
				spriteRendererDic.Add(bullet, spriteRenderer);

				bullet.gameObject.SetActive(false);

				bulletPool.Enqueue(bullet);
            }

			_spriteRendererList.Add(spriteRendererDic);
			_enemyBulletsPool.Add(bulletPool);
		}
    }

	/// <summary>
	/// エネミーの弾を追加で生成する
	/// </summary>
	/// <param name="bulletKindsNumber">弾の種類</param>
	private void AddEnemyBulletPool(int bulletKindsNumber)
    {
		Bullet bullet = Instantiate(_enemyBulletPrefabs[bulletKindsNumber], _enemyBulletParent);

		SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();

		// 弾のスクリプトをKeyとしてSpriteRendererを追加する
		_spriteRendererList[bulletKindsNumber].Add(bullet, spriteRenderer);

		bullet.gameObject.SetActive(false);

		_enemyBulletsPool[bulletKindsNumber].Enqueue(bullet);
	}

	/// <summary>
	/// 弾をエネミーに貸し出す
	/// </summary>
	/// <param name="shotPos">弾を配置する座標</param>
	/// <param name="bulletNumber">弾の番号</param>
	/// <returns>弾のスクリプト</returns>
	public Bullet LendEnemyBullet(Vector3 shotPos, int bulletNumber)
	{
		// 弾の種類を判別する（0 or 1）
		int bulletKindsNumber = bulletNumber % 2;

		// プールの中身がなかったら
		if (_enemyBulletsPool[bulletKindsNumber].Count <= 0)
		{
			AddEnemyBulletPool(bulletKindsNumber);
		}

		Bullet bullet = _enemyBulletsPool[bulletKindsNumber].Dequeue();

		// 取り出した弾のスクリプトを使って、それに対応するSpriteRendererを取り出しす
		// 取り出した弾のSpriteを弾の番号に対応するSpriteに変更する
		_spriteRendererList[bulletKindsNumber][bullet].sprite = _enemyBulletSprite[bulletNumber];

		bullet.transform.position = shotPos;

		bullet.SettingShooterType = Bullet.ShooterType.Enemy;

		// 弾のスクリプトに弾の番号をセットする
		bullet.SettingBulletNumber = bulletNumber;

		bullet.gameObject.SetActive(true);

		return bullet;
	}

	/// <summary>
	/// 弾を返却する
	/// </summary>
	/// <param name="bullet">返却する弾のスクリプト</param>
	public void ReturnBullet(Bullet bullet, int bulletNumber)
	{
        bullet.gameObject.SetActive(false);

		_enemyBulletsPool[bulletNumber % 2].Enqueue(bullet);
	}

	#endregion
}