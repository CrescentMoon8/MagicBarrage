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
    /// <summary>
    /// 射手は誰か
    /// ０：撃たれていない
    /// １：プレイヤー
    /// ２：エネミー
    /// </summary>
    public enum ShooterType
    {
        None,
        Player,
        Enemy
    }

    [SerializeField]
    private ShooterType _shooterType = ShooterType.None;

    /// <summary>
    /// 弾の動き方
    /// ０：直線
    /// １：追尾
    /// ２：曲線
    /// </summary>
    public enum MoveType
    {
        Line,
        Tracking,
        Curve
    }

    [SerializeField]
    private MoveType _moveType = MoveType.Line;

    private GameObject _playerObject = default;
    private IPlayerPos _iPlayerPos = default;
    // プレイヤーが持つダメージ用インターフェース
    private IDamageable _playerIDamageable = default;

    // エネミーとプレイヤーのベクトル差分
    [SerializeField]
    private Vector3 _distanceVector = Vector3.zero;

    private int _bulletNumber = 0;

    private IObjectPool<Bullet> _bulletPool = default;
    private IObjectPool<BulletParticle> _particlePool = default;
    private IEnemyList _iEnemyList = default;
    #endregion

    #region プロパティ
    public ShooterType SettingShooterType { set { _shooterType = value; } }
    public MoveType SettingMoveType {  set { _moveType = value; } }
    public int SettingBulletNumber {  set { _bulletNumber = value; } }
    #endregion

    #region メソッド
    /// <summary>
    /// 変数の初期化処理
    /// </summary>
    private void Awake()
	{
        _playerObject = GameObject.FindWithTag("Player");
        _iPlayerPos = _playerObject.GetComponent<IPlayerPos>();
        _playerIDamageable = _playerObject.GetComponent<IDamageable>();
        _bulletPool = GameObject.FindWithTag("Scripts").GetComponentInChildren<IObjectPool<Bullet>>();
        _particlePool = GameObject.FindWithTag("Scripts").GetComponentInChildren<IObjectPool<BulletParticle>>();
        _iEnemyList = GameObject.FindWithTag("Scripts").GetComponentInChildren<IEnemyList>();
    }

    /// <summary>
    /// 弾の初期化処理
    /// </summary>
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
                        _distanceVector = GetNearEnemyPos();
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
    /// 物理挙動の更新処理
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
                        float direction = Calculation.TargetDirectionAngle(_iPlayerPos.PlayerPos, this.transform.position);
                        Quaternion targetRotation = Quaternion.Euler(Vector3.forward * direction);
                        //（現在角度、目標方向、どれぐらい曲がるか）
                        transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, 0.5f);

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

    /// <summary>
    /// プレイヤーと弾の距離、プレイヤーとエネミーの距離を比べる
    /// </summary>
    /// <param name="targetPos">追尾対象の</param>
    /// <returns></returns>
    private bool ExistsEnemyPosList(Vector3 targetPos)
    {
        if (Calculation.TargetDistance(targetPos, _iPlayerPos.PlayerPos) <= Calculation.TargetDistance(this.transform.position, _iPlayerPos.PlayerPos))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 一番近いエネミーの座標を取得する
    /// </summary>
    /// <returns></returns>
    private Vector3 GetNearEnemyPos()
    {
        int nearEnemyIndex = 0;
        float nearEnemyDistance = Calculation.TargetDistance(_iEnemyList.EnemyPhaseList[(int)_iEnemyList.NowPhaseState][0].transform.position, this.transform.position);


        for (int i = 1; i < _iEnemyList.EnemyPhaseList[(int)_iEnemyList.NowPhaseState].Count; i++)
        {
            float enemyDistance = Calculation.TargetDistance(_iEnemyList.EnemyPhaseList[(int)_iEnemyList.NowPhaseState][i].transform.position, this.transform.position);
            if (enemyDistance < nearEnemyDistance)
            {
                nearEnemyDistance = enemyDistance;
                nearEnemyIndex = i;
            }
        }

        Vector3 nearEnemyVector = _iEnemyList.EnemyPhaseList[(int)_iEnemyList.NowPhaseState][nearEnemyIndex].transform.position;
        return nearEnemyVector;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("ReturnPool"))
        {
            _bulletPool.ReturnPool(this, _bulletNumber);

            _shooterType = ShooterType.None;
        }

        if((collision.CompareTag("Player") && _shooterType == ShooterType.Enemy) ||
           (collision.CompareTag("Enemy") && _shooterType == ShooterType.Player) || 
           (collision.CompareTag("Boss") && _shooterType == ShooterType.Player))
		{
            // 誰が撃った弾か判断する
            switch (_shooterType)
            {
                case ShooterType.Player:
                    // GetSiblingIndexで当たったオブジェクトが同じ階層で上から何番目かを取得する
                    // Enemyの情報をヒエラルキーの上から順に取得しているためちゃんと動いている
                    _iEnemyList.EnemyIDamageableList[(int)_iEnemyList.NowPhaseState][collision.transform.GetSiblingIndex()].Damage();
                    BulletParticle playerParticle = _particlePool.LendPlayer(this.transform.position, -1);
                    playerParticle.Play();
                    break;

                case ShooterType.Enemy:
                    _playerIDamageable.Damage();
                    BulletParticle enemyParticle = _particlePool.LendEnemy(this.transform.position, _bulletNumber);
                    enemyParticle.Play();
                    break;

                default:
                    break;
            }
            
            _bulletPool.ReturnPool(this, _bulletNumber);

            _shooterType = ShooterType.None;
        }
    }
    #endregion
}