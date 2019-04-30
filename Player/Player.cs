using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    public PlayerCombatManager CombatManager { get; private set; }
    public PlayerMovementController MovementController { get; private set; }
    public FPSCameraController CameraController { get; private set; }
    public Animator Animator { get; private set; }

    public GameObject m_camera;

    public delegate void DeathDelegate();
    public event DeathDelegate onDeath;

    Timer m_FocusTimer;
    Timer m_PlayerInvincibilityTimer;

    public int SizeOfFocusChunks = 40;
    public int SizeOfStaminaChunks = 40;

    public float m_StaminaRegenAmount = 18;

    public float m_CurrentFocus;
    public float m_MaxFocus = 120;
    public float m_AmountOfFocusChunks;

    public float m_CurrentStamina;
    public float m_MaxStamina = 120;
    public float m_AmountOfStaminaChunks;

    public bool m_InCutscene;
    public bool m_TutorialUseFocus;
    public bool m_InAcid;
    public bool m_OnFire;

    float HealTime = 0.0f;

    public Tutorial m_Tutorial;

    Quaternion InitalRotation;
    Quaternion InitalCameraRotation;

    CheckPoint LastCheckPoint = null;

    // Setting Reference to images for the bars 
    public Image m_Image_HealthBar;

    //Setting reference
    public Text m_Text_HealthValue;

    public GameObject Prefab_FocusChunk;
    public GameObject Prefab_StaminaChunk;
    public GameObject Canvas_Focus;
    public GameObject Canvas_Stamina;
    public GameObject m_FocusParticleEffect;

    public GameObject m_AcidScreen;
    public GameObject m_FireScreen;
    public GameObject m_DamageScreen;
    public GameObject m_DamageScreenLessThan50;
    public GameObject m_DamageScreenLessThan25;





    float LerpStart;

    public List<BarChunks> BarChunksFocus;
    public List<BarChunks> BarChunksStamina;
    public List<GameObject> m_ParticleEffectHeal;

    float timeSinceLastDamageFlash;
    float damageflashTime = 0.7f;

    bool m_isDead = false;

    void Awake()
    {
        Animator = GetComponentInChildren<Animator>();

        CombatManager = GetComponent<PlayerCombatManager>();
        MovementController = GetComponent<PlayerMovementController>();
        CameraController = GetComponent<FPSCameraController>();

        InitalRotation = transform.rotation;
        LerpStart = 0.0f;
        // InitalCameraRotation = camera.transform.rotation;

        timeSinceLastDamageFlash = 0.0f;

        Services.GameManager.RegisterPlayer(this);
    }

    public override void Start()
    {
        base.Start();

        m_HealthBarSize.Set(4.2f, 1.2f, 0.9f);
        m_MaxHealth = 500;
        m_MaxFocus = 120;
        m_MaxStamina = 120;

        m_InAcid = false;
        m_OnFire = false;

        m_FocusTimer = Services.TimerManager.CreateTimer("Focus", Constants.HealDelay, false);
        m_PlayerInvincibilityTimer = Services.TimerManager.CreateTimer("m_PlayerInvincibilityTimer", 0.4f, false);
        //Setting the current values to max
        m_CurrentStamina = m_MaxStamina;
        m_CurrentHealth = m_MaxHealth;
        m_CurrentFocus = 0;

        //Dividing the Maxfocus with how many chunks it will need to make
        m_AmountOfFocusChunks = m_MaxFocus / SizeOfFocusChunks;

        // Find the toad boss

        //Dividing the Maxfocus with how many chunks it will need to make
        m_AmountOfStaminaChunks = m_MaxStamina / SizeOfStaminaChunks;

        ////For loop to set up all the chunks for Focus
        for (int i = (int)m_AmountOfStaminaChunks; i > 0; i--)
        {
            //Setting up a temp value
            GameObject Temp = Instantiate<GameObject>(Prefab_StaminaChunk, Canvas_Stamina.gameObject.transform);
            //Setting its position to a place on screen
            Temp.transform.localPosition = new Vector3(46 * i - 525, -220, 0);
            //Giving the Chunk a Maximum value and a reference to the player
            Temp.GetComponent<BarChunks>().SetUpChunk(SizeOfStaminaChunks * i, this, BarChunks.BarChunktype.Stamina);

            //Giving the List Of BarChunks the temp variable
            BarChunksStamina.Add(Temp.GetComponent<BarChunks>());
        }

        //For loop to set up all the chunks for Focus
        for (int i = (int)m_AmountOfFocusChunks; i > 0; i--)
        {
            //Setting up a temp value
            GameObject Temp = Instantiate<GameObject>(Prefab_FocusChunk, Canvas_Focus.gameObject.transform);
            //Setting its position to a place on screen
            Temp.transform.localPosition = new Vector3(46 * i - 525, -195, 0);
            //Giving the Chunk a Maximum value and a reference to the player
            Temp.GetComponent<BarChunks>().SetUpChunk(SizeOfFocusChunks * i, this, BarChunks.BarChunktype.Focus);

            //Giving the List Of BarChunks the temp variable
            BarChunksFocus.Add(Temp.GetComponent<BarChunks>());
        }

    }

    public void SetIsInAcid(bool aInAcid)
    {
        m_InAcid = aInAcid;
    }

    public void SetIsOnFire(bool aOnFire)
    {
        m_OnFire = aOnFire;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        //Updates the healthbar
        UpdateHealthBar();

        m_CurrentStamina += m_StaminaRegenAmount * Time.deltaTime;
        m_CurrentStamina = Mathf.Clamp(m_CurrentStamina, 0, m_MaxStamina);
        m_CurrentHealth = (int)Mathf.Clamp(m_CurrentHealth, 0, m_MaxHealth);
        m_CurrentFocus = (int)Mathf.Clamp(m_CurrentFocus, 0, m_MaxFocus);

        //checks if the player is in acid water to check if we should show the poison screen overlay
        if (m_InAcid == true)
        {
            m_AcidScreen.gameObject.SetActive(true);
        }
        else
        {
            m_AcidScreen.gameObject.SetActive(false);
        }

        if (m_OnFire == true)
        {
            m_FireScreen.gameObject.SetActive(true);
        }
        else
        {
            m_FireScreen.gameObject.SetActive(false);
        }



        if (Services.InputManager.GetActionDown(InputAction.Heal) && !m_FocusTimer.IsRunning())
        {
            if (m_CurrentFocus > 0)
            {
                HealTime += 1.5f;
                m_FocusTimer.StartTimer();
                m_CurrentFocus -= 40;

                // Play heal sfx
                Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Heal);
            }
        }


        if (timeSinceLastDamageFlash > damageflashTime)
        {
            if (m_DamageScreen.activeSelf)
            {
                float progress = Time.time - LerpStart;
                Color tempColor = m_DamageScreen.GetComponent<Image>().color;
                tempColor.a = Mathf.Lerp(1.0f, 0, progress / 0.3f);
                m_DamageScreen.GetComponent<Image>().color = tempColor;

                if (progress > 0.3f)
                {
                    tempColor.a = 1.0f;
                    m_DamageScreen.GetComponent<Image>().color = tempColor;

                    m_DamageScreen.SetActive(false);
                    timeSinceLastDamageFlash = 0.0f;
                }
            }
        }

        Debug.Log(HealTime);

        if (HealTime>0.0f)
        {
            m_CurrentHealth += 75 * Time.deltaTime;
            HealTime -= Time.deltaTime;
        }

        if (m_CurrentHealth <= m_MaxHealth / 2)
        {
            m_DamageScreenLessThan50.SetActive(true);
        }
        else
        {
            m_DamageScreenLessThan50.SetActive(false);
        }

        if (m_CurrentHealth <= m_MaxHealth / 4)
        {
            m_DamageScreenLessThan25.SetActive(true);
        }
        else
        {
            m_DamageScreenLessThan25.SetActive(false);
        }



        if (m_FocusTimer.OnFinish())
        {
            // m_CurrentFocus -= 40;
            //m_CurrentHealth += 300;
        }

        if (!m_DamageScreen.activeSelf)
        {
            timeSinceLastDamageFlash += Time.deltaTime;
        }

        Animator.gameObject.transform.localEulerAngles = Vector3.zero;
    }

    void UpdateHealthBar()
    {
        //Gets the ratio of the healths maximum possible and current.
        float HealthRatio = (float)m_CurrentHealth / (float)m_MaxHealth;

        if (m_Image_HealthBar != null)
        {
            m_Image_HealthBar.fillAmount = HealthRatio;
        }
        if (m_Text_HealthValue != null)
        {
            m_Text_HealthValue.text = m_CurrentHealth.ToString();
        }
    }

    public void AddFocusChunk()
    {
        m_MaxFocus += SizeOfFocusChunks;
        m_CurrentFocus = m_MaxFocus;
        int chunkcount = BarChunksFocus.Count + 1;
        //Setting up a temp value
        GameObject Temp = Instantiate<GameObject>(Prefab_FocusChunk, Canvas_Focus.gameObject.transform);
        //Setting its position to a place on screen
        Temp.transform.localPosition = new Vector3(41 * chunkcount - 400, -180, 0); ;
        //Giving the Chunk a Maximum value and a reference to the player
        Temp.GetComponent<BarChunks>().SetUpChunk(SizeOfFocusChunks * chunkcount, this, BarChunks.BarChunktype.Focus);
        //Giving the List Of BarChunks the temp variable
        BarChunksFocus.Add(Temp.GetComponent<BarChunks>());
    }

    public void AddStaminaChunk()
    {
        m_MaxStamina += SizeOfStaminaChunks;
        m_CurrentStamina = m_MaxFocus;
        int chunkcount = BarChunksFocus.Count + 1;
        //Setting up a temp value
        GameObject Temp = Instantiate<GameObject>(Prefab_StaminaChunk, Canvas_Stamina.gameObject.transform);
        //Setting its position to a place on screen
        Temp.transform.localPosition = new Vector3(41 * chunkcount - 400, -210, 0);
        //Giving the Chunk a Maximum value and a reference to the player
        Temp.GetComponent<BarChunks>().SetUpChunk(SizeOfStaminaChunks * chunkcount, this, BarChunks.BarChunktype.Stamina);
        //Giving the List Of BarChunks the temp variable
        BarChunksStamina.Add(Temp.GetComponent<BarChunks>());
    }


    public void AddFocus(float a_FocusBy)
    {
        m_CurrentFocus += a_FocusBy;
    }

    protected override void OnHit()
    {
        if (timeSinceLastDamageFlash >= damageflashTime && m_DamageScreen.activeSelf == false)
        {
            m_DamageScreen.SetActive(true);

            //Player hurt sfx
            Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Hurt);

            LerpStart = Time.time;
        }
    }

    protected override void OnDeath()
    {
        // Play player death sfx
        if (m_isDead == false)
        {
            m_isDead = true;
            Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Death_Chord);
            Services.GameManager.PlayerWon = false;
            Services.UIManager.PushScreen(UIManager.Screen.GameEnd);
            Debug.Log("death function being called");

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CheckPoint")
        {
            LastCheckPoint = other.GetComponent<CheckPoint>();
            LastCheckPoint.ActivateCheckPoint();
        }

        if (other.tag == "Tongue")
        {
            if (m_PlayerInvincibilityTimer.IsRunning() == false)
            {
                TakeDamage(15);
                m_PlayerInvincibilityTimer.Restart();

                // TODO: Play player hurt sound
            }
        }
    }

    public void Respawn()
    {
        m_isDead = false;
        m_CurrentHealth = m_MaxHealth;
        m_CurrentStamina = m_MaxStamina;
        transform.position = LastCheckPoint.transform.position;
        GetComponent<PlayerMovementController>().OutsideForce = Vector3.zero;
        transform.rotation = InitalRotation;

        if (Services.GameManager.OdelBossTriggered)
        {
            if (!Services.GameManager.Odeldefeated)
            {
                Services.GameManager.ToadBoss.Reset();
            }
        }

        if (Services.GameManager.KingTriggered)
        {
            if (!Services.GameManager.Kingdefeated)
            {
                Services.GameManager.KingBoss.Reset();
            }
        }
        MovementController.m_playerMovementDissabled = false;
        CameraController.Respawn();
    }
}
