// ---------------------------------------------------------
// Enemy3.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;

public class Enemy3 : EnemyBase
{
	#region 変数
    private const string PLAYER_BULLET_TAG = "PlayerBullet";

    private const int ENEMY_HP = 20;

	// 撃ちたい角度 (Unity基準)
	private int _centerAngle = 180;
	// 角度を何分割するか
	private int _angleSplit = 12;
	// 撃ちたい角度の±いくらか
	private int _angleWidth = 180;
    // 自分のオブジェクトの半径
    private float _radius = 0f;

    private float _shotTime = 0f;
	private const float SHOT_INTERVAL = 2f;

	[SerializeField]
	private GameObject _test;
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

		_radius = this.transform.localScale.x / 2;

        // _splineContainer.Splines[0].EvaluatePosition(0) → Spline0の始点の座標
        // _splineContainer.Splines[1].EvaluatePosition(0) → Spline1の始点の座標
        // Debug.Log(_splineContainer.Splines[0].EvaluatePosition(0).y);
    }

    /// <summary>
    /// 配置位置の確認用メソッド
    /// </summary>
    /// <param name="shooterPos"></param>
    /// <param name="angle"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    private Vector3 CirclePosCalculate(Vector3 shooterPos, float angle, float radius)
    {
        Vector3 circlePos = shooterPos;

        circlePos.x += Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        circlePos.y += Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        return circlePos;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update ()
	{
		_shotTime += Time.deltaTime;

		base._enemyMove.Move();

		base.FollowHpBar(this.transform.position);


		if ( _shotTime > SHOT_INTERVAL )
		{
            /*// 三方向に扇形の弾を撃つ
			for (int i = 0; i < 3; i++)
			{
				base.RoundShot(this.transform.position, _maxAngle, _angleSplit, _direction, 0, Bullet.MoveType.Line);
				_direction -= 90;
			}*/
            base._puttingEnemyBullet.FanShot(this.transform.position, _centerAngle, _angleSplit, _angleWidth, 8, Bullet.MoveType.Line);

            int minAngle = _centerAngle - _angleWidth;
            int maxAngle = 2 * _angleWidth;

            // テスト用
            for (int i = 0; i <= _angleSplit; i++)
            {
                Vector3 bulletPos = CirclePosCalculate(this.transform.position, (maxAngle / _angleSplit) * i + minAngle + 90, this.transform.localScale.x / 2);
                Instantiate(_test, bulletPos, Quaternion.identity);
            }
            /*base.LineShot()*/
            _shotTime = 0f;
			//_direction = 315;
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