// ---------------------------------------------------------
// Bullet.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

/// <summary>
/// 弾の移動処理、回転処理、プレイヤー・エネミーのダメージ処理の呼び出しを行う
/// </summary>
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

    /// <summary>
    /// 弾の速度変化
    /// ０：なし
    /// １：加速
    /// ２：減速
    /// </summary>
    public enum SpeedChangeType
    {
        None,
        Acceleration,
        Deceleration
    }

    [SerializeField]
    private SpeedChangeType _speedChangeType = SpeedChangeType.None;

    private GameObject _playerObject = default;
    private IPlayerPos _iPlayerPos = default;
    // プレイヤーが持つダメージ用インターフェース
    private IDamageable _playerIDamageable = default;
    // プレイヤーの弾の速度を調整する（高くすれば遅く、低くすれば早くなる）
    private float _playerBulletSpeedDevisor = 3f;

    // 弾の速度
    private const float LOW_SPEED = 30f;
    private const float MIDDLE_SPEED = 15f;
    private const float HIGH_SPEED = 10f;
    // 弾の加速率
    private const float ACCELERATION_RATE = 0.1f;
    //弾の減速率
    private const float DECELERATION_RATE = 0.1f;
    // エネミーの弾の速度を調整する（高くすれば遅く、低くすれば早くなる）
    private float _enemyBulletSpeedDevisor = 15f;

    // エネミーとプレイヤーのベクトル差分
    [SerializeField]
    private Vector3 _distanceVector = Vector3.zero;

    // どの敵からどの弾が撃たれたかの判別用番号
    private int _bulletNumber = 0;

    private ParticlePool _particlePool = default;
    private IEnemyList _iEnemyList = default;
    #endregion

    #region プロパティ
    public ShooterType SettingShooterType { set { _shooterType = value; } }
    public MoveType SettingMoveType {  set { _moveType = value; } }
    public SpeedChangeType SettingSpeedChangeType { set { _speedChangeType = value; } }
    public int SettingBulletNumber {  set { _bulletNumber = value; } }
    #endregion

    #region メソッド
    /// <summary>
    /// 変数の初期化処理
    /// </summary>
    private void Start()
	{
        _playerObject = GameObject.FindWithTag("Player");
        _iPlayerPos = _playerObject.GetComponent<IPlayerPos>();
        _playerIDamageable = _playerObject.GetComponent<PlayerManager>().GettingPlayerHp;
        _particlePool = GameObject.FindWithTag("Scripts").GetComponentInChildren<ParticlePool>();
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
                        GetNearEnemyPos();
                        break;
                    case MoveType.Curve:
                        break;
                    default:
                        break;
                }
                break;

            case ShooterType.Enemy:
                switch (_speedChangeType)
                {
                    case SpeedChangeType.None:
                        _enemyBulletSpeedDevisor = MIDDLE_SPEED;
                        break;

                    case SpeedChangeType.Acceleration:
                        _enemyBulletSpeedDevisor = LOW_SPEED;
                        break;

                    case SpeedChangeType.Deceleration:
                        _enemyBulletSpeedDevisor = HIGH_SPEED;
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
                        transform.Translate(Vector3.up / _playerBulletSpeedDevisor);
                        break;

                    case MoveType.Tracking:
                        //Debug.LogWarning("追尾");
                        int angle = Calculation.TargetDirectionAngle(_distanceVector, this.transform.position);

                        Quaternion targetRotation = Quaternion.Euler(Vector3.forward * angle);
                        transform.rotation = targetRotation;

                        transform.Translate(Vector3.up / _playerBulletSpeedDevisor);

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
                        SpeedChange();
                        transform.Translate(Vector3.up / _enemyBulletSpeedDevisor);
                        break;
                    case MoveType.Tracking:
                        //SpeedChange();
                        float direction = Calculation.TargetDirectionAngle(_iPlayerPos.PlayerPos, this.transform.position);
                        Quaternion targetRotation = Quaternion.Euler(Vector3.forward * direction);
                        //（現在角度、目標方向、どれぐらい曲がるか）
                        transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, 0.5f);

                        transform.Translate(Vector3.up / _enemyBulletSpeedDevisor);
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

    private void SpeedChange()
    {
        switch (_speedChangeType)
        {
            case SpeedChangeType.None:
                break;

            case SpeedChangeType.Acceleration:
                // 速度上限になるまで加速する
                if(_enemyBulletSpeedDevisor >= HIGH_SPEED)
                {
                    _enemyBulletSpeedDevisor -= ACCELERATION_RATE;
                }
                break;

            case SpeedChangeType.Deceleration:
                // 速度下限になるまで減速する
                if (_enemyBulletSpeedDevisor <= LOW_SPEED)
                {
                    _enemyBulletSpeedDevisor += DECELERATION_RATE;
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// プレイヤーと弾の距離、プレイヤーとエネミーの距離を比べる
    /// </summary>
    /// <param name="targetPos">追尾対象の座標</param>
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
    private void GetNearEnemyPos()
    {
        // 一番近いエネミーの番号
        int nearEnemyIndex = 0;
        // 初期値を一番目のエネミーとの距離にすることで、初期値ゼロより処理を一回減らす
        float nearEnemyDistance = Calculation.TargetDistance(_iEnemyList.CurrentPhaseEnemyList[0].transform.position, _iPlayerPos.PlayerPos);

        for (int i = 1; i < _iEnemyList.CurrentPhaseEnemyList.Count; i++)
        {
            float enemyDistance = Calculation.TargetDistance(_iEnemyList.CurrentPhaseEnemyList[i].transform.position, _iPlayerPos.PlayerPos);

            // エネミーの距離の比較
            if (enemyDistance < nearEnemyDistance)
            {
                // 一番近いエネミーの距離の更新
                nearEnemyDistance = enemyDistance;
                nearEnemyIndex = i;
            }
        }

        // 一番近いエネミーの座標を代入する
        _distanceVector = _iEnemyList.CurrentPhaseEnemyList[nearEnemyIndex].transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("ReturnPool"))
        {
            // 誰が撃った弾か判断する
            switch (_shooterType)
            {
                case ShooterType.Player:
                    PlayerBulletPool.Instance.ReturnBullet(this);
                    break;

                case ShooterType.Enemy:
                    EnemyBulletPool.Instance.ReturnBullet(this, _bulletNumber);
                    break;

                default:
                    break;
            }

            // 各状態を初期化する
            _shooterType = ShooterType.None;
            _moveType = MoveType.Line;
            _speedChangeType = SpeedChangeType.None;
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
                    _iEnemyList.CurrentPhaseIDamageableList[collision.transform.GetSiblingIndex()].Damage();
                    BulletParticle playerParticle = _particlePool.LendPlayerParticle(this.transform.position);
                    playerParticle.Play();
                    PlayerBulletPool.Instance.ReturnBullet(this);
                    break;

                case ShooterType.Enemy:
                    _playerIDamageable.Damage();
                    BulletParticle enemyParticle = _particlePool.LendEnemyParicle(this.transform.position, _bulletNumber);
                    enemyParticle.Play();
                    EnemyBulletPool.Instance.ReturnBullet(this, _bulletNumber);
                    break;

                default:
                    break;
            }

            // 各状態を初期化する
            _shooterType = ShooterType.None;
            _moveType = MoveType.Line;
            _speedChangeType = SpeedChangeType.None;

            _distanceVector = Vector3.zero;
            this.transform.rotation = Quaternion.identity;
        }
    }
    #endregion
}