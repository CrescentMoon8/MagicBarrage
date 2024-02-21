// ---------------------------------------------------------
// PuttingEnemyBullet.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

public class EnemyShot
{
    #region 変数
    private const int ADJUST_ANGLE = 90;
    private float _radius = 0f;

    private BulletPool _bulletPool = default;

    public EnemyShot(BulletPool bulletPool, float radius)
	{
        _radius = radius;
		_bulletPool = bulletPool;
	}
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 弾を指定された方向に直線で撃ち出す
	/// </summary>
    public void LineShot(Vector3 shooterPos, int angle, int bulletNumber, Bullet.MoveType moveType)
    {
        Bullet bullet = _bulletPool.LendEnemyBullet(shooterPos, bulletNumber);

        bullet.SettingMoveType = moveType;

        bullet.transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    /// <summary>
    /// 弾を扇形に撃ち出す
    /// 撃つ範囲を引数で設定する
	/// 例：maxAngle = 90, angleSplit = 9, direction = 180の場合は下向きの中心角が90度の扇形の弧を9分割する
    /// </summary>
    /// <param name="shooterPos">射手の座標</param>
    /// <param name="centerAngle">撃ちたい角度</param>
    /// <param name="angleSplit">角度を何分割するか</param>
    /// <param name="angleWidth">撃ちたい角度からの角度幅</param>
    /// <param name="bulletNumber">弾の種類</param>
    /// <param name="moveType">弾の軌道</param>
    public void FanShot(Vector3 shooterPos, int centerAngle, int angleSplit, int angleWidth, int bulletNumber, Bullet.MoveType moveType)
	{
        // 座標計算を始める角度
        int minAngle = centerAngle - angleWidth;
        // 座標計算を行う角度（minAngleからの角度）
        int maxAngle = 2 * angleWidth;

        for (int i = 0; i <= angleSplit; i++)
        {
			// 0の位置がUnity上の-90にあたるため、ADJUST_ANGLEを足すことでUnityに合わせる
            // そのうえで、開始位置をずらすためにminAngleを足す
            Vector3 bulletPos = Calculation.CirclePosCalculate(shooterPos, (maxAngle / angleSplit) * i + minAngle + ADJUST_ANGLE, _radius);

			Bullet bullet = _bulletPool.LendEnemyBullet(bulletPos, bulletNumber);

			bullet.SettingMoveType = moveType;

			bullet.transform.rotation = Quaternion.Euler(Vector3.forward * ((maxAngle / angleSplit) * i + minAngle));
        }
    }

    /// <summary>
    /// 円形に弾幕を撃つ
    /// </summary>
    /// <param name="shooterPos">射手の位置</param>
    /// <param name="angleSplit">角度を何分割するか</param>
    /// <param name="shiftAngle">生成開始角度</param>
    /// <param name="bulletNumber">弾の番号</param>
    /// <param name="moveType">弾の軌道</param>
    public void RoundShot(Vector3 shooterPos, float angleSplit, int shiftAngle, int bulletNumber, Bullet.MoveType moveType)
    {
        // 中心角の最大
        float maxAngle = 360;

        for (int i = 0; i < angleSplit; i++)
        {
            // 0の位置がUnity上の-90にあたるため、ADJUST_ANGLEを足すことでUnityに合わせる
            Vector3 bulletPos = Calculation.CirclePosCalculate(shooterPos, (maxAngle / angleSplit) * i + shiftAngle, _radius);

            Bullet bullet = _bulletPool.LendEnemyBullet(bulletPos, bulletNumber);

            bullet.SettingMoveType = moveType;

            bullet.transform.rotation = Quaternion.Euler(Vector3.forward * ((maxAngle / angleSplit) * i+ shiftAngle));
        }
    }
    #endregion
}