#if Shin_Overide_Spline

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Splines;
#endif

using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Interpolators = UnityEngine.Splines.Interpolators;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.GraphicsBuffer;
using Google.Protobuf.Collections;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[RequireComponent(typeof(SplineContainer), typeof(MeshRenderer), typeof(MeshFilter))]
public class SplineCreater : MonoBehaviour
{
    [SerializeField]
    List<SplineData<float>> m_Widths = new List<SplineData<float>>();

    public List<SplineData<float>> Widths
    {
        get
        {
            foreach (var width in m_Widths)
            {
                if (width.DefaultValue == 0)
                    width.DefaultValue = 1f;
            }

            return m_Widths;
        }
    }

    [SerializeField]
    SplineContainer m_Spline;

    public SplineContainer Container
    {
        get
        {
            if (m_Spline == null)
                m_Spline = GetComponent<SplineContainer>();

            return m_Spline;
        }
        set => m_Spline = value;
    }

    [SerializeField]
    Mesh m_Mesh;

    [SerializeField]
    float m_TextureScale = 1f;

    public bool normalInvert = false;

    [SerializeField]
    float m_SegmentsPerMeter = 1;

    [Space(15)]
    [FoldOut("Height", true)]
    [SerializeField]
    float height = 1;

    [FoldOut("Height")] public bool heightDirection = false;

    [SerializeField]
    [FoldOut("Height")] float heightSegmentsPerMeter = 1;

    [FoldOut("Height")]
    public AnimationCurve heightPerCurve = AnimationCurve.Constant(
       timeStart: 0f,
       timeEnd: 1f,
       value: 0f
   );
    [FoldOut("Height")] public float heightCurveWeight = 1;


    public IReadOnlyList<Spline> splines => LoftSplines;

    public IReadOnlyList<Spline> LoftSplines
    {
        get
        {
            if (m_Spline == null)
                m_Spline = GetComponent<SplineContainer>();

            if (m_Spline == null)
            {
                Debug.LogError("Cannot loft road mesh because Spline reference is null");
                return null;
            }

            return m_Spline.Splines;
        }
    }

    [Obsolete("Use LoftMesh instead.", false)]
    public Mesh mesh => LoftMesh;
    public Mesh LoftMesh
    {
        get
        {
            if (m_Mesh != null)
                return m_Mesh;

            m_Mesh = new Mesh();
            return m_Mesh;
        }
    }

    [Obsolete("Use SegmentsPerMeter instead.", false)]
    public float segmentsPerMeter => SegmentsPerMeter;
    public float SegmentsPerMeter => Mathf.Min(10, Mathf.Max(0.0001f, m_SegmentsPerMeter));
    public float heightPerMeter => Mathf.Min(10, Mathf.Max(0.0001f, heightSegmentsPerMeter));

    [Space(15)]
    [FoldOut("Width", true)]
    public AnimationCurve widthCurvePerHeight = AnimationCurve.Constant(
        timeStart: 0f,
        timeEnd: 1f,
        value: 1f
    );
    [FoldOut("Width")] public float MultipleWidth = 1;

    [FoldOut("isSurface", true)] public bool up = true;
    [FoldOut("isSurface")] public bool down = true;
    [FoldOut("isSurface")] public bool left = true;
    [FoldOut("isSurface")] public bool right = true;
    [FoldOut("isSurface")] public bool forward = true;
    [FoldOut("isSurface")] public bool back = true;

    bool m_up => heightDirection ? down : up;
    bool m_down => heightDirection ? up : down;

    [FoldOut("Noise", true)] public bool isNoise = false;
    [FoldOut("Noise")] public Noise noise;
    [FoldOut("Noise")] public float noisePower = 1;
    [FoldOut("Noise")] public bool upNoise = true;
    [FoldOut("Noise")] public bool downNoise = true;
    [FoldOut("Noise")] public bool acrossNoise = true;
    [FoldOut("Noise")] public bool forwardNoise = true;
    [FoldOut("Noise")] public bool backNoise = true;

