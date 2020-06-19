using UnityEngine;

public class Water : MonoBehaviour
{
    private LineRenderer _line;

    //配列
    private float[] _xPoints;
    private float[] _yPoints;
    private float[] _velocities;
    private float[] _accelerations;

    //メッシュ・コライダー
    private GameObject[] _meshObjs;
    private GameObject[] _colliders;
    private Mesh[] _meshes;

    private GameObject _collider;       //プレイヤーが給水するための当たり判定

    [SerializeField]
    private Material _mat = default;
    [SerializeField]
    private GameObject _waterMash = default;

    //定数
    private const float _springConstant = 0.02f;
    private const float _damPing = 0.04f;
    private const float _spread = 0.05f;
    private const float _z = -1f;

    //水の性質
    private float _baseHeight;
    private float _left;
    private float _bottom;

    [SerializeField]
    private float _water_Left = default;
    [SerializeField]
    private float _water_Width = default;
    [SerializeField]
    private float _water_Top = default;
    [SerializeField]
    private float _water_Bottom = default;


    void Start()
    {
        //水を産む
        SpawnWater(_water_Left, _water_Width, _water_Top, _water_Bottom);
    }

    /// <summary>
    /// 波を発生させる
    /// </summary>
    /// <param name="xpos"></param>
    /// <param name="velocity"></param>
    public void Splash(float xpos,float velocity)
    {
        //位置が水の境界内にある場合
        if(xpos>=_xPoints[0]　&&　xpos<=_xPoints[_xPoints.Length - 1])
        {
            // x位置を左側からの距離にオフセット
            xpos -= _xPoints[0];
            //触れているスプリングを見つける
            int index = Mathf.RoundToInt
                ((_xPoints.Length - 1) * (xpos / (_xPoints[_xPoints.Length - 1] - _xPoints[0])));
            //落下オブジェクトの速度の制限
            velocity *= 0.1f;
            //落下オブジェクトの速度をスプリングに追加
            _velocities[index] += velocity;
        }
    }

    /// <summary>
    /// 水のオブジェクトを生成する
    /// </summary>
    /// <param name="left"></param>
    /// <param name="width"></param>
    /// <param name="top"></param>
    /// <param name="bottom"></param>
    private void SpawnWater(float left, float width, float top, float bottom)
    {
        //レイヤーの設定
        gameObject.layer = 4;
        //エッジとノードの数を計算
        int edgeCount = Mathf.RoundToInt(width) * 5;
        int nodeCount = edgeCount + 1;
        //ラインレンダラーの追加・設定
        _line = gameObject.AddComponent<LineRenderer>();
        _line.material = _mat;
        _line.material.renderQueue = 1000;
        _line.positionCount = nodeCount;
        _line.startWidth = 0.1f;
        _line.endWidth = 0.1f;
        //配列を宣言
        _xPoints = new float[nodeCount];
        _yPoints = new float[nodeCount];
        _velocities = new float[nodeCount];
        _accelerations = new float[nodeCount];
        //メッシュ配列の宣言
        _meshObjs = new GameObject[edgeCount];
        _meshes = new Mesh[edgeCount];
        _colliders = new GameObject[edgeCount];
        //変数設定
        _baseHeight = top;
        _bottom = bottom;
        _left = left;

        //プレイヤーが給水するための当たり判定設定
        _collider = new GameObject();
        _collider.name = "WaterBody";
        _collider.AddComponent<BoxCollider2D>();
        _collider.transform.parent = transform;
        _collider.tag = "inwater";
        _collider.layer = 4;
        _collider.GetComponent<BoxCollider2D>().offset =
            new Vector2(left + width / 2.0f, (top + bottom) / 2.0f);
        _collider.GetComponent<BoxCollider2D>().size =
            new Vector2(width, top - bottom);
        _collider.GetComponent<BoxCollider2D>().isTrigger = true;

        //各ノードに対して、ラインレンダラーと配列を設定
        for(int i = 0; i < nodeCount; i++)
        {
            _yPoints[i] = top;
            _xPoints[i] = left + width * i / edgeCount;
            _line.SetPosition(i, new Vector3(_xPoints[i], top, _z));
            _accelerations[i] = 0;
            _velocities[i] = 0;
        }
        //今すぐメッシュを設定
        for(int i = 0; i < edgeCount; i++)
        {
            //メッシュを作成
            _meshes[i] = new Mesh();
            //角を作成
            Vector3[] Vertices = new Vector3[4];
            Vertices[0] = new Vector3(_xPoints[i], _yPoints[i], _z);
            Vertices[1] = new Vector3(_xPoints[i + 1], _yPoints[i + 1], _z);
            Vertices[2] = new Vector3(_xPoints[i], bottom, _z);
            Vertices[3] = new Vector3(_xPoints[i + 1], bottom, _z);
            //テクスチャのUVを設定
            Vector2[] UVs = new Vector2[4];
            UVs[0] = new Vector2(0, 1);
            UVs[1] = new Vector2(1, 1);
            UVs[2] = new Vector2(0, 0);
            UVs[3] = new Vector2(1, 0);
            //三角形の位置を設定
            int[] tris = new int[6] { 0, 1, 3, 3, 2, 0 };
            //このデータをすべてメッシュに追加
            _meshes[i].vertices = Vertices;
            _meshes[i].uv = UVs;
            _meshes[i].triangles = tris;
            //メッシュのホルダーを作成し、それをマネージャーの子に設定
            _meshObjs[i] = Instantiate(_waterMash, Vector3.zero, Quaternion.identity) as GameObject;
            _meshObjs[i].GetComponent<MeshFilter>().mesh = _meshes[i];
            _meshObjs[i].transform.parent = transform;
            //コライダーを作成し、それらを子に設定
            _colliders[i] = new GameObject();
            _colliders[i].name = "Trigger";
            _colliders[i].AddComponent<BoxCollider2D>();
            _colliders[i].transform.parent = transform;
            //_colliders[i].transform.tag = "inwater";
            _colliders[i].layer = 4;
            //位置とスケールを正しい寸法に設定
            _colliders[i].transform.position = 
                new Vector3(left + width * (i + 0.5f) / edgeCount, top - 0.5f, 0);
            _colliders[i].transform.localScale = new Vector3(width / edgeCount, 1, 1);
            // WaterDetectorを追加し、それらがトリガーであることを確認
            _colliders[i].GetComponent<BoxCollider2D>().isTrigger = true;
            _colliders[i].AddComponent<WaterDetector>();
        }
    }

