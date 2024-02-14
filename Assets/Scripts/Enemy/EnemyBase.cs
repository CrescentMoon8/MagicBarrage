// ---------------------------------------------------------
// EnemyBase.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public abstract class EnemyBase : MonoBehaviour
{
	#region 変数
	private const float HPBAR_POS_Y = 0.44f;
    [SerializeField]
	protected Slider _hpSlider = default;
	protected int _hpValue = 0;

	// 子クラスで作るべき？
	/*protected GameObject _player = default;
	protected Vector3 _playerPos = Vector3.zero;*/

	// Splinesパッケージで代用
	//protected Vector3[,] _movePattern = new Vector3[10,3];

	protected abstract void OnTriggerEnter2D(Collider2D collision);

	protected BulletPool _bulletPool = default;
	protected PuttingEnemyBullet _puttingEnemyBullet = default;

	private delegate void DownEnemyCount();
	private DownEnemyCount _downEnemyCountCallBack = default;

	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
        _bulletPool = GameObject.FindWithTag("Scripts").GetComponentInChildren<BulletPool>();
		_puttingEnemyBullet = new PuttingEnemyBullet(_bulletPool);

		_downEnemyCountCallBack = GameObject.FindWithTag("Scripts").GetComponentInChildren<EnemyManager>().DownEnemyCount;
	}

	/*protected void EnemyMove(int moveNumber)
    {
        //this.transform.position = BezierCalculation(_movePattern[moveNumber, 0], _movePattern[moveNumber, 1], _movePattern[moveNumber, 2], );
    }

    public Vector2 BezierCalculation(Vector2 start, Vector2 relay, Vector2 goal, float time)
    {
        return (Mathf.Pow((1 - time), 2) * start) + (2 * (1 - time) * time * relay) + (Mathf.Pow(time, 2) * goal);
    }*/

	/// <summary>
	/// 敵の上にHPバーを追従させる
	/// </summary>
	/// <param name="enemyPos">追従対象のエネミーの座標</param>
	protected void FollowHpBar(Vector3 enemyPos)
	{
        Vector3 hpBarPos = enemyPos;
		hpBarPos.y += HPBAR_POS_Y;
        _hpSlider.transform.position = hpBarPos;
    }

	protected void EnemyDamage()
	{
		// 初めてダメージを受けたときにHpバーを表示させる
        if (_hpSlider.maxValue == _hpValue)
        {
			_hpSlider.gameObject.SetActive(true);
        }

        if (_hpValue > 0)
		{
			_hpValue -= 1;
			_hpSlider.value = _hpValue;
		}

		if(_hpValue <= 0)
		{
			EnemyDead();
		}
	}

	private void EnemyDead()
	{
		_downEnemyCountCallBack();
		_hpSlider.gameObject.SetActive(false);
		this.gameObject.SetActive(false);
	}
	#endregion
}