using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(order:-100)]
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    [Header("ÊÂ¼þ¼àÌý")]
    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDataEvent;

    private List<ISavable> savableList = new List<ISavable>();

    private Data saveData;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        saveData = new Data();

    }
    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += Save;
        loadDataEvent.OnEventRaised += Load;
    }

    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
        loadDataEvent.OnEventRaised -= Load;


    }
    private void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
        }
    }

    public void RegisterSaveDate(ISavable saveable)
    {
        if (!savableList.Contains(saveable))
        {
            savableList.Add(saveable);
        }
    }

    public void UnRegisterSaveData(ISavable savable)
    {
        savableList.Remove(savable); 
    }

    public void Save()
    {
        foreach(var savable in savableList)
        {
            savable.GetSaveData(saveData);
        }

        foreach(var item in saveData.characterPosDict)
        {
            Debug.Log(item.Key + "   " + item.Value);
        }
    }

    public void Load()
    {
        foreach(var savable in savableList)
        {
            savable.LoadData(saveData);
        }
    }
}
