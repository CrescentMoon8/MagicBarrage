// ---------------------------------------------------------
// Item.cs
//
// 作成日:2024/02/28
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;

public class Item : MonoBehaviour
{
	#region 変数
	[SerializeField]
	private int scorePoint = 100;

	private BoxCollider2D _boxCollider2D = default;

	public delegate void ReturnPool(Item item);
	private ReturnPool _returnPoolCallBack = default;

	private ScoreManager _scoreManager = default;
	#endregion

	#region プロパティ
	public ReturnPool RturnPoolCallBack { set { _returnPoolCallBack = value; } }
	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
		_scoreManager = GameObject.FindWithTag("Scripts").GetComponentInChildren<ScoreManager>();
		_boxCollider2D = GetComponent<BoxCollider2D>();
		_boxCollider2D.enabled = true;
	}

	/// <summary>
	/// 更新処理
	/// </summary>
	private void FixedUpdate ()
	{
		this.transform.Translate(Vector3.down / 45);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
			_boxCollider2D.enabled = false;
			_scoreManager.AddScore(scorePoint);
			_scoreManager.ChangeScoreText();
			_returnPoolCallBack(this);
		}

		if(collision.CompareTag("ReturnPool"))
        {
			_returnPoolCallBack(this);
        }
    }
    #endregion
}