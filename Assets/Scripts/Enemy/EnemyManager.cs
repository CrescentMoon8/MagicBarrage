// ---------------------------------------------------------
// EnemyManager.cs
//
// 作成日:2024/02/11
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EnemyManager : MonoBehaviour
{
	#region 変数
	private enum PhaseState
	{
		First,
		Second,
		Third,
		Boss,
		End
	}

	[SerializeField]
	private PhaseState _phaseNumber = PhaseState.First;

	private GameObject _enemysObject = default;
	[SerializeField]
	private List<GameObject> _firstPhaseEnemys = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _secondPhaseEnemys = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _thirdPhaseEnemys = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _bossPhaseEnemys = new List<GameObject>();
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
    {
        PhaseListClear();

        PhaseListInitialize();
    }

    private void PhaseListInitialize()
    {
        _enemysObject = GameObject.FindWithTag("Enemys");

        for (int phaseCount = 1; phaseCount <= _enemysObject.transform.childCount; phaseCount++)
        {
            for (int enemyCount = 1; enemyCount <= _enemysObject.transform.GetChild(phaseCount - 1).childCount; enemyCount++)
            {
                switch (phaseCount)
                {
                    case 1:
                        _firstPhaseEnemys.Add(_enemysObject.transform.GetChild(phaseCount - 1).GetChild(enemyCount - 1).gameObject);
                        break;

                    case 2:
                        _secondPhaseEnemys.Add(_enemysObject.transform.GetChild(phaseCount - 1).GetChild(enemyCount - 1).gameObject);
                        break;

                    case 3:
                        _thirdPhaseEnemys.Add(_enemysObject.transform.GetChild(phaseCount - 1).GetChild(enemyCount - 1).gameObject);
                        break;

                    case 4:
                        _bossPhaseEnemys.Add(_enemysObject.transform.GetChild(phaseCount - 1).GetChild(enemyCount - 1).gameObject);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    private void PhaseListClear()
    {
        _firstPhaseEnemys.Clear();
        _secondPhaseEnemys.Clear();
        _thirdPhaseEnemys.Clear();
        _bossPhaseEnemys.Clear();
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
		switch (_phaseNumber)
		{
			case PhaseState.First:

				break;
			case PhaseState.Second:
				break;
			case PhaseState.Third:
				break;
			case PhaseState.Boss:
				break;
			case PhaseState.End:
				break;
			default:
				break;
		}
	}
	#endregion
}