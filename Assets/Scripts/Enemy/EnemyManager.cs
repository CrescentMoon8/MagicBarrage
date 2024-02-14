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
                    for(int i = 0; i < _secondPhaseEnemys.Count; i++)
                    {
                        SettingEnemyCount();
                        _secondPhaseEnemys[i].SetActive(true);
                    }
                }
				break;
			case PhaseState.Second:
                if (_enemyCount <= 0)
                {
                    _phaseState = PhaseState.Third;
                    for (int i = 0; i < _thirdPhaseEnemys.Count; i++)
                    {
                        SettingEnemyCount();
                        _thirdPhaseEnemys[i].SetActive(true);
                    }
                }
                break;
			case PhaseState.Third:
                if (_enemyCount <= 0)
                {
                    _phaseState = PhaseState.Boss;
                    for(int i = 0; i < _bossPhaseEnemys.Count; i++)
                    {
                        SettingEnemyCount();
                        _bossPhaseEnemys[i].SetActive(true);
                        _bossPhaseEnemys[i].GetComponent<Boss1>().SplineIndex = 2;
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