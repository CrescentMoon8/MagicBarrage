// ---------------------------------------------------------
// Player.cs
//
// 作成日:2024/02/05
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

/// <summary>
/// プレイヤーのすべてを担うクラス（旧）
/// </summary>
public class Player : MonoBehaviour, IDamageable, IPlayerPos
{
    #region 変数
    private Vector3 _startMousePos = Vector3.zero;
    private Vector2 _startObjectPos = Vector2.zero;
    private Vector2 _cursorPosDistance = Vector2.zero;
    [SerializeField]
    private bool _isMove = false;

    [SerializeField]
    private bool _isShot = false;
    [SerializeField]
    private float _shotTime = 0.1f;
    private const float SHOT_INTERVAL = 0.1f;
    private const float SHOT_POS_DIFFERENCE_Y = 0.7f;
    private const float SHOT_POS_DIFFERENCE_X = 0.2f;
    private const int LINE_BULLET_AMOUNT = 1;
    private const int TRACKING_BULLET_AMOUNT = 2;

    private Vector2 _maxMoveLimitPos = Vector2.zero;
    private Vector2 _minMoveLimitPos = Vector2.zero;

    [SerializeField]
    private RectTransform _header = default;
    [SerializeField]
    private RectTransform _footer = default;

    [SerializeField]
    private ParticleSystem _touchParticle = default;

    private bool _isDamage = false;
    private int _lifeCount = 3;
    [SerializeField]
    private float _damageAnimationTime = 0;
    private const float DAMAGE_ANIMATION = 3f;
    [SerializeField]
    private Image[] _lifeUi = new Image[3];
    [SerializeField]
    private Sprite _brokenHeart = default;

    [SerializeField]
    private AudioClip _damageSe = default;
    [SerializeField]
    private AudioClip _deadSe = default;

    private SpriteRenderer _spriteRenderer = default;
    private CircleCollider2D _circleCollider2D = default;
    private Animator _playerAnimator = default;
    private AudioSource _audioSource = default;
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
        _audioSource = GetComponent<AudioSource>();
        _playerAnimator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider2D = GetComponent<CircleCollider2D>();


        Vector3 radius = Vector3.zero;
        
        // プレイヤースプライトの半径を取得する
        radius = transform.localScale;

        int MAX_MOVE_POS_INDEX = 3;
        int MIN_MOVE_POS_INDEX = 1;

        // UIオブジェクトの頂点の座標を取得し、移動制限の大きさを調整する
        // 座標の取得順は左下、左上、右上、右下
        Vector3[] headerCorners = new Vector3[4];
        _header.GetWorldCorners(headerCorners);
        _maxMoveLimitPos = headerCorners[MAX_MOVE_POS_INDEX] - radius;

