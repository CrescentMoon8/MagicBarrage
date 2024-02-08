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

public abstract class EnemyBase : MonoBehaviour
{
	#region 変数
	[SerializeField]
	protected Slider _hpSlider = default;
	protected int _hpValue = 0;

	protected GameObject _player = default;
	protected Vector3 _playerPos = Vector3.zero;

	protected Vector3[,] _movePattern = new Vector3[10,3];

	protected BulletPool _bulletPool = default;
	protected PuttingEnemyBullet _puttingEnemyBullet = default;

	protected abstract void OnTriggerEnter2D(Collider2D collision);
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
	}

	/// <summary>
	/// 更新前処理
	/// </summary>
	private void Start ()
	{
		
	}

	/// <summary>
	/// 更新処理
	/// </summary>
	private void Update ()
	{
		
	}

    protected void EnemyMove(int moveNumber)
    {
        //this.transform.position = BezierCalculation(_movePattern[moveNumber, 0], _movePattern[moveNumber, 1], _movePattern[moveNumber, 2], );
    }

    public Vector2 BezierCalculation(Vector2 start, Vector2 relay, Vector2 goal, float time)
    {
        return (Mathf.Pow((1 - time), 2) * start) + (2 * (1 - time) * time * relay) + (Mathf.Pow(time, 2) * goal);
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
			_hpSlider.gameObject.SetActive(false);
		}
	}

	private void EnemyDead()
	{
		this.gameObject.SetActive(false);
	}
	#endregion
}