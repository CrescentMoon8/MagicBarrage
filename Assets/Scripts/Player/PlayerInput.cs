// ---------------------------------------------------------
// PlayerInput.cs
//
// 作成日:2024/03/01
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using UnityEngine.AddressableAssets;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

/// <summary>
/// プレイヤーの入力の取得と入力時のパーティクルの再生を行うクラス
/// </summary>
public class PlayerInput
{
    #region 変数
    private ParticleSystem _touchParticle = default;
    #endregion

    #region メソッド
    /// <summary>
    /// 指が触れている部分のパーティクルをロードする
    /// </summary>
    public void Initialize()
    {
        GameObject particleObject = Addressables.InstantiateAsync("TouchParticle").WaitForCompletion();
        _touchParticle = particleObject.GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// 入力があるかどうか
    /// </summary>
    /// <returns></returns>
    public Touch InputTouch()
    {
        // タッチの入力を取得する
        Touch activeTouch = Touch.activeTouches[0];

        Vector3 particlePos = Vector3.zero;

        switch (activeTouch.phase)
        {
            // 指がふれたとき
            case TouchPhase.Began:
                _touchParticle.gameObject.SetActive(true);
                _touchParticle.Play();

                particlePos = Camera.main.ScreenToWorldPoint(activeTouch.startScreenPosition);
                // カメラのZ座標が入るため０にする
                particlePos.z = 0;
                _touchParticle.gameObject.transform.position = particlePos;
                break;

            // 指が動いてるとき
            case TouchPhase.Moved:
                particlePos = Camera.main.ScreenToWorldPoint(activeTouch.screenPosition);
                // カメラのZ座標が入るため０にする
                particlePos.z = 0;
                _touchParticle.gameObject.transform.position = particlePos;
                break;

            // 指が離れたとき
            case TouchPhase.Ended:
                _touchParticle.gameObject.SetActive(false);
                break;

            // 何らかの理由で入力が途切れたとき
            case TouchPhase.Canceled:
                _touchParticle.gameObject.SetActive(false);
                break;

            default:
                break;
        }

        return activeTouch;
    }
	#endregion
}