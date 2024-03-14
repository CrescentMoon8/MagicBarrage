// ---------------------------------------------------------
// GameManager.cs
//
// 作成日:2024/02/15
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
	#region 変数
	public enum GameState
    {
		Start,
		Pose,
		Play,
		GameOver,
		Result
    }

	private GameState _gameState = GameState.Start;

	private string _gameOverSceneName = "GameOver";
	private string _clearSceneName = "Result";

	[SerializeField]
	private Image _posePanel = default;
	[SerializeField]
	private Image _poseText = default;
	[SerializeField]
	private Button _retryButton = default;

	#endregion

	#region プロパティ
	public GameState SettingGameState { set { _gameState = value; } }
    #endregion

    #region メソッド
#if UNITY_IOS
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
#elif UNITY_STANDALONE_WIN
    private void Awake()
    {
        Application.targetFrameRate = 0;
    }
#endif

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update ()
	{
        switch (_gameState)
        {
			// ゲーム開始前処理
            case GameState.Start:
				// ポーズからリトライするとタイムスケールが０のままのため、１に変更する
				if(Time.timeScale <= 0)
				{
					Time.timeScale = 1;
				}

				PlayerBulletPool.Instance.BulletAwake();
				EnemyBulletPool.Instance.BulletAwake();

				_gameState = GameState.Play;
                break;

            case GameState.Pose:
                break;

            case GameState.Play:
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

    public void OnPose()
    {
        if (Time.timeScale >= 1.0f)
        {
            Time.timeScale = 0f;
			_posePanel.gameObject.SetActive(true);
			_poseText.gameObject.SetActive(true);
			_retryButton.gameObject.SetActive(true);
			_gameState = GameState.Pose;
        }
        else
        {
            Time.timeScale = 1.0f;
            _posePanel.gameObject.SetActive(false);
            _poseText.gameObject.SetActive(false);
            _retryButton.gameObject.SetActive(false);
            _gameState = GameState.Play;
        }
    }
#endregion
}