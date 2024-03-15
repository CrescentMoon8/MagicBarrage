// ---------------------------------------------------------
// BlueSlime.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BlueSlime : EnemyHp
{
	#region 変数
	// 撃ちたい角度
	private float _targetAngle = 0;
	// 撃ちたい角度の±いくらか
	private int _angleWidth = 0;
	// 角度を何分割するか
	private int _angleSplit = 0;

	private const float SHOT_INTERVAL = 2f;

	private EnemyShot _enemyBulletPut = default;
	private EnemyMove _enemyMove = default;
	private BulletInfo _bulletInfo = default;
	private EnemyDataBase _enemyDataBase = default;
	[SerializeField]
	private BarrageTemplate _barrageTemplate = default;
	#endregion

	#region メソッド
	/// <summary>
	/// 更新前処理
	/// </summary>
	private void OnEnable ()
	{
		_enemyBulletPut = new EnemyShot(this.transform.localScale.x / 2);
		_enemyMove = new EnemyMove();

		_bulletInfo = Addressables.LoadAssetAsync<BulletInfo>("BulletInfo").WaitForCompletion();
		_enemyDataBase = Addressables.LoadAssetAsync<EnemyDataBase>("EnemyDataBase").WaitForCompletion();

		EnemyData enemyData = _enemyDataBase._enemyDataList[_enemyDataBase.BLUE_SLIME];
		base._enemyNumber = _enemyDataBase.BLUE_SLIME;

		base._hpValue = enemyData._maxHp;
		base._hpSlider.maxValue = base._hpValue;
		base._hpSlider.value = base._hpValue;

        _targetAngle = _barrageTemplate.TargetAngle;
        _angleWidth = _barrageTemplate.AngleWidth;
        _angleSplit = _barrageTemplate.AngleSplit;

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
		this.transform.position = _enemyMove.NextMovePos();

		base.FollowHpBar(this.transform.position);

		if (_isInsideCamera && _enemyBulletPut.IsShot(SHOT_INTERVAL))
		{
			// 射撃に必要なパラメータを生成する
			ShotParameter fanShotParameter = new ShotParameter(this.transform.position, _targetAngle, _angleSplit, _angleWidth, _bulletInfo.BLUE_NOMAL_BULLET, Bullet.MoveType.Line, Bullet.SpeedType.Middle);
			_enemyBulletPut.FanShot(fanShotParameter);

			_enemyBulletPut.ResetShotTime();
		}
	}
    #endregion
}