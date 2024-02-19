// ---------------------------------------------------------
// PurpleSlime.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;

public class PurpleSlime : EnemyBase
{
	#region 変数
	private const int MOVE_PATTERN_INDEX = 2;

	private const string PLAYER_BULLET_TAG = "PlayerBullet";

    private const int ENEMY_HP = 20;

	// 撃ちたい角度 (Unity基準)
	private int _centerAngle = 180;
	// 角度を何分割するか
	private int _angleSplit = 72;
	// 撃ちたい角度の±いくらか
	private int _angleWidth = 180;

    private float _shotTime = 0f;
	private const float SHOT_INTERVAL = 2f;
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 更新前処理
	/// </summary>
	private void OnEnable ()
	{
		base._hpSlider.maxValue = ENEMY_HP;
		base._hpSlider.value = ENEMY_HP;
		base._hpValue = ENEMY_HP;

		_enemyMove.SetSplineContainer(MOVE_PATTERN_INDEX);
		_enemyMove.DifferencePosInitialize(this.transform.position);

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

		this.transform.position = base._enemyMove.MovePosCalculate();

		base.FollowHpBar(this.transform.position);


		if ( _shotTime > SHOT_INTERVAL )
		{
            /*// 三方向に扇形の弾を撃つ
			for (int i = 0; i < 3; i++)
			{
				base.RoundShot(this.transform.position, _maxAngle, _angleSplit, _direction, 0, Bullet.MoveType.Line);
				_direction -= 90;
			}*/
            base._puttingEnemyBullet.RoundShot(this.transform.position, _angleSplit, 8, Bullet.MoveType.Line);

            _shotTime = 0f;
		}
	}

	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(PLAYER_BULLET_TAG))
		{
			base.EnemyDamage();
		}
	}
	#endregion
}