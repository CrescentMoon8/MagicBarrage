// ---------------------------------------------------------
// PoseButton.cs
//
// 作成日:2024/02/22
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

public class PoseButton : MonoBehaviour
{
	#region 変数

	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	public void OnPose()
	{
		if(Time.timeScale >= 1.0f)
		{
            Time.timeScale = 0f;
        }
		else
		{
            Time.timeScale = 1.0f;
        }
	}
	#endregion
}