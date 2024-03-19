// ---------------------------------------------------------
// PlayerManager.cs
//
// 作成日:2024/03/01
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>, IPlayerPos
{
	#region 変数
	// 入力を情報を保存するための変数
	private Touch _activeTouch = default;

	private bool _isMove = false;
	private bool _isShot = false;
	[SerializeField]
	private bool _isHard = false;

	// プレイヤーの残機UIの親オブジェクト
	[SerializeField]
	private Transform _lifeUiParent = default;
	// プレイヤーの残機減少後のSprite
	[SerializeField]
    private Sprite _brokenHeart = default;

	private bool _isDead = false;

    private PlayerInput _playerInput = default;
	private PlayerMove _playerMove = default;
	private PlayerShot _playerShot = default;
	private PlayerHp _playerHp = default;

#if UNITY_STANDALONE_WIN
	private Vector3 _startMousePos = Vector3.zero;
	private Vector2 _startObjectPos = Vector2.zero;
	private Vector2 _cursorPosDistance = Vector2.zero;
#endif
	#endregion

	#region プロパティ
	public Vector3 PlayerPos { get {  return this.transform.position; } }
	public PlayerHp GettingPlayerHp { get { return _playerHp; } }
	public bool IsDead { get { return _isDead; } }
	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	public void PlayerAwake()
	{
		base.Awake();

		// Touch型を使用するための機能を有効化
		EnhancedTouchSupport.Enable();

		FieldSize.Instance.SetFieldSize();

		_playerInput = new PlayerInput();
		_playerMove = new PlayerMove();
		_playerShot = new PlayerShot();
		_playerHp = new PlayerHp(GetComponent<CircleCollider2D>(),
								GetComponent<SpriteRenderer>(),
								GetComponent<Animator>(),
								PlayerParticlePool.Instance,
								this);

		_playerInput.Initialize();
		_playerMove.Initialize(this.transform.localScale);
		_playerHp.Initialize(_lifeUiParent, _brokenHeart);
	}

    /// <summary>
    /// 更新処理
    /// </summary>
    public void PlayerUpdate ()
	{
		if (_playerHp.IsDead)
		{
			if(!_playerHp.ParticleIsPlaying())
            {
				_isDead = true;
            }

			return;
		}
		else
		{
			_playerHp.AnimationPlayTime();
		}

		_playerShot.AddShotTime();

	#if UNITY_IOS || UNITY_EDITOR
		// 触れている指が一本以上あるか
		// Touch型ではNullかどうか検証できないためここで実行（改善策を要検討）
		if (Touch.activeTouches.Count >= 1)
        {
			_activeTouch = _playerInput.InputTouch();
			_playerMove.MovePos(_activeTouch, this.transform.position);
			_isMove = true;
			_isShot = true;
		}
		else
        {
			_playerMove.ResetCursorPosDistance();
			_isMove = false;
			_isShot = false;
        }

		// PCビルド用にここで移動座標計算を行う
	#elif UNITY_STANDALONE_WIN
		if (Input.GetMouseButtonDown(0))
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

	private void FixedUpdate()
    {
		if(_isMove)
        {
	#if UNITY_IOS || UNITY_EDITOR
			this.transform.position = _playerMove.Move();
	#elif UNITY_STANDALONE_WIN
			this.transform.position = _playerMove.Move(_startObjectPos, _cursorPosDistance);
	#endif
		}

		if (_isShot)
        {
			_playerShot.Shot(this.transform.position, _isHard);
        }
    }
	#endregion
}