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

#if UNITY_STANDALONE_WIN
    private Vector3 _maxWindowVector = Vector3.zero;
    private Vector3 _minWindowVector = Vector3.zero;
#endif
    #endregion

    #region プロパティ
    public Vector3 MaxFieldVector { get { return _maxFieldVector; } }
    public Vector3 MinFieldVector { get { return _minFieldVector; } }
#if UNITY_STANDALONE_WIN
    public Vector3 MaxWindowVector { get { return _maxWindowVector; } }
    public Vector3 MinWindowVector { get { return _minWindowVector; } }
#endif
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

#if UNITY_STANDALONE_WIN
        // 取得する座標の番号
        int MAX_WINDOW_POS_INDEX = 2;
        int MIN_WINDOW_POS_INDEX = 0;

        // UIオブジェクトの頂点の座標を取得し、移動制限の大きさを調整する
        // 座標の取得順は左下、左上、右上、右下
        Vector3[] headerCorners_pc = new Vector3[4];
        _header.GetWorldCorners(headerCorners_pc);
        _maxWindowVector = headerCorners_pc[MAX_WINDOW_POS_INDEX];

        Vector3[] footerCorners_pc = new Vector3[4];
        _footer.GetWorldCorners(footerCorners_pc);
        _minWindowVector = footerCorners_pc[MIN_WINDOW_POS_INDEX];
#endif
    }
	#endregion
}