// ---------------------------------------------------------
// RedSlime.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.AddressableAssets;

public class RedSlime : EnemyHp
{
	#region 変数
	// 撃ちたい角度
	private float _targetAngle = 180;
	// 角度を何分割するか
	private int _angleSplit = 10;
	// 撃ちたい角度の±いくらか
	private int _angleWidth = 45;

    // 弾を撃つ間隔
    private const float BULLET_INTERVAL = 0.25f;
    // 弾を撃つ間隔を計る
    private float _bulletTime = 0f;
    // 撃つ弾の数
    private const int BULLET_AMOUNT = 5;
    // 撃った弾の数
    private int _bulletCount = 0;

	// 全体の弾を撃つ間隔
	private const float SHOT_INTERVAL = 2f;

	private EnemyBulletPut _enemyBulletPut = default;
	private EnemyMove _enemyMove = default;
	private BulletInfo _bulletInfo = default;
	private EnemyDataBase _enemyDataBase = default;
/*    [SerializeField]
    private BarrageTemplate _barrageTemplate = default;*/
    #endregion

    #region メソッド
    /// <summary>
    /// 更新前処理
    /// </summary>
    private void OnEnable ()
	{
        // 移動と攻撃のためのスクリプトを生成する
        _enemyBulletPut = new EnemyBulletPut(this.transform.localScale.x / 2);
		_enemyMove = new EnemyMove();

        // 弾の情報を敵のデータベースをロードする
        _bulletInfo = Addressables.LoadAssetAsync<BulletInfo>("BulletInfo").WaitForCompletion();
		_enemyDataBase = Addressables.LoadAssetAsync<EnemyDataBase>("EnemyDataBase").WaitForCompletion();

        // 敵のデータベースから必要な情報を取得する
        EnemyData enemyData = _enemyDataBase._enemyDataList[_enemyDataBase.RED_SLIME];
		base._enemyNumber = _enemyDataBase.RED_SLIME;

        // 不要になった敵のデータベースをアンロードする
        Addressables.Release(_enemyDataBase);

        // HPの設定を行う
        base._hpValue = enemyData._maxHp;
		base._hpSlider.maxValue = base._hpValue;
		base._hpSlider.value = base._hpValue;

        // 倒されたときに獲得できるスコアの設定
        base._killPoint = EnemyHp.NOMAL_KILL_POINT;

        /*_targetAngle = _barrageTemplate.TargetAngle;
        _angleWidth = _barrageTemplate.AngleWidth;
        _angleSplit = _barrageTemplate.AngleSplit;*/

        // 敵の移動経路の設定
        _enemyMove.SetSplineContainer(_enemyDataBase._enemyDataList[_enemyDataBase.RED_SLIME]._splineIndex);
		_enemyMove.DifferencePosInitialize(this.transform.position);
    }

    /// <summary>
    /// 非アクティブになったときに弾の情報をアンロードする
    /// </summary>
    private void OnDisable()
    {
		Addressables.Release(_bulletInfo);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update ()
	{
		if (base._isDead)
		{
			return;
		}

		_bulletTime += Time.deltaTime;

		this.transform.position = _enemyMove.NextMovePos();

		base.FollowHpBar(this.transform.position);

        // 画面内にいて、弾を撃つ間隔が過ぎていれば
        if (_isInsideCamera && _enemyBulletPut.IsShot(SHOT_INTERVAL))
		{
			/* 
			 * 指定した秒数間隔で指定した回数撃つ
			 */
			if (_bulletTime < BULLET_INTERVAL)
            {
				return;
            }

			// プレイヤーに向けて撃つかどうか
			if(base._isPlayerTarget)
			{
				// プレイヤーへの角度を計算する
                _targetAngle = Calculation.Instance.TargetDirectionAngle(base._iPlayerPos.PlayerPos, this.transform.position);
            }

			if (_bulletTime >= BULLET_INTERVAL && _bulletCount < BULLET_AMOUNT)
            {
				// 追尾弾の初弾と同時に扇形の通常弾を打つ
				if(_bulletCount == 0)
                {
					// 発射に必要なパラメータを生成する
					ShotParameter fanShotParameter = new ShotParameter(this.transform.position, _targetAngle, _angleSplit, _angleWidth, _bulletInfo.RED_NOMAL_BULLET, Bullet.MoveType.Line, Bullet.SpeedType.Middle);
					_enemyBulletPut.FanShot(fanShotParameter);
				}
                // 発射に必要なパラメータを生成する
                ShotParameter lineShotParameter = new ShotParameter(this.transform.position, _targetAngle, _bulletInfo.RED_NEEDLE_BULLET, Bullet.MoveType.Tracking, Bullet.SpeedType.Middle);
				_enemyBulletPut.LineShot(lineShotParameter);

				_bulletCount++;
				_bulletTime = 0;
			}
			else
            {
				_bulletCount = 0;
				_enemyBulletPut.ResetShotTime();
			}
		}
	}
    #endregion
}