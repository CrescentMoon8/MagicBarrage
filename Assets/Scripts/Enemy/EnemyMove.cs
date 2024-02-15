// ---------------------------------------------------------
// EnemyMove.cs
//
// 作成日:2024/02/15
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.Splines;
using System;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
	#region 変数
	private enum MoveState
	{
		BeforeEnter,
		Enter,
		Stay,
		Exit,
		Stop
	}

	private MoveState _moveState = MoveState.BeforeEnter;

	private const float EXIT_MOVE_TIME = 15f;
	private const float MOVE_STOP_RATIO = 1.0f;
	private float _moveTime = 0f;
	// Splineの長さに対する現在位置の割合（始点：０　終点：１）
	private float _moveRatio = 0f;
	private Vector3 differencePos = Vector3.zero;

	private SplineContainer _enterSplineContainer = default;
	private SplineContainer _exitSplineContainer = default;
	private int _splineIndex = 0;
	#endregion

	#region プロパティ
	public int SplineIndex { set { _splineIndex = value; } }
	#endregion

	#region メソッド
	public void SetSplineContainer()
    {
		_enterSplineContainer = GameObject.Find("EnterSpline").GetComponent<SplineContainer>();
		_exitSplineContainer = GameObject.Find("ExitSpline").GetComponent<SplineContainer>();
	}

	/// <summary>
	/// 初期化処理
	/// </summary>
	private void DifferencePosInitialize(SplineContainer splineContainer)
	{
		float nowEnemyPosX = this.transform.position.x;
		float nowEnemyPosY = this.transform.position.y;

		float firstSplinePosX = splineContainer.Splines[_splineIndex].EvaluatePosition(0).x;
		float firstSplinePosY = splineContainer.Splines[_splineIndex].EvaluatePosition(0).y;

		differencePos = new Vector3(nowEnemyPosX - firstSplinePosX, nowEnemyPosY - firstSplinePosY, 0);
	}

	public void Move()
	{
		switch (_moveState)
		{
			case MoveState.BeforeEnter:
				DifferencePosInitialize(_enterSplineContainer);
				_moveState = MoveState.Enter;
				break;
			case MoveState.Enter:
				_moveRatio += Time.deltaTime / 4;

				// EnemySplineの指定したSplineの座標を取得する
				Vector3 movePos = _enterSplineContainer.Splines[_splineIndex].EvaluatePosition(_moveRatio);
				// 
				this.transform.position = movePos + differencePos;

				if (_moveRatio >= MOVE_STOP_RATIO)
				{
					_moveRatio = 0f;
					_moveState = MoveState.Stay;
				}
				break;
			case MoveState.Stay:
				_moveTime += Time.deltaTime;

				if (_moveTime >= EXIT_MOVE_TIME)
				{
					DifferencePosInitialize(_exitSplineContainer);
					_moveState = MoveState.Exit;
				}
				break;
			case MoveState.Exit:
				_moveRatio += Time.deltaTime / 4;

				// EnemySplineの指定したSplineの座標を取得する
				movePos = _exitSplineContainer.Splines[_splineIndex].EvaluatePosition(_moveRatio);
				// 
				this.transform.position = movePos + differencePos;
				break;

			default:
				break;
		}
	}
	#endregion
}