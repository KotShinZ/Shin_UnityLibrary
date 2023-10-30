using Shin_UnityLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UniRx;
using UnityEngine;

public interface RankingInterface
{
    public void Add(RankingData data);
    public void Delete(RankingData data);
    public void AllDelete();
}

[System.Serializable]
public class RankingData
{
    public DataSet data = new(0,0);

    public RankingData(int n) { data.num.value = n; }

    [System.Serializable]
    public struct DataSet
    {
        public BaseDataInt rank;
        public BaseDataInt num;

        public DataSet(int _rank, int _num) { rank = new(_rank); num = new(_num); }
    }
}
