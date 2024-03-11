// ---------------------------------------------------------
// FPSCounter.cs
//
// 作成日:2024/03/11
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
	#region 変数
	private int _frameCount;
	private float _prevTime;
	private float _fps;

	private TMP_Text _fpsText = default;
	#endregion

	#region メソッド
	/// <summary>
	/// 初期化処理
	/// </summary>
	private void Awake()
	{
		_fpsText = GetComponent<TMP_Text>();
		_frameCount = 0;
		_prevTime = 0.0f;
	}

	/// <summary>
	/// 更新処理
	/// </summary>
	private void Update ()
	{
		_frameCount++;
		float time = Time.realtimeSinceStartup - _prevTime;

		if (time >= 0.5f)
		{
			_fps = _frameCount / time;
			_fpsText.SetText("FPS:" + Mathf.Round(_fps));

			_frameCount = 0;
			_prevTime = Time.realtimeSinceStartup;
		}
	}
	#endregion
}