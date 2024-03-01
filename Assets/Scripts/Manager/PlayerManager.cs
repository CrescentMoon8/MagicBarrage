// ---------------------------------------------------------
// PlayerManager.cs
//
// 作成日:2024/03/01
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PlayerManager : MonoBehaviour
{
	#region 変数
	private Touch _activeTouch = default;

	private bool _isMove = false;
	private bool _isShot = false;
	[SerializeField]
	private bool _isHard = false;

	private PlayerInput _playerInput = default;
	private PlayerMove _playerMove = default;
	private PlayerShot _playerShot = default;
	private PlayerHp _playerHp = default;
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
		EnhancedTouchSupport.Enable();

		_playerInput = new PlayerInput();
		_playerMove = new PlayerMove();
		_playerShot = new PlayerShot();
		_playerHp = new PlayerHp(GetComponent<CircleCollider2D>(), GetComponent<SpriteRenderer>(), GetComponent<Animator>());

		_playerInput.Initialize();
		_playerMove.Initialize(this.transform.localScale);
	}

	/// <summary>
	/// 更新処理
	/// </summary>
	private void Update ()
	{
		_playerShot.AddShotTime();

#if UNITY_IOS || UNITY_EDITOR
		// 触れている指が一本以上あるか
		// Touch型ではNullかどうか検証できないためここで実行
		if (Touch.activeTouches.Count >= 1)
        {
			_activeTouch = _playerInput.InputTouch();
			_isMove = _playerMove.MovePos(_activeTouch, this.transform.position);
		}
		else
        {
			_isMove = false;
			_isShot = false;
        }

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

		_playerHp.AnimationPlayTime();
	}

	private void FixedUpdate()
    {
		if(_isMove)
        {
			_isShot = true;

			this.transform.position = _playerMove.Move();
		}

		if(_isShot)
        {
			_playerShot.Shot(this.transform.position, _isHard);
        }
    }
	#endregion
}