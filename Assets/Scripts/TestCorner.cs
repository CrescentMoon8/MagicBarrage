// ---------------------------------------------------------
// TestCorner.cs
//
// 作成日:
// 作成者:
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class TestCorner : MonoBehaviour
{
	#region 変数

	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
		RectTransform rect = this.gameObject.GetComponent<RectTransform>();
		Debug.Log(rect.anchoredPosition);
		Vector3 test = RectTransformUtility.WorldToScreenPoint(Camera.main, rect.position);
		RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, test, Camera.main, out test);
		Debug.Log(test);

		// UIオブジェクトの頂点の座標を取得し、移動制限の大きさを調整する
		// 座標の取得順は左下、左上、右上、右下
		Vector3[] headerCorners = new Vector3[4];
		rect.GetWorldCorners(headerCorners);
		//_maxFieldVector = headerCorners[MAX_MOVE_POS_INDEX];

		for (var i = 0; i < headerCorners.Length; i++)
		{
			Debug.Log($"World Corners[{i}] : {headerCorners[i]}");
		}
	}

	/// <summary>
	/// 更新前処理
	/// </summary>
	private void Start ()
	{
		
	}

	/// <summary>
	/// 更新処理
	/// </summary>
	private void Update ()
	{
		
	}
	#endregion
}