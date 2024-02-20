// ---------------------------------------------------------
// EnemyBase.cs
//
// 作成日:2024/02/06
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class EnemyBase : MonoBehaviour
{
	#region 変数
	public enum EnemyType
    {
		Nomal,
		Boss
    }

	private const float HPBAR_ADJUST_POS_Y = 0.44f;
    [SerializeField]
	protected Slider _hpSlider = default;
	protected int _hpValue = 0;

	// 子クラスで作るべき？
	/*protected GameObject _player = default;
	protected Vector3 _playerPos = Vector3.zero;*/

	// Splinesパッケージで代用
	//protected Vector3[,] _movePattern = new Vector3[10,3];

	private BulletPool _bulletPool = default;
	protected EnemyShot _puttingEnemyBullet = default;
	protected EnemyMove _enemyMove = default;

	private delegate void DownEnemyCount(GameObject enemy);
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
		_puttingEnemyBullet = new EnemyShot(_bulletPool, this.transform.localScale.x / 2);
		_enemyMove = new EnemyMove();

		_downEnemyCountCallBack = GameObject.FindWithTag("Scripts").GetComponentInChildren<EnemyManager>().DownEnemyCount;
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
		_downEnemyCountCallBack(this.gameObject);
		_hpSlider.gameObject.SetActive(false);
		this.gameObject.SetActive(false);
	}
	#endregion
}