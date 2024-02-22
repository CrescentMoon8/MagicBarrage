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
    /*public enum MoveState
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
    private float _moveTime = 0f;*/

    // Splineの長さに対する現在位置の割合（始点：０　終点：１）
    private float _moveRatio = 0f;
	private Vector3 differencePos = Vector3.zero;
    private int _splineIndex = 0;

	private SplineContainer _enterSplineContainer = default;
	//private SplineContainer _exitSplineContainer = default;
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
    /// <summary>
    /// スプラインコンテナの取得と軌道の選択に使用する値を代入する
    /// </summary>
    /// <param name="splineIndex"></param>
	public void SetSplineContainer(int splineIndex)
    {
		_enterSplineContainer = GameObject.Find("EnterSpline").GetComponent<SplineContainer>();
        //_exitSplineContainer = GameObject.Find("ExitSpline").GetComponent<SplineContainer>();
        _splineIndex = splineIndex;
	}

    /// <summary>
    /// 移動開始点から自分の座標の差を記録する
    /// </summary>
    public void DifferencePosInitialize(Vector3 enemyPos)
	{
        // _splineContainer.Splines[0].EvaluatePosition(0) → Spline0の始点の座標
        // _splineContainer.Splines[1].EvaluatePosition(0) → Spline1の始点の座標

        float nowEnemyPosX = enemyPos.x;
        float nowEnemyPosY = enemyPos.y;

        float firstSplinePosX = _enterSplineContainer.Splines[_splineIndex].EvaluatePosition(0).x;
        float firstSplinePosY = _enterSplineContainer.Splines[_splineIndex].EvaluatePosition(0).y;

        differencePos = new Vector3(nowEnemyPosX - firstSplinePosX, nowEnemyPosY - firstSplinePosY, 0);
    }

    /// <summary>
    /// 移動先の座標を取得する
    /// </summary>
    /// <returns></returns>
    public Vector3 NextMovePos()
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