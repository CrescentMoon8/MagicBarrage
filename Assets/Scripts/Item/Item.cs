// ---------------------------------------------------------
// Item.cs
//
// 作成日:2024/02/28
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

/// <summary>
/// アイテムの点数や移動処理を行うクラス
/// </summary>
public class Item : MonoBehaviour
{
	#region 変数
	[SerializeField]
	private int scorePoint = 100;

	// アイテムの落下速度（大きければ遅く、小さければ早くなる）
	private float _gravityRate = 45f;

	private BoxCollider2D _boxCollider2D = default;

	public delegate void ReturnPool(Item item);
	private ReturnPool _returnPoolCallBack = default;
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
		_boxCollider2D = GetComponent<BoxCollider2D>();
	}

    private void OnEnable()
    {
		_boxCollider2D.enabled = true;
	}

    /// <summary>
    /// 更新処理
    /// </summary>
    private void FixedUpdate ()
	{
		// アイテムの落下
		this.transform.Translate(Vector3.down / _gravityRate);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
			// 処理が複数回行われないようにColliderを無効化する
			_boxCollider2D.enabled = false;

			// スコアの加算処理とテキストの更新処理
			ScoreManager.Instance.AddScore(scorePoint);
			ScoreManager.Instance.ChangeScoreText();
			
			// アイテムをプールに返却する
			_returnPoolCallBack(this);
		}

		if(collision.CompareTag("ReturnPool"))
        {
			_returnPoolCallBack(this);
        }
    }
    #endregion
}