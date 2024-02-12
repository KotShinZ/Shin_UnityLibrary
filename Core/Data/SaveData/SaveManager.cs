using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UniRx;

namespace Shin_UnityLibrary
{
    public class SaveManager : SingletonMonoBehaviour<SaveManager>
    {
        string filePath;
        public List<SaveData> InitSaveDatas = new();
        public List<SaveData> saveDatas = new();
        public bool isInitData = true;

        protected override void Init()
        {
            base.Init();
            filePath = Application.persistentDataPath + "/" + ".savedata.json";
            if (!isInitData)
            {
                Load();
            }
            else { Delete(); }

            if (saveDatas == null || saveDatas.Count == 0) Delete();
        }

        /// <summary>
        /// セーブ
        /// </summary>
        public void Save()
        {
            string json = JsonUtility.ToJson(saveDatas);
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(json); streamWriter.Flush();
            streamWriter.Close();
        }

        /// <summary>
        /// セーブデータをロード
        /// </summary>
        public void Load()
        {
            if (File.Exists(filePath))
            {
                StreamReader streamReader;
                streamReader = new StreamReader(filePath);
                string data = streamReader.ReadToEnd();
                streamReader.Close();
                saveDatas = JsonUtility.FromJson<List<SaveData>>(data);
            }
        }


        /// <summary>
        /// 全データを初期化
        /// </summary>
        public void Delete()
        {
            saveDatas = new();
            foreach (SaveData data in InitSaveDatas)
            {
                var ins = Instantiate<SaveData>(data);
                saveDatas.Add(ins);
            }
            Save();
        }


        /// <summary>
        /// T型のデータを取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public T GetData<T>(int n = 0) where T : SaveData
        {
            return (T)saveDatas.FindAll(s => s is T)[n];
        }

        /// <summary>
        /// T型の初期データを取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public T GetInitData<T>(int n = 0) where T : SaveData
        {
            return (T)InitSaveDatas.FindAll(s => s is T)[n];
        }

        /// <summary>
        /// T型のデータをセット
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void SetData<T>(T data) where T : SaveData
        {
            var n = saveDatas.FindIndex(s => s is T);
            saveDatas[n] = data;
        }

        /// <summary>
        /// T型のデータを初期化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ClearData<T>() where T : SaveData, new()
        {
            var n = saveDatas.FindAll(s => s is T);
            n.ForEach(s => s = Instantiate(GetInitData<T>()));
        }


        /// <summary>
        /// フィールドを取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldData"></param>
        public object GetField(GetField fieldData)
        {
            return fieldData.GetValueFromList(saveDatas);
        }
        /// <summary>
        /// フィールドをT型で取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldData"></param>
        public T GetField<T>(GetField fieldData) where T : class
        {
            return fieldData.GetValueFromList(saveDatas) as T;
        }

        /// <summary>
        /// フィールドをセット
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldData"></param>
        public void SetField(SetField fieldData)
        {
            fieldData.SetValue(saveDatas);
        }

        /// <summary>
        /// フィールドにstring型でsubscribe
        /// </summary>
        /// <param name="fieldData"></param>
        public void SubscribeField(GetField fieldData, IObserver<string> observer)
        {
            var data = fieldData.GetValueFromList(saveDatas);
            var f = data as IObservableStr;
            f.SubscribeToString(observer);
        }

        /*public void SaveGame()
        {
            GetData<GamingSaveData>().gamingPlayerData = GameManeger.player.data;
            Save();
        }*/
    }
}
