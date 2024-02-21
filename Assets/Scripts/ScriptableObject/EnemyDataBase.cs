// ---------------------------------------------------------
// EnemyDataBase.cs
//
// 作成日:2024/02/21
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObject/EnemyDataBase", fileName = "EnemyDataBase")]
public class EnemyDataBase : ScriptableObject
{
	public List<EnemyData> _enemyDataList = new List<EnemyData>();

	public readonly int RED_SLIME = 0;
	public readonly int BLUE_SLIME = 1;
	public readonly int YELLOW_SLIME = 2;
	public readonly int GREEN_SLIME = 3;
	public readonly int PURPLE_SLIME = 4;
    public readonly int BOSS_SLIME = 5;

}