using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable 
{
    DataDefination GetDataID();
    void RegisterSaveData()=>DataManager.instance.RegisterSaveDate(this);

    void UnRegisterSaveData() => DataManager.instance.UnRegisterSaveData(this);

    void GetSaveData(Data data);

    void LoadData(Data data);
}
