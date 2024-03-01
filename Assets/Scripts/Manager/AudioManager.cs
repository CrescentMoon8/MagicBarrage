// ---------------------------------------------------------
// AudioManager.cs
//
// 作成日:2024/03/01
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
	#region 変数
	[SerializeField]
	private AudioClip _damageSe = default;
	[SerializeField]
	private AudioClip _deadSe = default;

	private AudioSource _audioSource = default;
	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	public void PlayDamageSe()
    {
		_audioSource.PlayOneShot(_damageSe);
    }

	public void PlayDeadSe()
	{
		_audioSource.PlayOneShot(_deadSe);
	}
	#endregion
}