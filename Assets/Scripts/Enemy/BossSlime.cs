// ---------------------------------------------------------
// SlimeBoss.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

public class BossSlime : EnemyBase
{
	#region 変数
	private const int MOVE_PATTERN_INDEX = 3;

	private const string PLAYER_BULLET_TAG = "PlayerBullet";

    private const int BOSS_HP = 120;

	// 撃ちたい角度
	private int _centerAngle = 180;
	// 角度を何分割するか
	private int _angleSplit = 18;
	// 撃ちたい角度の±いくらか
	private int _angleWidth = 45;

	private float _shotTime = 0f;
	private const float SHOT_INTERVAL = 2f;

	private Event _bossShot;
	#endregion

	#region プロパティ
	
	#endregion

	#region メソッド
    private void OnEnable ()
    {
		base._hpSlider.maxValue = BOSS_HP;
		base._hpSlider.value = BOSS_HP;
		base._hpValue = BOSS_HP;

		_enemyMove.SetSplineContainer(MOVE_PATTERN_INDEX);
		_enemyMove.DifferencePosInitialize(this.transform.position);
	}

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update ()
	{
		_shotTime += Time.deltaTime;

		this.transform.position = base._enemyMove.MovePosCalculate();

		//base._playerPos = base._player.transform.position;

		if ( _shotTime > SHOT_INTERVAL )
		{
            /*// 三方向に扇形の弾を撃つ
			for (int i = 0; i < 3; i++)
			{
				base.RoundShot(this.transform.position, _maxAngle, _angleSplit, _direction, 0, Bullet.MoveType.Line);
				_direction -= 90;
			}*/
            base._puttingEnemyBullet.FanShot(this.transform.position, _centerAngle, _angleSplit, _angleWidth, 0, Bullet.MoveType.Line);

            _shotTime = 0f;
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