// ---------------------------------------------------------
// PlayerMove.cs
//
// 作成日:2024/03/01
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

/// <summary>
/// プレイヤーの移動座標の計算を行うクラス
/// </summary>
public class PlayerMove
{
    #region 変数
    // プレイヤーの移動前座標
	private Vector2 _startObjectPos = Vector2.zero;
    // 入力の始点からの移動距離
    private Vector2 _cursorPosDistance = Vector2.zero;

    // プレイヤーが移動できる座標の最大・最小
    private Vector2 _maxMoveLimitPos = Vector2.zero;
    private Vector2 _minMoveLimitPos = Vector2.zero;
    #endregion

    #region プロパティ

    #endregion

    #region メソッド
    /// <summary>
    /// 移動制限に必要な座標を計算する
    /// </summary>
    /// <param name="radius">プレイヤーコアの半径</param>

    public void Initialize(Vector3 radius)
    {
        _maxMoveLimitPos = FieldSize.Instance.MaxFieldVector - radius;

        _minMoveLimitPos = FieldSize.Instance.MinFieldVector + radius;

        /*Debug.Log(_maxMoveLimitPos);
        Debug.Log(_minMoveLimitPos);*/
    }

    /// <summary>
    /// 移動可能かどうかと、可能なら入力の始点からの移動距離の計算を行う
    /// </summary>
    /// <param name="activeTouch">入力情報</param>
    /// <param name="playerPos">プレイヤーの移動前座標</param>
    public void MovePos(Touch activeTouch, Vector3 playerPos)
    {
        switch (activeTouch.phase)
        {
            // 指がふれたとき
            case TouchPhase.Began:
                _startObjectPos = playerPos;
                break;

            // 指が動いてるとき
            case TouchPhase.Moved:
                // 入力座標をそれぞれワールド座標に変換して計算する
                _cursorPosDistance = Camera.main.ScreenToWorldPoint(activeTouch.screenPosition)
                                        - Camera.main.ScreenToWorldPoint(activeTouch.startScreenPosition);
                break;

            // 指が離れたとき
            case TouchPhase.Ended:
                break;

            // 何らかの理由で入力が途切れたとき
            case TouchPhase.Canceled:
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 指を離すと座標の更新がされずに値が残るため
    /// 入力の始点からの移動距離をリセットする
    /// </summary>
    public void ResetCursorPosDistance()
    {
        _cursorPosDistance = Vector2.zero;
    }

    /// <summary>
    /// IOSビルド用移動処理
    /// </summary>
    /// <returns>移動先座標</returns>
    public Vector3 Move()
    {
        Vector3 movePos = _startObjectPos + _cursorPosDistance;

        return InStage(movePos);
    }

    /// <summary>
    /// Windowsビルド用移動処理
    /// </summary>
    /// <returns>移動先座標</returns>
    public Vector3 Move(Vector3 startObjectPos, Vector3 cursorPosDistance)
    {
        Vector3 movePos = startObjectPos + cursorPosDistance;

        return InStage(movePos);
    }

    /// <summary>
    /// プレイヤーが画面外に行くのを防ぐ
    /// </summary>
    private Vector3 InStage(Vector3 movePos)
    {
        Vector3 clampPos = movePos;

        // プレイヤーが画面内にいるなら処理を飛ばす
        if ((clampPos.y <= _maxMoveLimitPos.y) && (clampPos.y >= _minMoveLimitPos.y) &&
            (clampPos.x <= _maxMoveLimitPos.x) && (clampPos.x >= _minMoveLimitPos.x))
        {
            return clampPos;
        }

        if (clampPos.y >= _maxMoveLimitPos.y)
        {
            clampPos = new Vector2(clampPos.x, _maxMoveLimitPos.y);
        }

        if (clampPos.y <= _minMoveLimitPos.y)
        {
            clampPos = new Vector2(clampPos.x, _minMoveLimitPos.y);
        }

        if (clampPos.x >= _maxMoveLimitPos.x)
        {
            clampPos = new Vector2(_maxMoveLimitPos.x, clampPos.y);
        }

        if (clampPos.x <= _minMoveLimitPos.x)
        {
            clampPos = new Vector2(_minMoveLimitPos.x, clampPos.y);
        }

        return clampPos;
    }
    #endregion
}