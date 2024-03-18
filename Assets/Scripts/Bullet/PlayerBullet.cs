// ---------------------------------------------------------
// PlayerBullet.cs
//
// 作成日:2024/03/15
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

public class PlayerBullet : Bullet
{
	#region 変数
	private float _playerBulletSpeedDevisor = 3f;

	// エネミーとプレイヤーのベクトル差分
	[SerializeField]
	private Vector3 _distanceVector = Vector3.zero;

	private IEnemyList _iEnemyList = default;
	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
		base.GetIPlayerPos();

		_iEnemyList = EnemyPhaseManager.Instance;
	}

	/// <summary>
	/// 弾の初期化処理
	/// </summary>
	public override void Initialize()
    {
		switch (base._moveType)
		{
			case MoveType.Line:
				this.transform.rotation = Quaternion.identity;
				break;
			case MoveType.Tracking:
				GetNearEnemyPos();
				break;
			case MoveType.Curve:
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
				movePos += transform.up / _playerBulletSpeedDevisor;
				break;

			case MoveType.Tracking:
				int angle = Calculation.TargetDirectionAngle(_distanceVector, this.transform.position);

				Quaternion targetRotation = Quaternion.Euler(Vector3.forward * angle);
				//（現在角度、目標方向、どれぐらい曲がるか）
				rotateAngle = Quaternion.RotateTowards(this.transform.rotation, targetRotation, 100f);

				movePos += transform.up / _playerBulletSpeedDevisor;

				if (ExistsEnemyPosList(_distanceVector))
				{
					_moveType = MoveType.Line;
				}
				break;

			case MoveType.Curve:
				break;

			default:
				break;
		}

		this.transform.SetPositionAndRotation(movePos, rotateAngle);
	}

	/// <summary>
	/// プレイヤーと弾の距離、プレイヤーとエネミーの距離を比べる
	/// </summary>
	/// <param name="targetPos">追尾対象の座標</param>
	/// <returns></returns>
	private bool ExistsEnemyPosList(Vector3 targetPos)
	{
		if (Calculation.TargetDistance(targetPos, _iPlayerPos.PlayerPos) <= Calculation.TargetDistance(this.transform.position, _iPlayerPos.PlayerPos))
		{
			return true;
		}

		return false;
	}

	/// <summary>
	/// 一番近いエネミーの座標を取得する
	/// </summary>
	private void GetNearEnemyPos()
	{
		// 一番近いエネミーの番号
		int nearEnemyIndex = 0;
		// 初期値を一番目のエネミーとの距離にすることで、初期値ゼロより処理を一回減らす
		float nearEnemyDistance = Calculation.TargetDistance(_iEnemyList.CurrentPhaseEnemyList[0].transform.position, _iPlayerPos.PlayerPos);

		for (int i = 1; i < _iEnemyList.CurrentPhaseEnemyList.Count; i++)
		{
			float enemyDistance = Calculation.TargetDistance(_iEnemyList.CurrentPhaseEnemyList[i].transform.position, _iPlayerPos.PlayerPos);

			// エネミーの距離の比較
			if (enemyDistance < nearEnemyDistance)
			{
				// 一番近いエネミーの距離の更新
				nearEnemyDistance = enemyDistance;
				nearEnemyIndex = i;
			}
		}

		// 一番近いエネミーの座標を代入する
		_distanceVector = _iEnemyList.CurrentPhaseEnemyList[nearEnemyIndex].transform.position;
	}

	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("ReturnPool"))
		{
			PlayerBulletPool.Instance.ReturnBullet(this);

			// 各状態を初期化する
			base._moveType = MoveType.Line;
		}

		if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
		{
			// GetSiblingIndexで当たったオブジェクトが同じ階層で上から何番目かを取得する
			// Enemyの情報をヒエラルキーの上から順に取得しているためちゃんと動いている
			_iEnemyList.CurrentPhaseIDamageableList[collision.transform.GetSiblingIndex()].Damage();
			ParticleScript playerParticle = PlayerParticlePool.Instance.LendPlayerParticle(this.transform.position);
			playerParticle.Play();
			PlayerBulletPool.Instance.ReturnBullet(this);

			// 各状態を初期化する
			base._moveType = MoveType.Line;

			_distanceVector = Vector3.zero;
			this.transform.rotation = Quaternion.identity;
		}
	}
	#endregion
}