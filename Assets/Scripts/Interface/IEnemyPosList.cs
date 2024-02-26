// ---------------------------------------------------------
// IEnemyPosList.cs
//
// 作成日:2024/02/26
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections.Generic;

public interface IEnemyList
{
    public List<List<GameObject>> EnemyPhaseList { get; }
    public List<List<IDamageable>> EnemyIDamageableList { get; }
    public EnemyManager.PhaseState NowPhaseState { get; }
}