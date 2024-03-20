// ---------------------------------------------------------
// GameDataManager.cs
//
// 作成日:2024/03/20
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;

public class GameDataManager : SingletonMonoBehaviour<GameDataManager>
{
	#region 変数
	// Easyならfalse、Hardならtrue
	[SerializeField]
	private bool _playerDifficult = false; 
	#endregion

	#region プロパティ
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