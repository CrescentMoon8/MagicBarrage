// ---------------------------------------------------------
// Bullet.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

/// <summary>
/// 弾の移動処理、回転処理、プレイヤー・敵のダメージ処理の呼び出しを行う
/// </summary>
public abstract class Bullet : MonoBehaviour
{
    #region 変数
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
    protected MoveType _moveType = MoveType.Line;

    /// <summary>
    /// 弾の速度変化
    /// ０：なし
    /// １：加速
    /// ２：減速
    /// </summary>
    public enum SpeedType
    {
        Low,
        Middle,
        High,
        Acceleration,
        Deceleration
    }

    [SerializeField]
    protected SpeedType _speedType = SpeedType.Middle;

    // プレイヤーの位置参照用インターフェース
    protected IPlayerPos _iPlayerPos = default;
    #endregion

    #region プロパティ
    public MoveType SettingMoveType {  set { _moveType = value; } }
    public SpeedType SettingSpeedType { set { _speedType = value; } }
    #endregion

    #region メソッド
    /// <summary>
    /// プレイヤーの位置参照用インターフェースを取得する
    /// </summary>
    protected void GetIPlayerPos()
    {
        _iPlayerPos = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
    }

    /// <summary>
    /// 弾の初期化処理
    /// </summary>
    public abstract void Initialize();

    protected abstract void OnTriggerEnter2D(Collider2D collision);
    #endregion
}