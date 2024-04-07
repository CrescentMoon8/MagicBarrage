// ---------------------------------------------------------
// ItemPool.cs
//
// 作成日:2024/02/28
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 得点アイテム用のオブジェクトプールクラス
/// </summary>
public class ItemPool : MonoBehaviour
{
	#region 変数
	// 生成するアイテムの数
	private const int MAX_GENERATE_ITEM_AMOUNT = 10;
	
	[SerializeField]
	private GameObject _itemPrefab = default;
	// 生成するアイテムの親オブジェクト
	[SerializeField]
	private Transform _itemParent = default;
	
	private Queue<Item> _itemPool = new Queue<Item>();
	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
		GenerateItemPool();
	}

	/// <summary>
	/// アイテムのオブジェクトプールを生成する
	/// </summary>
	private void GenerateItemPool()
    {
		// 指定した数分まで生成する
        for (int i = 0; i < MAX_GENERATE_ITEM_AMOUNT; i++)
        {
			Item item = Instantiate(_itemPrefab, _itemParent).GetComponent<Item>();

			// アイテムのdelegateに返却用メソッドを登録する
			item.RturnPoolCallBack = ReturnPool;

			item.gameObject.SetActive(false);

			_itemPool.Enqueue(item);
        }
    }

	/// <summary>
	/// アイテムを追加で生成する
	/// </summary>
	private void AddItemPool()
    {
        Item item = Instantiate(_itemPrefab, _itemParent).GetComponent<Item>();

        item.RturnPoolCallBack = ReturnPool;

        item.gameObject.SetActive(false);

        _itemPool.Enqueue(item);
    }

	/// <summary>
	/// アイテムを貸し出す
	/// </summary>
	/// <param name="startPos"></param>
	public void LendItem(Vector3 startPos)
    {
		// プールの中身がなかったら
		if(_itemPool.Count <= 0)
        {
			AddItemPool();
        }

		Item item = _itemPool.Dequeue();

		item.transform.position = startPos;

		item.gameObject.SetActive(true);
    }

	/// <summary>
	/// アイテムを返却する
	/// </summary>
	/// <param name="item">返却するアイテムクラス</param>
	public void ReturnPool(Item item)
    {
		item.gameObject.SetActive(false);

		_itemPool.Enqueue(item);
    }
	#endregion
}