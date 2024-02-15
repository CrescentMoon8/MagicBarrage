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
		First,
		Second,
		Third,
		Boss,
		End
	}

	[SerializeField]
	private PhaseState _phaseState = PhaseState.First;

    private int _enemyCount = 0;

	private GameObject _enemysObject = default;
	[SerializeField]
	private List<EnemyBase> _firstPhaseEnemys = new List<EnemyBase>();
    [SerializeField]
    private List<EnemyBase> _secondPhaseEnemys = new List<EnemyBase>();
    [SerializeField]
    private List<EnemyBase> _thirdPhaseEnemys = new List<EnemyBase>();
    [SerializeField]
    private List<EnemyBase> _bossPhaseEnemys = new List<EnemyBase>();
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

        SettingEnemyCount();
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
                        _firstPhaseEnemys.Add(_enemysObject.transform.GetChild(phaseCount - 1).GetChild(enemyCount - 1).GetComponent<EnemyBase>());
                        break;

                    case 2:
                        _secondPhaseEnemys.Add(_enemysObject.transform.GetChild(phaseCount - 1).GetChild(enemyCount - 1).GetComponent<EnemyBase>());
                        break;

                    case 3:
                        _thirdPhaseEnemys.Add(_enemysObject.transform.GetChild(phaseCount - 1).GetChild(enemyCount - 1).GetComponent<EnemyBase>());
                        break;

                    case 4:
                        _bossPhaseEnemys.Add(_enemysObject.transform.GetChild(phaseCount - 1).GetChild(enemyCount - 1).GetComponent<EnemyBase>());
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

    private void SettingEnemyCount()
    {
        switch (_phaseState)
        {
            case PhaseState.First:
                _enemyCount = _firstPhaseEnemys.Count;
                break;
            case PhaseState.Second:
                _enemyCount = _secondPhaseEnemys.Count;
                break;
            case PhaseState.Third:
                _enemyCount = _thirdPhaseEnemys.Count;
                break;
            case PhaseState.Boss:
                _enemyCount = _bossPhaseEnemys.Count;
                break;
            case PhaseState.End:
                break;
            default:
                break;
        }
    }

	/// <summary>
	/// 更新処理
	/// </summary>
	private void Update ()
	{
		switch (_phaseState)
		{
			case PhaseState.First:
                if(_enemyCount <= 0)
                {
                    _phaseState = PhaseState.Second;
                    SettingEnemyCount();
                    for (int i = 0; i < _secondPhaseEnemys.Count; i++)
                    {
                        _secondPhaseEnemys[i].gameObject.SetActive(true);
                    }
                }
				break;
			case PhaseState.Second:
                if (_enemyCount <= 0)
                {
                    _phaseState = PhaseState.Third;
                    SettingEnemyCount();
                    for (int i = 0; i < _thirdPhaseEnemys.Count; i++)
                    {
                        _thirdPhaseEnemys[i].gameObject.SetActive(true);
                    }
                }
                break;
			case PhaseState.Third:
                if (_enemyCount <= 0)
                {
                    _phaseState = PhaseState.Boss;
                    SettingEnemyCount();
                    for (int i = 0; i < _bossPhaseEnemys.Count; i++)
                    {
                        _bossPhaseEnemys[i].gameObject.SetActive(true);
                        _bossPhaseEnemys[i].SettingSplineIndex(3);
                    }
                }
                break;
			case PhaseState.Boss:
                if (_enemyCount <= 0)
                {
                    _phaseState = PhaseState.End;
                }
                break;
			case PhaseState.End:
                SceneManager.LoadScene("Result");
				break;
			default:
				break;
		}
	}

    public void DownEnemyCount()
    {
        _enemyCount--;
    }
	#endregion
}