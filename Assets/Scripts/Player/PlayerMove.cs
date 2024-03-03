// ---------------------------------------------------------
// PlayerMove.cs
//
// 作成日:2024/03/01
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.AddressableAssets;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class PlayerMove
{
    #region 変数
    private Vector3 _startMousePos = Vector3.zero;
	private Vector2 _startObjectPos = Vector2.zero;
	private Vector2 _cursorPosDistance = Vector2.zero;

    private Vector2 _maxMoveLimitPos = Vector2.zero;
    private Vector2 _minMoveLimitPos = Vector2.zero;
    #endregion

    #region プロパティ

    #endregion

    #region メソッド
    /// <summary>
    /// 移動制限に必要な座標を計算する
    /// </summary>
    /// <param name="radius"></param>
    public void Initialize(Vector3 radius, RectTransform header, RectTransform footer)
    {
        int MAX_MOVE_POS_INDEX = 3;
        int MIN_MOVE_POS_INDEX = 1;

        // UIオブジェクトの頂点の座標を取得し、移動制限の大きさを調整する
        // 座標の取得順は左下、左上、右上、右下
        Vector3[] headerCorners = new Vector3[4];
        header.GetWorldCorners(headerCorners);
        _maxMoveLimitPos = headerCorners[MAX_MOVE_POS_INDEX] - radius;

        Vector3[] footerCorners = new Vector3[4];
        footer.GetWorldCorners(footerCorners);
        _minMoveLimitPos = footerCorners[MIN_MOVE_POS_INDEX] + radius;
    }

    /// <summary>
    /// 移動可能かどうかと、可能なら入力の始点からの移動距離の計算を行う
    /// </summary>
    /// <param name="activeTouch">入力</param>
    /// <param name="playerPos">プレイヤーの移動前座標</param>
    /// <returns></returns>
    public bool MovePos(Touch activeTouch, Vector3 playerPos)
    {
        switch (activeTouch.phase)
        {
            // 指がふれたとき
            case TouchPhase.Began:
                _startObjectPos = playerPos;
                break;

            // 指が動いてるとき
            case TouchPhase.Moved:
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

        return true;
    }

    /// <summary>
    /// 入力の始点からの移動距離をリセットする
    /// </summary>
    public void ResetCursorPosDistance()
    {
        _cursorPosDistance = Vector2.zero;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Vector3 Move()
    {
        Vector3 movePos = _startObjectPos + _cursorPosDistance;

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