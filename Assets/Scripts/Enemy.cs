// ---------------------------------------------------------
// Enemy.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class Enemy : EnemyShot
{
	#region 変数
	private const int MAX_ANGLE = 360;
	private int _angleSplit = 120;

	[SerializeField]
	private GameObject _test;
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
	/// 更新前処理
	/// </summary>
	private void Start ()
	{
		// テスト用
        /*for (int i = 0; i < _angleSplit; i++)
        {
            Vector3 bulletPos = CirclePositionCalculate(this.transform.position, (MAX_ANGLE / _angleSplit) * i, this.transform.localScale.x / 2);
			Instantiate(_test, bulletPos, Quaternion.identity);
        }*/
    }

	/// <summary>
	/// 更新処理
	/// </summary>
	private void Update ()
	{
		
	}
	#endregion
}