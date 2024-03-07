// ---------------------------------------------------------
// PuttingShot.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

/// <summary>
/// エネミーの弾の配置処理を行うクラス
/// </summary>
public class EnemyShot
{
    #region 変数
    private float _shotTime = 0f;

    private float _radius = 0f;

    public EnemyShot(float radius)
	{
        _radius = radius;
	}
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 弾を指定された方向に直線で撃ち出す
	/// </summary>
    public void LineShot(Vector3 shooterPos, float angle, int bulletNumber, Bullet.MoveType moveType)
    {
        Bullet bullet = EnemyBulletPool.Instance.LendEnemyBullet(shooterPos, bulletNumber);

        bullet.SettingMoveType = moveType;

        bullet.transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    /// <summary>
    /// 弾を扇形に撃ち出す
    /// 撃つ範囲を引数で設定する
	/// 例：centerAngle = 180, angleSplit = 9, angleWidth = 45の場合は下向きの中心角が90度の扇形の弧を9分割する
    /// </summary>
    /// <param name="shooterPos">射手の座標</param>
    /// <param name="centerAngle">撃ちたい角度</param>
    /// <param name="angleSplit">角度を何分割するか</param>
    /// <param name="angleWidth">撃ちたい角度からの角度幅</param>
    /// <param name="bulletNumber">弾の種類</param>
    /// <param name="moveType">弾の軌道</param>
    public void FanShot(Vector3 shooterPos, float centerAngle, float angleSplit, int angleWidth, int bulletNumber, Bullet.MoveType moveType)
	{
        // 座標計算を始める角度
        float minAngle = centerAngle - angleWidth;
        // 座標計算を行う角度（minAngleからの角度）
        float maxAngle = 2 * angleWidth;

        for (int i = 0; i < angleSplit; i++)
        {
			// 0の位置がUnity上の-90にあたるため、ADJUST_ANGLEを足すことでUnityに合わせる
            // そのうえで、開始位置をずらすためにminAngleを足す
            Vector3 shotPos = Calculation.CirclePosCalculate(shooterPos, (maxAngle / angleSplit) * i + minAngle, _radius);

			Bullet bullet = EnemyBulletPool.Instance.LendEnemyBullet(shotPos, bulletNumber);

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
    public void RoundShot(Vector3 shooterPos, float angleSplit, float shiftAngle, int bulletNumber, Bullet.MoveType moveType)
    {
        // 中心角の最大
        float maxAngle = 360;

        for (int i = 0; i < angleSplit; i++)
        {
            // 0の位置がUnity上の-90にあたるため、ADJUST_ANGLEを足すことでUnityに合わせる
            Vector3 shotPos = Calculation.CirclePosCalculate(shooterPos, (maxAngle / angleSplit) * i + shiftAngle, _radius);

            Bullet bullet = EnemyBulletPool.Instance.LendEnemyBullet(shotPos, bulletNumber);

            bullet.SettingMoveType = moveType;

            bullet.transform.rotation = Quaternion.Euler(Vector3.forward * ((maxAngle / angleSplit) * i + shiftAngle));
        }
    }

    private void ShotTimeCount()
    {
        _shotTime += Time.deltaTime;
    }

    public bool IsShot(float shotInterval)
    {
        ShotTimeCount();

        if(_shotTime >= shotInterval)
        {
            return true;
        }

        return false;
    }

    public void ResetShotTime()
    {
        _shotTime = 0;
    }
    #endregion
}