    //前のメッシュのコードと同じ、新しいメッシュ位置を設定
    private void UpdateMeshes()
    {
        for (int i = 0; i < _meshes.Length; i++)
        {
            Vector3[] Vertices = new Vector3[4];
            Vertices[0] = new Vector3(_xPoints[i], _yPoints[i], _z);
            Vertices[1] = new Vector3(_xPoints[i + 1], _yPoints[i + 1], _z);
            Vertices[2] = new Vector3(_xPoints[i], _bottom, _z);
            Vertices[3] = new Vector3(_xPoints[i + 1], _bottom, _z);

            _meshes[i].vertices = Vertices;
        }

    }

    private void FixedUpdate()
    {
        //オイラーメソッドを使用して、スプリングのすべての物理を処理
        for(int i = 0; i < _xPoints.Length; i++)
        {
            float force = _springConstant * (_yPoints[i] - _baseHeight) + _velocities[i] * _damPing;
            _accelerations[i] = -force;
            _yPoints[i] += _velocities[i];
            _velocities[i] += _accelerations[i];
            _line.SetPosition(i, new Vector3(_xPoints[i], _yPoints[i], _z));
        }
        //高さの違いを保存
        float[] leftDeltas = new float[_xPoints.Length];
        float[] rightDeltas = new float[_xPoints.Length];
        //流動性のために8つの小さなパスを作成
        for(int j = 0; j < 8; j++)
        {
            for(int i = 0; i < _xPoints.Length; i++)
            {
                //近くのノードの高さを確認し、それに応じて速度を調整し、高さの違いを記録
                if (i > 0)
                {
                    leftDeltas[i] = _spread * (_yPoints[i] - _yPoints[i - 1]);
                    _velocities[i - 1] += leftDeltas[i];
                }

                if(i < _xPoints.Length - 1)
                {
                    rightDeltas[i] = _spread * (_yPoints[i] - _yPoints[i + 1]);
                    _velocities[i + 1] += rightDeltas[i];
                }
            }
            //位置の違いを適用
            for (int i = 0; i < _xPoints.Length; i++)
            {
                if (i > 0) _yPoints[i - 1] += leftDeltas[i];
                if (i < _xPoints.Length - 1) _yPoints[i + 1] += rightDeltas[i];
            }
        }
        //最後に、これを反映するようにメッシュを更新
        UpdateMeshes();
    }
}
