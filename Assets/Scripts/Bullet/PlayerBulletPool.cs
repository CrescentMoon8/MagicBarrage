// ---------------------------------------------------------
// PlayerBulletPool.cs
//
// 作成日:2024/03/04
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーが撃った弾用のオブジェクトプールクラス
/// </summary>
public class PlayerBulletPool : SingletonMonoBehaviour<PlayerBulletPool>
{
	#region 変数
	private const int MAX_GENERATE_PLAYER_BULLET = 45;

	private const int PLAYER_BULLET_NUMBER = -1;

	[SerializeField]
	private PlayerBullet _playerBulletPrefab = default;

	private Transform _playerBulletParent = default;

	private Queue<PlayerBullet> _playerBulletsPool = new Queue<PlayerBullet>();

	// 貸し出されているプレイヤーの弾を格納するリスト
	//private List<PlayerBullet> _lendedPlayerBulletList = new List<PlayerBullet>();
	#endregion

	#region プロパティ
	
	#endregion

	#region メソッド
	/// <summary>
	/// 弾のオブジェクトプールの初期化処理
	/// </summary>
	public void BulletAwake()
	{
		base.Awake();

		_playerBulletsPool.Clear();

		_playerBulletParent = GameObject.FindWithTag("PlayerBulletPool").transform;

		GenerateBulletPool();
	}

	/// <summary>
	/// 弾のオブジェクトプールを生成する
	/// </summary>
	private void GenerateBulletPool()
	{
		for (int i = 0; i < MAX_GENERATE_PLAYER_BULLET; i++)
		{
			PlayerBullet bullet = Instantiate(_playerBulletPrefab, _playerBulletParent);

			bullet.gameObject.SetActive(false);

			_playerBulletsPool.Enqueue(bullet);
		}
	}

	/// <summary>
	/// プレイヤーの弾を追加で生成する
	/// </summary>
	private void AddPlayerBulletPool()
	{
		PlayerBullet bullet = Instantiate(_playerBulletPrefab, _playerBulletParent);

		bullet.gameObject.SetActive(false);

		_playerBulletsPool.Enqueue(bullet);
	}

	/// <summary>
	/// 弾をプレイヤーに貸し出す
	/// </summary>
	/// <returns>弾のスクリプト</returns>
	public PlayerBullet LendPlayerBullet(Vector3 shotPos)
	{
		if (_playerBulletsPool.Count <= 0)
		{
			AddPlayerBulletPool();
		}

		PlayerBullet bullet = _playerBulletsPool.Dequeue();

		bullet.transform.position = shotPos;

		bullet.SettingBulletNumber = PLAYER_BULLET_NUMBER;

		bullet.gameObject.SetActive(true);

		//_lendedPlayerBulletList.Add(bullet);

		return bullet;
	}

	/// <summary>
	/// 弾を返却する
	/// </summary>
	/// <param name="bullet">返却する弾のスクリプト</param>
	public void ReturnBullet(PlayerBullet bullet)
	{
		bullet.gameObject.SetActive(false);

		_playerBulletsPool.Enqueue(bullet);

		//_lendedPlayerBulletList.Remove(bullet);
	}
	#endregion
}