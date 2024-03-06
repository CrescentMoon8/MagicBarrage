// ---------------------------------------------------------
// AudioManager.cs
//
// 作成日:2024/03/01
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

/// <summary>
/// ゲームに使用するBGM、SEを全て管理するクラス
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
	#region 変数
	// プレイヤーがダメージを受けたときのSE
	[SerializeField]
	private AudioClip _playerDamageSe = default;
	// プレイヤーが死亡した時のSE
	[SerializeField]
	private AudioClip _playerDeadSe = default;
	[SerializeField]
	private AudioClip _playerShotSe = default;

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

	/// <summary>
	/// プレイヤーがダメージを受けたときのSEを再生する
	/// </summary>
	public void PlayPlayerDamageSe()
    {
		_audioSource.PlayOneShot(_playerDamageSe);
    }

	/// <summary>
	/// プレイヤーが死亡した時のSEを再生する
	/// </summary>
	public void PlayPlayerDeadSe()
	{
		_audioSource.PlayOneShot(_playerDeadSe);
	}

	/// <summary>
	/// プレイヤーが弾を撃った時のSEを再生する
	/// </summary>
	public void PlayPlayerShotSe()
    {
		Debug.Log("a");
		_audioSource.PlayOneShot(_playerShotSe);
	}
	#endregion
}