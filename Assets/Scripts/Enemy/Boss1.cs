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

    private const int BOSS_HP = 120;

	private SplineContainer _splineContainer = default;
	private int _splineIndex = 0;

	private float _moveTime = 0;
	private Vector3 differencePos = Vector3.zero;

	// 撃ちたい角度
	private int _centerAngle = 270;
	// 角度を何分割するか
	private int _angleSplit = 9;
	// 撃ちたい角度の±いくらか
	private int _angleWidth = 45;
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
    private void OnEnable ()
    {
		base._hpSlider.maxValue = BOSS_HP;
		base._hpSlider.value = BOSS_HP;
		base._hpValue = BOSS_HP;

		_radius = this.transform.localScale.x / 2;

		float firstEnemyPosX = this.transform.position.x;
		float firstEnemyPosY = this.transform.position.y;

		_splineContainer = GameObject.Find("FirstSpline").GetComponent<SplineContainer>();
		float firstBezierPosX = _splineContainer.Splines[_splineIndex].EvaluatePosition(0).x;
		float firstBezierPosY = _splineContainer.Splines[_splineIndex].EvaluatePosition(0).y;

		differencePos = new Vector3(firstEnemyPosX - firstBezierPosX, firstEnemyPosY - firstBezierPosY, 0);
	}

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update ()
	{
		_shotTime += Time.deltaTime;
		_moveTime += Time.deltaTime / 5;

		// base.FollowHpBar(this.transform.position);

		Vector3 movePos = _splineContainer.Splines[_splineIndex].EvaluatePosition(_moveTime);
		this.transform.position = movePos + differencePos;
		//base._playerPos = base._player.transform.position;

		if( _shotTime > SHOT_INTERVAL )
		{
            /*// 三方向に扇形の弾を撃つ
			for (int i = 0; i < 3; i++)
			{
				base.RoundShot(this.transform.position, _maxAngle, _angleSplit, _direction, 0, Bullet.MoveType.Line);
				_direction -= 90;
			}*/
            base._puttingEnemyBullet.RoundShot(this.transform.position, _centerAngle, _angleSplit, _angleWidth, _radius, 0, Bullet.MoveType.Line);
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