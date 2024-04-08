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
    // 撃つ間隔
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
    /// プレイヤーの弾を配置する
    /// </summary>
    /// <param name="shotPos">配置する位置</param>
    /// <param name="isHard">ゲームの難易度</param>
    public void Put(Vector3 shotPos, bool isHard)
    {
        if (_shotTime >= SHOT_INTERVAL)
        {
            // 縦の位置を調整する
            shotPos.y += SHOT_POS_DIFFERENCE_Y;

            // 直線弾を配置する
            for (int bulletCount = 0; bulletCount < LINE_BULLET_AMOUNT; bulletCount++)
            {
                if (bulletCount % 2 == 0)
                {
                    // 横の位置を調整する
                    shotPos.x += SHOT_POS_DIFFERENCE_X * bulletCount;
                }
                else
                {
                    // 横の位置を調整する
                    shotPos.x -= SHOT_POS_DIFFERENCE_X * bulletCount;
                }

                PlayerBullet bullet = PlayerBulletPool.Instance.LendBullet(shotPos);

                bullet.SettingMoveType = Bullet.MoveType.Line;

                bullet.Initialize();

                _bulletCount++;
            }

            // 追尾弾を配置する（Hard時は直線弾）
            // 要改良部分
            for (int bulletCount = _bulletCount; bulletCount < TRACKING_BULLET_AMOUNT + _bulletCount; bulletCount++)
            {
                if (bulletCount % 2 == 0)
                {
                    // 横の位置を調整する
                    shotPos.x += SHOT_POS_DIFFERENCE_X * bulletCount;
                }
                else
                {
                    // 横の位置を調整する
                    shotPos.x -= SHOT_POS_DIFFERENCE_X * bulletCount;
                }

                // 改良対象部分
                if (isHard)
                {
                    PlayerBullet bullet = PlayerBulletPool.Instance.LendBullet(shotPos);


                    bullet.SettingMoveType = Bullet.MoveType.Line;

                    bullet.Initialize();
                }
                else
                {
                    PlayerBullet bullet = PlayerBulletPool.Instance.LendBullet(shotPos);

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