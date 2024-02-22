// ---------------------------------------------------------
// Calculatinon.cs
//
// 作成日:2024/02/18
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

public static class Calculation
{
    #region 定数
    // 角度をUnity基準に合わせるための定数
    private const int ADJUST_ANGLE = 90;
    #endregion
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
        Vector3 distanceVector = targetPos - shooterPos;

        // 三角関数ではAtan2はX軸を中心として、下が-180～0、上が0～180の範囲となる
        // Unity基準の角度にするために90度足す
        float toTargetAngle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg + ADJUST_ANGLE;

        if (toTargetAngle > 0)
        {
            return 180 + Mathf.RoundToInt(Mathf.Abs(toTargetAngle));
        }
        else
        {
            return 180 - Mathf.RoundToInt(Mathf.Abs(toTargetAngle));
        }
    }

    /// <summary>
    /// 与えられた角度を使って、半径radiusの円の円周上の座標を返す
    /// </summary>
    /// <param name="shooterPos">射手の座標</param>
    /// <param name="radius">配置したい円の半径</param>
    /// <param name="angle">中心角</param>
    /// <returns>円周上の座標</returns>
    public static Vector3 CirclePosCalculate(Vector3 shooterPos, float angle, float radius)
    {
        Vector3 circlePos = shooterPos;

        // Unity基準の角度にするために90度足す
        circlePos.x += Mathf.Cos((angle + ADJUST_ANGLE) * Mathf.Deg2Rad) * radius;
        circlePos.y += Mathf.Sin((angle + ADJUST_ANGLE) * Mathf.Deg2Rad) * radius;

        // ラジアンに変換しなかったら、よくわからん挙動した（参考：スクリーンショットフォルダ）
        /*circlePosition.x = Mathf.Cos(angle) * radius;
        circlePosition.y = Mathf.Sin(angle) * radius;*/
        return circlePos;
    }
    #endregion
}