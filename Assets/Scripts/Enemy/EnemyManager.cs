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
	public enum PhaseState
	{
		First = 0,
		Second = 1,
		Third = 2,
		Boss = 3,
		End = 4
	}

	[SerializeField]
	private PhaseState _phaseState = PhaseState.First;

    private GameObject _enemysObject = default;
    [SerializeField]
    private List<List<GameObject>> _enemyPhaseList = new List<List<GameObject>>();
    private List<List<IDamageable>> _enemyIDamageableList = new List<List<IDamageable>>();
    #endregion

    #region プロパティ
    public List<List<GameObject>> EnemyPhaseList { get { return _enemyPhaseList; } }
    public List<List<IDamageable>> EnemyIDamageableList { get { return _enemyIDamageableList; } }
    public PhaseState NowPhaseState { get { return _phaseState; } }
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {
        GenerateEnemyList();
        for (int i = 0; i < _enemyPhaseList[(int)PhaseState.First].Count; i++)
        {
            _enemyPhaseList[(int)PhaseState.First][i].gameObject.SetActive(true);
        }
        Debug.LogWarning("生成");
    }

	/// <summary>
	/// 更新処理
	/// </summary>
	private void Update ()
	{
		switch (_phaseState)
		{
			case PhaseState.First:
                if (_enemyPhaseList[(int)PhaseState.First].Count <= 0)
                {
                    _phaseState = PhaseState.Second;

                    for (int i = 0; i < _enemyPhaseList[(int)PhaseState.Second].Count; i++)
                    {
                        _enemyPhaseList[(int)PhaseState.Second][i].gameObject.SetActive(true);
                    }
                }
				break;
			case PhaseState.Second:
                if (_enemyPhaseList[(int)PhaseState.Second].Count <= 0)
                {
                    _phaseState = PhaseState.Third;

                    for (int i = 0; i < _enemyPhaseList[(int)PhaseState.Third].Count; i++)
                    {
                        _enemyPhaseList[(int)PhaseState.Third][i].gameObject.SetActive(true);
                    }
                }
                break;
			case PhaseState.Third:
                if (_enemyPhaseList[(int)PhaseState.Third].Count <= 0)
                {
                    _phaseState = PhaseState.Boss;

                    for (int i = 0; i < _enemyPhaseList[(int)PhaseState.Boss].Count; i++)
                    {
                        _enemyPhaseList[(int)PhaseState.Boss][i].gameObject.SetActive(true);
                    }
                }
                break;
			case PhaseState.Boss:
                if (_enemyPhaseList[(int)PhaseState.Boss].Count <= 0)
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

    /// <summary>
    /// リストにそれぞれのエネミーを追加する
    /// </summary>
    private void GenerateEnemyList()
    {
        _enemysObject = GameObject.FindWithTag("Enemys");

        for (int phaseCount = 1; phaseCount <= _enemysObject.transform.childCount; phaseCount++)
        {
            List<GameObject> enemyList = new List<GameObject>();
            List<IDamageable> interfaceList = new List<IDamageable>();

            for (int enemyCount = 1; enemyCount <= _enemysObject.transform.GetChild(phaseCount - 1).childCount; enemyCount++)
            {
                enemyList.Add(_enemysObject.transform.GetChild(phaseCount - 1).GetChild(enemyCount - 1).gameObject);
                interfaceList.Add(_enemysObject.transform.GetChild(phaseCount - 1).GetChild(enemyCount - 1).GetComponent<IDamageable>());

            }

            _enemyPhaseList.Add(enemyList);
            _enemyIDamageableList.Add(interfaceList);
        }
    }

    public void DownEnemyCount(GameObject enemy)
    {
        _enemyPhaseList[(int)_phaseState].Remove(enemy);
    }
	#endregion
}