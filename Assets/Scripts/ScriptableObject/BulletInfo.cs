// ---------------------------------------------------------
// BulletInfo.cs
//
// 作成日:2024/02/21
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObject/Bulletonfo", fileName = "BulletInfo")]
public class BulletInfo : ScriptableObject
{
	#region 変数
	public readonly int RED_NOMAL_BULLET = 0;
	public readonly int RED_NEEDLE_BULLET = 1;
	public readonly int BLUE_NOMAL_BULLET = 2;
	public readonly int BLUE_NEEDLE_BULLET = 3;
	public readonly int YERROW_NOMAL_BULLET = 4;
	public readonly int YERROW_NEEDLE_BULLET = 5;
	public readonly int GREEN_NOMAL_BULLET = 6;
	public readonly int GREEN_NEEDLE_BULLET = 7;
	public readonly int PURPLE_NOMAL_BULLET = 8;
	public readonly int PURPLE_NEEDLE_BULLET = 9;
	#endregion
}