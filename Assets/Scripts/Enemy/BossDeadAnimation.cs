// ---------------------------------------------------------
// BossDeadAnimation.cs
//
// 作成日:2024/03/19
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

public class BossDeadAnimation : MonoBehaviour
{
	#region 変数
	[SerializeField]
	private GameObject _deadParticles = default;

	[SerializeField]
	private List<ParticleSystem> _deadParticleList = new List<ParticleSystem>();

	[SerializeField]
	private SpriteRenderer _spriteRenderer = default;
	private Color _color = default;
	private const float ANIMATION_TIME = 3.6f;
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
		GameObject deadParticles = Instantiate(_deadParticles);

		_color = _spriteRenderer.color;

        for (int i = 0; i < deadParticles.transform.childCount; i++)
        {
			_deadParticleList.Add(deadParticles.transform.GetChild(i).GetComponent<ParticleSystem>());
        }
	}

	/// <summary>
	/// ボスが死んだときのパーティクルをすべて再生する
	/// </summary>
	public void Play()
    {
        for (int i = 0; i < _deadParticleList.Count; i++)
        {
			_deadParticleList[i].Play();
		}
    }

	/// <summary>
	/// アニメーションが終了したかどうか
	/// </summary>
	/// <returns>アニメーションが終了であればtrue</returns>
	public bool EndDeadAnimation()
    {
		if(!IsPlaying())
        {
			return true;
        }

		_color.a -= 1 / ANIMATION_TIME * Time.deltaTime;

		_spriteRenderer.color = _color;

		return false;
    }

	private bool IsPlaying()
    {
		// リストの一番最後の再生が終わったら
		if(_deadParticleList[_deadParticleList.Count - 1].isPlaying)
        {
			return true;
        }

		return false;
    }
	#endregion
}