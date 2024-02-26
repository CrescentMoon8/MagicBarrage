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

	// 撃ちたい角度
	private int _targetAngle = 180;
	// 角度を何分割するか
	private int _angleSplit = 9;
	// 撃ちたい角度の±いくらか
	private int _angleWidth = 15;

	private const float BULLET_INTERVAL = 0.15f;
	private float _bulletTime = 0f;
	private const int BULLET_AMOUNT = 36;
	private int _bulletCount = 0;

	private float _shotTime = 0f;
	private const float SHOT_INTERVAL = 1f;

	private Event _bossShot;

	private BulletInfo _bulletInfo = default;
	private EnemyDataBase _enemyDataBase = default;
    [SerializeField]
    private BarrageTemplate _barrageTemplate = default;
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

        this.transform.position = base._enemyMove.NextMovePos();

		//base._playerPos = base._player.transform.position;

		ShootWindmillBullet();


        if (_shotTime > SHOT_INTERVAL)
        {
			/*// 三方向に扇形の弾を撃つ
			for (int i = 0; i < 3; i++)
			{
				base.RoundShot(this.transform.position, _maxAngle, _angleSplit, _direction, 0, Bullet.MoveType.Line);
				_direction -= 90;
			}*/

			//_targetAngle = 180;
			base._puttingEnemyBullet.FanShot(this.transform.position, _targetAngle, _angleSplit, _angleWidth, _bulletInfo.RED_NOMAL_BULLET, Bullet.MoveType.Line);
			base._puttingEnemyBullet.FanShot(this.transform.position, _targetAngle + 180, _angleSplit, _angleWidth, _bulletInfo.RED_NOMAL_BULLET, Bullet.MoveType.Line);

            _shotTime = 0f;
        }
    }

    private void ShootWindmillBullet()
    {
		_bulletTime += Time.deltaTime;

		/* 
		 * 指定した秒数間隔で指定した回数撃つ
		 */
		if (_bulletTime < BULLET_INTERVAL)
		{
			return;
		}

		if (_bulletTime >= BULLET_INTERVAL && _bulletCount < BULLET_AMOUNT)
		{
			base._puttingEnemyBullet.RoundShot(this.transform.position, _angleSplit, _targetAngle, _bulletInfo.PURPLE_NOMAL_BULLET, Bullet.MoveType.Line);
			_bulletCount++;
			_bulletTime = 0;
			_targetAngle += 360 / BULLET_AMOUNT;
		}
		else
		{
			_targetAngle = 180;
			_bulletCount = 0;
		}
    }
    #endregion
}