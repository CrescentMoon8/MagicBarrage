// ---------------------------------------------------------
// EnemyShot.cs
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
    public void LineShot(ShotParameter shotParameter)
    {
        Bullet bullet = EnemyBulletPool.Instance.LendEnemyBullet(shotParameter.ShooterPos, shotParameter.BulletNumber);

        bullet.SettingMoveType = shotParameter.MoveType;

        bullet.transform.rotation = Quaternion.Euler(Vector3.forward * shotParameter.CenterAngle);
    }

    /// <summary>
    /// 弾を扇形に撃ち出す
    /// 撃つ範囲を引数で設定する
	/// 例：centerAngle = 180, angleSplit = 9, angleWidth = 45の場合は下向きの中心角が90度の扇形の弧を9分割する
    /// </summary>
    public void FanShot(ShotParameter shotParameter)
	{
        // 座標計算を始める角度
        float minAngle = shotParameter.CenterAngle - shotParameter.AngleWidth;
        // 座標計算を行う角度（minAngleからの角度）
        float maxAngle = 2 * shotParameter.AngleWidth;

        for (int i = 0; i < shotParameter.AngleSplit; i++)
        {
			// 0の位置がUnity上の-90にあたるため、ADJUST_ANGLEを足すことでUnityに合わせる
            // そのうえで、開始位置をずらすためにminAngleを足す
            Vector3 shotPos = Calculation.CirclePosCalculate(shotParameter.ShooterPos, (maxAngle / shotParameter.AngleSplit) * i + minAngle, _radius);

			Bullet bullet = EnemyBulletPool.Instance.LendEnemyBullet(shotPos, shotParameter.BulletNumber);

            bullet.SettingMoveType = shotParameter.MoveType;

			bullet.transform.rotation = Quaternion.Euler(Vector3.forward * ((maxAngle / shotParameter.AngleSplit) * i + minAngle));
        }
    }

    /// <summary>
    /// 円形に弾幕を撃つ
    /// </summary>
    public void RoundShot(ShotParameter shotParameter)
    {
        // 中心角の最大
        float maxAngle = 360;

        for (int i = 0; i < shotParameter.AngleSplit; i++)
        {
            // 0の位置がUnity上の-90にあたるため、ADJUST_ANGLEを足すことでUnityに合わせる
            Vector3 shotPos = Calculation.CirclePosCalculate(shotParameter.ShooterPos, (maxAngle / shotParameter.AngleSplit) * i + shotParameter.CenterAngle, _radius);

            Bullet bullet = EnemyBulletPool.Instance.LendEnemyBullet(shotPos, shotParameter.BulletNumber);

            bullet.SettingMoveType = shotParameter.MoveType;

            bullet.transform.rotation = Quaternion.Euler(Vector3.forward * ((maxAngle / shotParameter.AngleSplit) * i + shotParameter.CenterAngle));
        }
    }

    /// <summary>
    /// 次の射撃までの時間を加算する
    /// </summary>
    private void ShotTimeCount()
    {
        _shotTime += Time.deltaTime;
    }

    /// <summary>
    /// 指定された時間が経過したか
    /// </summary>
    /// <param name="shotInterval"></param>
    /// <returns></returns>
    public bool IsShot(float shotInterval)
    {
        ShotTimeCount();

        if(_shotTime >= shotInterval)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 次の射撃までの時間をリセットする
    /// </summary>
    public void ResetShotTime()
    {
        _shotTime = 0;
    }
    #endregion
}