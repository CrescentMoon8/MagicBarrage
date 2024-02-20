// ---------------------------------------------------------
// RedSlime.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;

public class RedSlime : EnemyBase
{
	#region 変数
	private const int MOVE_PATTERN_INDEX = 0;
    private const string PLAYER_BULLET_TAG = "PlayerBullet";

    private const int ENEMY_HP = 20;

	// 撃ちたい角度
	private int _centerAngle = 180;
	// 角度を何分割するか
	private int _angleSplit = 10;
	// 撃ちたい角度の±いくらか
	private int _angleWidth = 45;
    // 自分のオブジェクトの半径
    private float _radius = 0f;

	private const float BULLET_INTERVAL = 0.25f;
	private float _bulletTime = 0f;
	private const int BULLET_AMOUNT = 5;
	private int _bulletCount = 0;

	private GameObject _playerObject = default;
	private Vector3 _playerPos = Vector3.zero;

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

		_enemyMove.SetSplineContainer(MOVE_PATTERN_INDEX);
		_enemyMove.DifferencePosInitialize(this.transform.position);

		// _splineContainer.Splines[0].EvaluatePosition(0) → Spline0の始点の座標
		// _splineContainer.Splines[1].EvaluatePosition(0) → Spline1の始点の座標
		// Debug.Log(_splineContainer.Splines[0].EvaluatePosition(0).y);

		_playerObject = GameObject.FindWithTag("Player");
        _playerPos = _playerObject.transform.position;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update ()
	{
		_shotTime += Time.deltaTime;
		_bulletTime += Time.deltaTime;

		this.transform.position = base._enemyMove.MovePosCalculate();

		base.FollowHpBar(this.transform.position);

		_playerPos = _playerObject.transform.position;

		if ( _shotTime >= SHOT_INTERVAL )
		{
			/* 
			 * 指定した秒数間隔で指定した回数撃つ
			 */
			if(_bulletTime < BULLET_INTERVAL)
            {
				return;
            }

			if(_bulletTime >= BULLET_INTERVAL && _bulletCount < BULLET_AMOUNT)
            {
				base._puttingEnemyBullet.LineShot(this.transform.position, Calculation.TargetDirectionAngle(_playerPos, this.transform.position), 1, Bullet.MoveType.Tracking);

				_bulletCount++;
				_bulletTime = 0;
			}
			else
            {
				_bulletCount = 0;
				_shotTime = 0f;
				base._puttingEnemyBullet.FanShot(this.transform.position, Calculation.TargetDirectionAngle(_playerPos, this.transform.position), _angleSplit, _angleWidth, 0, Bullet.MoveType.Line);
			}

			
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYER_BULLET_TAG))
        {
            base.EnemyDamage();
        }
    }
    #endregion
}