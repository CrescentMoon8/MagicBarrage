// ---------------------------------------------------------
// PlayerHp.cs
//
// 作成日:2024/03/01
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHp : IDamageable
{
	#region 変数
	private bool _isDamage = false;
	private int _lifeCount = 3;
    private float _damageAnimationTime = 0;
    private const float DAMAGE_ANIMATION = 3f;

    private Image[] _lifeUi = new Image[3];
    private Sprite _brokenHeart = default;

    private CircleCollider2D _circleCollider2D = default;
    private SpriteRenderer _spriteRenderer = default;
    private Animator _playerAnimator = default;

    public PlayerHp(CircleCollider2D circleCollider2D, SpriteRenderer spriteRenderer, Animator playerAnimator, Sprite brokenHeartSprite)
    {
        _brokenHeart = brokenHeartSprite;
        _circleCollider2D = circleCollider2D;
        _spriteRenderer = spriteRenderer;
        _playerAnimator = playerAnimator;
    }
    #endregion

    #region プロパティ

    #endregion

    #region メソッド
    public void Initialize(Transform lifeUiParent)
    {
        for (int i = 0; i < _lifeUi.Length; i++)
        {
            _lifeUi[i] = lifeUiParent.transform.GetChild(i).GetComponent<Image>();
        }
    }
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
        _lifeUi[_lifeCount].sprite = _brokenHeart;

        if (_lifeCount <= 0)
        {
            Dead();
        }

        AudioManager.Instance.PlayDamageSe();
    }

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

    private void Dead()
    {
        AudioManager.Instance.PlayDeadSe();
        SceneManager.LoadScene("GameOver");
    }
    #endregion
}