    bool m_upN => heightDirection ? downNoise : upNoise;
    bool m_downN => heightDirection ? upNoise : downNoise;

    List<Vector3> m_Positions = new List<Vector3>();
    List<Vector3> m_Normals = new List<Vector3>();
    List<Vector2> m_Textures = new List<Vector2>();
    List<int> m_Indices = new List<int>();
    bool m_normalInvert => heightDirection ^ normalInvert;

    public void OnEnable()
    {
        // Avoid to point to an existing instance when duplicating the GameObject
        if (m_Mesh != null)
            m_Mesh = null;

        if (m_Spline == null)
            m_Spline = GetComponent<SplineContainer>();

        LoftAllRoads();

#if UNITY_EDITOR
        EditorSplineUtility.AfterSplineWasModified += OnAfterSplineWasModified;
        EditorSplineUtility.RegisterSplineDataChanged<float>(OnAfterSplineDataWasModified);
        Undo.undoRedoPerformed += LoftAllRoads;
#endif

        SplineContainer.SplineAdded += OnSplineContainerAdded;
        SplineContainer.SplineRemoved += OnSplineContainerRemoved;
        SplineContainer.SplineReordered += OnSplineContainerReordered;
        Spline.Changed += OnSplineChanged;

        if (noise == null) noise = Resources.Load("Assets/Project/Scripts/Shin_UnityLibrary/OverideAssets/Splines/Resources/DefoltNoise.asset") as Noise;
    }

    public void OnDisable()
    {
#if UNITY_EDITOR
        EditorSplineUtility.AfterSplineWasModified -= OnAfterSplineWasModified;
        EditorSplineUtility.UnregisterSplineDataChanged<float>(OnAfterSplineDataWasModified);
        Undo.undoRedoPerformed -= LoftAllRoads;
#endif

        if (m_Mesh != null)
#if UNITY_EDITOR
            DestroyImmediate(m_Mesh);
#else
                Destroy(m_Mesh);
#endif

        SplineContainer.SplineAdded -= OnSplineContainerAdded;
        SplineContainer.SplineRemoved -= OnSplineContainerRemoved;
        SplineContainer.SplineReordered -= OnSplineContainerReordered;
        Spline.Changed -= OnSplineChanged;
    }

    void OnSplineContainerAdded(SplineContainer container, int index)
    {
        if (container != m_Spline)
            return;

        if (m_Widths.Count < LoftSplines.Count)
        {
            var delta = LoftSplines.Count - m_Widths.Count;
            for (var i = 0; i < delta; i++)
            {
#if UNITY_EDITOR
                Undo.RecordObject(this, "Modifying Widths SplineData");
#endif
                m_Widths.Add(new SplineData<float>() { DefaultValue = 1f });
            }
        }

        LoftAllRoads();
    }

    void OnSplineContainerRemoved(SplineContainer container, int index)
    {
        if (container != m_Spline)
            return;

        if (index < m_Widths.Count)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Modifying Widths SplineData");
#endif
            m_Widths.RemoveAt(index);
        }

