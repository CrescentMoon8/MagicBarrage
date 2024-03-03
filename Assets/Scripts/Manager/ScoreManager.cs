// ---------------------------------------------------------
// ScoreManager.cs
//
// 作成日:2024/03/01
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
	#region 変数
	private int _score = 0;

	[SerializeField]
	private TMP_Text _scoreText = default;
	#endregion

	#region メソッド
	public void AddScore(int scorePoint)
    {
		_score += scorePoint;
    }

	public void ChangeScoreText()
    {
		_scoreText.SetText(_score.ToString("d7"));
    }
	#endregion
}