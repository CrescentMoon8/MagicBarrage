// ---------------------------------------------------------
// PlayerBulletPut.cs
//
// 作成日:2024/02/14
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

/// <summary>
/// プレイヤーの弾の配置処理を行うクラス
/// </summary>
public class PlayerBulletPut
{
    #region 変数
    private float _shotTime = 0.1f;
    private const float SHOT_INTERVAL = 0.1f;

    // 弾の開始位置を調整するための変数
    private const float SHOT_POS_DIFFERENCE_Y = 0.7f;
    private const float SHOT_POS_DIFFERENCE_X = 0.15f;
    
    // 直線弾を撃つ数(奇数個)
    private const int LINE_BULLET_AMOUNT = 1;
    // 追尾弾を撃つ数(偶数個)
    private const int TRACKING_BULLET_AMOUNT = 2;

    // 二種の弾を撃つ数
    private int _bulletCount = 0;
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

    /// <summary>
    /// プレイヤーの
    /// </summary>
    /// <param name="shotPos"></param>
    /// <param name="isHard"></param>
    public void Shot(Vector3 shotPos, bool isHard)
    {
        if (_shotTime >= SHOT_INTERVAL)
        {
            shotPos.y += SHOT_POS_DIFFERENCE_Y;

            for (int bulletCount = 0; bulletCount < LINE_BULLET_AMOUNT; bulletCount++)
            {
                if (bulletCount % 2 == 0)
                {
                    shotPos.x += SHOT_POS_DIFFERENCE_X * bulletCount;
                }
                else
                {
                    shotPos.x -= SHOT_POS_DIFFERENCE_X * bulletCount;
                }

                Bullet bullet = PlayerBulletPool.Instance.LendPlayerBullet(shotPos);

                bullet.SettingMoveType = Bullet.MoveType.Line;

                bullet.Initialize();

                _bulletCount++;
            }

            for (int bulletCount = _bulletCount; bulletCount < TRACKING_BULLET_AMOUNT + _bulletCount; bulletCount++)
            {
                if (bulletCount % 2 == 0)
                {
                    shotPos.x += SHOT_POS_DIFFERENCE_X * bulletCount;
                }
                else
                {
                    shotPos.x -= SHOT_POS_DIFFERENCE_X * bulletCount;
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

            AudioManager.Instance.PlayPlayerShotSe();

            _shotTime = 0;
            _bulletCount = 0;
        }
    }
    #endregion
}