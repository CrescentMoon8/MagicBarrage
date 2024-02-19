// ---------------------------------------------------------
// Player.cs
//
// 作成日:2024/02/05
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class Player : MonoBehaviour
{
    #region 変数
    private const int LINE_BULLET_AMOUNT = 1;
    private const int TRACKING_BULLET_AMOUNT = 2;
    private Vector3 _startMousePos = Vector3.zero;
    [SerializeField]
    private Vector2 _startObjectPos = Vector2.zero;
    [SerializeField]
    private Vector2 _cursorPosDistance = Vector2.zero;
    [SerializeField]
    private bool _isMove = false;

    private bool _isShot = false;
    private float _shotTime = 0f;
    private const float SHOT_INTERVAL = 0.1f;
    private const float SHOT_POS_DIFFERENCE_Y = 0.7f;
    private const float SHOT_POS_DIFFERENCE_X = 0.2f;

    [SerializeField]
    private Vector3 _radius = Vector3.zero;
    private const int MAX_MOVE_POS_INDEX = 3;
    private const int MIN_MOVE_POS_INDEX = 1;
    [SerializeField]
    private Vector2 _maxMovePos = Vector2.zero;
    [SerializeField]
    private Vector2 _minMovePos = Vector2.zero;

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
    private Animator _playerAnimator = default;
    private BulletPool _bulletPool = default;

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
        _maxMovePos = headerCorners[MAX_MOVE_POS_INDEX] - _radius;

        Vector3[] footerCorners = new Vector3[4];
        _footer.GetWorldCorners(footerCorners);
        _minMovePos = footerCorners[MIN_MOVE_POS_INDEX] + _radius;

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
            this.transform.position = _startObjectPos + _cursorPosDistance;

            InStage();
        }

        if(_isShot)
        {
            PlayerShot();
        }
    }

    private void PlayerShot()
    {
        if (_shotTime >= SHOT_INTERVAL)
        {
            Vector3 shotPos = this.transform.position;
            shotPos.y += SHOT_POS_DIFFERENCE_Y;

            for (int bulletCount = 1; bulletCount <= LINE_BULLET_AMOUNT; bulletCount++)
            {
                _bulletPool.LendPlayerBullet(shotPos, Bullet.MoveType.Line);
            }

            for (int bulletCount = 1; bulletCount <= TRACKING_BULLET_AMOUNT; bulletCount++)
            {
                if (bulletCount % 2 == 1)
                {
                    shotPos.x -= SHOT_POS_DIFFERENCE_X * bulletCount;
                }
                else
                {
                    shotPos.x += SHOT_POS_DIFFERENCE_X * bulletCount;
                }

                _bulletPool.LendPlayerBullet(shotPos, Bullet.MoveType.Tracking);
            }

            _shotTime = 0;
        }
    }

    private void PlayerInput()
	{
#if UNITY_IOS || UNITY_EDITOR
        if (Touch.activeTouches.Count >= 1)
        {
            Touch active = Touch.activeTouches[0];

            _isShot = true;

            // 指がふれたとき
            if (active.phase == TouchPhase.Began)
            {
                _startObjectPos = this.transform.position;
            }

            // 指が動いてるとき
            if (active.phase == TouchPhase.Moved)
            {
                _isMove = true;
                _cursorPosDistance = Camera.main.ScreenToWorldPoint(active.screenPosition) - Camera.main.ScreenToWorldPoint(active.startScreenPosition);
            }
        }
        else
        {
            _isMove = false;
            _isShot = false;
        }

#elif UNITY_STANDALONE_WIN
        if(Input.GetMouseButtonDown(0))
        {
            _startObjectPosition = this.transform.position;
            _startMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            _isMove = true;
            _isShot = true;
            
            _cursorPositionDistance = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(_startMousePos);
        }
        else
        {
            _isMove = false;
            _isShot = false;
            _cursorPositionDistance = Vector2.zero;
        }
#endif
    }

    /// <summary>
    /// プレイヤーが画面外に行くのを防ぐ
    /// </summary>
    private void InStage()
    {
        // プレイヤーが画面内にいるなら処理を飛ばす
        if ((this.transform.position.y <= _maxMovePos.y) && (this.transform.position.y >= _minMovePos.y) &&
            (this.transform.position.x <= _maxMovePos.x) && (this.transform.position.x >= _minMovePos.x))
        {
            return;
        }

        if (this.transform.position.y >= _maxMovePos.y)
        {
            this.transform.position = new Vector2(this.transform.position.x, _maxMovePos.y);
        }

        if (this.transform.position.y <= _minMovePos.y)
        {
            this.transform.position = new Vector2(this.transform.position.x, _minMovePos.y);
        }

        if (this.transform.position.x >= _maxMovePos.x)
        {
            this.transform.position = new Vector2(_maxMovePos.x, this.transform.position.y);
        }

        if (this.transform.position.x <= _minMovePos.x)
        {
            this.transform.position = new Vector2(_minMovePos.x, this.transform.position.y);
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