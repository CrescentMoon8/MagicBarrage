// ---------------------------------------------------------
// Bullet.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    #region 変数
    public enum ShooterType
    {
        None,
        Player,
        Enemy
    }

    [SerializeField]
    private ShooterType _shooterType = ShooterType.None;

    public enum MoveType
    {
        Line,
        Tracking,
        Curve
    }

    [SerializeField]
    private MoveType _moveType = MoveType.Line;

    private UnityEngine.GameObject _playerObject;
    private Vector3 _playerPos = Vector3.zero;

    private int _bulletNumber = 0;

    BulletPool _bulletPool = default;
    #endregion

    #region プロパティ
    public ShooterType SettingShooterType { set { _shooterType = value; } }
    public MoveType SettingMoveType {  set { _moveType = value; } }
    public int SettingBulletNumber {  set { _bulletNumber = value; } }
    public Vector3 SettingPlayerPos {  set { _playerPos = value; } }
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
	{
		
	}

	/// <summary>
	/// 更新前処理
	/// </summary>
	private void Start ()
	{
        _playerObject = UnityEngine.GameObject.FindWithTag("Player");
        _bulletPool = UnityEngine.GameObject.FindWithTag("Scripts").GetComponentInChildren<BulletPool>();
	}

	/// <summary>
	/// 更新処理
	/// </summary>
	private void FixedUpdate ()
	{
        switch (_shooterType)
        {
            case ShooterType.Player:
                switch (_moveType)
                {
                    case MoveType.Line:
                        transform.Translate(Vector3.up / 3);
                        break;
                    case MoveType.Tracking:
                        break;
                    case MoveType.Curve:
                        break;
                    default:
                        break;
                }
                
                break;
            case ShooterType.Enemy:
                switch (_moveType)
                {
                    case MoveType.Line:
                        transform.Translate(Vector3.up / 15);
                        break;
                    case MoveType.Tracking:
                        _playerPos = _playerObject.transform.position;
                        // 座標計算、相対的にどれぐらい離れているか
                        Vector3 direction = _playerPos - this.transform.position;
                        // LookRotationではうまく回転できなかった
                        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, direction);
                        //（現在角度、目標方向、どれぐらい曲がるか）
                        transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, 1f);
                        transform.Translate(Vector3.up / 15);
                        break;
                    case MoveType.Curve:
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.CompareTag("Player") && _shooterType == ShooterType.Enemy) ||
           (collision.CompareTag("Enemy") && _shooterType == ShooterType.Player) ||
            collision.CompareTag("ReturnPool"))
		{
            Debug.Log($"{this} {_shooterType}");
			_bulletPool.ReturnBullet(this, _bulletNumber, _shooterType);

            _shooterType = ShooterType.None;
        }
    }
    #endregion
}