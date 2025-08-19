using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Character : MonoBehaviour,ISavable
{
    [Header("�����¼�")]
    public VoidEventSO newGameEvent;
    [Header("��������")]
    public float maxHealth;
    public float currentHealth;
    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed;

    [Header("�����޵�")]
    public float invulnerableDuration;//�޵�ʱ��
    [HideInInspector]public float invulnerableCounter;//��ʱ��
    public bool invulnerable;//�޵�״̬

    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage;//�����¼�
    public UnityEvent OnDie;
    private void NewGame()
    {
        currentHealth = maxHealth;
        currentPower = maxPower;
        OnHealthChange?.Invoke(this);
    }
    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
        ISavable savable = this;
        savable.RegisterSaveData();
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        ISavable savabele = this;
        savabele.UnRegisterSaveData();
    }


    private void Update()
    {
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter<=0)
            {
                invulnerable = false;
            }
        }
        if (currentPower<maxPower)
        {
            currentPower += Time.deltaTime * powerRecoverSpeed;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("water"))
        {
            if (currentHealth > 0)
            {
                currentHealth = 0;
                OnHealthChange?.Invoke(this);
                OnDie?.Invoke();
            }
        }
    }
    public void TakeDamage(Attack attacker)
    {
        if (invulnerable) return;
        if (currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            //ִ�����˵��¼�
            OnTakeDamage?.Invoke(attacker.transform);
            TriggerInvulnerable();
        }
        else
        {
            currentHealth = 0;
            //��������
            OnDie?.Invoke();
        }

        OnHealthChange?.Invoke(this);
       
    }
    private void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
    public void OnSlide(int cost)
    {
        currentPower -= cost;
        OnHealthChange?.Invoke(this);

    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID]=transform.position;
            data.floatSaveData[(GetDataID().ID + "health")] = this.currentHealth;
            data.floatSaveData[(GetDataID().ID + "power")] = this.currentPower;

        }
        else
        {
            data.characterPosDict.Add(GetDataID().ID, transform.position);
            data.floatSaveData.Add(GetDataID().ID + "health", this.currentHealth);
            data.floatSaveData.Add(GetDataID().ID + "power", this.currentPower);

        }
    }

    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPosDict[GetDataID().ID];
            this.currentHealth = data.floatSaveData[(GetDataID().ID + "health")];
            this.currentPower = data.floatSaveData[(GetDataID().ID + "power")];

            //֪ͨui����
            OnHealthChange?.Invoke(this);
        }   
    }
}
