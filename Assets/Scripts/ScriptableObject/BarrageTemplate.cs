// ---------------------------------------------------------
// BarrageTemplate.cs
//
// 作成日:2024/02/26
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

/// <summary>
/// 弾のテンプレートを管理するクラス
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObject/BarrageTemplate", fileName = "BarrageTemplate")]
public class BarrageTemplate : ScriptableObject
{
	enum BarrageType
    {

    }
	[Header("撃ちたい角度")][SerializeField]
	private float _targetAngle = 0;
	[Header("撃ちたい角度の幅")][SerializeField]
	private int _angleWidth = 0;
	[Header("角度を何分割するか")][SerializeField]
	private int _angleSplit = 0;

	public float TargetAngle { get { return _targetAngle; } }
	public int AngleWidth { get { return _angleWidth; } }
	public int AngleSplit { get { return _angleSplit; } }
}