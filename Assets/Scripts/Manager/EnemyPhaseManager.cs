// ---------------------------------------------------------
// EnemyPhaseManager.cs
//
// 作成日:2024/02/11
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// フェーズの管理を行うクラス
/// </summary>
public class EnemyPhaseManager : SingletonMonoBehaviour<EnemyPhaseManager>, IEnemyList
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

    // フェーズごとの敵オブジェクトをまとめているオブジェクト
    private GameObject _enemysObject = default;

    // それぞれのフェーズに出てくる敵のリスト
    private List<List<GameObject>> _enemyPhaseList = new List<List<GameObject>>();
    // それぞれのフェーズに出てくる敵のダメージ用インターフェースのリスト
    private List<List<IDamageable>> _enemyIDamageableList = new List<List<IDamageable>>();

    // 現在のフェーズに出てくる敵のリスト
    private List<GameObject> _currentPhaseEnemyList = new List<GameObject>();
    // 現在のフェーズに出てくる敵のダメージ用インターフェースのリスト
    private List<IDamageable> _currentPhaseIDamageableList = new List<IDamageable>();

    // すべてのフェーズが終了したか
    [SerializeField]
    private bool _isEnd = false;

    private BossDeadAnimation _bossDeadAnimation = default;
    #endregion

    #region プロパティ
    public List<GameObject> CurrentPhaseEnemyList { get { return _currentPhaseEnemyList; } }
    public List<IDamageable> CurrentPhaseIDamageableList { get { return _currentPhaseIDamageableList; } }
    public PhaseState GettingPhaseState { get { return _phaseState; } }
    public bool IsEnd { get { return _isEnd; } }
    #endregion

    #region メソッド
    /// <summary>
    /// フェーズの初期化処理
    /// </summary>
    public void EnemyPhaseAwake()
    {
        base.Awake();

        _bossDeadAnimation = GetComponent<BossDeadAnimation>();

        GenerateEnemyList();
        for (int i = 0; i < _enemyPhaseList[(int)PhaseState.First].Count; i++)
        {
            _enemyPhaseList[(int)PhaseState.First][i].SetActive(true);
        }
        AddCurrentPhaseList((int)PhaseState.First);

        _bossDeadAnimation.DeadAnimationAwake();
    }

	/// <summary>
	/// 更新処理
	/// </summary>
	public void EnemyPhaseUpdate ()
	{
		switch (_phaseState)
		{
			case PhaseState.First:
                if (_enemyPhaseList[(int)PhaseState.First].Count <= 0)
                {
                    _phaseState = PhaseState.Second;

                    NextPhasePreparation((int)PhaseState.Second);
                }
                break;

			case PhaseState.Second:
                if (_enemyPhaseList[(int)PhaseState.Second].Count <= 0)
                {
                    _phaseState = PhaseState.Third;

                    NextPhasePreparation((int)PhaseState.Third);
                }
                break;

			case PhaseState.Third:
                if (_enemyPhaseList[(int)PhaseState.Third].Count <= 0)
                {
                    _phaseState = PhaseState.Boss;

                    NextPhasePreparation((int)PhaseState.Boss);
                }
                break;

			case PhaseState.Boss:
                if (_enemyPhaseList[(int)PhaseState.Boss].Count <= 0)
                {
                    _bossDeadAnimation.Play();
                    _phaseState = PhaseState.End;
                }
                break;

			case PhaseState.End:
                if(_bossDeadAnimation.EndDeadAnimation())
                {
                    _isEnd = true;
                }
                break;

			default:
				break;
		}
	}

    /// <summary>
    /// 現在のフェーズの敵リストの更新とオブジェクトの表示をを行う
    /// </summary>
    /// <param name="nextPhaseNumber">次のフェーズ番号</param>
    private void NextPhasePreparation(int nextPhaseNumber)
    {
        ClearCurrentPhaseList();
        AddCurrentPhaseList(nextPhaseNumber);

        for (int i = 0; i < _enemyPhaseList[nextPhaseNumber].Count; i++)
        {
            _enemyPhaseList[nextPhaseNumber][i].SetActive(true);
        }
    }

    /// <summary>
    /// リストにそれぞれのエネミーを追加する
    /// </summary>
    private void GenerateEnemyList()
    {
        _enemysObject = GameObject.FindWithTag("Enemys");

        // フェーズのオブジェクトの数分処理する
        for (int phaseCount = 1; phaseCount <= _enemysObject.transform.childCount; phaseCount++)
        {
            List<GameObject> enemyList = new List<GameObject>();
            List<IDamageable> interfaceList = new List<IDamageable>();

            // フェーズのオブジェクトの子になっている敵の数分処理する
            for (int enemyCount = 1; enemyCount <= _enemysObject.transform.GetChild(phaseCount - 1).childCount; enemyCount++)
            {
                Transform enemy = _enemysObject.transform.GetChild(phaseCount - 1).GetChild(enemyCount - 1);
                enemyList.Add(enemy.gameObject);
                interfaceList.Add(enemy.GetComponent<IDamageable>());
            }

            _enemyPhaseList.Add(enemyList);
            _enemyIDamageableList.Add(interfaceList);
        }

        foreach(List<GameObject> enemyList in _enemyPhaseList)
        {
            foreach(GameObject enemy in enemyList)
            {
                // エネミーの親クラスの初期化メソッド
                enemy.GetComponent<EnemyHp>().EnemyAwake();
            }
        }
    }

    /// <summary>
    /// 現在のフェーズの敵オブジェクトとインターフェースをリストに追加する
    /// </summary>
    /// <param name="phaseNumber"></param>
    private void AddCurrentPhaseList(int phaseNumber)
    {
        _currentPhaseEnemyList = _enemyPhaseList[phaseNumber];
        _currentPhaseIDamageableList = _enemyIDamageableList[phaseNumber];
    }

    /// <summary>
    /// 現在のフェーズの敵オブジェクトとインターフェースのリストを初期化する
    /// </summary>
    private void ClearCurrentPhaseList()
    {
        _currentPhaseEnemyList.Clear();
        _currentPhaseIDamageableList.Clear();
    }

    /// <summary>
    /// 現在のフェーズの敵リストから死んだ敵を削除する
    /// </summary>
    /// <param name="enemy"></param>
    public void DownEnemyCount(GameObject enemy)
    {
        _currentPhaseEnemyList.Remove(enemy);
    }
	#endregion
}