// ---------------------------------------------------------
// Calculatinon.cs
//
// 作成日:2024/02/18
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

public static class Calculation
{
    #region メソッド
    /// <summary>
    /// 自身と対象の距離を計算する
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="myPos"></param>
    /// <returns></returns>
    public static float TargetDistance(Vector3 targetPos, Vector3 myPos)
    {
        Vector3 distanceVector = targetPos - myPos;
        return Mathf.Pow(distanceVector.x, 2) + Mathf.Pow(distanceVector.y, 2);
    }

    /// <summary>
    /// 自身から対象への角度を計算する
    /// </summary>
    /// <param name="targetPos">対象の座標</param>
    /// <param name="shooterPos">射手の座標</param>
    /// <returns>対象への角度</returns>
    public static int TargetDirectionAngle(Vector3 targetPos, Vector3 shooterPos)
    {
        float distance = TargetDistance(targetPos, shooterPos);
        // Atan2はY軸を中心として、右が0～180、左が-180～0の範囲となる
        float toTargetAngle = Mathf.Atan(distance) * Mathf.Rad2Deg;

        if (toTargetAngle < 0)
        {
            return Mathf.RoundToInt(Mathf.Abs(toTargetAngle));
        }
        else
        {
            return 360 - Mathf.RoundToInt(toTargetAngle);
        }
    }

    /// <summary>
    /// 自身から対象への角度を計算する
    /// </summary>
    /// <param name="distance">自身と対象の距離</param>
    /// <returns>対象への角度</returns>
    public static int AngleFromEnemyCalculate(float distance)
    {
        // Atan2はY軸を中心として、右が0～180、左が-180～0の範囲となる
        float toTargetAngle = Mathf.Atan(distance) * Mathf.Rad2Deg;

        if (toTargetAngle < 0)
        {
            return Mathf.RoundToInt(Mathf.Abs(toTargetAngle));
        }
        else
        {
            return 360 - Mathf.RoundToInt(toTargetAngle);
        }
    }

    /// <summary>
    /// 与えられた角度を使って、半径radiusの円の円周上の座標を返す
    /// </summary>
    /// <param name="ShooterPos">射手の座標</param>
    /// <param name="angle">中心角</param>
    /// <returns>円周上の座標</returns>
    public static Vector3 CirclePosCalculate(Vector3 shooterPos, float angle, float radius)
    {
        Vector3 circlePos = shooterPos;

        circlePos.x += Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        circlePos.y += Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        // ラジアンに変換しなかったら、よくわからん挙動した（参考：スクリーンショットフォルダ）
        /*circlePosition.x = Mathf.Cos(angle) * radius;
        circlePosition.y = Mathf.Sin(angle) * radius;*/
        return circlePos;
    }
    #endregion
}