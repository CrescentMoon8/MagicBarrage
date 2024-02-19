// ---------------------------------------------------------
// Bullet.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;

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

    private GameObject _playerObject;
    private Vector3 _playerPos = Vector3.zero;

    private Vector3 _distanceVector = Vector3.zero;
    private const float BULLET_ALIVE_TIME = 1f;
    private float _aliveTime = 0f;

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
        _playerObject = GameObject.FindWithTag("Player");
        _bulletPool = GameObject.FindWithTag("Scripts").GetComponentInChildren<BulletPool>();
	}

    private void OnEnable()
    {
        switch (_shooterType)
        {
            case ShooterType.Player:
                switch (_moveType)
                {
                    case MoveType.Line:
                        this.transform.rotation = Quaternion.identity;
                        break;
                    case MoveType.Tracking:
                        _distanceVector = GetNearEnemyDistance();
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
                        //Debug.LogWarning("追尾");
                        int angle = Calculation.TargetDirectionAngle(_distanceVector, this.transform.position);

                        Quaternion targetRotation = Quaternion.Euler(Vector3.forward * angle);
                        transform.rotation = targetRotation;

                        transform.Translate(Vector3.up / 3);

                        _aliveTime += Time.deltaTime;

                        if(_aliveTime > BULLET_ALIVE_TIME)
                        {
                            _bulletPool.ReturnBullet(this, _bulletNumber, _shooterType);

                            _shooterType = ShooterType.None;

                            _aliveTime = 0;
                        }
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

                        float direction = Calculation.TargetDirectionAngle(_playerPos, this.transform.position);
                        Quaternion targetRotation = Quaternion.Euler(Vector3.forward * direction);
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

    private Vector3 GetNearEnemyDistance()
    {
        int nearEnemyIndex = 0;
        float nearEnemyDistance = Calculation.TargetDistance(CurrentPhaseEnemyPosition._currentEnemyPos[0], this.transform.position);

        for (int i = 1; i < CurrentPhaseEnemyPosition._currentEnemyPos.Count; i++)
        {
            if (Calculation.TargetDistance(CurrentPhaseEnemyPosition._currentEnemyPos[i], this.transform.position) < nearEnemyDistance)
            {
                nearEnemyDistance = Calculation.TargetDistance(CurrentPhaseEnemyPosition._currentEnemyPos[i], this.transform.position);
                nearEnemyIndex = i;
            }
        }

        Vector3 nearEnemyVector = CurrentPhaseEnemyPosition._currentEnemyPos[nearEnemyIndex];
        return nearEnemyVector;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.CompareTag("Player") && _shooterType == ShooterType.Enemy) ||
           (collision.CompareTag("Enemy") && _shooterType == ShooterType.Player) ||
           (collision.CompareTag("Boss") && _shooterType == ShooterType.Player) ||
            collision.CompareTag("ReturnPool"))
		{
			_bulletPool.ReturnBullet(this, _bulletNumber, _shooterType);

            _shooterType = ShooterType.None;
        }
    }
    #endregion
}