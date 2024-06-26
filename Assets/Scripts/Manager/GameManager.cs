// ---------------------------------------------------------
// GameManager.cs
//
// 作成日:2024/02/15
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームの流れを管理するクラス
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager>
{
	#region 変数
	public enum GameState
    {
		Start,
		Pause,
		Play,
        EndAnimation,
		GameOver,
		Result
    }

    [SerializeField]
	private GameState _gameState = GameState.Start;

    // スタート処理が終わっているか
    private bool _isStarted = false;

	private string _gameOverSceneName = "GameOver";
	private string _clearSceneName = "Result";

	[SerializeField]
	private Image _pausePanel = default;
	[SerializeField]
	private Image _pauseText = default;
	[SerializeField]
	private Button _retryButton = default;

	#endregion

	#region プロパティ

    #endregion

    #region メソッド
    protected override void Awake()
    {
        base.Awake();
#if UNITY_IOS
        Application.targetFrameRate = 60;
#elif UNITY_STANDALONE_WIN
        Application.targetFrameRate = 0;
#endif
    }


    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update ()
	{
        switch (_gameState)
        {
			// ゲーム開始前処理
            case GameState.Start:
                PlayerManager.Instance.PlayerAwake();
                EnemyPhaseManager.Instance.EnemyPhaseAwake();
				PlayerBulletPool.Instance.BulletAwake();
				EnemyBulletPool.Instance.BulletAwake();

                _isStarted = true;
				_gameState = GameState.Play;
                break;

            case GameState.Pause:
                break;

            case GameState.Play:
                PlayerManager.Instance.PlayerUpdate();
                EnemyPhaseManager.Instance.EnemyPhaseUpdate();

                if(PlayerManager.Instance.IsDead)
                {
                    _gameState = GameState.GameOver;
                }

                if(EnemyPhaseManager.Instance.GettingPhaseState == EnemyPhaseManager.PhaseState.End)
                {
                    _gameState = GameState.EndAnimation;
                }
                break;

            case GameState.EndAnimation:
                EnemyPhaseManager.Instance.EnemyPhaseUpdate();

                PlayerManager.Instance.FalseCollider();

                if(EnemyPhaseManager.Instance.IsEnd)
                {
                    _gameState = GameState.Result;
                }
                break;

            case GameState.GameOver:
				ScoreManager.Instance.ScoreSave();
				SceneManager.LoadScene(_gameOverSceneName, LoadSceneMode.Single);
				break;

			case GameState.Result:
#if UNITY_IOS
                Application.targetFrameRate = 30;
#endif
				ScoreManager.Instance.ScoreSave();
				SceneManager.LoadScene(_clearSceneName, LoadSceneMode.Single);
				break;

            default:
                break;
        }
    }

    /// <summary>
    /// ポーズ状態への移行、プレイ状態への移行を行う
    /// </summary>
    public void OnPause()
    {
        if (Time.timeScale >= 1.0f)
        {
            Time.timeScale = 0f;
			_pausePanel.gameObject.SetActive(true);
			_pauseText.gameObject.SetActive(true);
			_retryButton.gameObject.SetActive(true);
			_gameState = GameState.Pause;
        }
        else
        {
            Time.timeScale = 1.0f;
            _pausePanel.gameObject.SetActive(false);
            _pauseText.gameObject.SetActive(false);
            _retryButton.gameObject.SetActive(false);
            if(!_isStarted)
            {
                _gameState = GameState.Start;
                return;
            }
            _gameState = GameState.Play;
        }
    }

#if UNITY_IOS
    /// <summary>
    /// バックグラウンドに移動したとき、スマホをロックしたときにポーズ状態に移行
    /// </summary>
    private void OnApplicationPause()
    {
        Debug.Log("強制Pause");
        Time.timeScale = 0f;
        _pausePanel.gameObject.SetActive(true);
        _pauseText.gameObject.SetActive(true);
        _retryButton.gameObject.SetActive(true);
        _gameState = GameState.Pause;
    }
#endif
#endregion
}