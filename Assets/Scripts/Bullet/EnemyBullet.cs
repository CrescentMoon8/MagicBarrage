// ---------------------------------------------------------
// EnemyBullet.cs
//
// 作成日:2024/03/15
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

/// <summary>
/// 弾の移動処理、回転処理、敵のダメージ処理の呼び出しを行う
/// </summary>
public class EnemyBullet : Bullet
{
	#region 変数
	// 弾の速度
	private const float LOW_SPEED = 30f;
	private const float MIDDLE_SPEED = 15f;
	private const float HIGH_SPEED = 10f;
	// 弾の加速率
	private const float ACCELERATION_RATE = 0.2f;
	//弾の減速率
	private const float DECELERATION_RATE = 0.1f;
	// 敵の弾の速度を調整する（高くすれば遅く、低くすれば早くなる）
	[SerializeField]
	private float _speedDevisor = 15f;

	// プレイヤーが持つダメージ用インターフェース
	private IDamageable _playerIDamageable = default;

    // どの敵からどの弾が撃たれたかの判別用番号
    private int _bulletNumber = 0;
    #endregion

    #region プロパティ
    public int SettingBulletNumber { set { _bulletNumber = value; } }
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
	{
		base.GetIPlayerPos();

		_playerIDamageable = GameObject.FindWithTag("Player").GetComponent<PlayerManager>().GettingPlayerHp;
	}

	/// <summary>
	/// 弾の初期化処理
	/// </summary>
	public override void Initialize()
	{
		switch (base._speedType)
		{
			case SpeedType.Low:
				_speedDevisor = LOW_SPEED;
				break;

			case SpeedType.Middle:
				_speedDevisor = MIDDLE_SPEED;
				break;

			case SpeedType.High:
				_speedDevisor = HIGH_SPEED;
				break;

			case SpeedType.Acceleration:
				_speedDevisor = LOW_SPEED;
				break;

			case SpeedType.Deceleration:
				_speedDevisor = HIGH_SPEED;
				break;

			default:
				break;
		}
	}

	/// <summary>
	/// 更新処理
	/// </summary>
	private void FixedUpdate ()
	{
		Quaternion rotateAngle = this.transform.rotation;
		Vector3 movePos = this.transform.position;

		switch (_moveType)
		{
			case MoveType.Line:
				SpeedChange();

                // 移動量の計算
                movePos += transform.up / _speedDevisor;
				break;

			case MoveType.Tracking:
				//SpeedChange();
				// 弾からプレイヤーへの角度を計算
				float direction = Calculation.Instance.TargetDirectionAngle(_iPlayerPos.PlayerPos, this.transform.position);
				Quaternion targetRotation = Quaternion.Euler(Vector3.forward * direction);
				//（現在角度、目標方向、どれぐらい曲がるか）
				rotateAngle = Quaternion.RotateTowards(this.transform.rotation, targetRotation, 0.5f);

				// 移動量の計算
				movePos += transform.up / _speedDevisor;
				break;

			case MoveType.Curve:
				break;
			default:
				break;
		}

		// 移動と回転を同時に行う
		this.transform.SetPositionAndRotation(movePos, rotateAngle);
	}

	/// <summary>
	/// 弾のスピードを変化させる
	/// </summary>
	private void SpeedChange()
	{
		switch (_speedType)
		{
			case SpeedType.Low:
				_speedDevisor = LOW_SPEED;
				break;

			case SpeedType.Middle:
				_speedDevisor = MIDDLE_SPEED;
				break;

			case SpeedType.High:
				_speedDevisor = HIGH_SPEED;
				break;

			case SpeedType.Acceleration:
				// 速度上限になるまで加速する
				if (_speedDevisor >= HIGH_SPEED)
				{
					_speedDevisor -= ACCELERATION_RATE;
				}
				break;

			case SpeedType.Deceleration:
				// 速度下限になるまで減速する
				if (_speedDevisor <= LOW_SPEED)
				{
					_speedDevisor += DECELERATION_RATE;
				}
				break;

			default:
				break;
		}
	}

	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("ReturnPool"))
		{
			EnemyBulletPool.Instance.ReturnBullet(this, _bulletNumber);

			// 各状態を初期化する
			base._moveType = MoveType.Line;
			base._speedType = SpeedType.Middle;
			_speedDevisor = MIDDLE_SPEED;
		}

		if (collision.CompareTag("Player"))
		{
			_playerIDamageable.Damage();

			// 弾が壊れた時のパーティクルを取り出して再生する
			ParticleScript enemyParticle = EnemyParticlePool.Instance.LendEnemyParicle(this.transform.position, _bulletNumber);
			enemyParticle.Play();

			EnemyBulletPool.Instance.ReturnBullet(this, _bulletNumber);

			// 各状態を初期化する
			base._moveType = MoveType.Line;
			base._speedType = SpeedType.Middle;
			this.transform.rotation = Quaternion.identity;
			_speedDevisor = MIDDLE_SPEED;
		}
	}
	#endregion
}