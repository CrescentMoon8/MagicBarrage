// ---------------------------------------------------------
// GameManager.cs
//
// 作成日:2024/02/15
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	#region 変数
	private enum GameState
    {
		Start,
		Pose,
		Play,
		Score,
		End
    }

	private GameState _gameState = GameState.Start;

	[SerializeField]
	private Image _posePanel = default;
	[SerializeField]
	private Image _poseText = default;
	[SerializeField]
	private Button _retryButton = default;

	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
		
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
        switch (_gameState)
        {
			// ゲーム開始前処理
            case GameState.Start:
				// ポーズからリトライするとタイムスケールが０のままのため、１に変更する
				if(Time.timeScale <= 0)
				{
					Time.timeScale = 1;
				}
                break;

            case GameState.Pose:
                break;

            case GameState.Play:
                break;

            case GameState.Score:
                break;

            case GameState.End:
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