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

public class EnemyMove
{
	#region 変数
	public enum MoveState
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
	public void DifferencePosInitialize(Vector3 enemyPos)
	{
        float nowEnemyPosX = enemyPos.x;
        float nowEnemyPosY = enemyPos.y;

        float firstSplinePosX = _enterSplineContainer.Splines[_splineIndex].EvaluatePosition(0).x;
        float firstSplinePosY = _enterSplineContainer.Splines[_splineIndex].EvaluatePosition(0).y;

        differencePos = new Vector3(nowEnemyPosX - firstSplinePosX, nowEnemyPosY - firstSplinePosY, 0);
    }

    public Vector3 MovePosCalculate()
    {
        _moveRatio += Time.deltaTime / 4;

        // EnemySplineの指定したSplineの座標を取得する
        Vector3 movePos = _enterSplineContainer.Splines[_splineIndex].EvaluatePosition(_moveRatio);

        // 
        return movePos + differencePos;
    }

    /*public void Move(Vector3 enemyPos)
    {
        switch (_moveState)
        {
            case MoveState.BeforeEnter:
                SetSplineContainer();
                DifferencePosInitialize(_enterSplineContainer, enemyPos);
                _moveState = MoveState.Enter;
                break;
            case MoveState.Enter:
                

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
    }*/
    #endregion
}