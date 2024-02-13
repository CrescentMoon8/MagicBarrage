// ---------------------------------------------------------
// Enemy1.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;

public class Enemy1 : EnemyBase
{
	#region 変数
    private const string PLAYER_BULLET_TAG = "PlayerBullet";

    private const int ENEMY_HP = 20;

	private SplineContainer _splineContainer = default;
	private int _splineIndex = 0;

	private float _moveTime = 0;
	private Vector3 differencePos = Vector3.zero;

	// 撃ちたい角度
	private int _centerAngle = 180;
	// 角度を何分割するか
	private int _angleSplit = 10;
	// 撃ちたい角度の±いくらか
	private int _angleWidth = 45;
    // 自分のオブジェクトの半径
    private float _radius = 0f;
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

		_radius = this.transform.localScale.x / 2;

		float firstEnemyPosX = this.transform.position.x;
		float firstEnemyPosY = this.transform.position.y;

        _splineContainer = GameObject.Find("EnemySpline").GetComponent<SplineContainer>();
		float firstBezierPosX = _splineContainer.Splines[_splineIndex].EvaluatePosition(0).x;
		float firstBezierPosY = _splineContainer.Splines[_splineIndex].EvaluatePosition(0).y;

		differencePos = new Vector3(firstEnemyPosX - firstBezierPosX, firstEnemyPosY - firstBezierPosY, 0);



        // _splineContainer.Splines[0].EvaluatePosition(0) → Spline0の始点の座標
        // _splineContainer.Splines[1].EvaluatePosition(0) → Spline1の始点の座標
        // Debug.Log(_splineContainer.Splines[0].EvaluatePosition(0).y);

        _playerObject = GameObject.FindWithTag("Player");
        _playerPos = _playerObject.transform.position;

        
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
		_moveTime += Time.deltaTime / 4;

		// EnemySplineの指定したSplineの座標を取得する
		Vector3 movePos = _splineContainer.Splines[_splineIndex].EvaluatePosition(_moveTime);
		// 
		this.transform.position = movePos + differencePos;

		base.FollowHpBar(this.transform.position);

		_playerPos = _playerObject.transform.position;

		if ( _shotTime > SHOT_INTERVAL )
		{
            /*// 三方向に扇形の弾を撃つ
			for (int i = 0; i < 3; i++)
			{
				base.RoundShot(this.transform.position, _maxAngle, _angleSplit, _direction, 0, Bullet.MoveType.Line);
				_direction -= 90;
			}*/
            base._puttingEnemyBullet.RoundShot(this.transform.position, base._puttingEnemyBullet.AngleFromEnemyCalculate(_playerPos, this.transform.position), _angleSplit, _angleWidth, _radius, 0, Bullet.MoveType.Line);
			
			/*int minAngle = base._puttingEnemyBullet.AngleFromEnemyCalculate(_playerPos, this.transform.position) - _angleWidth;
			int maxAngle = 2 * _angleWidth;

			// テスト用
			for (int i = 0; i <= _angleSplit; i++)
			{
				Vector3 bulletPos = CirclePosCalculate(this.transform.position, (maxAngle / _angleSplit) * i + minAngle + 90, this.transform.localScale.x / 2);
				Instantiate(_test, bulletPos, Quaternion.identity);
			}
			Debug.Log(this.name + base._puttingEnemyBullet.AngleFromEnemyCalculate(_playerPos, this.transform.position));*/
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