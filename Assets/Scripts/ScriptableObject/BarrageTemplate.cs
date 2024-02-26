// ---------------------------------------------------------
// BarrageTemplate.cs
//
// 作成日:2024/02/26
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/BarrageTemplate", fileName = "BarrageTemplate")]
public class BarrageTemplate : ScriptableObject
{
	[Header("撃ちたい角度")]
	public int _centerAngle = 180;
	[Header("撃ちたい角度の幅")]
	public int _angleWidth = 45;
	[Header("角度を何分割するか")]
	public int _angleSplit = 10;
}