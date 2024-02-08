// ---------------------------------------------------------
// EnemyBase.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class EnemyBase : MonoBehaviour
{
	#region 変数
	[SerializeField]
	protected Slider _hpSlider = default;
	protected int _hpValue = 0;

	protected Vector3[] _movePattern = default;

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
		
	}

	/// <summary>
	/// 更新処理
	/// </summary>
	private void Update ()
	{
		
	}

	protected void EnemyDamage()
	{
		if(_hpValue > 0)
		{
			_hpValue -= 1;
			_hpSlider.value = _hpValue;
		}

		if(_hpValue <= 0)
		{
			EnemyDead();
			_hpSlider.gameObject.SetActive(false);
		}
	}

	private void EnemyDead()
	{
		this.gameObject.SetActive(false);
	}
	#endregion
}