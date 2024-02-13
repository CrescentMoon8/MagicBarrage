// ---------------------------------------------------------
// MainMove.cs
//
// 作成日:2024/02/13
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class MainMove : MonoBehaviour
{
	#region 変数

	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	public void MoveMain()
    {
		SceneManager.LoadScene("Main");
    }
	#endregion
}