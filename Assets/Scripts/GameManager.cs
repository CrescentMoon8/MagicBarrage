// ---------------------------------------------------------
// GameManager.cs
//
// 作成日:2024/02/15
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;

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
	#endregion
}