using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Reflection;
using UnityEngine.EventSystems;
using Component = UnityEngine.Component;
using System.Data;
using UnityEngine.InputSystem;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using System.Text.RegularExpressions;
using UnityEditor;

namespace Shin_UnityLibrary
{
    public static partial class Utils
    {
        /// <summary>
        /// リストからランダムに取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T GetRandomInList<T>(this List<T> list)
        {
            if (list.Count == 0)
            {
                Debug.Log("List is Zero");
                return default(T);
            }
            var n = UnityEngine.Random.Range(0, list.Count);
            T t;
            try { t = list[n]; }
            catch
            {
                Debug.Log("ListCount is " + list.Count + "  N is " + n);
                return list[0];
            }
            return t;
        }

        /// <summary>
        /// 確率でTrueを返す
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static bool Probability(int percent)
        {
            Mathf.Clamp(percent, 0, 100);
            if (Random.Range(0, 100) <= percent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 確率でTrueを返す
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static bool Probability(float percent)
        {
            float p = Mathf.Clamp(percent, 0, 1);
            if (Random.Range(0, (float)1) <= p)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 正規分布に基づいてランダムな値を生成する関数
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="standardDeviation"></param>
        /// <returns></returns>
        public static float NormalRandom(float mean, float standardDeviation)
        {
            float u1 = UnityEngine.Random.value;
            float u2 = UnityEngine.Random.value;

            float z0 = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Cos(2f * Mathf.PI * u2);
            float randomValue = mean + standardDeviation * z0;

            return randomValue;
        }

        /// <summary>
        /// 正規分布に基づいてランダムな値を生成する関数
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static float NormalRandom5Percent(float mean, float percent)
        {
            var standardDeviation = (float)(mean * percent / 1.96);
            float u1 = UnityEngine.Random.value;
            float u2 = UnityEngine.Random.value;

            float z0 = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Cos(2f * Mathf.PI * u2);
            float randomValue = mean + standardDeviation * z0;

            return randomValue;
        }

        /// <summary>
        /// 正規化する関数
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<float> Normalization(this List<float> list)
        {
            var sum = list.Sum(x => x);
            var list2 = new List<float>(list);
            for( int i = 0; i < list2.Count; i++)
            {
                list2[i] = list2[i] / sum;
            }
            return list2;
        }

        /// <summary>
        /// 正規化した値の確率でそのindexを返す関数
        /// </summary>
        /// <param name="percents"></param>
        /// <returns></returns>
        public static int SelectRandomNumbers(List<float> percents)
        {
            var normaled = percents.Normalization();
            var random = Random.value;
            float pre = 0;
            int num = 0;
            foreach( var x in normaled)
            {
                if(random < x + pre)
                {
                    return num;
                }
                num++;
                pre += x;
            }
            return num;
        }

        /// <summary>
        /// 値がMax,Minの範囲の中にあるかを確認する
        /// </summary>
        /// <param name="number"></param>
        /// <param name="Max"></param>
        /// <param name="Min"></param>
        /// <param name="IsContainMaxMin"></param>
        /// <returns></returns>
        public static bool NumberInRange(float number, float Max, float Min, bool IsContainMaxMin = true)
        {
            if (IsContainMaxMin == true)
            {
                if (number <= Max && number >= Min)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (number < Max && number > Min)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// テクスチャをfloatに
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static float[,] TexToFloat(Texture2D texture)
        {
            float[,] f = new float[texture.width, texture.height];

            for (int i = 0; i < texture.width; i++)
            {
                for (int j = 0; j < texture.height; j++)
                {
                    f[i, j] = texture.GetPixel(i, j).grayscale;
                }
            }
            return f;
        }

        /// <summary>
        /// テクスチャをpngでpathに保存
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="path"></param>
        public static void SaveTexturePng(Texture2D texture, string path)
        {
            var bytes = texture.EncodeToPNG();
            File.WriteAllBytes(path + "/" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png", bytes);
        }

        /// <summary>
        /// テクスチャの解像度を変更
        /// </summary>
        /// <param name="srcTexture"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        public static Texture2D ResizeTexture(Texture2D texture, int width, int height)
        {
            // ���T�C�Y��̃T�C�Y������RenderTexture���쐬���ď�������
            var rt = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(texture, rt);

            // ���T�C�Y��̃T�C�Y������Texture2D���쐬����RenderTexture���珑������
            var preRT = RenderTexture.active;
            RenderTexture.active = rt;
            var ret = new Texture2D(width, height);
            ret.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            ret.Apply();
            RenderTexture.active = preRT;

            RenderTexture.ReleaseTemporary(rt);
            return ret;
        }

        /// <summary>
        /// すべて１の二次元配列を返す
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static float[,] ReturnFloats(int width, int height)
        {
            float[,] f = new float[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    f[i, j] = 1;
                }
            }

            return f;
        }
        /// <summary>
        ///すべてvalueの二次元配列を返す
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static float[,] ReturnFloats(int width, int height, int value)
        {
            float[,] f = new float[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    f[i, j] = value;
                }
            }

            return f;
        }

        public static Vector2Int VectorThreeToTwoInt(this Vector3 p)
        {
            return new Vector2Int(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.z));
        }
        public static Vector2 VectorThreeToTwo(this Vector3 p)
        {
            return new Vector2(Mathf.Round(p.x), Mathf.Round(p.z));
        }
        public static Vector3 VectorTwoToThree(this Vector2 p)
        {
            return new Vector3(Mathf.Round(p.x), Mathf.Round(p.y), 0);
        }

        /// <summary>
        /// ディクショナリを結合
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dict"></param>
        /// <param name="add"></param>
        public static void AddDictionary<K, V>(this Dictionary<K, V> dict, Dictionary<K, V> add)
        {
            List<KeyValuePair<K, V>> pairs = add.ToList();
            pairs.ForEach(pair => dict.Add(pair.Key, pair.Value));
        }

        /// <summary>
        /// カメラの正面方向の単位ベクトルを返す
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetCameraForward()
        {
            var angle = Camera.main.transform.eulerAngles;
            angle.x = 0;
            var q = Quaternion.Euler(angle);
            return q * Vector3.forward;
        }
        public static Vector3 GetCameraRight()
        {
            var angle = Camera.main.transform.eulerAngles;
            angle.z = 0;
            var q = Quaternion.Euler(angle);
            return q * Vector3.right;
        }
        public static Vector3 GetCameraUp()
        {
            return Vector3.up;
        }

        /// <summary>
        /// 左クリックしてTであれば返す。それまで待つ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public static async UniTask<T> WaitForClick<T>(int layermask = int.MaxValue, CancellationToken cancellation = default) where T : MonoBehaviour
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Debug.DrawRay(ray.origin, ray.direction * 100, Color.cyan, 0.2f);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
                    {
                        if (hit.collider.gameObject.TryGetComponent(out T component) == true)
                        {
                            return component;
                        }
                    }
                }

                await UniTask.Yield(); // 1フレーム待機
                cancellation.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// directionと平行な正射影ベクトルを返す
        /// </summary>
        /// <param name="source"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector3 Projection(Vector3 source, Vector3 direction)
        {
            return Vector3.Dot(source, direction) / Mathf.Pow(direction.magnitude, 2) * direction;
        }

        /// <summary>
        /// コンポーネントをコピーする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static T CopyFrom<T>(this T self, T other) where T : Component
        {
            Type type = typeof(T);

            FieldInfo[] fields = type.GetFields();
            foreach (var field in fields)
            {
                if (field.IsStatic) continue;
                field.SetValue(self, field.GetValue(other));
            }

            PropertyInfo[] props = type.GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanWrite || !prop.CanRead || prop.Name == "name") continue;
                prop.SetValue(self, prop.GetValue(other));
            }

            return self as T;
        }
        public static T AddComponent<T>(this GameObject self, T other) where T : Component
        {
            return self.AddComponent<T>().CopyFrom(other);
        }
        public static T AddComponent<T>(this GameObject self, GameObject other) where T : Component
        {
            return self.AddComponent<T>().CopyFrom(other.GetComponent<T>());
        }

