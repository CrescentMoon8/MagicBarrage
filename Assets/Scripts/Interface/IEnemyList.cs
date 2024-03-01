// ---------------------------------------------------------
// IEnemyList.cs
//
// 作成日:2024/02/26
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

public interface IEnemyList
{
    public List<GameObject> CurrentPhaseEnemyList { get; }
    public List<IDamageable> CurrentPhaseIDamageableList { get; }
}