        Vector3[] footerCorners = new Vector3[4];
        _footer.GetWorldCorners(footerCorners);
        _minMoveLimitPos = footerCorners[MIN_MOVE_POS_INDEX] + radius;

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
            PlayerShot(false);
        }
    }

    private void PlayerShot(bool isHard)
    {
        if (_shotTime >= SHOT_INTERVAL)
        {
            Vector3 shotPos = this.transform.position;
            shotPos.y += SHOT_POS_DIFFERENCE_Y;

            for (int bulletCount = 1; bulletCount <= LINE_BULLET_AMOUNT; bulletCount++)
            {
                PlayerBullet bullet = PlayerBulletPool.Instance.LendBullet(shotPos);

                bullet.SettingMoveType = Bullet.MoveType.Line;

                bullet.Initialize();
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

                if(isHard)
                {
                    PlayerBullet bullet = PlayerBulletPool.Instance.LendBullet(shotPos);

                    bullet.SettingMoveType = Bullet.MoveType.Line;

                    bullet.Initialize();
                }
                else
                {
                    PlayerBullet bullet = PlayerBulletPool.Instance.LendBullet(shotPos);

                    bullet.SettingMoveType = Bullet.MoveType.Tracking;

                    bullet.Initialize();
                }
                
            }

            _shotTime = 0;
        }
    }

    private void PlayerInput()
	{
#if UNITY_IOS || UNITY_EDITOR
        // 指が一本以上触れているか
        if (Touch.activeTouches.Count >= 1)
        {
            // タッチの入力を取得する
            Touch active = Touch.activeTouches[0];
            Vector3 particlePos = Vector3.zero;

            _isShot = true;

            // 指がふれたとき
            if (active.phase == TouchPhase.Began)
            {
                _startObjectPos = this.transform.position;

                _touchParticle.gameObject.SetActive(true);
                _touchParticle.Play();

                particlePos = Camera.main.ScreenToWorldPoint(active.startScreenPosition);
                // カメラのZ座標が入るため０にする
                particlePos.z = 0;
                _touchParticle.gameObject.transform.position = particlePos;
            }

            // 指が動いてるとき
            if (active.phase == TouchPhase.Moved)
            {
                _isMove = true;
                _cursorPosDistance = Camera.main.ScreenToWorldPoint(active.screenPosition) - Camera.main.ScreenToWorldPoint(active.startScreenPosition);

                particlePos = Camera.main.ScreenToWorldPoint(active.screenPosition);
                // カメラのZ座標が入るため０にする
                particlePos.z = 0;
                _touchParticle.gameObject.transform.position = particlePos;
            }
        }
        else
        {
            _isMove = false;
            _isShot = false;

            _touchParticle.gameObject.SetActive(false);
        }

#elif UNITY_STANDALONE_WIN
        if(Input.GetMouseButtonDown(0))
        {
            _startObjectPos = this.transform.position;
            _startMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            _isMove = true;
            _isShot = true;
            
            _cursorPosDistance = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(_startMousePos);
        }
        else
        {
            _isMove = false;
            _isShot = false;
            _cursorPosDistance = Vector2.zero;
        }
#endif
    }

    /// <summary>
    /// プレイヤーが画面外に行くのを防ぐ
    /// </summary>
    private void InStage()
    {
        // プレイヤーが画面内にいるなら処理を飛ばす
        if ((this.transform.position.y <= _maxMoveLimitPos.y) && (this.transform.position.y >= _minMoveLimitPos.y) &&
            (this.transform.position.x <= _maxMoveLimitPos.x) && (this.transform.position.x >= _minMoveLimitPos.x))
        {
            return;
        }

        if (this.transform.position.y >= _maxMoveLimitPos.y)
        {
            this.transform.position = new Vector2(this.transform.position.x, _maxMoveLimitPos.y);
        }

        if (this.transform.position.y <= _minMoveLimitPos.y)
        {
            this.transform.position = new Vector2(this.transform.position.x, _minMoveLimitPos.y);
        }

        if (this.transform.position.x >= _maxMoveLimitPos.x)
        {
            this.transform.position = new Vector2(_maxMoveLimitPos.x, this.transform.position.y);
        }

        if (this.transform.position.x <= _minMoveLimitPos.x)
        {
            this.transform.position = new Vector2(_minMoveLimitPos.x, this.transform.position.y);
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            _circleCollider2D.enabled = false;
            Damage();
        }
    }*/

    public void Damage()
    {
        /* 
         * コライダーに複数のオブジェクトが同時に接触すると複数回呼び出されるため、
         * コライダーがOFFならば二回目以降の処理をスキップする
         */
        if(!_circleCollider2D.enabled)
        {
            return;
        }

        _circleCollider2D.enabled = false;
        _spriteRenderer.enabled = false;
        _playerAnimator.SetTrigger("IsDamage");

        _isDamage = true;

        _lifeCount--;
        _lifeUi[_lifeCount].sprite = _brokenHeart;

        if (_lifeCount <= 0)
        {
            _audioSource.PlayOneShot(_deadSe);
            SceneManager.LoadScene("GameOver");
        }

        _audioSource.PlayOneShot(_damageSe);
    }
    #endregion
}