// ---------------------------------------------------------
// EnemyData.cs
//
// 作成日:2024/02/21
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EnemyData", fileName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("エネミー名")]
    public string _enemyName = default;
    [Header("最大HP")]
    public int _maxHp = 0;
    [Header("移動する経路の番号（SplineIndex）")]
    public int _splineIndex = 0;
}