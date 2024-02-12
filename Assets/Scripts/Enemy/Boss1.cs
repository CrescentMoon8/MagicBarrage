// ---------------------------------------------------------
// Boss1.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.Splines;

public class Boss1 : EnemyBase
{
    #region 変数
    private const string PLAYER_BULLET_TAG = "PlayerBullet";

    private const int ENEMY_HP = 120;

	[SerializeField]
	private SplineContainer _splineContainer = default;
	private int _splineIndex = 0;

	private float _moveTime = 0;
	private float _firstEnemyPosX = 0;
	private float _firstEnemyPosY = 0;
	private float _firstBezierPosX = 0;
	private float _firstBezierPosY = 0;

	// 角度の最大
	private int _maxAngle = 90;
	// 最大角度を何分割するか
	private int _angleSplit = 9;
	// 初弾の位置調整用の変数
	private int _direction = 225;
    // 自分のオブジェクトの半径
    private float _radius = 0f;

    private float _shotTime = 0f;
	private const float SHOT_INTERVAL = 2f;

	[SerializeField]
	private GameObject _test;
	#endregion

	#region プロパティ
	public int SplineIndex { set { _splineIndex = value; } }
	#endregion

	#region メソッド
	/// <summary>
	/// 更新前処理
	/// </summary>
	private void Start ()
	{
		base._hpSlider.maxValue = ENEMY_HP;
		base._hpSlider.value = ENEMY_HP;
		base._hpValue = ENEMY_HP;

		_radius = this.transform.localScale.x / 2;

		_firstEnemyPosX = this.transform.position.x;
		_firstEnemyPosY = this.transform.position.y;

        _splineContainer = UnityEngine.GameObject.Find("Spline").GetComponent<SplineContainer>();
		_firstBezierPosX = _splineContainer.Splines[0].EvaluatePosition(0).x;
		_firstBezierPosY = _splineContainer.Splines[0].EvaluatePosition(0).y;

		

		// _splineContainer.Splines[0].EvaluatePosition(0) → Spline0の始点の座標
		// _splineContainer.Splines[1].EvaluatePosition(0) → Spline1の始点の座標
		// Debug.Log(_splineContainer.Splines[0].EvaluatePosition(0).y);

		/*base._player = GameObject.FindWithTag("Player");
		base._playerPos = _player.transform.position;*/
		// テスト用
		/*for (int i = 0; i < _angleSplit; i++)
		{
			Vector3 bulletPos = CirclePositionCalculate(this.transform.position, 270, this.transform.localScale.x / 2);
			Instantiate(_test, bulletPos, Quaternion.identity);
		}*/
	}

	/// <summary>
	/// 更新処理
	/// </summary>
	private void Update ()
	{
		_shotTime += Time.deltaTime;
		_moveTime += Time.deltaTime / 5;

		base.FollowHpBar();

		Vector3 movePos = _splineContainer.Splines[0].EvaluatePosition(_moveTime);
		movePos.x += _firstEnemyPosX - _firstBezierPosX;
		movePos.y += _firstEnemyPosY - _firstBezierPosY;
		this.transform.position = movePos;
		//base._playerPos = base._player.transform.position;

		if( _shotTime > SHOT_INTERVAL )
		{
            /*// 三方向に扇形の弾を撃つ
			for (int i = 0; i < 3; i++)
			{
				base.RoundShot(this.transform.position, _maxAngle, _angleSplit, _direction, 0, Bullet.MoveType.Line);
				_direction -= 90;
			}*/
            base._puttingEnemyBullet.RoundShot(this.transform.position, _maxAngle, _angleSplit, _direction, _radius, 0, Bullet.MoveType.Line);
			//Debug.Log(this.transform.position);
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