// ---------------------------------------------------------
// TitleButton.cs
//
// 作成日:2024/02/21
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルに移動させるボタン
/// </summary>
public class TitleButton : MonoBehaviour
{
	#region 変数

	#endregion

	#region メソッド
	public void MoveTitle()
    {
		// ゲームシーンでポーズからタイトルへ戻ったときに１に戻すため
		if (Time.timeScale <= 0)
		{
			Time.timeScale = 1;
		}

		SceneManager.LoadScene("Title");
    }
	#endregion
}