        /// <summary>
        /// ゲームオブジェクトを複製する
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static GameObject InstantiateCopy(GameObject gameObject)
        {
            var ins = GameObject.Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
            ins.transform.SetParent(gameObject.transform.parent);
            ins.transform.localScale = gameObject.transform.localScale;
            return ins;
        }

        /// <summary>
        /// LayerMaskが適用出来るかをチェックする
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public static bool LayerCheck(this GameObject obj, LayerMask layerMask)
        {
            return LayerCheck(obj, layerMask.value);
        }
        public static bool LayerCheck(this GameObject obj, int layerMaskValue)
        {
            return ((1 << obj.layer) & layerMaskValue) != 0;
        }

        /// <summary>
        /// ベクトルを回転させる関数
        /// </summary>
        /// <param name="vectorToRotate"></param>
        /// <param name="angleDegrees"></param>
        /// <returns></returns>
        public static Vector3 RotateVector(this Vector3 vectorToRotate, Vector3 angleDegrees)
        {
            // ベクトルを回転軸を中心に指定の角度だけ回転させる
            Quaternion rotationQuaternionx = Quaternion.AngleAxis(angleDegrees.x, Vector3.right);
            Quaternion rotationQuaterniony = Quaternion.AngleAxis(angleDegrees.y, Vector3.up);
            Quaternion rotationQuaternionz = Quaternion.AngleAxis(angleDegrees.z, Vector3.forward);
            Vector3 rotatedVector = rotationQuaternionx * rotationQuaterniony * rotationQuaternionz * vectorToRotate;
            return rotatedVector;
        }
        public static Vector3 RotateVector(this Vector3 vectorToRotate, float anglex, float angley, float anglez)
        {
            var v = new Vector3(anglex, angley, anglez);
            return RotateVector(vectorToRotate, v);
        }

