// ---------------------------------------------------------
// PlayerBullet.cs
//
// 作成日:2024/03/15
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

/// <summary>
/// 弾の移動処理、回転処理、プレイヤーのダメージ処理の呼び出しを行う
/// </summary>
public class PlayerBullet : Bullet
{
    #region 変数
    // プレイヤーの弾の速度を調整する（高くすれば遅く、低くすれば早くなる）
    private float _playerBulletSpeedDevisor = 3f;

	// 敵とプレイヤーのベクトル差分
	[SerializeField]
	private Vector3 _distanceVector = Vector3.zero;

	// 一番近い敵の番号
	private int _nearEnemyIndex = 0;

    // 現在フェーズの敵情報のリスト参照用のインターフェース
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
				GetNearEnemyIndex();
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
                // 移動量の計算
                movePos += transform.up / _playerBulletSpeedDevisor;
				break;

			case MoveType.Tracking:
				GetNearEnemyPos(_nearEnemyIndex);

				if (ExistsEnemyPosList(_distanceVector, _nearEnemyIndex))
				{
					_moveType = MoveType.Line;
				}

                // 弾から敵への角度を計算
                int angle = Calculation.Instance.TargetDirectionAngle(_distanceVector, this.transform.position);

                rotateAngle = Quaternion.Euler(Vector3.forward * angle);

                // 移動量の計算
                movePos += transform.up / _playerBulletSpeedDevisor;
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
	/// プレイヤーと弾の距離、プレイヤーと敵の距離を比べる
	/// </summary>
	/// <param name="targetPos">追尾対象の座標</param>
	/// <returns>敵が死亡したか</returns>
	private bool ExistsEnemyPosList(Vector3 targetPos, int nearEnemyIndex)
	{
		//Debug.Log(targetPos);

		// nearEnemyIndexが最大値をとっていた場合にエラーが出るため、最初にリストの長さと比べる
		if(_iEnemyList.CurrentPhaseEnemyList.Count <= nearEnemyIndex)
        {
			return true;
        }

		if (_iEnemyList.CurrentPhaseEnemyList[nearEnemyIndex].transform.position == targetPos)
		{
			return false;
		}
		else
        {
			return true;
		}
	}

	private void GetNearEnemyIndex()
    {
		// 一番近い敵の番号
		_nearEnemyIndex = 0;
		// 初期値を一番目の敵との距離にすることで、初期値ゼロより処理を一回減らす
		float nearEnemyDistance = Calculation.Instance.TargetDistance(_iEnemyList.CurrentPhaseEnemyList[0].transform.position, _iPlayerPos.PlayerPos);

		for (int i = 1; i < _iEnemyList.CurrentPhaseEnemyList.Count; i++)
		{
			float enemyDistance = Calculation.Instance.TargetDistance(_iEnemyList.CurrentPhaseEnemyList[i].transform.position, _iPlayerPos.PlayerPos);

			// 敵の距離の比較
			if (enemyDistance < nearEnemyDistance)
			{
				// 一番近い敵の距離の更新
				nearEnemyDistance = enemyDistance;
				_nearEnemyIndex = i;
			}
		}
	}

	/// <summary>
	/// 一番近い敵の座標を取得する
	/// </summary>
	private void GetNearEnemyPos(int nearEnemyIndex)
	{
		if(nearEnemyIndex < _iEnemyList.CurrentPhaseEnemyList.Count)
        {
			_distanceVector = _iEnemyList.CurrentPhaseEnemyList[nearEnemyIndex].transform.position;
		}
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
			_iEnemyList.CurrentPhaseIDamageableList[collision.transform.GetSiblingIndex()].Damage();

            // 弾が壊れた時のパーティクルを取り出して再生する
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