// ---------------------------------------------------------
// PurpleSlime.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PurpleSlime : EnemyHp
{
	#region 変数
	// 撃ちたい角度
	private float _targetAngle = 180;
	// 角度を何分割するか
	private int _angleSplit = 36;

    private float _shotTime = 0f;
	private const float SHOT_INTERVAL = 2f;

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
    /// <summary>
    /// 更新前処理
    /// </summary>
    private void OnEnable ()
	{
		_enemyBulletPut = new EnemyShot(this.transform.localScale.x / 2);
		_enemyMove = new EnemyMove();

		_bulletInfo = Addressables.LoadAssetAsync<BulletInfo>("BulletInfo").WaitForCompletion();
		_enemyDataBase = Addressables.LoadAssetAsync<EnemyDataBase>("EnemyDataBase").WaitForCompletion();

		base._hpValue = _enemyDataBase._enemyDataList[_enemyDataBase.PURPLE_SLIME]._maxHp;
		base._hpSlider.maxValue = base._hpValue;
		base._hpSlider.value = base._hpValue;

		_enemyMove.SetSplineContainer(_enemyDataBase._enemyDataList[_enemyDataBase.PURPLE_SLIME]._splineIndex);
		_enemyMove.DifferencePosInitialize(this.transform.position);

		Addressables.Release(_enemyDataBase);
		// _splineContainer.Splines[0].EvaluatePosition(0) → Spline0の始点の座標
		// _splineContainer.Splines[1].EvaluatePosition(0) → Spline1の始点の座標
		// Debug.Log(_splineContainer.Splines[0].EvaluatePosition(0).y);
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
			ShotParameter roundShotParameter = new ShotParameter(this.transform.position, _targetAngle, _angleSplit, _bulletInfo.PURPLE_NOMAL_BULLET, Bullet.MoveType.Line);
			_enemyBulletPut.RoundShot(roundShotParameter);

			_enemyBulletPut.ResetShotTime();
		}
	}
    #endregion
}