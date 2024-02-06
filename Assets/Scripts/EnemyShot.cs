// ---------------------------------------------------------
// EnemyShot.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

public class EnemyShot : EnemyBase
{
	#region 変数
	private const int MAX_ANGLE = 360;
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
		
	}

	/// <summary>
	/// 弾を直線に撃ち出す
	/// </summary>
    protected void LineShot()
    {
        
    }

	/// <summary>
	/// 弾を円形に撃ち出す
	/// 撃つ範囲を引数で設定する
	/// </summary>
	protected void RoundShot(Vector3 shotPosition, int angleSplit, float radius)
	{
        for (int i = 0; i < angleSplit; i++)
        {
            Vector3 bulletPos = CirclePositionCalculate(this.transform.position, (MAX_ANGLE / angleSplit) * i, this.transform.localScale.x / 2);
            

        }
    }

	protected Vector3 CirclePositionCalculate(Vector3 shotPosition, float angle, float radius)
	{
		Vector3 circlePosition = Vector3.zero;

		circlePosition.x = Mathf.Cos(angle) * radius;
		circlePosition.y = Mathf.Sin(angle) * radius;

		return circlePosition;
	}
    #endregion
}