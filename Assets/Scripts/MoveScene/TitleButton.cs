// ---------------------------------------------------------
// TitleButton.cs
//
// 作成日:2024/02/21
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
	#region 変数

	#endregion

	#region メソッド
	public void MoveTitle()
    {
		SceneManager.LoadScene("Title");
    }
	#endregion
}