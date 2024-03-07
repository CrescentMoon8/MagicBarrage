// ---------------------------------------------------------
// EndButton.cs
//
// 作成日:2024/02/22
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

public class EndButton : MonoBehaviour
{

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
	{
#if UNITY_IOS
        this.gameObject.SetActive(false);
#endif
    }

    public void EndGame()
    {
        Application.Quit();
    }
    #endregion

}