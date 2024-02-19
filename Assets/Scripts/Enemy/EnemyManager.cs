// ---------------------------------------------------------
// EnemyManager.cs
//
// 作成日:2024/02/11
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
	#region 変数
	private enum PhaseState
	{
		First = 0,
		Second = 1,
		Third = 2,
		Boss = 3,
		End = 4
	}

	[SerializeField]
	private PhaseState _phaseState = PhaseState.First;

    private EnemyPhaseList _enemyPhaseList = default;
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
    {
        _enemyPhaseList = GetComponent<EnemyPhaseList>();
        _enemyPhaseList.Initialize();
        CurrentPhaseEnemyPosition.AddEnemyPos(_enemyPhaseList.EnemyList[(int)PhaseState.First]);
    }

	/// <summary>
	/// 更新処理
	/// </summary>
	private void Update ()
	{
		switch (_phaseState)
		{
			case PhaseState.First:
                CurrentPhaseEnemyPosition.UpdateEnemyPos(_enemyPhaseList.EnemyList[(int)PhaseState.First]);
                if (_enemyPhaseList.EnemyList[(int)PhaseState.First].Count <= 0)
                {
                    _phaseState = PhaseState.Second;

                    CurrentPhaseEnemyPosition.RemoveEnemyPos(_enemyPhaseList.EnemyList[(int)PhaseState.First]);
                    
                    for (int i = 0; i < _enemyPhaseList.EnemyList[(int)PhaseState.Second].Count; i++)
                    {
                        _enemyPhaseList.EnemyList[(int)PhaseState.Second][i].gameObject.SetActive(true);
                    }
                }
				break;
			case PhaseState.Second:
                CurrentPhaseEnemyPosition.UpdateEnemyPos(_enemyPhaseList.EnemyList[(int)PhaseState.Second]);
                if (_enemyPhaseList.EnemyList[(int)PhaseState.Second].Count <= 0)
                {
                    _phaseState = PhaseState.Third;

                    CurrentPhaseEnemyPosition.RemoveEnemyPos(_enemyPhaseList.EnemyList[(int)PhaseState.Second]);
                    
                    for (int i = 0; i < _enemyPhaseList.EnemyList[(int)PhaseState.Third].Count; i++)
                    {
                        _enemyPhaseList.EnemyList[(int)PhaseState.Third][i].gameObject.SetActive(true);
                    }
                }
                break;
			case PhaseState.Third:
                CurrentPhaseEnemyPosition.UpdateEnemyPos(_enemyPhaseList.EnemyList[(int)PhaseState.Third]);
                if (_enemyPhaseList.EnemyList[(int)PhaseState.Third].Count <= 0)
                {
                    _phaseState = PhaseState.Boss;

                    CurrentPhaseEnemyPosition.RemoveEnemyPos(_enemyPhaseList.EnemyList[(int)PhaseState.Third]);
                    
                    for (int i = 0; i < _enemyPhaseList.EnemyList[(int)PhaseState.Boss].Count; i++)
                    {
                        _enemyPhaseList.EnemyList[(int)PhaseState.Boss][i].gameObject.SetActive(true);
                    }
                }
                break;
			case PhaseState.Boss:
                CurrentPhaseEnemyPosition.UpdateEnemyPos(_enemyPhaseList.EnemyList[(int)PhaseState.Boss]);
                if (_enemyPhaseList.EnemyList[(int)PhaseState.Boss].Count <= 0)
                {
                    _phaseState = PhaseState.End;

                    CurrentPhaseEnemyPosition.RemoveEnemyPos(_enemyPhaseList.EnemyList[(int)PhaseState.Boss]);
                }
                break;
			case PhaseState.End:
                SceneManager.LoadScene("Result");
				break;
			default:
				break;
		}
	}

    public void DownEnemyCount(GameObject enemy)
    {
        _enemyPhaseList.RemoveEnemy((int)_phaseState, enemy);
    }
	#endregion
}