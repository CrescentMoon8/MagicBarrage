// ---------------------------------------------------------
// IObjectPool.cs
//
// 作成日:
// 作成者:
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;

public interface IObjectPool<T>
{
    T LendPlayer(Vector3 startPos, int targetNumber);

    T LendEnemy(Vector3 startPos, int targetNumber);

    void ReturnPool(T targetClass, int targetNumber);
}