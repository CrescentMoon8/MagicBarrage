// ---------------------------------------------------------
// ItemPool.cs
//
// 作成日:2024/02/28
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

public class ItemPool : MonoBehaviour
{
	#region 変数
	private const int MAX_GENERATE_ITEM_AMOUNT = 4;
	private GameObject _itemPrefab = default;
	private Transform _itemParent = default;
	private Queue<Item> _itemPool = new Queue<Item>();
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
		GenerateItemPool();
	}

	private void GenerateItemPool()
    {
        for (int i = 0; i < MAX_GENERATE_ITEM_AMOUNT; i++)
        {
			_itemPool.Enqueue(Instantiate(_itemPrefab, _itemParent).GetComponent<Item>());
        }
    }

	private void AddItemPool()
    {
		_itemPool.Enqueue(Instantiate(_itemPrefab, _itemParent).GetComponent<Item>());
	}

	public void LendItem(Vector3 startPos)
    {
		if(_itemPool.Count <= 0)
        {
			AddItemPool();
        }

		Item item = _itemPool.Dequeue();

		item.transform.position = startPos;


    }

	public void ReturnPool(Item item)
    {
		item.gameObject.SetActive(false);

		_itemPool.Enqueue(item);
    }
	#endregion
}