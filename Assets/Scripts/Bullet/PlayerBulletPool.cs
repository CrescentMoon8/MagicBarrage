// ---------------------------------------------------------
// PlayerBulletPool.cs
//
// 作成日:2024/03/04
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーの弾用のオブジェクトプールクラス
/// </summary>
public class PlayerBulletPool : SingletonMonoBehaviour<PlayerBulletPool>
{
    #region 変数
    // 生成する弾の数
    private const int MAX_GENERATE_BULLET = 45;

	[SerializeField]
	private PlayerBullet _bulletPrefab = default;

    // 生成する弾の親オブジェクト
    private Transform _bulletParent = default;

	private Queue<PlayerBullet> _bulletsPool = new Queue<PlayerBullet>();
	#endregion

	#region メソッド
	/// <summary>
	/// 弾のオブジェクトプールの初期化処理
	/// </summary>
	public void BulletAwake()
	{
		base.Awake();

		_bulletsPool.Clear();

		_bulletParent = GameObject.FindWithTag("PlayerBulletPool").transform;

		GenerateBulletPool();
	}

	/// <summary>
	/// 弾のオブジェクトプールを生成する
	/// </summary>
	private void GenerateBulletPool()
	{
        // 指定した数分だけ生成する
        for (int i = 0; i < MAX_GENERATE_BULLET; i++)
		{
			PlayerBullet bullet = Instantiate(_bulletPrefab, _bulletParent);

			bullet.gameObject.SetActive(false);

			_bulletsPool.Enqueue(bullet);
		}
	}

	/// <summary>
	/// プレイヤーの弾を追加で生成する
	/// </summary>
	private void AddBulletPool()
	{
		PlayerBullet bullet = Instantiate(_bulletPrefab, _bulletParent);

		bullet.gameObject.SetActive(false);

		_bulletsPool.Enqueue(bullet);
	}

	/// <summary>
	/// 弾をプレイヤーに貸し出す
	/// </summary>
	/// <param name="shotPos">生成位置</param>
	/// <returns>弾のスクリプト</returns>
	public PlayerBullet LendBullet(Vector3 shotPos)
	{
		// プールに弾がなければ
		if (_bulletsPool.Count <= 0)
		{
			AddBulletPool();
		}

		PlayerBullet bullet = _bulletsPool.Dequeue();

		bullet.transform.position = shotPos;

		bullet.gameObject.SetActive(true);

		return bullet;
	}

	/// <summary>
	/// 弾を返却する
	/// </summary>
	/// <param name="bullet">返却する弾のスクリプト</param>
	public void ReturnBullet(PlayerBullet bullet)
	{
		bullet.gameObject.SetActive(false);

		_bulletsPool.Enqueue(bullet);
	}
	#endregion
}