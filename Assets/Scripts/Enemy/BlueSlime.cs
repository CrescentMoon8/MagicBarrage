// ---------------------------------------------------------
// BlueSlime.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BlueSlime : EnemyBase
{
	#region 変数
	// 撃ちたい角度
	private float _targetAngle = 0;
	// 撃ちたい角度の±いくらか
	private int _angleWidth = 0;
	// 角度を何分割するか
	private int _angleSplit = 0;

	private const float SHOT_INTERVAL = 2f;

	private EnemyShot _enemyShot = default;
	private EnemyMove _enemyMove = default;
	private BulletInfo _bulletInfo = default;
	private EnemyDataBase _enemyDataBase = default;
	[SerializeField]
	private BarrageTemplate _barrageTemplate = default;
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 更新前処理
	/// </summary>
	private void OnEnable ()
	{
		_enemyShot = new EnemyShot(this.transform.localScale.x / 2);
		_enemyMove = new EnemyMove();

		_bulletInfo = Addressables.LoadAssetAsync<BulletInfo>("BulletInfo").WaitForCompletion();
		_enemyDataBase = Addressables.LoadAssetAsync<EnemyDataBase>("EnemyDataBase").WaitForCompletion();

		base._hpValue = _enemyDataBase._enemyDataList[_enemyDataBase.BLUE_SLIME]._maxHp;
		base._hpSlider.maxValue = base._hpValue;
		base._hpSlider.value = base._hpValue;

        _targetAngle = _barrageTemplate.TargetAngle;
        _angleWidth = _barrageTemplate.AngleWidth;
        _angleSplit = _barrageTemplate.AngleSplit;

        _enemyMove.SetSplineContainer(_enemyDataBase._enemyDataList[_enemyDataBase.BLUE_SLIME]._splineIndex);
		_enemyMove.DifferencePosInitialize(this.transform.position);

		Addressables.Release(_enemyDataBase);
		// _splineContainer.Splines[0].EvaluatePosition(0) → Spline0の始点の座標
		// _splineContainer.Splines[1].EvaluatePosition(0) → Spline1の始点の座標
		// Debug.Log(_splineContainer.Splines[0].EvaluatePosition(0).y);
	}

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update ()
	{
		this.transform.position = _enemyMove.NextMovePos();

		base.FollowHpBar(this.transform.position);

		if (_enemyShot.IsShot(SHOT_INTERVAL))
		{
            /*// 三方向に扇形の弾を撃つ
			for (int i = 0; i < 3; i++)
			{
				base.RoundShot(this.transform.position, _maxAngle, _angleSplit, _direction, 0, Bullet.MoveType.Line);
				_direction -= 90;
			}*/

            _enemyShot.FanShot(this.transform.position, _targetAngle, _angleSplit, _angleWidth, _bulletInfo.BLUE_NOMAL_BULLET, Bullet.MoveType.Line);

			_enemyShot.ResetShotTime();
		}
	}
    #endregion
}