// ---------------------------------------------------------
// BossSlime.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BossSlime : EnemyHp
{
    #region 変数
    #region 通常
    // 撃ちたい角度
    private float _targetAngle = 180;
	// 角度を何分割するか
	private int _angleSplit = 9;
	// 撃ちたい角度の±いくらか
	private int _angleWidth = 30;
    #endregion

    #region 風車
	// 弾を撃つ間隔を計る
    private float _windmillBulletTime = 0f;
	// 撃った弾の数
	private int _windmillBulletCount = 0;
	// 撃ちたい角度
	private float _windmillTargetAngle = 180;
	// 角度を何分割するか
	private int _windmillAngleSplit = 9;
	// 開始点を何度ずつずらすかを(360 / BULLET_AMOUNT)で決める、撃つ弾の数
	private const float BULLET_AMOUNT = 108;
    // 弾を撃つ間隔
    private const float WINDMILL_BULLET_INTERVAL = 0.175f;
    #endregion

    #region 格子
    // 弾を撃つ間隔を計る
    private float _gridBulletTime = 0f;
    // 撃った弾の数
    private int _gridBulletCount = 0;
	// 撃ちたい角度
	private float _gridTargetAngle = 180;
	// 縦を何分割するか
	private const int VERTICAL_SPLIT = 6;
	// 横を何分割するか
	private const int HORIZONTAL_SPLIT = 10;
    // 弾を撃つ間隔
    private const float GRID_BULLET_INTERVAL = 0.5f;
	#endregion

	// 最大角度
	private const int MAX_ANGLE = 360;
	// 左方向
	private const int DIRECTION_LEFT = 90;
	// 下方向
	private const int DIRECTION_DOWN = 180;

	// 全体の弾を撃つ間隔
    private const float SHOT_INTERVAL = 1f;

	private EnemyBulletPut _enemyBulletPut = default;
	private EnemyMove _enemyMove = default;
	private BulletInfo _bulletInfo = default;
	private EnemyDataBase _enemyDataBase = default;
    #endregion

    #region メソッド
    private void OnEnable ()
    {
		// 移動と攻撃のためのスクリプトを生成する
		_enemyBulletPut = new EnemyBulletPut(this.transform.localScale.x / 2);
		_enemyMove = new EnemyMove();

		// 弾の情報を敵のデータベースをロードする
		_bulletInfo = Addressables.LoadAssetAsync<BulletInfo>("BulletInfo").WaitForCompletion();
		_enemyDataBase = Addressables.LoadAssetAsync<EnemyDataBase>("EnemyDataBase").WaitForCompletion();

		// 敵のデータベースから必要な情報を取得する
		EnemyData enemyData = _enemyDataBase._enemyDataList[_enemyDataBase.BOSS_SLIME];
		base._enemyNumber = _enemyDataBase.BOSS_SLIME;

        // 不要になった敵のデータベースをアンロードする
        Addressables.Release(_enemyDataBase);

        // HPの設定を行う
        base._hpValue = enemyData._maxHp;
		base._hpSlider.maxValue = base._hpValue;
		base._hpSlider.value = base._hpValue;

		// 倒されたときに獲得できるスコアの設定
        base._killPoint = EnemyHp.BOSS_KILL_POINT;

		// 敵の移動経路の設定
        _enemyMove.SetSplineContainer(enemyData._splineIndex);
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
		if(base._isDead)
        {
			return;
        }

        this.transform.position = _enemyMove.NextMovePos();

		ShootWindmillBullet(WINDMILL_BULLET_INTERVAL);

		// 難易度がHardなら
		if(GameDataManager.Instance.PlayerDifficult)
		{
            ShootGridBullet(GRID_BULLET_INTERVAL);
        }

        // 画面内にいて、弾を撃つ間隔が過ぎていれば
        if (_isInsideCamera && _enemyBulletPut.IsShot(SHOT_INTERVAL))
        {
			_targetAngle = _windmillTargetAngle;

            // 発射に必要なパラメータを生成する
            ShotParameter upFanShotParameter = new ShotParameter(this.transform.position, _targetAngle, _angleSplit, _angleWidth, _bulletInfo.RED_NOMAL_BULLET, Bullet.MoveType.Line, Bullet.SpeedType.Middle);
            _enemyBulletPut.FanShot(upFanShotParameter);

            // 発射に必要なパラメータを生成する
            ShotParameter downFanShotParameter = new ShotParameter(this.transform.position, _targetAngle + 180, _angleSplit, _angleWidth, _bulletInfo.RED_NOMAL_BULLET, Bullet.MoveType.Line, Bullet.SpeedType.Middle);
            _enemyBulletPut.FanShot(downFanShotParameter);

            _enemyBulletPut.ResetShotTime();
        }
    }

	private void ShootWindmillBullet(float bulletInterval)
    {
		_windmillBulletTime += Time.deltaTime;

		/* 
		 * 指定した秒数間隔で指定した回数撃つ
		 */
		if (_windmillBulletTime < bulletInterval)
		{
			return;
		}

		if (_windmillBulletTime >= bulletInterval && _windmillBulletCount < BULLET_AMOUNT)
		{
            // 発射に必要なパラメータを生成する
            ShotParameter roundShotParameter = new ShotParameter(this.transform.position, _windmillTargetAngle, _windmillAngleSplit, _bulletInfo.PURPLE_NOMAL_BULLET, Bullet.MoveType.Line, Bullet.SpeedType.Middle);
			_enemyBulletPut.RoundShot(roundShotParameter);

			// 発射角度をずらす
            _windmillTargetAngle += MAX_ANGLE / BULLET_AMOUNT;

            _windmillBulletCount++;
			_windmillBulletTime = 0;
		}
		else
		{
			_windmillTargetAngle = DIRECTION_DOWN;
			_windmillBulletCount = 0;
		}
    }

	private void ShootGridBullet(float bulletInterval)
	{
		_gridBulletTime += Time.deltaTime;

		/* 
		 * 指定した秒数間隔で指定した回数撃つ
		 */
		if (_gridBulletTime < bulletInterval)
		{
			return;
		}

		if (_gridBulletTime >= bulletInterval && _gridBulletCount < BULLET_AMOUNT)
		{
			// 発射方向を下にする
			_gridTargetAngle = DIRECTION_DOWN;

			// フィールドの大きさを計算する
			Vector3 lengthVector = FieldSize.Instance.MaxFieldVector - FieldSize.Instance.MinFieldVector;

			// 縦方向の格子
            for (int i = 0; i <= VERTICAL_SPLIT; i++)
            {
                // 生成位置を右上に設定する
                Vector3 startPos = FieldSize.Instance.MaxFieldVector;
				// 生成位置を左上に移動
				startPos.x = FieldSize.Instance.MinFieldVector.x;
				// 何列目に生成するかを計算する
				startPos.x += lengthVector.x / VERTICAL_SPLIT * i;

                // 発射に必要なパラメータを生成する
                ShotParameter roundShotParameter = new ShotParameter(startPos, _gridTargetAngle, _bulletInfo.GREEN_NOMAL_BULLET, Bullet.MoveType.Line, Bullet.SpeedType.Low);
                _enemyBulletPut.LineShot(roundShotParameter);
            }

			// 発射方向を下にする
			_gridTargetAngle = DIRECTION_LEFT;

			// 横方向の格子
			for (int i = 0; i <= HORIZONTAL_SPLIT; i++)
			{
				// 生成位置を右上に設定する
				Vector3 startPos = FieldSize.Instance.MaxFieldVector;
				// 何段目に生成するかを計算する
				startPos.y -= lengthVector.y / HORIZONTAL_SPLIT * i;

                // 発射に必要なパラメータを生成する
                ShotParameter roundShotParameter = new ShotParameter(startPos, _gridTargetAngle, _bulletInfo.GREEN_NOMAL_BULLET, Bullet.MoveType.Line, Bullet.SpeedType.Low);
				_enemyBulletPut.LineShot(roundShotParameter);
			}

			_gridBulletCount++;
			_gridBulletTime = 0;
		}
		else
		{
			_gridBulletCount = 0;
		}
	}
	#endregion
}