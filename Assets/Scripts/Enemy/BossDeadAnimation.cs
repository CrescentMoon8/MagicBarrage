// ---------------------------------------------------------
// BossDeadAnimation.cs
//
// 作成日:2024/03/19
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ボスが死んだときのアニメーションを管理するクラス
/// </summary>
public class BossDeadAnimation : MonoBehaviour
{
	#region 変数
	// 死亡時に生成するパーティクル
	[SerializeField]
	private GameObject _deadParticle = default;

	// 死亡時に再生するパーティクルのリスト
	[SerializeField]
	private List<ParticleSystem> _deadParticleList = new List<ParticleSystem>();

	[SerializeField]
	private SpriteRenderer _bossSpriteRenderer = default;
	// ボスの透明度を変化させるためのColor
	private Color _color = default;
	// ボスのアニメーションにかかる時間
	private const float ANIMATION_TIME = 3.6f;
	#endregion

	#region プロパティ

	#endregion

	#region メソッド
	/// <summary>
	/// ボス死亡時のアニメーションの初期化処理
	/// </summary>
	public void DeadAnimationAwake()
	{
		GameObject deadParticles = Instantiate(_deadParticle);

		_color = _bossSpriteRenderer.color;

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

		// 徐々に透明にする
		_color.a -= 1 / ANIMATION_TIME * Time.deltaTime;

		_bossSpriteRenderer.color = _color;

		return false;
    }

	// アニメーションが再生中か
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