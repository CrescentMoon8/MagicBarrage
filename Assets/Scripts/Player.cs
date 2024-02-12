// ---------------------------------------------------------
// Player.cs
//
// 作成日:2024/02/05
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private bool _isShot = false;
    private float _shotTime = 0f;
    private const float SHOT_INTERVAL = 0.1f;
    private Vector3 _shotPosition = Vector3.zero;
    private const float SHOT_POSITION_DIFFERENCE_Y = 0.75f;

    [SerializeField]
    private Vector3 _radius = Vector3.zero;
    private const int MAX_MOVE_POSITION_INDEX = 3;
    private const int MIN_MOVE_POSITION_INDEX = 1;
    [SerializeField]
    private Vector2 _maxMovePosition = Vector2.zero;
    [SerializeField]
    private Vector2 _minMovePosition = Vector2.zero;

    [SerializeField]
    private RectTransform _header = default;
    [SerializeField]
    private RectTransform _footer = default;

    private bool _isDamage = false;
    private int _damageCount = 0;
    private const int GAMEOVER_MOVE_COUNT = 3;
    private float _damageAnimationTime = 0;
    private const float DAMAGE_ANIMATION = 3f;

    private SpriteRenderer _spriteRenderer = default;
    private CircleCollider2D _circleCollider2D = default;
    private BulletPool _bulletPool = default;
    private Animator _playerAnimator = default;

    #endregion

    #region プロパティ
    public Vector3 PlayerPos { get { return this.transform.position; } }
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
	{
        _bulletPool = UnityEngine.GameObject.FindWithTag("Scripts").GetComponentInChildren<BulletPool>();
        _playerAnimator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider2D = GetComponent<CircleCollider2D>();

        // 半径を取得する
        _radius = transform.GetChild(0).localScale / 2;

        // UIオブジェクトの頂点の座標を取得し、移動制限の大きさを調整する
        // 座標の取得順は左下、左上、右上、右下
        Vector3[] headerCorners = new Vector3[4];
        _header.GetWorldCorners(headerCorners);
        _maxMovePosition = headerCorners[MAX_MOVE_POSITION_INDEX] - _radius;

        Vector3[] footerCorners = new Vector3[4];
        _footer.GetWorldCorners(footerCorners);
        _minMovePosition = footerCorners[MIN_MOVE_POSITION_INDEX] + _radius;

        EnhancedTouchSupport.Enable();
    }

    private void Update()
    {
        _shotTime += Time.deltaTime;
        PlayerInput();

        if(_isDamage)
        {
            _damageAnimationTime += Time.deltaTime;

            if(_damageAnimationTime >= DAMAGE_ANIMATION)
            {
                _damageAnimationTime = 0;
                _spriteRenderer.enabled = true;
                _circleCollider2D.enabled = true;
                _isDamage = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isMove)
        {
            this.transform.position = _startObjectPosition + _cursorPositionDistance;

            InStage();
        }

        if(_isShot)
        {
            if(_shotTime >= SHOT_INTERVAL)
            {
                _shotPosition = this.transform.position;
                _shotPosition.y += SHOT_POSITION_DIFFERENCE_Y;

                _bulletPool.LendPlayerBullet(_shotPosition);

                _shotTime = 0;
            }
        }
    }

    private void PlayerInput()
	{
        if (Touch.activeTouches.Count >= 1)
        {
            Touch active = Touch.activeTouches[0];

            _isShot = true;

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
        else
        {
            _isShot = false;
        }
    }

    /// <summary>
    /// プレイヤーが画面外に行くのを防ぐ
    /// </summary>
    private void InStage()
    {
        // プレイヤーが画面内にいるなら処理を飛ばす
        if ((this.transform.position.y <= _maxMovePosition.y) && (this.transform.position.y >= _minMovePosition.y) &&
            (this.transform.position.x <= _maxMovePosition.x) && (this.transform.position.x >= _minMovePosition.x))
        {
            return;
        }

        if ((this.transform.position.y) >= _maxMovePosition.y)
        {
            this.transform.position = new Vector2(this.transform.position.x, _maxMovePosition.y);
        }

        if ((this.transform.position.y) <= _minMovePosition.y)
        {
            this.transform.position = new Vector2(this.transform.position.x, _minMovePosition.y);
        }

        if (this.transform.position.x >= _maxMovePosition.x)
        {
            this.transform.position = new Vector2(_maxMovePosition.x, this.transform.position.y);
        }

        if (this.transform.position.x <= _minMovePosition.x)
        {
            this.transform.position = new Vector2(_minMovePosition.x, this.transform.position.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            _playerAnimator.SetTrigger("IsDamage");
            _spriteRenderer.enabled = false;
            _circleCollider2D.enabled = false;
            _isDamage = true;

            _damageCount++;

            if(_damageCount > GAMEOVER_MOVE_COUNT)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }
    #endregion
}