// ---------------------------------------------------------
// Player.cs
//
// 作成日:2024/02/05
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class Player : MonoBehaviour
{
    #region 変数
    [SerializeField]
    private Vector2 _startObjectPosition = Vector2.zero;
    [SerializeField]
    private Vector2 _cursorPositionDistance = Vector2.zero;
    [SerializeField]
    private bool _isMove = false;

    private Vector3 _radius = Vector3.zero;

    [SerializeField]
    private Vector2 _maxCameraPosition = default;
    [SerializeField]
    private Vector2 _minCameraPosition = default;

    #endregion

    #region プロパティ

    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
	{
        // 円形のため、半径の取得にはスケールｘの値で行い、yにも使う
        _radius = transform.localScale / 2;

        _maxCameraPosition = Camera.main.ViewportToWorldPoint(Vector3.one) - _radius;
        _minCameraPosition = Camera.main.ViewportToWorldPoint(Vector3.zero) + _radius;
        
        EnhancedTouchSupport.Enable();
    }

    private void Update()
    {
        PlayerMove();
    }

    private void FixedUpdate()
    {
        if (_isMove)
        {
            this.transform.position = _startObjectPosition + _cursorPositionDistance;

            InStage();
        }
    }

    private void PlayerMove()
	{
        if (Touch.activeTouches.Count >= 1)
        {
            Touch active = Touch.activeTouches[0];

            // 指がふれたとき
            if (active.phase == TouchPhase.Began)
            {
                _startObjectPosition = this.transform.position;
            }

            // 指が動いてるとき
            if (active.phase == TouchPhase.Moved)
            {
                _isMove = true;
                _cursorPositionDistance = Camera.main.ScreenToWorldPoint(active.screenPosition) - Camera.main.ScreenToWorldPoint(active.startScreenPosition);
            }

            // 指が離れているとき
            if (active.phase == TouchPhase.Ended)
            {
                _isMove = false;
            }
        }
    }

    /// <summary>
    /// プレイヤーが画面外に行くのを防ぐ
    /// </summary>
    private void InStage()
    {
        // プレイヤーが画面内にいるなら処理を飛ばす
        if ((this.transform.position.y <= _maxCameraPosition.y) && (this.transform.position.y >= _minCameraPosition.y) &&
            (this.transform.position.x <= _maxCameraPosition.x) && (this.transform.position.x >= _minCameraPosition.x))
        {
            return;
        }

        if ((this.transform.position.y) >= _maxCameraPosition.y)
        {
            this.transform.position = new Vector2(this.transform.position.x, _maxCameraPosition.y);
        }

        if ((this.transform.position.y) <= _minCameraPosition.y)
        {
            this.transform.position = new Vector2(this.transform.position.x, _minCameraPosition.y);
        }

        if (this.transform.position.x >= _maxCameraPosition.x)
        {
            this.transform.position = new Vector2(_maxCameraPosition.x, this.transform.position.y);
        }

        if (this.transform.position.x <= _minCameraPosition.x)
        {
            this.transform.position = new Vector2(_minCameraPosition.x, this.transform.position.y);
        }
    }
    #endregion
}