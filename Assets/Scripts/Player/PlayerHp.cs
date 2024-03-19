// ---------------------------------------------------------
// PlayerHp.cs
//
// 作成日:2024/03/01
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーのダメージ処理とダメージアニメーションを行うクラス
/// </summary>
public class PlayerHp : IDamageable
{
	#region 変数
	private bool _isDamage = false;
    // プレイヤーの残機
	private int _lifeCount = 3;
    // アニメーション開始からの時間を計測する
    private float _damageAnimationTime = 0;
    // アニメーションの時間
    private const float DAMAGE_ANIMATION = 3f;

    // プレイヤーの残機UIを格納する配列
    private Image[] _lifeUi = new Image[3];
    // プレイヤーの残機減少後のSprite
    private Sprite _brokenHeart = default;

    private bool _isDead = false;
    private ParticleScript _particleScript = default;

    private CircleCollider2D _circleCollider2D = default;
    private SpriteRenderer _spriteRenderer = default;
    private Animator _playerAnimator = default;
    private PlayerParticlePool _playerParticlePool = default;
    private IPlayerPos _iPlayerPos = default;

    public PlayerHp(CircleCollider2D circleCollider2D, SpriteRenderer spriteRenderer, Animator playerAnimator, PlayerParticlePool particlePool, IPlayerPos iPlayerPos)
    {
        _circleCollider2D = circleCollider2D;
        _spriteRenderer = spriteRenderer;
        _playerAnimator = playerAnimator;
        _playerParticlePool = particlePool;
        _iPlayerPos = iPlayerPos;
    }
    #endregion

    #region プロパティ
    public bool IsDead { get { return _isDead; } }
    #endregion

    #region メソッド
    /// <summary>
    /// 親オブジェクトからプレイヤーの残機UIを取得する
    /// </summary>
    /// <param name="lifeUiParent">プレイヤーの残機UIの親オブジェクト</param>
    public void Initialize(Transform lifeUiParent, Sprite brokenHeartSprite)
    {
        for (int i = 0; i < _lifeUi.Length; i++)
        {
            _lifeUi[i] = lifeUiParent.transform.GetChild(i).GetComponent<Image>();
        }

        _brokenHeart = brokenHeartSprite;
    }

    /// <summary>
    /// プレイヤーのダメージ処理
    /// </summary>
    public void Damage()
    {
        /* 
         * コライダーに複数のオブジェクトが同時に接触すると複数回呼び出されるため、
         * コライダーがOFFならば二回目以降の処理をスキップする
         */
        if (!_circleCollider2D.enabled)
        {
            return;
        }

        // 当たり判定とコアのスプライトを消す
        _circleCollider2D.enabled = false;
        _spriteRenderer.enabled = false;

        _playerAnimator.SetTrigger("IsDamage");

        _isDamage = true;

        _lifeCount--;
        // 残機UIを残機減少後のSpriteに入れ替える
        _lifeUi[_lifeCount].sprite = _brokenHeart;

        if (_lifeCount <= 0)
        {
            Dead();
        }

        AudioManager.Instance.PlayPlayerDamageSe();
    }

    /// <summary>
    /// アニメーションが開始されてからの時間を計測し、終了時間を過ぎたら元に戻す
    /// </summary>
    public void AnimationPlayTime()
    {
        if (_isDamage)
        {
            _damageAnimationTime += Time.deltaTime;

            if (_damageAnimationTime >= DAMAGE_ANIMATION)
            {
                _damageAnimationTime = 0;
                _spriteRenderer.enabled = true;
                _circleCollider2D.enabled = true;
                _isDamage = false;
            }
        }
    }

    // プレイヤーの死亡処理
    private void Dead()
    {
        _particleScript = _playerParticlePool.SetPlayerDeadParticle(_iPlayerPos.PlayerPos);
        _particleScript.Play();
        AudioManager.Instance.PlayPlayerDeadSe();
        _isDead = true;
    }

    public bool ParticleIsPlaying()
    {
        return _particleScript.IsPlaying();
    }
    #endregion
}