// ---------------------------------------------------------
// SlimeBoss.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BossSlime : EnemyBase
{
	#region 変数
	private const int MOVE_PATTERN_INDEX = 4;

	private const string PLAYER_BULLET_TAG = "PlayerBullet";

    private const int BOSS_HP = 360;

	// 撃ちたい角度
	private int _centerAngle = 180;
	// 角度を何分割するか
	private int _angleSplit = 18;
	// 撃ちたい角度の±いくらか
	private int _angleWidth = 45;

	private float _shotTime = 0f;
	private const float SHOT_INTERVAL = 2f;

	private Event _bossShot;

	private BulletInfo _bulletInfo = default;
	private EnemyDataBase _enemyDataBase = default;
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	private void OnEnable ()
    {
		_bulletInfo = Addressables.LoadAssetAsync<BulletInfo>("BulletInfo").WaitForCompletion();
		_enemyDataBase = Addressables.LoadAssetAsync<EnemyDataBase>("EnemyDataBase").WaitForCompletion();

		base._hpValue = _enemyDataBase._enemyDataList[_enemyDataBase.BOSS_SLIME]._maxHp;
		base._hpSlider.maxValue = base._hpValue;
		base._hpSlider.value = base._hpValue;

		_enemyMove.SetSplineContainer(_enemyDataBase._enemyDataList[_enemyDataBase.BOSS_SLIME]._splineIndex);
		_enemyMove.DifferencePosInitialize(this.transform.position);

		Addressables.Release(_enemyDataBase);
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
            base._puttingEnemyBullet.FanShot(this.transform.position, _centerAngle, _angleSplit, _angleWidth, _bulletInfo.RED_NOMAL_BULLET, Bullet.MoveType.Line);

            _shotTime = 0f;
		}
	}
    #endregion
}