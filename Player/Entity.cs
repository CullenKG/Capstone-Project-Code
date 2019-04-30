using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackDirection
{
    NorthWest,
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,

    _NumberOfDirections,
    Any,
    Null,
}

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected string m_EntityName;

    [SerializeField] public float m_MaxHealth;
    [SerializeField] public float m_CurrentHealth;

    [SerializeField] protected GameObject m_HitEffectPrefab;


    protected Vector3 m_HealthBarSize;

    Timer InvincibilityTimer;

    public bool Immortality { get; set; }

    public bool Alive = true;

    private void Awake()
    {

    }

    public virtual void Start()
    {
        InvincibilityTimer = Services.TimerManager.CreateTimer("InvincibilityTimer" + m_EntityName, 0.5f, false);
        Immortality = false;
        m_CurrentHealth = m_MaxHealth;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    private void StartInvicibility()
    {
        //Immortality = true;
        InvincibilityTimer.Restart();
    }

    public void SetImmortalityToTrue()
    {
        //Immortality = true;
    }

    public void SetImmortalityToFalse()
    {
        //Immortality = false;
    }

    protected abstract void OnHit();

    protected abstract void OnDeath();

    public virtual void TakeDamage(float aValue)
    {
        if (Immortality == false)
        {
            m_CurrentHealth -= aValue;
            OnHit();
            

            if (m_CurrentHealth <= 0 && Services.GameManager.CurrentGameState == GameState.Playing)
            {
                OnDeath();
                Alive = false;
                m_CurrentHealth = 0;
            }

            if (this.name == "Player")
            {
               // StartInvicibility();
            }

        }
    }

    public virtual void AddHealth(float aValue)
    {
        m_CurrentHealth += aValue;
    }

    public void SetCurrentHealthAsMaxHealth()
    {
        m_CurrentHealth = m_MaxHealth;
    }
}
