// ---------------------------------------------------------
// YellowSlime.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.AddressableAssets;

public class YellowSlime : EnemyHp
{
    #region 変数
    // 撃ちたい角度
    private float _targetAngle = 180;
    // 角度を何分割するか
    private int _angleSplit = 6;

    private const float BULLET_INTERVAL = 0.15f;
    private float _bulletTime = 0f;
    private const float BULLET_AMOUNT = 36;
    private int _bulletCount = 0;

    private const float SHOT_INTERVAL = 0f;

    private EnemyShot _enemyBulletPut = default;
    private EnemyMove _enemyMove = default;
    private BulletInfo _bulletInfo = default;
    private EnemyDataBase _enemyDataBase = default;
    [SerializeField]
    private BarrageTemplate _barrageTemplate = default;
    #endregion

    #region メソッド
    /// <summary>
    /// 更新前処理
    /// </summary>
    private void OnEnable()
    {
        _enemyBulletPut = new EnemyShot(this.transform.localScale.x / 2);
        _enemyMove = new EnemyMove();

        _bulletInfo = Addressables.LoadAssetAsync<BulletInfo>("BulletInfo").WaitForCompletion();
        _enemyDataBase = Addressables.LoadAssetAsync<EnemyDataBase>("EnemyDataBase").WaitForCompletion();

        EnemyData enemyData = _enemyDataBase._enemyDataList[_enemyDataBase.YELLOW_SLIME];
        base._enemyNumber = _enemyDataBase.YELLOW_SLIME;

        base._hpValue = enemyData._maxHp;
        base._hpSlider.maxValue = base._hpValue;
        base._hpSlider.value = base._hpValue;

        _enemyMove.SetSplineContainer(enemyData._splineIndex);
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
    private void Update()
    {
        _bulletTime += Time.deltaTime;

        this.transform.position = _enemyMove.NextMovePos();

        base.FollowHpBar(this.transform.position);

        if (_isInsideCamera && _enemyBulletPut.IsShot(SHOT_INTERVAL))
        {
            /* 
			 * 指定した秒数間隔で指定した回数撃つ
			 */
            if (_bulletTime < BULLET_INTERVAL)
            {
                return;
            }

            if (_bulletTime >= BULLET_INTERVAL && _bulletCount < BULLET_AMOUNT)
            {
                // 射撃に必要なパラメータを生成する
                ShotParameter lineShotParameter = new ShotParameter(this.transform.position, _targetAngle, _bulletInfo.YERROW_NOMAL_BULLET, Bullet.MoveType.Line, Bullet.SpeedType.Middle);
                _enemyBulletPut.LineShot(lineShotParameter);
                //base._puttingEnemyBullet.FanShot(this.transform.position, _centerAngle, _angleSplit, _angleWidth, 4, Bullet.MoveType.Line);
                //base._puttingEnemyBullet.RoundShot(this.transform.position, _angleSplit, _targetAngle, _bulletInfo.YERROW_NOMAL_BULLET, Bullet.MoveType.Line);

                _bulletCount++;
                _bulletTime = 0;
                // +=で反時計回り、-=で時計回り
                _targetAngle -= 360 / BULLET_AMOUNT;
            }
            else
            {
                _targetAngle = 180;
                _bulletCount = 0;
                _enemyBulletPut.ResetShotTime();
            }
        }
    }
    #endregion
}