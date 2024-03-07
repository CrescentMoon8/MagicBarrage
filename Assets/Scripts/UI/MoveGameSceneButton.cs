// ---------------------------------------------------------
// MoveGameSceneButton.cs
//
// 作成日:2024/02/13
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveGameSceneButton : MonoBehaviour
{
	#region 変数

	#endregion

	#region メソッド
	public void MoveGameScene()
    {
		SceneManager.LoadScene("Main");
    }
	#endregion
}