// ---------------------------------------------------------
// CurrentPhaseEnemyPosition.cs
//
// 作成日:2024/02/16
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

public static class CurrentPhaseEnemyPosition
{
    #region 変数
    public static List<Vector3> _currentEnemyPos = new List<Vector3>();
    #endregion

    #region メソッド
    public static void AddEnemyPos(List<GameObject> enemyList)
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            _currentEnemyPos.Add(enemyList[i].transform.position);
        }
    }

    public static void UpdateEnemyPos(List<GameObject> enemyList)
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            _currentEnemyPos[i] = enemyList[i].transform.position;
        }
    }

    public static void RemoveEnemyPos(List<GameObject> enemyList)
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            _currentEnemyPos.Remove(enemyList[i].transform.position);
        }
    }
    #endregion
}