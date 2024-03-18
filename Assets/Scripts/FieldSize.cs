// ---------------------------------------------------------
// FieldSize.cs
//
// 作成日:2024/03/15
// 作成者:小林慎
// ---------------------------------------------------------
using UnityEngine;

public class FieldSize : SingletonMonoBehaviour<FieldSize>
{
    #region 変数
    private Vector3 _maxFieldVector = Vector3.zero;
    private Vector3 _minFieldVector = Vector3.zero;

    // 画面上部のUIエリア
    [SerializeField]
    private RectTransform _header = default;
    // 画面下部のUIエリア
    [SerializeField]
    private RectTransform _footer = default;
    #endregion

    #region プロパティ
    public Vector3 MaxFieldVector { get { return _maxFieldVector; } }
    public Vector3 MinFieldVector { get { return _minFieldVector; } }
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    public void SetFieldSize()
	{
        base.Awake();

        // 取得する座標の番号
        int MAX_MOVE_POS_INDEX = 3;
        int MIN_MOVE_POS_INDEX = 1;

        // UIオブジェクトの頂点の座標を取得し、移動制限の大きさを調整する
        // 座標の取得順は左下、左上、右上、右下
        Vector3[] headerCorners = new Vector3[4];
        _header.GetWorldCorners(headerCorners);
        _maxFieldVector = headerCorners[MAX_MOVE_POS_INDEX];

        Vector3[] footerCorners = new Vector3[4];
        _footer.GetWorldCorners(footerCorners);
        _minFieldVector = footerCorners[MIN_MOVE_POS_INDEX];
    }
	#endregion
}