// ---------------------------------------------------------
// ScoreManager.cs
//
// 作成日:2024/03/01
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using TMPro;
using System.IO;
using System.Text;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
	#region 変数
	private int _score = 0;

	[SerializeField]
	private TMP_Text _scoreText = default;
	#endregion

	#region メソッド
	/// <summary>
	/// 点数を加算する
	/// </summary>
	/// <param name="scorePoint">アイテムの点数</param>
	public void AddScore(int scorePoint)
    {
		_score += scorePoint;
    }

	/// <summary>
	/// スコアテキストの更新
	/// </summary>
	public void ChangeScoreText()
    {
		_scoreText.SetText(_score.ToString("d7"));
    }

	/// <summary>
	/// 点数をCSVファイルに出力する
	/// </summary>
	public void ScoreSave()
    {
		string path = Application.persistentDataPath + "/Score.csv";

		using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.GetEncoding("UTF-8")))
        {
			streamWriter.Write(_score.ToString("d7"));
        }
    }
	#endregion
}