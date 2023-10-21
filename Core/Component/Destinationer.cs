using Cysharp.Threading.Tasks;
using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Destinationer : MonoBehaviour
{
    [TitleDescription] public string title = "目的地まで到達させる";

    [Space]
    public Rigidbody _rigidbody;
    //public RaySensor raySensor;

    [Space]
    [Label("目的地に移動するかどうか")]public bool isMove = false;
    [Label("目的地のTransform(nullだとVector3になる)")] public Transform destinationTransform;
    [Label("目的地の座標(Transformが優先)")]public Vector3 destination;
    [Label("速さ")]public float speed;
    [Label("移動し始める最小距離")] public float minDistance = -1;
    [Label("下の穴に落ちないかどうか")] public bool isDetectFloor = false;
    [Label("到着と決める最小距離")] public float stopThreshold = 1;

    [Space]
    [Readonly] public bool isArrivaled = false;
    [Readonly] public bool isStoping = false;

    private float stopTime;
    private Vector3 stopedPosition = default;
    

    private void Awake()
    {
        stopTime = 0;
    }

    void Update()
    {
        /*var vec = destination;
        vec.y = transform.position.y;
        transform.LookAt(vec);

        isArrivaled = isArrival();
        isStoping = isStopping();

        if (isMove && !isArrivaled)
        {
            if (raySensor != null && isDetectFloor && raySensor.DetectedObjects.Count != 0)
            {
                _rigidbody.MovePosition(transform.forward * speed * Time.deltaTime + transform.position);
            }
        }*/
    }

    public void SetDestination(Vector3 _destination)
    {
        destination = _destination;
        isMove = true;
    }
    public void SetDestination(Transform _destination)
    {
        destination = _destination.position;
        isMove = true;
    }

    /// <summary>
    /// 到着しているかどうか
    /// </summary>
    /// <returns></returns>
    bool isArrival()
    {
        return Vector3.Distance(transform.position, destination) < 0.3f;
    }

    /// <summary>
    /// 止まっているかどうか
    /// </summary>
    bool isStopping()
    {
        if(Vector3.Distance(stopedPosition, transform.position) < 0.3f) //止まっているなら
        {
            stopedPosition = transform.position;
            stopTime += Time.deltaTime;
        }
        else { stopTime = 0; }

        if(stopTime > stopThreshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// パトロールをし続ける
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask RamdomPatrolTask(Vector3 center , float range , CancellationToken token)
    {
        while (true)
        {
            token.ThrowIfCancellationRequested();

            SetDestinationRandom(center, range);

            await UniTask.WaitUntil(()=> isStoping == true);
        }
    }
    public async UniTask RamdomPatrolTask(List<Transform> points, CancellationToken token = default, bool isRandom = true)
    {
        var nowDestinationPoint = 0;
        while (true)
        {
            token.ThrowIfCancellationRequested();

            if(isRandom == true)
            {
                destination = points.GetRandomInList().position;
            }
            else
            {
                destination = points[nowDestinationPoint].position;
                nowDestinationPoint += 1;
                if(nowDestinationPoint >= points.Count) nowDestinationPoint = 0;
            }

            await UniTask.WaitUntil(()=> isStoping == true);
        }
    }

    /// <summary>
    /// 範囲内のランダムな場所に移動
    /// </summary>
    void SetDestinationRandom(Vector3 center, float range)
    {
        destination = new Vector3(range, 0, range) * (Random.value - 0.5f) + center;
    }

    public void SetMoveActive(bool active)
    {
        isMove = active;
    }
}