        /// <summary>
        /// n秒間待つ
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public static async UniTask Delay(float second)
        {
            await UniTask.Delay((int)(second * 1000));
        }

        /// <summary>
        /// 待った後のゲームオブジェクトを破壊する
        /// </summary>
        /// <param name="ins"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static async UniTask DelayDestroy(this GameObject ins, float delay)
        {
            await Delay(delay);
            if (ins != null) GameObject.Destroy(ins);
        }

        /// <summary>
        /// 待った後アクションを実行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        public static void DelayAction(float delay, Action action)
        {
            UniTask.Action(async () =>
            {
                await Delay(delay);
                action.Invoke();
            })();
        }

        /// <summary>
        /// 待った後アクションを実行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        public static void DelayFrameAction(int delay, Action action)
        {
            UniTask.Action(async () =>
            {
                await UniTask.DelayFrame(delay);
                action.Invoke();
            })();
        }

        /// <summary>
        /// tagが同じかどうかチェック
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static bool CompereTag(this GameObject obj, List<string> tag)
        {
            return tag.Any(s => s == obj.tag);
        }

        /// <summary>
        /// Vector3がNaNかどうか
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNaN(this UnityEngine.Vector3 value)
        {
            return float.IsNaN(value.x) &&
                float.IsNaN(value.y) &&
                float.IsNaN(value.z);
        }

        /// <summary>
        /// すべてのゲームオブジェクトの継承されているインターフェースの関数を実行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        public static void ExecuteAllEvent<T>(ExecuteEvents.EventFunction<T> action) where T : IEventSystemHandler
        {
            foreach (var t in GameObject.FindObjectsOfType<GameObject>())
            {
                ExecuteEvents.Execute<T>(t, null, action);
            }
        }

        /// <summary>
        /// GetColliderEventのparentのオブジェクトも含めてGetComponentする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T GetComponentWithGetColliderEvent<T>(this UnityEngine.GameObject obj)
        {
            return obj.transform.GetComponentWithGetColliderEvent<T>();
        }
        public static T GetComponentWithGetColliderEvent<T>(this UnityEngine.Component obj)
        {
            T com;
            var b = obj.TryGetComponent<T>(out com);
            if (b) { return com; }

            GetColliderEvent colliderEvent;
            b = obj.TryGetComponent<GetColliderEvent>(out colliderEvent);
            if (b)
            {
                foreach (var c in colliderEvent.parents)
                {
                    b = c.TryGetComponent<T>(out com);
                    if (b) { return com; }
                }
            }
            return com;
        }

