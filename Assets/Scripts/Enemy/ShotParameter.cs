// ---------------------------------------------------------
// ShotParameter.cs
//
// 作成日:2024/03/11
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

/// <summary>
/// Shotに必要なパラメータを受け渡しするためのクラス
/// </summary>
public class ShotParameter
{
    #region 変数
	// 射手の位置
    private Vector3 _shooterPos = default;
	// 撃ちたい角度
	private float _centerAngle = default;
	// 角度を何分割するか
	private int _angleSplit = default;
    // 撃ちたい角度の±いくらか
    private int _angleWidth = default;
	// 撃つ弾の番号
	private int _bulletNumber = default;
	// 弾の動き方
	private Bullet.MoveType _moveType = default;
	// 弾の速度変化
	private Bullet.SpeedType _speedType = default;
    #endregion

    #region コンストラクタ
    /// <summary>
    /// LineShot用コンストラクタ
    /// </summary>
    /// <param name="shooterPos">射手の座標</param>
    /// <param name="centerAngle">撃ちたい角度</param>
    /// <param name="bulletNumber">弾の種類</param>
    /// <param name="moveType">弾の軌道</param>
    public ShotParameter(Vector3 shooterPos, float centerAngle, int bulletNumber, Bullet.MoveType moveType, Bullet.SpeedType speedType)
	{
		_shooterPos = shooterPos;
		_centerAngle = centerAngle;
		_bulletNumber = bulletNumber;
		_moveType = moveType;
		_speedType = speedType;
	}

	/// <summary>
	/// FanShot用コンストラクタ
	/// </summary>
	/// <param name="shooterPos">射手の座標</param>
	/// <param name="centerAngle">撃ちたい角度</param>
	/// <param name="angleSplit">角度を何分割するか</param>
	/// <param name="angleWidth">撃ちたい角度からの角度幅</param>
	/// <param name="bulletNumber">弾の種類</param>
	/// <param name="moveType">弾の軌道</param>
	public ShotParameter(Vector3 shooterPos, float centerAngle, int angleSplit, int angleWidth, int bulletNumber, Bullet.MoveType moveType, Bullet.SpeedType speedType)
    {
		_shooterPos = shooterPos;
		_centerAngle = centerAngle;
		_angleSplit = angleSplit;
		_angleWidth = angleWidth;
		_bulletNumber = bulletNumber;
		_moveType = moveType;
		_speedType = speedType;
	}

	/// <summary>
	/// RoundShot用コンストラクタ
	/// </summary>
	/// <param name="shooterPos">射手の座標</param>
	/// <param name="centerAngle">撃ちたい角度</param>
	/// <param name="angleSplit">角度を何分割するか</param>
	/// <param name="bulletNumber">弾の種類</param>
	/// <param name="moveType">弾の軌道</param>
	public ShotParameter(Vector3 shooterPos, float centerAngle, int angleSplit, int bulletNumber, Bullet.MoveType moveType, Bullet.SpeedType speedType)
	{
		_shooterPos = shooterPos;
		_centerAngle = centerAngle;
		_angleSplit = angleSplit;
		_bulletNumber = bulletNumber;
		_moveType = moveType;
		_speedType = speedType;
	}
    #endregion

    #region プロパティ
    public Vector3 ShooterPos { get { return _shooterPos; } }
	public float CenterAngle { get { return _centerAngle; } }
	public int AngleSplit { get { return _angleSplit; } }
	public int AngleWidth { get { return _angleWidth; } }
	public int BulletNumber { get { return _bulletNumber; } }
	public Bullet.MoveType MoveType { get { return _moveType; } }
	public Bullet.SpeedType SpeedType { get { return _speedType; } }
    #endregion
}