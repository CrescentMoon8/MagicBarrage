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

	// 角度を何分割するか
	private int _windmillAngleSplit = 9;

	private const float BULLET_INTERVAL = 0.15f;
	private float _bulletTime = 0f;
	// 開始点を何度ずつずらすかを(360 / BULLET_AMOUNT)で決める
	private const float BULLET_AMOUNT = 108;
	private int _bulletCount = 0;

	private const float SHOT_INTERVAL = 1f;

	private Event _bossShot;

	private EnemyShot _enemyBulletPut = default;
	private EnemyMove _enemyMove = default;
	private BulletInfo _bulletInfo = default;
	private EnemyDataBase _enemyDataBase = default;
    [SerializeField]
    private BarrageTemplate _barrageTemplate = default;
    #endregion

    #region プロパティ

    #endregion

    #region メソッド
    private void OnEnable ()
    {
		_enemyBulletPut = new EnemyShot(this.transform.localScale.x / 2);
		_enemyMove = new EnemyMove();

		_bulletInfo = Addressables.LoadAssetAsync<BulletInfo>("BulletInfo").WaitForCompletion();
		_enemyDataBase = Addressables.LoadAssetAsync<EnemyDataBase>("EnemyDataBase").WaitForCompletion();

		base._hpValue = _enemyDataBase._enemyDataList[_enemyDataBase.BOSS_SLIME]._maxHp;
		base._hpSlider.maxValue = base._hpValue;
		base._hpSlider.value = base._hpValue;

		_enemyMove.SetSplineContainer(_enemyDataBase._enemyDataList[_enemyDataBase.BOSS_SLIME]._splineIndex);
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
        this.transform.position = _enemyMove.NextMovePos();

		ShootWindmillBullet();

		if (_isInsideCamera && _enemyBulletPut.IsShot(SHOT_INTERVAL))
		{
			// 射撃に必要なパラメータを生成する
			ShotParameter fanShotParameter1 = new ShotParameter(this.transform.position, _targetAngle, _angleSplit, _angleWidth, _bulletInfo.RED_NOMAL_BULLET, Bullet.MoveType.Line);
			_enemyBulletPut.FanShot(fanShotParameter1);

			// 射撃に必要なパラメータを生成する
			ShotParameter fanShotParameter2 = new ShotParameter(this.transform.position, _targetAngle + 180, _angleSplit, _angleWidth, _bulletInfo.RED_NOMAL_BULLET, Bullet.MoveType.Line);
			_enemyBulletPut.FanShot(fanShotParameter2);

			_enemyBulletPut.ResetShotTime();
		}
	}

    private void ShootWindmillBullet()
    {
		_bulletTime += Time.deltaTime;

		/* 
		 * 指定した秒数間隔で指定した回数撃つ
		 */
		if (_bulletTime < BULLET_INTERVAL)
		{
			return;
		}

		if (_bulletTime >= BULLET_INTERVAL && _bulletCount < BULLET_AMOUNT)
		{
			ShotParameter roundShotParameter = new ShotParameter(this.transform.position, _targetAngle, _windmillAngleSplit, _bulletInfo.PURPLE_NOMAL_BULLET, Bullet.MoveType.Line);
			_enemyBulletPut.RoundShot(roundShotParameter);
			_bulletCount++;
			_bulletTime = 0;
			_targetAngle += 360 / BULLET_AMOUNT;
		}
		else
		{
			_targetAngle = 180;
			_bulletCount = 0;
		}
    }
    #endregion
}