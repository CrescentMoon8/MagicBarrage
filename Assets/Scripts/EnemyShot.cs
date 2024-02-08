// ---------------------------------------------------------
// EnemyShot.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

public class EnemyShot : EnemyBase
{
    #region 変数
    // 指定されたスケールの半径
    protected float _radius = 0f;

	protected GameObject _player = default;
	protected Vector3 _playerPos = Vector3.zero;

	protected BulletPool _bulletPool = default;
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
		_radius = this.transform.localScale.x / 2;
    }

	/// <summary>
	/// 弾を指定された方向に直線で撃ち出す
	/// </summary>
    protected void LineShot(Vector3 shooterPos, Vector3 direction)
    {
        
    }


    /// <summary>
    /// 弾を円形に撃ち出す
    /// 撃つ範囲を引数で設定する
    /// </summary>
    /// <param name="shooterPos">射手の座標</param>
    /// <param name="maxAngle">中心角の大きさ</param>
    /// <param name="angleSplit">maxAngleを何分割するか</param>
    /// <param name="direction">中心角をどのくらい回転させるか</param>
    /// <param name="bulletNumber">弾の種類</param>
    /// <param name="moveType">弾の軌道</param>
    protected void RoundShot(Vector3 shooterPos, int maxAngle, int angleSplit, int direction, int bulletNumber, Bullet.MoveType moveType)
	{
        for (int i = 0; i < angleSplit; i++)
        {
			// 0の位置がUnity上の-90にあたるため、
			// 例えば弾の進行方向を下向きにするためにdirectionに180を入れたら最初の位置を下にするのにdirectionの半分の90を使用する
            Vector3 bulletPos = CirclePosCalculate(shooterPos, (maxAngle / angleSplit) * i - direction / 2);

			Bullet bullet = _bulletPool.LendEnemyBullet(bulletPos, bulletNumber);

			bullet.SettingMoveType = moveType;

			bullet.transform.rotation = Quaternion.Euler(Vector3.forward * ((maxAngle / angleSplit) * i - direction));
        }
    }

	/// <summary>
	/// エネミーからプレイヤーへの角度を計算する
	/// </summary>
	/// <returns></returns>
	/*protected Vector3 AngleFromEnemyCalculate()
	{
		Vector3 playerDirection 
	}*/

	/// <summary>
	/// 与えられた角度を使って、半径radiusの円の円周上の座標を返す
	/// </summary>
	/// <param name="ShooterPos">射手の座標</param>
	/// <param name="angle">中心角</param>
	/// <returns>円周上の座標</returns>
	protected Vector3 CirclePosCalculate(Vector3 ShooterPos, float angle)
	{
		Vector3 circlePos = Vector3.zero;

		circlePos.x = Mathf.Cos(angle * Mathf.Deg2Rad) * _radius;
		circlePos.y = Mathf.Sin(angle * Mathf.Deg2Rad) * _radius;

		// ラジアンに変換しなかったら、よくわからん挙動した（参考：スクリーンショットフォルダ）
        /*circlePosition.x = Mathf.Cos(angle) * radius;
        circlePosition.y = Mathf.Sin(angle) * radius;*/
        return circlePos;
	}
    #endregion
}