// ---------------------------------------------------------
// YellowSlime.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.AddressableAssets;

public class YellowSlime : EnemyBase
{
    #region 変数
    private const string PLAYER_BULLET_TAG = "PlayerBullet";

    // 撃ちたい角度
    private int _targetAngle = 180;
    // 角度を何分割するか
    private int _angleSplit = 6;

    private const float BULLET_INTERVAL = 0.15f;
    private float _bulletTime = 0f;
    private const int BULLET_AMOUNT = 36;
    private int _bulletCount = 0;

    private float _shotTime = 0f;
    private const float SHOT_INTERVAL = 0f;

    private BulletInfo _bulletInfo = default;
    private EnemyDataBase _enemyDataBase = default;
    #endregion

    #region プロパティ

    #endregion

    #region メソッド
    /// <summary>
    /// 更新前処理
    /// </summary>
    private void OnEnable()
    {
        _bulletInfo = Addressables.LoadAssetAsync<BulletInfo>("BulletInfo").WaitForCompletion();
        _enemyDataBase = Addressables.LoadAssetAsync<EnemyDataBase>("EnemyDataBase").WaitForCompletion();

        base._hpValue = _enemyDataBase._enemyDataList[_enemyDataBase.YELLOW_SLIME]._maxHp;
        base._hpSlider.maxValue = base._hpValue;
        base._hpSlider.value = base._hpValue;

        _enemyMove.SetSplineContainer(_enemyDataBase._enemyDataList[_enemyDataBase.YELLOW_SLIME]._splineIndex);
        _enemyMove.DifferencePosInitialize(this.transform.position);

        Addressables.Release(_enemyDataBase);
        // _splineContainer.Splines[0].EvaluatePosition(0) → Spline0の始点の座標
        // _splineContainer.Splines[1].EvaluatePosition(0) → Spline1の始点の座標
        // Debug.Log(_splineContainer.Splines[0].EvaluatePosition(0).y);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        _shotTime += Time.deltaTime;
        _bulletTime += Time.deltaTime;

        this.transform.position = base._enemyMove.NextMovePos();

        base.FollowHpBar(this.transform.position);

        if (_shotTime >= SHOT_INTERVAL)
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
                base._puttingEnemyBullet.LineShot(this.transform.position, _targetAngle, _bulletInfo.YERROW_NOMAL_BULLET, Bullet.MoveType.Line);
                //base._puttingEnemyBullet.FanShot(this.transform.position, _centerAngle, _angleSplit, _angleWidth, 4, Bullet.MoveType.Line);
                //base._puttingEnemyBullet.RoundShot(this.transform.position, _angleSplit, _targetAngle, _bulletInfo.YERROW_NOMAL_BULLET, Bullet.MoveType.Line);

                _bulletCount++;
                _bulletTime = 0;
                _targetAngle -= 10;
            }
            else
            {
                _targetAngle = 180;
                _bulletCount = 0;
                _shotTime = 0f;
            }
        }
    }
    #endregion
}