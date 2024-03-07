// ---------------------------------------------------------
// ScoreDisplay.cs
//
// 作成日:2024/03/07
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using TMPro;
using System.IO;
using System.Text;

public class ScoreDisplay : MonoBehaviour
{
	#region 変数
	[SerializeField]
	private TMP_Text _scoreText = default;
    #endregion

    #region メソッド
    private void Awake()
    {
        ScoreLoad();
    }

    /// <summary>
    /// 点数をCSVファイルから読み込み表示する
    /// </summary>
    private void ScoreLoad()
    {
		string path = Application.persistentDataPath + "/Score.csv";

		using (StreamReader streamReader = new StreamReader(path, Encoding.GetEncoding("UTF-8")))
        {
			string score = streamReader.ReadLine();
			_scoreText.SetText(score);
        }
    }
	#endregion
}