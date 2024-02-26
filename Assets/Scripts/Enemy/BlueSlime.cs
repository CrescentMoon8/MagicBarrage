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
	private int _centerAngle = 180;
	// 撃ちたい角度の±いくらか
	private int _angleWidth = 45;
	// 角度を何分割するか
	private int _angleSplit = 10;

    private float _shotTime = 0f;
	private const float SHOT_INTERVAL = 2f;

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
		_bulletInfo = Addressables.LoadAssetAsync<BulletInfo>("BulletInfo").WaitForCompletion();
		_enemyDataBase = Addressables.LoadAssetAsync<EnemyDataBase>("EnemyDataBase").WaitForCompletion();

		base._hpValue = _enemyDataBase._enemyDataList[_enemyDataBase.BLUE_SLIME]._maxHp;
		base._hpSlider.maxValue = base._hpValue;
		base._hpSlider.value = base._hpValue;

		if(_barrageTemplate != null)
        {
			_centerAngle = _barrageTemplate._centerAngle;
			_angleWidth = _barrageTemplate._angleWidth;
			_angleSplit = _barrageTemplate._angleSplit;
		}
		
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
		_shotTime += Time.deltaTime;

		this.transform.position = base._enemyMove.NextMovePos();

		base.FollowHpBar(this.transform.position);

		if ( _shotTime > SHOT_INTERVAL )
		{
            /*// 三方向に扇形の弾を撃つ
			for (int i = 0; i < 3; i++)
			{
				base.RoundShot(this.transform.position, _maxAngle, _angleSplit, _direction, 0, Bullet.MoveType.Line);
				_direction -= 90;
			}*/

            base._puttingEnemyBullet.FanShot(this.transform.position, _centerAngle, _angleSplit, _angleWidth, _bulletInfo.BLUE_NOMAL_BULLET, Bullet.MoveType.Line);
			
            _shotTime = 0f;
		}
	}
    #endregion
}