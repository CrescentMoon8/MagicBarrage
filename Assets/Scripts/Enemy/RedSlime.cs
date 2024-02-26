// ---------------------------------------------------------
// RedSlime.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.AddressableAssets;

public class RedSlime : EnemyBase
{
	#region 変数
	// 撃ちたい角度
	private int _centerAngle = 180;
	// 角度を何分割するか
	private int _angleSplit = 10;
	// 撃ちたい角度の±いくらか
	private int _angleWidth = 45;

	private const float BULLET_INTERVAL = 0.25f;
	private float _bulletTime = 0f;
	private const int BULLET_AMOUNT = 5;
	private int _bulletCount = 0;

    private float _shotTime = 0f;
	private const float SHOT_INTERVAL = 2f;

	private BulletInfo _bulletInfo = default;
	private EnemyDataBase _enemyDataBase = default;
    [SerializeField]
    private BarrageTemplate _barrageTemplate = default;
    #endregion

    #region メソッド
    /// <summary>
    /// 更新前処理
    /// </summary>
    private void OnEnable ()
	{
		_bulletInfo = Addressables.LoadAssetAsync<BulletInfo>("BulletInfo").WaitForCompletion();
		_enemyDataBase = Addressables.LoadAssetAsync<EnemyDataBase>("EnemyDataBase").WaitForCompletion();

		base._hpValue = _enemyDataBase._enemyDataList[_enemyDataBase.RED_SLIME]._maxHp;
		base._hpSlider.maxValue = base._hpValue;
		base._hpSlider.value = base._hpValue;

        _centerAngle = _barrageTemplate._centerAngle;
        _angleWidth = _barrageTemplate._angleWidth;
        _angleSplit = _barrageTemplate._angleSplit;

        _enemyMove.SetSplineContainer(_enemyDataBase._enemyDataList[_enemyDataBase.RED_SLIME]._splineIndex);
		_enemyMove.DifferencePosInitialize(this.transform.position);

		Addressables.Release(_enemyDataBase);
    }

	/// <summary>
	/// 非アクティブになったときにBulletInfoをアンロードする
	/// </summary>
    private void OnDisable()
    {
		Addressables.Release(_bulletInfo);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update ()
	{
		_shotTime += Time.deltaTime;
		_bulletTime += Time.deltaTime;

		this.transform.position = base._enemyMove.NextMovePos();

		base.FollowHpBar(this.transform.position);

		if ( _shotTime >= SHOT_INTERVAL )
		{
			/* 
			 * 指定した秒数間隔で指定した回数撃つ
			 */
			if (_bulletTime < BULLET_INTERVAL)
            {
				return;
            }
			if(base._isPlayerTarget)
			{
                _centerAngle = Calculation.TargetDirectionAngle(base._iPlayerPos.PlayerPos, this.transform.position);
            }

			if (_bulletTime >= BULLET_INTERVAL && _bulletCount < BULLET_AMOUNT)
            {
				// 追尾弾の初弾と同時に扇形の通常弾を打つ
				if(_bulletCount == 0)
                {
					base._puttingEnemyBullet.FanShot(this.transform.position, _centerAngle, _angleSplit, _angleWidth, _bulletInfo.RED_NOMAL_BULLET, Bullet.MoveType.Line);
				}
				base._puttingEnemyBullet.LineShot(this.transform.position, _centerAngle, _bulletInfo.RED_NEEDLE_BULLET, Bullet.MoveType.Tracking);

				_bulletCount++;
				_bulletTime = 0;
			}
			else
            {
				_bulletCount = 0;
				_shotTime = 0f;
			}
		}
	}
    #endregion
}