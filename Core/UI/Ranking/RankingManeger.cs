using Shin_UnityLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class RankingManeger : MonoBehaviour, RankingInterface
{
    public string pass = "Shin_UnityLibrary/Core/Shin_UnityLibrary_Data";
    public string dataName = "RankingData";
    public bool autoSave = true;

    [Space]
    public int canCount = 5;

    [System.Serializable]
    public class AllRankingData
    {
        public List<RankingData> rankingDatas = new List<RankingData>();
    }

    public AllRankingData alldata = new AllRankingData();

    public void Awake()
    {
        Load();
        if(alldata == null) alldata = new AllRankingData();
        if(alldata.rankingDatas == null) alldata.rankingDatas = new List<RankingData>();
    }

    public void Add(RankingData data)
    {
        if(alldata.rankingDatas.Count <= canCount || canCount == -1)
        {
            alldata.rankingDatas.Add(data);
        }
        else
        {
            var min = GetMin();
            if(min != null)
            {
                min = data;
            }
        }
        Sort();
        if(autoSave) Save();
    }
    [ContextMenu("AllDelete")]
    public void AllDelete()
    {
        alldata.rankingDatas = new();
        if (autoSave) Save();
    }
    public void Delete(RankingData data)
    {
        alldata.rankingDatas.Remove(data);
        if (autoSave) Save();
    }

    public void Save()
    {
        Utils.SaveJson(alldata, pass , dataName);
    }
    public void Load()
    {
        alldata = Utils.LoadJson<AllRankingData>(pass, dataName);
    }

    public void Sort()
    {
        alldata.rankingDatas.Sort((d1, d2) => d2.data.num.value - d1.data.num.value);
        for(int i = 0; i < alldata.rankingDatas.Count; i++)
        {
            alldata.rankingDatas[i].data.rank.value = i;
        }
    }

    public RankingData GetMin()
    {
        int n = -1;
        RankingData rankingData = null;
        foreach (RankingData data in alldata.rankingDatas)
        {
            if(n == -1 ||  data.data.num.value < n)
            {
                n = data.data.num.value;
                rankingData = data;
            }
        }
        return rankingData;
    }

    public bool Get(int n, out RankingData data)
    {
        if(n >= alldata.rankingDatas.Count)
        {
            data = null;
            return false;
        }
        else
        {
            data = alldata.rankingDatas[n];
            return true;
        }
    }

    public void DataReset()
    {
        AllDelete(); Save();
    }
}
