// ---------------------------------------------------------
// EnemyBullet.cs
//
// 作成日:2024/03/15
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

public class EnemyBullet : Bullet
{
	#region 変数
	// 弾の速度
	private const float LOW_SPEED = 30f;
	private const float MIDDLE_SPEED = 15f;
	private const float HIGH_SPEED = 10f;
	// 弾の加速率
	private const float ACCELERATION_RATE = 0.1f;
	//弾の減速率
	private const float DECELERATION_RATE = 0.1f;
	// エネミーの弾の速度を調整する（高くすれば遅く、低くすれば早くなる）
	private float _enemyBulletSpeedDevisor = 15f;

	// プレイヤーが持つダメージ用インターフェース
	private IDamageable _playerIDamageable = default;
	private EnemyParticlePool _enemyParticlePool = default;
	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
		base.GetIPlayerPos();

		_playerIDamageable = GameObject.FindWithTag("Player").GetComponent<PlayerManager>().GettingPlayerHp;
		_enemyParticlePool = GameObject.FindWithTag("Scripts").GetComponentInChildren<EnemyParticlePool>();
	}

	/// <summary>
	/// 弾の初期化処理
	/// </summary>
	public override void Initialize()
	{
		switch (base._speedType)
		{
			case SpeedType.Low:
				_enemyBulletSpeedDevisor = LOW_SPEED;
				break;

			case SpeedType.Middle:
				_enemyBulletSpeedDevisor = MIDDLE_SPEED;
				break;

			case SpeedType.High:
				_enemyBulletSpeedDevisor = HIGH_SPEED;
				break;

			case SpeedType.Acceleration:
				_enemyBulletSpeedDevisor = LOW_SPEED;
				break;

			case SpeedType.Deceleration:
				_enemyBulletSpeedDevisor = HIGH_SPEED;
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
				movePos += transform.up / _enemyBulletSpeedDevisor;
				break;

			case MoveType.Tracking:
				//SpeedChange();
				float direction = Calculation.TargetDirectionAngle(_iPlayerPos.PlayerPos, this.transform.position);
				Quaternion targetRotation = Quaternion.Euler(Vector3.forward * direction);
				//（現在角度、目標方向、どれぐらい曲がるか）
				rotateAngle = Quaternion.RotateTowards(this.transform.rotation, targetRotation, 0.5f);

				movePos += transform.up / _enemyBulletSpeedDevisor;
				break;

			case MoveType.Curve:
				break;
			default:
				break;
		}

		this.transform.SetPositionAndRotation(movePos, rotateAngle);
	}

	private void SpeedChange()
	{
		switch (_speedType)
		{
			case SpeedType.Low:
				_enemyBulletSpeedDevisor = LOW_SPEED;
				break;

			case SpeedType.Middle:
				_enemyBulletSpeedDevisor = MIDDLE_SPEED;
				break;

			case SpeedType.High:
				_enemyBulletSpeedDevisor = HIGH_SPEED;
				break;

			case SpeedType.Acceleration:
				// 速度上限になるまで加速する
				if (_enemyBulletSpeedDevisor >= HIGH_SPEED)
				{
					_enemyBulletSpeedDevisor -= ACCELERATION_RATE;
				}
				break;

			case SpeedType.Deceleration:
				// 速度下限になるまで減速する
				if (_enemyBulletSpeedDevisor <= LOW_SPEED)
				{
					_enemyBulletSpeedDevisor += DECELERATION_RATE;
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
			_moveType = MoveType.Line;
			_speedType = SpeedType.Middle;
			_enemyBulletSpeedDevisor = MIDDLE_SPEED;
		}

		if (collision.CompareTag("Player"))
		{
			_playerIDamageable.Damage();
			ParticleScript enemyParticle = _enemyParticlePool.LendEnemyParicle(this.transform.position, _bulletNumber);
			enemyParticle.Play();
			EnemyBulletPool.Instance.ReturnBullet(this, _bulletNumber);

			// 各状態を初期化する
			base._moveType = MoveType.Line;
			base._speedType = SpeedType.Middle;

			this.transform.rotation = Quaternion.identity;
			_enemyBulletSpeedDevisor = MIDDLE_SPEED;
		}
	}
	#endregion
}