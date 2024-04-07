// ---------------------------------------------------------
// MoveGameSceneButton.cs
//
// 作成日:2024/02/13
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ゲームシーンに移動するボタン
/// </summary>
public class MoveGameSceneButton : MonoBehaviour
{
    #region メソッド
    private void Awake()
    {
        if(gameObject.name == "EasyButton")
        {
            this.GetComponent<Button>().onClick.AddListener(GameDataManager.Instance.SetEasy);
        }
        else if(gameObject.name == "HardButton")
        {
            this.GetComponent<Button>().onClick.AddListener(GameDataManager.Instance.SetHard);
        }
    }
    public void MoveGameScene()
    {
		SceneManager.LoadScene("Main");
    }
	#endregion
}