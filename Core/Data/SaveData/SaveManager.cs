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
        /// �Z�[�u
        /// </summary>
        public void Save()
        {
            string json = JsonUtility.ToJson(saveDatas);
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(json); streamWriter.Flush();
            streamWriter.Close();
        }

        /// <summary>
        /// �Z�[�u�f�[�^�����[�h
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
        /// �S�f�[�^��������
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
        /// T�^�̃f�[�^���擾
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public T GetData<T>(int n = 0) where T : SaveData
        {
            return (T)saveDatas.FindAll(s => s is T)[n];
        }

        /// <summary>
        /// T�^�̏����f�[�^���擾
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public T GetInitData<T>(int n = 0) where T : SaveData
        {
            return (T)InitSaveDatas.FindAll(s => s is T)[n];
        }

        /// <summary>
        /// T�^�̃f�[�^���Z�b�g
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void SetData<T>(T data) where T : SaveData
        {
            var n = saveDatas.FindIndex(s => s is T);
            saveDatas[n] = data;
        }

        /// <summary>
        /// T�^�̃f�[�^��������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ClearData<T>() where T : SaveData, new()
        {
            var n = saveDatas.FindAll(s => s is T);
            n.ForEach(s => s = Instantiate(GetInitData<T>()));
        }


        /// <summary>
        /// �t�B�[���h���擾
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldData"></param>
        public object GetField(GetField fieldData)
        {
            return fieldData.GetValueFromList(saveDatas);
        }
        /// <summary>
        /// �t�B�[���h��T�^�Ŏ擾
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldData"></param>
        public T GetField<T>(GetField fieldData) where T : class
        {
            return fieldData.GetValueFromList(saveDatas) as T;
        }

        /// <summary>
        /// �t�B�[���h���Z�b�g
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldData"></param>
        public void SetField(SetField fieldData)
        {
            fieldData.SetValue(saveDatas);
        }

        /// <summary>
        /// �t�B�[���h��string�^��subscribe
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
