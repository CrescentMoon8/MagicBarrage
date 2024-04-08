// ---------------------------------------------------------
// BulletPool.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 敵の弾用のオブジェクトプールクラス
/// </summary>
public class EnemyBulletPool : SingletonMonoBehaviour<EnemyBulletPool>
{
	#region 変数
	// 生成する弾の数
	private int[] _maxGenerateBullet = { 250, 50 };
	
	// 生成する弾の種類
	private const int BULLET_KINDS = 2;

	[SerializeField]
	private List<EnemyBullet> _bulletPrefabs = default;
	[SerializeField]
	private List<Sprite> _bulletSprite = default;

	// 生成する弾の親オブジェクト
	private Transform _bulletParent = default;

	private List<Queue<EnemyBullet>> _bulletsPool = new List<Queue<EnemyBullet>>();

	// 貸し出した弾のSpriteを変えるためのSpriteRendererを保管するDictionaryのList
	private List<Dictionary<EnemyBullet, SpriteRenderer>> _spriteRendererList = new List<Dictionary<EnemyBullet, SpriteRenderer>>();
	#endregion

	#region メソッド
	/// <summary>
	/// 弾のオブジェクトプールの初期化処理
	/// </summary>
	public void BulletAwake()
	{
		base.Awake();

		_bulletsPool.Clear();

        _bulletParent = GameObject.FindWithTag("EnemyBulletPool").transform;

		GenerateBulletPool();
	}

	/// <summary>
	/// 弾のオブジェクトプールを生成する
	/// </summary>
    private void GenerateBulletPool()
	{
		// 弾の種類分だけ生成する
        for (int i = 0; i < BULLET_KINDS; i++)
        {
			Dictionary<EnemyBullet, SpriteRenderer> spriteRendererDic = new Dictionary<EnemyBullet, SpriteRenderer>();
			Queue<EnemyBullet> bulletPool = new Queue<EnemyBullet>();

			// 配列の値で指定した数分だけ生成する
			for (int k = 0; k < _maxGenerateBullet[i]; k++)
            {
				EnemyBullet bullet = Instantiate(_bulletPrefabs[i], _bulletParent);

				SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();

				// 弾のスクリプトをKeyとしてSpriteRendererを追加する
				spriteRendererDic.Add(bullet, spriteRenderer);

				bullet.gameObject.SetActive(false);

				bulletPool.Enqueue(bullet);
            }

			_spriteRendererList.Add(spriteRendererDic);
			_bulletsPool.Add(bulletPool);
		}
    }

	/// <summary>
	/// 敵の弾を追加で生成する
	/// </summary>
	/// <param name="bulletKindsNumber">弾の種類</param>
	private void AddBulletPool(int bulletKindsNumber)
    {
		EnemyBullet bullet = Instantiate(_bulletPrefabs[bulletKindsNumber], _bulletParent);

		SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();

		// 弾のスクリプトをKeyとしてSpriteRendererを追加する
		_spriteRendererList[bulletKindsNumber].Add(bullet, spriteRenderer);

		bullet.gameObject.SetActive(false);

		_bulletsPool[bulletKindsNumber].Enqueue(bullet);
	}

	/// <summary>
	/// 弾を敵に貸し出す
	/// </summary>
	/// <param name="shotPos">弾を配置する座標</param>
	/// <param name="bulletNumber">弾の番号</param>
	/// <returns>弾のスクリプト</returns>
	public EnemyBullet LendBullet(Vector3 shotPos, int bulletNumber)
	{
		// 弾の種類を判別する（0 or 1）
		int bulletKindsNumber = bulletNumber % 2;

		// プールの中身がなかったら
		if (_bulletsPool[bulletKindsNumber].Count <= 0)
		{
			AddBulletPool(bulletKindsNumber);
		}

		EnemyBullet bullet = _bulletsPool[bulletKindsNumber].Dequeue();

		// 取り出した弾のスクリプトを使って、それに対応するSpriteRendererを取り出しす
		// 取り出した弾のSpriteを弾の番号に対応するSpriteに変更する
		_spriteRendererList[bulletKindsNumber][bullet].sprite = _bulletSprite[bulletNumber];

		bullet.transform.position = shotPos;

		// 弾のスクリプトに弾の番号をセットする
		bullet.SettingBulletNumber = bulletNumber;

		bullet.gameObject.SetActive(true);

		return bullet;
	}

	/// <summary>
	/// 弾を返却する
	/// </summary>
	/// <param name="bullet">返却する弾のスクリプト</param>
	public void ReturnBullet(EnemyBullet bullet, int bulletNumber)
	{
        bullet.gameObject.SetActive(false);

		_bulletsPool[bulletNumber % 2].Enqueue(bullet);
	}

	#endregion
}