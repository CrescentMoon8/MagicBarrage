// ---------------------------------------------------------
// Enemy.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

public class Enemy : EnemyShot
{
	#region 変数
	private const string PLAYER_BULLET_TAG = "PlayerBullet";

	private const int ENEMY_HP = 20;

	// 角度の最大
	private int _maxAngle = 90;
	// 最大角度を何分割するか
	private int _angleSplit = 9;
	// 初弾の位置調整用の変数
	private int _direction = 315;

	private float _shotTime = 0f;
	private const float SHOT_INTERVAL = 2f;

	[SerializeField]
	private GameObject _test;
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
        base._bulletPool = GameObject.FindWithTag("Scripts").GetComponentInChildren<BulletPool>();
    }

	/// <summary>
	/// 更新前処理
	/// </summary>
	private void Start ()
	{
		base._hpSlider.maxValue = ENEMY_HP;
		base._hpSlider.value = ENEMY_HP;
		base._hpValue = ENEMY_HP;

		base._player = GameObject.FindWithTag("Player");
		base._playerPos = _player.transform.position;
		// テスト用
		/*for (int i = 0; i < _angleSplit; i++)
		{
			Vector3 bulletPos = CirclePositionCalculate(this.transform.position, 270, this.transform.localScale.x / 2);
			Instantiate(_test, bulletPos, Quaternion.identity);
		}*/
	}

	/// <summary>
	/// 更新処理
	/// </summary>
	private void Update ()
	{
		_shotTime += Time.deltaTime;

		base._playerPos = base._player.transform.position;

		if( _shotTime > SHOT_INTERVAL )
		{
			// 三方向に扇形の弾を撃つ
			for (int i = 0; i < 3; i++)
			{
				base.RoundShot(this.transform.position, _maxAngle, _angleSplit, _direction, 0, Bullet.MoveType.Tracking);
				_direction -= 90;
			}
			//base.RoundShot(this.transform.position, _maxAngle, _angleSplit, _direction, _radius, 0);
			/*base.LineShot()*/
			_shotTime = 0f;
			_direction = 315;
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(PLAYER_BULLET_TAG))
		{
			base.EnemyDamage();
		}
    }
    #endregion
}