// ---------------------------------------------------------
// EnemyPhaseList.cs
//
// 作成日:2024/02/16
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

public class EnemyPhaseList : MonoBehaviour
{
    #region 変数
    private GameObject _enemysObject = default;
    private List<List<GameObject>> _enemyPhaseList = new List<List<GameObject>>();
	#endregion

	#region プロパティ
    public List<List<GameObject>> EnemyList { get { return _enemyPhaseList; } }
	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	public void Initialize()
	{
        _enemyPhaseList.Clear();
        GenerateEnemyList();
	}

    /// <summary>
    /// リストにそれぞれのエネミーを追加する
    /// </summary>
    private void GenerateEnemyList()
    {
        _enemysObject = GameObject.FindWithTag("Enemys");

        for (int phaseCount = 1; phaseCount <= _enemysObject.transform.childCount; phaseCount++)
        {
            List<GameObject> enemyPool = new List<GameObject>();
            for (int enemyCount = 1; enemyCount <= _enemysObject.transform.GetChild(phaseCount - 1).childCount; enemyCount++)
            {
                enemyPool.Add(_enemysObject.transform.GetChild(phaseCount - 1).GetChild(enemyCount - 1).gameObject);
            }
            _enemyPhaseList.Add(enemyPool);
        }
    }

    public void RemoveEnemy(int phaseNumber, GameObject enemy)
    {
        _enemyPhaseList[phaseNumber].Remove(enemy);
    }
	#endregion
}