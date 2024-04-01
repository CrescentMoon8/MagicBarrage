// ---------------------------------------------------------
// SlimeBoss.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BossSlime : EnemyHp
{
	#region 変数
	// 撃ちたい角度
	private float _targetAngle = 180;
	// 角度を何分割するか
	private int _angleSplit = 9;
	// 撃ちたい角度の±いくらか
	private int _angleWidth = 30;

	private float _windmillBulletTime = 0f;
	private int _windmillBulletCount = 0;
	// 撃ちたい角度
	private float _windmillTargetAngle = 180;
	// 角度を何分割するか
	private int _windmillAngleSplit = 9;
	// 開始点を何度ずつずらすかを(360 / BULLET_AMOUNT)で決める
	private const float BULLET_AMOUNT = 108;
	private const float WINDMILL_BULLET_INTERVAL = 0.175f;

	private float _gridBulletTime = 0f;
	private int _gridBulletCount = 0;
	// 撃ちたい角度
	private float _gridTargetAngle = 180;
	private const int VERTICAL_SPLIT = 6;
	private const int HORIZONTAL_SPLIT = 10;
	private const float GRID_BULLET_INTERVAL = 0.5f;

	private const float SHOT_INTERVAL = 1f;

	private Event _bossShot;

	private EnemyShot _enemyBulletPut = default;
	private EnemyMove _enemyMove = default;
	private BulletInfo _bulletInfo = default;
	private EnemyDataBase _enemyDataBase = default;
    [SerializeField]
    private BarrageTemplate _barrageTemplate = default;
    #endregion

    #region メソッド
    private void OnEnable ()
    {
		_enemyBulletPut = new EnemyShot(this.transform.localScale.x / 2);
		_enemyMove = new EnemyMove();

		_bulletInfo = Addressables.LoadAssetAsync<BulletInfo>("BulletInfo").WaitForCompletion();
		_enemyDataBase = Addressables.LoadAssetAsync<EnemyDataBase>("EnemyDataBase").WaitForCompletion();

		EnemyData enemyData = _enemyDataBase._enemyDataList[_enemyDataBase.BOSS_SLIME];
		base._enemyNumber = _enemyDataBase.BOSS_SLIME;

		base._hpValue = enemyData._maxHp;
		base._hpSlider.maxValue = base._hpValue;
		base._hpSlider.value = base._hpValue;

        base._killPoint = EnemyHp.BOSS_KILL_POINT;

        _enemyMove.SetSplineContainer(enemyData._splineIndex);
		_enemyMove.DifferencePosInitialize(this.transform.position);

		Addressables.Release(_enemyDataBase);
	}

	/// <summary>
	/// 非アクティブになったときにBulletInfoをアンロードする
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

		//ShootGridBullet(GRID_BULLET_INTERVAL);

        if (_isInsideCamera && _enemyBulletPut.IsShot(SHOT_INTERVAL))
        {
			_targetAngle = _windmillTargetAngle;

            // 射撃に必要なパラメータを生成する
            ShotParameter fanShotParameter1 = new ShotParameter(this.transform.position, _targetAngle, _angleSplit, _angleWidth, _bulletInfo.RED_NOMAL_BULLET, Bullet.MoveType.Line, Bullet.SpeedType.Middle);
            _enemyBulletPut.FanShot(fanShotParameter1);

            // 射撃に必要なパラメータを生成する
            ShotParameter fanShotParameter2 = new ShotParameter(this.transform.position, _targetAngle + 180, _angleSplit, _angleWidth, _bulletInfo.RED_NOMAL_BULLET, Bullet.MoveType.Line, Bullet.SpeedType.Middle);
            _enemyBulletPut.FanShot(fanShotParameter2);

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
			ShotParameter roundShotParameter = new ShotParameter(this.transform.position, _windmillTargetAngle, _windmillAngleSplit, _bulletInfo.PURPLE_NOMAL_BULLET, Bullet.MoveType.Line, Bullet.SpeedType.Middle);
			_enemyBulletPut.RoundShot(roundShotParameter);
			_windmillBulletCount++;
			_windmillBulletTime = 0;
			_windmillTargetAngle += 360 / BULLET_AMOUNT;
		}
		else
		{
			_windmillTargetAngle = 180;
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
			_gridTargetAngle = 180;
			Vector3 lengthVector = FieldSize.Instance.MaxFieldVector - FieldSize.Instance.MinFieldVector;

            for (int i = 0; i <= VERTICAL_SPLIT; i++)
            {
				Vector3 startPos = FieldSize.Instance.MaxFieldVector;
				startPos.x = FieldSize.Instance.MinFieldVector.x;
				startPos.x += lengthVector.x / VERTICAL_SPLIT * i;

				ShotParameter roundShotParameter = new ShotParameter(startPos, _gridTargetAngle, _bulletInfo.GREEN_NOMAL_BULLET, Bullet.MoveType.Line, Bullet.SpeedType.Low);
                _enemyBulletPut.LineShot(roundShotParameter);
            }

			_gridTargetAngle = 90;

			for (int i = 0; i <= HORIZONTAL_SPLIT; i++)
			{
				Vector3 startPos = FieldSize.Instance.MaxFieldVector;
				startPos.y -= lengthVector.y / HORIZONTAL_SPLIT * i;

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