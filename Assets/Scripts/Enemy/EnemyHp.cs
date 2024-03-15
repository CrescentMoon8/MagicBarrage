// ---------------------------------------------------------
// EnemyHp.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// エネミーのHPを管理するクラス
/// RigidBody2DとCircleCollider2Dの実装を必須とする
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class EnemyHp : MonoBehaviour, IDamageable
{
	#region 変数
	protected int _enemyNumber = 0;

	// HPバーの位置を調整するための定数
	private const float HPBAR_ADJUST_POS_Y = 0.44f;
    [SerializeField]
	protected Slider _hpSlider = default;
	protected int _hpValue = 0;

	protected IPlayerPos _iPlayerPos = default;
	[SerializeField]
	protected bool _isPlayerTarget = false;

	protected bool _isInsideCamera = false;

	private ItemPool _itemPool = default;
	private EnemyParticlePool _particlePool = default;
	private CircleCollider2D _circleCollider2D = default;

	private delegate void DownEnemyCount(GameObject enemy);
	private DownEnemyCount _downEnemyCountCallBack = default;

	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
		_iPlayerPos = GameObject.FindWithTag("Player").GetComponent<IPlayerPos>();
        _itemPool = GameObject.FindWithTag("Scripts").GetComponentInChildren<ItemPool>();
        _particlePool = GameObject.FindWithTag("Scripts").GetComponentInChildren<EnemyParticlePool>();

		_circleCollider2D = GetComponent<CircleCollider2D>();

		_downEnemyCountCallBack = GameObject.FindWithTag("Scripts").GetComponentInChildren<EnemyPhaseManager>().DownEnemyCount;
    }

    private void OnBecameVisible()
    {
		_isInsideCamera = true;
    }

    /*public Vector2 BezierCalculation(Vector2 start, Vector2 relay, Vector2 goal, float time)
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
		hpBarPos.y += HPBAR_ADJUST_POS_Y;
        _hpSlider.transform.position = hpBarPos;
    }

	/// <summary>
	/// エネミーのダメージ処理を行う
	/// </summary>
	public void Damage()
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

		if (_hpValue <= 0 && _circleCollider2D.enabled)
		{
			// 複数回実行されるのを防ぐためにColliderを無効化する
			_circleCollider2D.enabled = false;
			EnemyDead();
		}
	}

	/// <summary>
	/// エネミーの死亡処理を行う
	/// </summary>
	private void EnemyDead()
	{
		_downEnemyCountCallBack(this.gameObject);
		_hpSlider.gameObject.SetActive(false);

		// アイテムをプールから取り出す
		_itemPool.LendItem(this.transform.position);
		
		// 死亡時のパーティクルを取り出し再生する
		ParticleScript particleScript = _particlePool.LendEnemyDeadParticle(this.transform.position, _enemyNumber);
		particleScript.Play();

		this.gameObject.SetActive(false);
	}
	#endregion
}