        /// <summary>
        /// Unitaskを用いてTweenを行う
        /// </summary>
        /// <param name="action">数字を適用する</param>
        /// <param name="startValue">初期値</param>
        /// <param name="targetValue">最終値</param>
        /// <param name="duration">かかる時間</param>
        /// <param name="deltaTime">待つ時間の最小単位</param>
        /// <param name="token">トークン</param>
        /// <returns></returns>
        public static async UniTask To(Action<float> action, float startValue, float targetValue, float duration, float deltaTime = 0.05f, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            float elapsedTime = 0; //経過時間
            float percentage; //進度

            while (elapsedTime < duration)
            {
                percentage = elapsedTime / duration;
                action(Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, percentage)));
                await UniTask.Delay(TimeSpan.FromSeconds(0.05f)); // 0.1秒ごとに更新
                elapsedTime += 0.05f;
            }
        }

        /// <summary>
        /// Enumの全要素のリストを返す
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Enum> GetEnumList(Type type)
        {
            return Enum.GetValues(type).Cast<Enum>().ToList();
        }

        /// <summary>
        /// あるクラスを継承しているクラスのリストを取得
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="isEqual"></param>
        /// <returns></returns>
        public static List<Type> GetInheriedType(this Type baseType, bool isEqual)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> typeList = new List<Type>();
            if(isEqual) typeList.Add(baseType);
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    // 指定された基本クラスを継承しているかどうかを確認
                    if (type.IsSubclassOf(baseType))
                    {
                        typeList.Add(type);
                    }
                }
            }
            return typeList;
        }

        /// <summary>
        /// 文字列からタイプを取得
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="assemblys"></param>
        /// <returns></returns>
        public static Type GetTypeFromString(string typeName, List<string> assemblys = null)
        {
            List<Assembly> m_assemblys = new List<Assembly>();

            if (assemblys == null || assemblys.Count == 0)
            {
                m_assemblys = AppDomain.CurrentDomain.GetAssemblies().ToList();
            }
            else {
                foreach(var a in assemblys)
                {
                    m_assemblys.Add(Assembly.Load(a));
                }
            }

            foreach (var assembly in m_assemblys)
            {
                Type type = assembly.GetType(typeName);
                if (type != null) { return type; }
            }

            return null;
        }

        /// <summary>
        /// json形式でオブジェクトを保存
        /// </summary>
        /// <param name="obj">オブジェクト</param>
        /// <param name="pass">(Assets/)+ Data + (/name) など</param>
        /// <param name="name">data + (.json) など</param>
        public static void SaveJson(object obj, string pass, string name = "data")
        {
            var str = JsonUtility.ToJson(obj);

            string filePass = "";
#if UNITY_EDITOR
            filePass = Application.dataPath + "/" + pass;
#else
            filePass = Application.persistentDataPath + "/" + pass;
#endif
            //ファイルがないなら作る
            if (!System.IO.Directory.Exists(filePass))
            {
                System.IO.Directory.CreateDirectory(filePass);
            }
            // ファイルに保存
            var writer = new StreamWriter(filePass + "/" + name + ".json", false);
            writer.Write(str);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// jsonをロード
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pass">(Assets/)+ Data/ + (name) など</param>
        /// <param name="name">data + (.json) など</param>
        /// <returns></returns>
        public static T LoadJson<T>(string pass, string name = "data")
        {
            try
            {
                string datastr = "";
                StreamReader reader;
                var filePass = "";
#if UNITY_EDITOR
                filePass = Application.dataPath + "/" + pass;
#else
            filePass = Application.persistentDataPath + "/" + pass;
#endif
                reader = new StreamReader(filePass + "/" + name + ".json");
                datastr = reader.ReadToEnd();
                reader.Close();

                return JsonUtility.FromJson<T>(datastr);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// json形式でオブジェクトを保存
        /// </summary>
        /// <param name="obj">オブジェクト</param>
        /// <param name="pass">(Assets/)+ Data + (/name) など</param>
        /// <param name="name">data + (.json) など</param>
        public static void SaveJsonDictionary<T, S>(IDictionary<T, S> dict, string pass, string name = "data")
        {
            var str = SerializeDictionaryToJson<T, S>(dict);

            //ファイルがないなら作る
            if (!System.IO.Directory.Exists(Application.dataPath + "/" + pass))
            {
                System.IO.Directory.CreateDirectory(Application.dataPath + "/" + pass);
            }

            // ファイルに保存
            var writer = new StreamWriter(Application.dataPath + "/" + pass + "/" + name + ".json", false);
            writer.Write(str);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// jsonをロード
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pass">(Assets/)+ Data/ + (name) など</param>
        /// <param name="name">data + (.json) など</param>
        /// <returns></returns>
        public static Dictionary<T, S> LoadJsonDictionary<T, S>(string pass, string name = "data")
        {
            try
            {
                string datastr = "";
                StreamReader reader;
                reader = new StreamReader(Application.dataPath + "/" + pass + "/" + name + ".json");
                datastr = reader.ReadToEnd();
                reader.Close();

                return DeserializeJsonToDictionary<T, S>(datastr);
            }
            catch
            {
                return null;
            }
        }

        public static string SerializeDictionaryToJson<T, S>(IDictionary<T, S> dict)
        {
            List<SerializableKeyValuePair<T, S>> list = new List<SerializableKeyValuePair<T, S>>();
            foreach (KeyValuePair<T, S> pair in dict)
            {
                SerializableKeyValuePair<T, S> serializablePair = new SerializableKeyValuePair<T, S>();
                serializablePair.key = pair.Key;
                serializablePair.value = pair.Value;
                list.Add(serializablePair);
            }
            ListExample<T, S> listExample = new ListExample<T, S>();
            listExample.list = list;
            return JsonUtility.ToJson(listExample);
        }

        public static Dictionary<T, S> DeserializeJsonToDictionary<T, S>(string json)
        {
            ListExample<T, S> deserializedList = JsonUtility.FromJson<ListExample<T, S>>(json);
            Dictionary<T, S> deserializedDict = new Dictionary<T, S>();
            foreach (SerializableKeyValuePair<T, S> serializablePair in deserializedList.list)
            {
                deserializedDict.Add(serializablePair.key, serializablePair.value);
            }
            return deserializedDict;
        }

        public static async UniTask SimpleTimer(Action action, float time, bool loop = true, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(loop)
            {
                while (loop)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await Utils.Delay(time);
                    action.Invoke();
                }
            }
            else
            {
                await Utils.Delay(time);
                action.Invoke();
            }
        }

        public static Vector3 GetDownPos(Vector3 vec, string[] layers, float length = 200)
        {
            return GetDownPos(new Vector2(vec.x, vec.z), layers, vec.y, length);
        }
        public static Vector3 GetDownPos(Vector2 vec, string[] layers, float startY = 100, float length = 200)
        {
            int layer = LayerMask.GetMask(layers);
            Physics.Raycast(new Vector3(vec.x, startY, vec.y), Vector3.down, out RaycastHit hit, length, layer);
            var pos = new Vector3(vec.x, startY - hit.distance, vec.y);
            return pos;
        }

        public static List<S> CastList<T,S>(this List<T> lists, Func<T,S> func)
        {
            var newList = new List<S>();
            foreach (var list in lists)
            {
                newList.Add(func(list));
            }
            return newList;
        }

        public static Vector2 GetRandomVector2(float min = 0, float max = 1)
        {
            return new Vector2(Random.Range(min, max), Random.Range(min, max));
        }
        public static Vector3 GetRandomVector3(float min = 0, float max = 1)
        {
            return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
        }

        /// <summary>
        /// 待って長押しかどうか判定する
        /// </summary>
        /// <param name="end"></param>
        /// <param name="holdedAction"></param>
        /// <returns></returns>
        public static async UniTask<bool> IsHoldedAction(this InputAction action, Action holdedAction = null)
        {
            var holded = false;
            CancellationTokenSource source = new CancellationTokenSource();
            UniTask.Action(async c => {
                c.ThrowIfCancellationRequested();
                await UniTask.WaitUntil(() => action.GetTimeoutCompletionPercentage() >= 1);
                holded = true;
                if (holdedAction != null) holdedAction?.Invoke();
            }, source.Token)();

            await UniTask.WaitUntil(() => action.WasReleasedThisFrame());
            source.Cancel();

            return holded;
        }
        public static async UniTask StopSlow(this AudioSource source, float duration = 1)
        {
            if(source.isPlaying)
            {
                var preVolume = source.volume;
                if (duration > 0)
                {
                    var t = DOVirtual.Float(preVolume, 0, duration, f => source.volume = f);
                    await Delay(duration);
                    t.Kill();
                }

                source.Stop();
                source.volume = preVolume;
            }
        }

        public static void Play(this AudioSource source, AudioClip clip)
        {
            source.clip = clip;
            source.Play();
        }

    }
}

[System.Serializable]
public class SerializableKeyValuePair<K, V>
{
    public K key;
    public V value;
}

[System.Serializable]
public class ListExample<T, S>
{
    public List<SerializableKeyValuePair<T, S>> list = new List<SerializableKeyValuePair<T, S>>();
}