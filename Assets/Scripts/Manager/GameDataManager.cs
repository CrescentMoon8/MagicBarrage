// ---------------------------------------------------------
// GameDataManager.cs
//
// 作成日:2024/03/20
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

/// <summary>
/// ゲームの難易度を管理するクラス
/// </summary>
public class GameDataManager : SingletonMonoBehaviour<GameDataManager>
{
	#region 変数
	// Easyならfalse、Hardならtrue
	[SerializeField]
	private bool _playerDifficult = false;
    #endregion

    #region プロパティ
    /// <summary>
    /// Easyならfalse、Hardならtrue
    /// </summary>
    public bool PlayerDifficult {  get { return _playerDifficult; } }
	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	protected override void Awake()
	{
		base._dontDestroyOnLoad = true;

		base.Awake();
	}

    public void SetEasy()
    {
		_playerDifficult = false;
    }

    public void SetHard()
    {
		_playerDifficult = true;
    }
    #endregion
}