        LoftAllRoads();
    }

    void OnSplineContainerReordered(SplineContainer container, int previousIndex, int newIndex)
    {
        if (container != m_Spline)
            return;

        LoftAllRoads();
    }

    void OnAfterSplineWasModified(Spline s)
    {
        if (LoftSplines == null)
            return;

        foreach (var spline in LoftSplines)
        {
            if (s == spline)
            {
                LoftAllRoads();
                break;
            }
        }
    }

    void OnSplineChanged(Spline spline, int knotIndex, SplineModification modification)
    {
        OnAfterSplineWasModified(spline);
    }

    void OnAfterSplineDataWasModified(SplineData<float> splineData)
    {
        foreach (var width in m_Widths)
        {
            if (splineData == width)
            {
                LoftAllRoads();
                break;
            }
        }
    }

    public void LoftAllRoads()
    {
        LoftMesh.Clear();
        m_Positions.Clear();
        m_Normals.Clear();
        m_Textures.Clear();
        m_Indices.Clear();

        for (var i = 0; i < LoftSplines.Count; i++)
            Loft(LoftSplines[i], i);

        LoftMesh.SetVertices(m_Positions);
        LoftMesh.SetNormals(m_Normals);
        LoftMesh.SetUVs(0, m_Textures);
        LoftMesh.subMeshCount = 1;
        LoftMesh.SetIndices(m_Indices, MeshTopology.Triangles, 0);
        LoftMesh.UploadMeshData(false);

        GetComponent<MeshFilter>().sharedMesh = m_Mesh;
    }

    public void Loft(Spline spline, int widthDataIndex)
    {

        if (spline == null || spline.Count < 2)
            return;

        LoftMesh.Clear();

        float length = spline.GetLength();

        if (length <= 0.001f)
            return;

        var segmentsPerLength = SegmentsPerMeter * length;
        var segments = Mathf.CeilToInt(segmentsPerLength);
        var segmentStepT = (1f / SegmentsPerMeter) / length;

        var steps = segments + 1;

        var heightsegmentsPerLength = heightPerMeter * height;
        var heightSegments = Mathf.Max(1, Mathf.CeilToInt(heightsegmentsPerLength));
        var heightSegmentStep = height / heightSegments;

        var vertexCount = steps * 2 * (heightSegments + 1);
        var triangleCount = segments * 6;
        var indicesCount = triangleCount * (1 + 2 * heightSegments);
        var prevVertexCount = m_Positions.Count;

        m_Positions.Capacity += vertexCount;
        m_Normals.Capacity += vertexCount;
        m_Textures.Capacity += vertexCount;
        m_Indices.Capacity += indicesCount;

        var t = 0f;
        int lastN = 0;

        for (int i = 0; i < steps; i++)
        {
            SplineUtility.Evaluate(spline, t, out var pos, out var dir, out var up);

            // If dir evaluates to zero (linear or broken zero length tangents?)
            // then attempt to advance forward by a small amount and build direction to that point
            if (math.length(dir) == 0)
            {
                var nextPos = spline.GetPointAtLinearDistance(t, 0.01f, out _);
                dir = math.normalizesafe(nextPos - pos);

                if (math.length(dir) == 0)
                {
                    nextPos = spline.GetPointAtLinearDistance(t, -0.01f, out _);
                    dir = -math.normalizesafe(nextPos - pos);
                }

                if (math.length(dir) == 0)
                    dir = new float3(0, 0, 1);
            }

            var scale = transform.lossyScale;
            var tangent = math.normalizesafe(math.cross(up, dir)) * new float3(1f / scale.x, 1f / scale.y, 1f / scale.z);

            var w = 1f;
            if (widthDataIndex < m_Widths.Count)
            {
                w = m_Widths[widthDataIndex].DefaultValue;
                if (m_Widths[widthDataIndex] != null && m_Widths[widthDataIndex].Count > 0)
                {
                    w = m_Widths[widthDataIndex].Evaluate(spline, t, PathIndexUnit.Normalized, new Interpolators.LerpFloat());
                    w = math.clamp(w, .001f, 10000f);
                }
            }
            t = math.min(1f, t + segmentStepT);

            SetPositions();
            SetNormals();
            SetUV();

            void SetPositions()
            {
                for (int s = 0; s < heightSegments + 1; s++)
                {
                    var heightParamter = Mathf.Clamp((float)s / heightSegments, 0, 1); //高くなるほどに1に近づく
                    var curveParamter = Mathf.Clamp((float)i / (steps - 1), 0, 1); //カーブが進むほどに1に近づく

                    var curveValue = widthCurvePerHeight.Evaluate(heightParamter);
                    var heightCurveValue = heightPerCurve.Evaluate(curveParamter);

                    var heightWithCurve = heightCurveValue * heightCurveWeight * heightParamter; //実際に高さを変える量（だんだん大きく)

                    SurfaceType surfaceType = GetSurfaceType();

                    switch (surfaceType)
                    {
                        case SurfaceType.up:
                            AddPosition(upNoise);
                            break;
                        case SurfaceType.down:
                            AddPosition(downNoise);
                            break;
                        case SurfaceType.forward:
                            AddPosition(forwardNoise);
                            break;
                        case SurfaceType.back:
                            AddPosition(backNoise);
                            break;
                        case SurfaceType.across:
                            AddPosition(acrossNoise);
                            break;
                    }


                    SurfaceType GetSurfaceType()
                    {
                        if (s == 0 && m_upN)
                        {
                            return SurfaceType.up;
                        }
                        else if (s == heightSegments)
                        {
                            return SurfaceType.down;
                        }
                        else if (i == 0)
                        {
                            return SurfaceType.forward;
                        }
                        else if (i == steps - 1)
                        {
                            return SurfaceType.back;
                        }
                        else
                        {
                            return SurfaceType.across;
                        }
                    }

                    void AddPosition(bool isnoise)
                    {
                        var width = w * curveValue * MultipleWidth;
                        var height = heightSegmentStep * s + heightWithCurve;

                        if (heightDirection)
                        {
                            var leftpos = pos - width * tangent + new float3(0, height, 0);
                            var rightpos = pos + width * tangent + new float3(0, height, 0);

                            if (isnoise && noise != null && isNoise)
                            {
                                m_Positions.Add(leftpos + noise.GetNoise3D(leftpos) * noisePower); //左面Position
                                m_Positions.Add(rightpos + noise.GetNoise3D(rightpos) * noisePower); //右面Position
                            }
                            else
                            {
                                m_Positions.Add(leftpos); //左面Position
                                m_Positions.Add(rightpos); //右面Position
                            }
                        }
                        else
                        {
                            var leftpos = pos - width * tangent - new float3(0, height, 0);
                            var rightpos = pos + width * tangent - new float3(0, height, 0);

                            if (isnoise && noise != null && isNoise)
                            {
                                m_Positions.Add(leftpos + noise.GetNoise3D(leftpos) * noisePower); //左面Position
                                m_Positions.Add(rightpos + noise.GetNoise3D(rightpos) * noisePower); //右面Position
                            }
                            else
                            {
                                m_Positions.Add(leftpos); //左面Position
                                m_Positions.Add(rightpos); //右面Position
                            }
                        }
                    }
                }
            }

            void SetNormals()
            {
                for (int s = 0; s < heightSegments + 1; s++)
                {
                    if (s == 0) //Normal
                    {
                        m_Normals.Add(up);//上面
                        m_Normals.Add(up);
                    }
                    else if (s == heightSegments)
                    {
                        m_Normals.Add(up * -1);//下面
                        m_Normals.Add(up * -1);
                    }
                    else
                    {
                        m_Normals.Add(-tangent); //側面
                        m_Normals.Add(tangent);
                    }
                }
            }

            void SetUV()
            {
                var value = 0.25f;
                var uvPerHeight = value / heightSegments;
                for (int s = 0; s < heightSegments + 1; s++)
                {
                    m_Textures.Add(new Vector2(value - uvPerHeight * s, t * m_TextureScale));
                    m_Textures.Add(new Vector2((1- value) + uvPerHeight * s, t * m_TextureScale));
                }
            }
            
        }

        SetSurfaces();

        void SetSurfaces()
        {
            for (int i = 0, n = prevVertexCount; i < triangleCount; i += 6, n += 2 + heightSegments * 2)
            {
                int nextPoint = 2 + heightSegments * 2; //次の段に行くまでに増える数
                lastN = n + nextPoint;//次の段の左上の角

                if (m_up)
                {
                    AddIndices(
                    n + nextPoint + 1,
                    n + 1,
                    n,
                    n,
                    n + nextPoint,
                    n + nextPoint + 1
                    );//上面
                }

                for (int h = 0; h < heightSegments; h++) //両端の面
                {
                    if (left)
                    {
                        AddIndices(
                   n + h * 2 + nextPoint + 2,
                   n + h * 2 + nextPoint,
                   n + h * 2,
                   n + h * 2 + nextPoint + 2,
                   n + h * 2,
                   n + h * 2 + 2
                   );//左面
                    }

                    if (right)
                    {
                        AddIndices(
                    n + h * 2 + nextPoint + 1,
                    n + h * 2 + 3,
                    n + h * 2 + 1,
                    n + h * 2 + nextPoint + 1,
                    n + h * 2 + nextPoint + 3,
                    n + h * 2 + 3
                    );//右面
                    }
                }

                if (m_down)
                {
                    AddIndices(
                    n + nextPoint + 1 + heightSegments * 2,
                    n + heightSegments * 2,
                    n + 1 + heightSegments * 2,
                    n + heightSegments * 2,
                    n + nextPoint + 1 + heightSegments * 2,
                    n + nextPoint + heightSegments * 2
                    );//下面
                }

                /*  
                m_Indices.Add((n + 5) % (prevVertexCount + vertexCount));
                m_Indices.Add((n + 1) % (prevVertexCount + vertexCount));
                m_Indices.Add((n + 0) % (prevVertexCount + vertexCount));
                m_Indices.Add((n + 0) % (prevVertexCount + vertexCount));
                m_Indices.Add((n + 4) % (prevVertexCount + vertexCount));
                m_Indices.Add((n + 5) % (prevVertexCount + vertexCount));

                m_Indices.Add((n + 6) % (prevVertexCount + vertexCount)); //左
                m_Indices.Add((n + 4) % (prevVertexCount + vertexCount));
                m_Indices.Add((n + 0) % (prevVertexCount + vertexCount));
                m_Indices.Add((n + 6) % (prevVertexCount + vertexCount));
                m_Indices.Add((n + 0) % (prevVertexCount + vertexCount));
                m_Indices.Add((n + 2) % (prevVertexCount + vertexCount));

                m_Indices.Add((n + 5) % (prevVertexCount + vertexCount)); //右
                m_Indices.Add((n + 3) % (prevVertexCount + vertexCount));
                m_Indices.Add((n + 1) % (prevVertexCount + vertexCount));
                m_Indices.Add((n + 5) % (prevVertexCount + vertexCount));
                m_Indices.Add((n + 7) % (prevVertexCount + vertexCount));
                m_Indices.Add((n + 3) % (prevVertexCount + vertexCount));*/
            }

            if (forward)
            {
                for (int k = 0; k < heightSegments; k++) //前面
                {
                    AddIndices(
                        2 * k + 2,
                        2 * k + 0,
                        2 * k + 1,
                        2 * k + 2,
                        2 * k + 1,
                        2 * k + 3
                        );
                }
            } //前面

            if (back)
            {
                for (int i = 0; i < heightSegments; i++) //後面
                {
                    AddIndices(
                    2 * i + 2 + lastN,
                    2 * i + 1 + lastN,
                    2 * i + 0 + lastN,
                    2 * i + 2 + lastN,
                        2 * i + 3 + lastN,
                        2 * i + 1 + lastN
                        );
                }
            } //後面
        }

        

        //面を作成
        void AddIndices(params int[] ints)
        {
            m_Indices.Add(ints[0] % (prevVertexCount + vertexCount));

            if (!m_normalInvert)
            {
                m_Indices.Add(ints[1] % (prevVertexCount + vertexCount));
                m_Indices.Add(ints[2] % (prevVertexCount + vertexCount));
            }
            else
            {
                m_Indices.Add(ints[2] % (prevVertexCount + vertexCount));
                m_Indices.Add(ints[1] % (prevVertexCount + vertexCount));
            }

            m_Indices.Add(ints[3] % (prevVertexCount + vertexCount));

            if (!m_normalInvert)
            {
                m_Indices.Add(ints[4] % (prevVertexCount + vertexCount));
                m_Indices.Add(ints[5] % (prevVertexCount + vertexCount));
            }
            else
            {
                m_Indices.Add(ints[5] % (prevVertexCount + vertexCount));
                m_Indices.Add(ints[4] % (prevVertexCount + vertexCount));
            }
        }
    }

    public enum SurfaceType
    {
        up, down, across, forward, back
    }
}
#endif