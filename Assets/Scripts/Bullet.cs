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

    [SerializeField]
    private Vector3 _distanceVector = Vector3.zero;

    private int _bulletNumber = 0;

    private EnemyManager _enemyManager = default;
    private BulletPool _bulletPool = default;
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
        _playerObject = GameObject.FindWithTag("Player");
        _bulletPool = GameObject.FindWithTag("Scripts").GetComponentInChildren<BulletPool>();
        _enemyManager = GameObject.FindWithTag("Scripts").GetComponentInChildren<EnemyManager>();
    }

    public void Initialize()
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

                        if(ExistsEnemyPosList(_distanceVector))
                        {
                            _moveType = MoveType.Line;
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
                        transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, 0.25f);

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
    private bool ExistsEnemyPosList(Vector3 targetPos)
    {
        _playerPos = _playerObject.transform.position;

        if (Calculation.TargetDistance(targetPos, _playerPos) <= Calculation.TargetDistance(this.transform.position, _playerPos))
        {
            return true;
        }

        return false;
    }

    private Vector3 GetNearEnemyDistance()
    {
        int nearEnemyIndex = 0;
        float nearEnemyDistance = Calculation.TargetDistance(_enemyManager.EnemyPhaseList[(int)_enemyManager.NowPhaseState][0].transform.position, this.transform.position);

        for (int i = 1; i < _enemyManager.EnemyPhaseList[(int)_enemyManager.NowPhaseState].Count; i++)
        {
            float enemyDistance = Calculation.TargetDistance(_enemyManager.EnemyPhaseList[(int)_enemyManager.NowPhaseState][i].transform.position, this.transform.position);
            if (enemyDistance < nearEnemyDistance)
            {
                nearEnemyDistance = enemyDistance;
                nearEnemyIndex = i;
            }
        }

        Vector3 nearEnemyVector = _enemyManager.EnemyPhaseList[(int)_enemyManager.NowPhaseState][nearEnemyIndex].transform.position;
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