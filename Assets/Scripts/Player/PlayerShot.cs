// ---------------------------------------------------------
// PlayerShot.cs
//
// 作成日:2024/02/14
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;

public class PlayerShot
{
    #region 変数
    private float _shotTime = 0.1f;
    private const float SHOT_INTERVAL = 0.1f;
    private const float SHOT_POS_DIFFERENCE_Y = 0.7f;
    private const float SHOT_POS_DIFFERENCE_X = 0.2f;
    private const int LINE_BULLET_AMOUNT = 1;
    private const int TRACKING_BULLET_AMOUNT = 2;
    #endregion

    #region プロパティ

    #endregion

    #region メソッド
    /// <summary>
    /// １　弾を撃つまでの時間を加算する
    /// ２　弾を撃ってからの時間を加算する
    /// </summary>
    public void AddShotTime()
    {
        _shotTime += Time.deltaTime;
    }

    public void Shot(Vector3 shotPos, bool isHard)
    {
        if (_shotTime >= SHOT_INTERVAL)
        {
            shotPos.y += SHOT_POS_DIFFERENCE_Y;

            for (int bulletCount = 1; bulletCount <= LINE_BULLET_AMOUNT; bulletCount++)
            {
                Bullet bullet = PlayerBulletPool.Instance.LendPlayerBullet(shotPos);

                bullet.SettingMoveType = Bullet.MoveType.Line;

                bullet.Initialize();
            }

            for (int bulletCount = 1; bulletCount <= TRACKING_BULLET_AMOUNT; bulletCount++)
            {
                if (bulletCount % 2 == 1)
                {
                    shotPos.x -= SHOT_POS_DIFFERENCE_X * bulletCount;
                }
                else
                {
                    shotPos.x += SHOT_POS_DIFFERENCE_X * bulletCount;
                }

                if (isHard)
                {
                    Bullet bullet = PlayerBulletPool.Instance.LendPlayerBullet(shotPos);


                    bullet.SettingMoveType = Bullet.MoveType.Line;

                    bullet.Initialize();
                }
                else
                {
                    Bullet bullet = PlayerBulletPool.Instance.LendPlayerBullet(shotPos);

                    bullet.SettingMoveType = Bullet.MoveType.Tracking;

                    bullet.Initialize();
                }

            }

            _shotTime = 0;
        }
    }
    #endregion
}