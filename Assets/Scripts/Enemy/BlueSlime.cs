// ---------------------------------------------------------
// BlueSlime.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BlueSlime : EnemyHp
{
	#region 変数
	// 撃ちたい角度
	private float _targetAngle = 0;
	// 撃ちたい角度の±いくらか
	private int _angleWidth = 0;
	// 角度を何分割するか
	private int _angleSplit = 0;

    // 全体の弾を撃つ間隔
    private const float SHOT_INTERVAL = 2f;

	private EnemyBulletPut _enemyBulletPut = default;
	private EnemyMove _enemyMove = default;
	private BulletInfo _bulletInfo = default;
	private EnemyDataBase _enemyDataBase = default;
	/*[SerializeField]
	private BarrageTemplate _barrageTemplate = default;*/
	#endregion

	#region メソッド
	/// <summary>
	/// 更新前処理
	/// </summary>
	private void OnEnable ()
	{
        // 移動と攻撃のためのスクリプトを生成する
        _enemyBulletPut = new EnemyBulletPut(this.transform.localScale.x / 2);
		_enemyMove = new EnemyMove();

        // 弾の情報を敵のデータベースをロードする
        _bulletInfo = Addressables.LoadAssetAsync<BulletInfo>("BulletInfo").WaitForCompletion();
		_enemyDataBase = Addressables.LoadAssetAsync<EnemyDataBase>("EnemyDataBase").WaitForCompletion();

        // 敵のデータベースから必要な情報を取得する
        EnemyData enemyData = _enemyDataBase._enemyDataList[_enemyDataBase.BLUE_SLIME];
		base._enemyNumber = _enemyDataBase.BLUE_SLIME;

        // 不要になった敵のデータベースをアンロードする
        Addressables.Release(_enemyDataBase);

        // HPの設定を行う
        base._hpValue = enemyData._maxHp;
		base._hpSlider.maxValue = base._hpValue;
		base._hpSlider.value = base._hpValue;

        // 倒されたときに獲得できるスコアの設定
        base._killPoint = EnemyHp.NOMAL_KILL_POINT;

        /*_targetAngle = _barrageTemplate.TargetAngle;
        _angleWidth = _barrageTemplate.AngleWidth;
        _angleSplit = _barrageTemplate.AngleSplit;*/

        // 敵の移動経路の設定
        _enemyMove.SetSplineContainer(enemyData._splineIndex);
		_enemyMove.DifferencePosInitialize(this.transform.position);
	}

    /// <summary>
    /// 非アクティブになったときに弾の情報をアンロードする
    /// </summary>
    private void OnDisable()
	{
		Addressables.Release(_bulletInfo);
	}

	/// <summary>
	/// 更新処理
	/// </summary>
	private void Update ()
	{
		if (base._isDead)
		{
			return;
		}

		this.transform.position = _enemyMove.NextMovePos();

		base.FollowHpBar(this.transform.position);

		// 画面内にいて、弾を撃つ間隔が過ぎていれば
		if (_isInsideCamera && _enemyBulletPut.IsShot(SHOT_INTERVAL))
		{
			// 発射に必要なパラメータを生成する
			ShotParameter fanShotParameter = new ShotParameter(this.transform.position, _targetAngle, _angleSplit, _angleWidth, _bulletInfo.BLUE_NOMAL_BULLET, Bullet.MoveType.Line, Bullet.SpeedType.Middle);
			_enemyBulletPut.FanShot(fanShotParameter);

			_enemyBulletPut.ResetShotTime();
		}
	}
    #endregion
}