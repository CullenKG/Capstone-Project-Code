using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIToadController : AIController
{
    public enum Behaviour
    {
        Aggressive,
        Defensive,
        Passive,
        Cinematic,
        Presentation
    }

    public enum Action
    {
        None,
        ProjectileAttack,
        ProjectileMines,
        TongueWhip,
        Jump,
        Charge,
        SpinHead,
        Stomp,
        Turn,
        Buck
    }

    // GameObjects
    GameObject m_Prefab_Projectile_Reference;

    public GameObject m_Prefab_Projectile_Reference_TL;
    public GameObject m_Prefab_Projectile_Reference_TR;
    public GameObject m_Prefab_Projectile_Reference_HOR;
    public GameObject m_Prefab_Projectile_Reference_VERT;

    public GameObject m_Prefab_ProjectileRain_Reference;

    public GameObject m_MineSplitPosition;
    public GameObject m_ProjectileSpawnPoint_Reference;

    public GameObject m_ToadLips;
    public GameObject m_Tongue;
    public GameObject m_PlayerWatcher;
    public GameObject m_TongueGrabGameOject;

    public GameObject m_PlayerCamera;
    public GameObject m_CutsceneCamera;

    // Jump Positions
    public GameObject[] m_JumpPosition;

    // Mine Positions
    public GameObject[] m_MinePositions;

    // Pillars
    public GameObject[] m_Pillars;

    public TriggerBoxObject m_PlayerRider;

    // Text (used for debugging)
    public Text m_BossActionText;
    public Text m_BossDecidedActionText;
    public Text m_PlayerDistanceText;

    // Projectiles 
    public List<BasicProjectile> m_ProjectilesAttackList;
    public List<MineProjectiles> m_ProjectilesMineList;
    public List<ToadWhipCollider> m_WhipColliders;

    string[] m_ToadAttackMoves;
    string[] EnemyActionNames = System.Enum.GetNames(typeof(Action));
    public string[] GetEnemyActionNames() { return EnemyActionNames; }


    Timer m_AttackCooldownTimer;
    public Animator m_Animator;

    // Varriables
    public int m_JumpID;
    public int m_NumberOfAttacks = 0;
    public int m_PillarToGoTo;

    public bool m_HitPlayer;
    public bool m_IsOnPillar;
    public bool m_GoToPillar;

    public float m_NextHealthThreshhold;
    public float m_CurrentHealth;
    public float m_MaxHealth;
    public float m_ActTimer;

    public string m_BossName;

    // Text (some are used for debugging)
    public Text m_BossText;
    //public Text m_BossActionText;
    //public Text m_BossDecidedActionText;

    // Colliders
    public SphereCollider m_BodyCollider;
    public BoxCollider m_ClimbBlocker;
    public ToadTongueWhipCube m_ToadTongueWhipCube;

    // Scripts
    public Player m_Player;
    public ToadBoss m_ToadBoss;
   

    Quaternion initalRotaton;

    Vector3 m_HealthBarSize;
    // Use this for initialization
    void Start()
    {
        m_AttackCooldownTimer = Services.TimerManager.CreateTimer("AttackCooldownTimer", 1.5f, false);

        m_Animator = GetComponentInChildren<Animator>();
        initalRotaton = transform.rotation;
        m_HealthBarSize.Set(0.65765f, 0.188084f, 0.188084f);

        m_BossName = "Odell the Abomination";

        m_BossText.text = m_BossName;
        //m_BossDecidedActionText = GameObject.Find("Text_BossDecidedAction").GetComponent<Text>();
        //m_BossActionText = GameObject.Find("Text_BossAction").GetComponent<Text>();

        m_ToadTongueWhipCube = GameObject.Find("ToadParryCube").GetComponent<ToadTongueWhipCube>();
        m_ToadTongueWhipCube.gameObject.SetActive(false);

        m_MaxHealth = m_ToadBoss.m_MaxHealth;
        m_CurrentHealth = m_ToadBoss.GetCurrentHealth();
        m_NextHealthThreshhold = (m_MaxHealth / 4) * 3;
        m_GoToPillar = false;

        // Get a random starting pillar
        m_PillarToGoTo = Random.Range(1, 4);

        Services.GameManager.RegisterToad(this);

        // Player Watcher Setup
        m_PlayerWatcher = GameObject.Find("PlayerWatcher");

        m_PlayerCamera = GameObject.Find("PlayerCamera");
        //m_CutsceneCamera = GameObject.Find("CutsceneCamera");

        // Face and Lips Setup
        m_ToadLips = GameObject.Find("Lips");

        //Tongue Setup
        m_Tongue = GameObject.Find("Tongue");

        m_TongueGrabGameOject = GameObject.Find("TongueGrabCollisionBox");
        m_MineSplitPosition = GameObject.Find("MineSplitPosition");

        m_ToadBoss = gameObject.GetComponent<ToadBoss>();

        // Jump Position initialization
        m_JumpPosition[0] = GameObject.Find("PillarJumpPosition1");
        m_JumpPosition[1] = GameObject.Find("PillarJumpPosition2");
        m_JumpPosition[2] = GameObject.Find("PillarJumpPosition3");
        m_JumpPosition[3] = GameObject.Find("GroundJumpPosition1");
        m_JumpPosition[4] = GameObject.Find("GroundJumpPosition2");
        m_JumpPosition[5] = GameObject.Find("GroundJumpPosition3");
        m_JumpPosition[6] = GameObject.Find("GroundJumpPosition4");
        m_JumpPosition[7] = GameObject.Find("GroundJumpPosition5");
        m_JumpPosition[8] = GameObject.Find("GroundJumpPosition6");

        // Mine Position initialization
        m_MinePositions[0] = GameObject.Find("MineSplitPosition");
        m_MinePositions[1] = GameObject.Find("MinePosition1");
        m_MinePositions[2] = GameObject.Find("MinePosition2");
        m_MinePositions[3] = GameObject.Find("MinePosition3");
        m_MinePositions[4] = GameObject.Find("MinePosition4");
        m_MinePositions[5] = GameObject.Find("MinePosition5");
        m_MinePositions[6] = GameObject.Find("MinePosition6");
        m_MinePositions[7] = GameObject.Find("MinePosition7");
        m_MinePositions[8] = GameObject.Find("MinePosition8");
        m_MinePositions[9] = GameObject.Find("MinePosition9");
        m_MinePositions[10] = GameObject.Find("MinePosition10");
        m_MinePositions[11] = GameObject.Find("MinePosition11");

        for (int i = 0; i < m_WhipColliders.Count; i++)
        {
            m_WhipColliders[i].gameObject.SetActive(false);
        }
      
        m_Prefab_Projectile_Reference_VERT.GetComponent<BasicProjectile>().m_FirstDamageDirection = AttackDirection.North;
        m_Prefab_Projectile_Reference_VERT.GetComponent<BasicProjectile>().m_SecondDamageDirection = AttackDirection.South;

        m_Prefab_Projectile_Reference_HOR.GetComponent<BasicProjectile>().m_FirstDamageDirection = AttackDirection.East;
        m_Prefab_Projectile_Reference_HOR.GetComponent<BasicProjectile>().m_SecondDamageDirection = AttackDirection.West;

        m_Prefab_Projectile_Reference_TL.GetComponent<BasicProjectile>().m_FirstDamageDirection = AttackDirection.NorthWest;
        m_Prefab_Projectile_Reference_TL.GetComponent<BasicProjectile>().m_SecondDamageDirection = AttackDirection.SouthEast;

        m_Prefab_Projectile_Reference_TR.GetComponent<BasicProjectile>().m_FirstDamageDirection = AttackDirection.NorthEast;
        m_Prefab_Projectile_Reference_TR.GetComponent<BasicProjectile>().m_SecondDamageDirection = AttackDirection.SouthWest;

        for (int i = 0; i < 4; i++)
        {

            if (i == 0)
            {
                m_ProjectilesAttackList.Add(Instantiate<BasicProjectile>(m_Prefab_Projectile_Reference_HOR.GetComponent<BasicProjectile>(), m_ProjectileSpawnPoint_Reference.transform));
            }
            if (i == 1)
            {
                m_ProjectilesAttackList.Add(Instantiate<BasicProjectile>(m_Prefab_Projectile_Reference_TL.GetComponent<BasicProjectile>(), m_ProjectileSpawnPoint_Reference.transform));
            }
            if (i == 2)
            {
                m_ProjectilesAttackList.Add(Instantiate<BasicProjectile>(m_Prefab_Projectile_Reference_VERT.GetComponent<BasicProjectile>(), m_ProjectileSpawnPoint_Reference.transform));
            }
            if (i == 3)
            {
                m_ProjectilesAttackList.Add(Instantiate<BasicProjectile>(m_Prefab_Projectile_Reference_TR.GetComponent<BasicProjectile>(), m_ProjectileSpawnPoint_Reference.transform));
            }

            Debug.Log(i);

            // Making a pool of projectiles to spawn
            // m_ProjectilesAttackList.Add(Instantiate<BasicProjectile>(m_Prefab_Projectile_Reference.GetComponent<BasicProjectile>(), m_ProjectileSpawnPoint_Reference.transform));


            Debug.Log(m_ProjectilesAttackList[i].GetComponent<BasicProjectile>().m_FirstDamageDirection);
            //Turning the Objects off 
            m_ProjectilesAttackList[i].gameObject.SetActive(false);
            m_ProjectilesAttackList[i].transform.parent = null;
            m_ProjectilesAttackList[i].m_HasDied = true;
        }

        for (int i = 0; i < 7; i++)
        {
            //Making a pool of projectiles to spawn
            m_ProjectilesMineList.Add(Instantiate<MineProjectiles>(m_Prefab_ProjectileRain_Reference.GetComponent<MineProjectiles>()));
            //Turning the Objects off 
            m_ProjectilesMineList[i].gameObject.SetActive(false);
            m_ProjectilesMineList[i].transform.parent = null;
        }
        
        m_AIBehaviours.Add((int)Behaviour.Aggressive, new ToadAgressiveBehaviour(this));
        m_AIBehaviours.Add((int)Behaviour.Defensive, new ToadDefensiveBehaviour(this));
        m_AIBehaviours.Add((int)Behaviour.Passive, new ToadPassiveBehaviour(this));
        m_AIBehaviours.Add((int)Behaviour.Cinematic, new ToadCutsceneBehaviour(this));
        m_AIActions.Add(((int)Action.ProjectileMines), new ToadProjectileMines(this));
        m_AIActions.Add(((int)Action.ProjectileAttack), new ToadProjectileAttack(this));
        m_AIActions.Add(((int)Action.SpinHead), new ToadSpinHead(this));
        m_AIActions.Add(((int)Action.Stomp), new ToadStomp(this));
        m_AIActions.Add(((int)Action.Charge), new ToadCharge(this));
        m_AIActions.Add(((int)Action.Turn), new ToadTurn(this));
        m_AIActions.Add(((int)Action.Jump), new ToadJump(this));
        m_AIActions.Add(((int)Action.TongueWhip), new ToadTongueWhip(this));
        m_AIActions.Add(((int)Action.Buck), new ToadBuck(this));
        m_AIActions.Add(((int)Action.None), new ToadNone(this));

        SetDecidedAction((int)AIToadController.Action.None);
        SetAction((int)AIToadController.Action.None);
        SetNextAction((int)AIToadController.Action.None);
        SetBehaviour((int)AIToadController.Behaviour.Passive);
    }

    protected override void Update()
    {
        base.Update();

       // ShowDebugStuff();

        if (GetDistanceToPlayer() < 35.0f && m_AiActive == false)
        {
            m_AiActive = true;
            SetAction((int)AIToadController.Action.None);
            SetBehaviour((int)AIToadController.Behaviour.Aggressive);
            m_MakeDecision = true;

            // Initialize toad boss scipt values
            m_MaxHealth = m_ToadBoss.m_MaxHealth;
            m_NextHealthThreshhold = (m_MaxHealth / 4) * 3;
        }

        // Enable the AI with button press
        //if (Input.GetKeyDown("m"))
        //{
        //    m_AiActive = true;
        //    SetAction((int)AIToadController.Action.ProjectileMines);
        //    //SetBehaviour((int)AIToadController.Behaviour.Passive);
        //}

        // Get the current health from the entity script
        m_CurrentHealth = m_ToadBoss.GetCurrentHealth();

        // If the AI is active
        if (m_AiActive == true)
        {         
            // And the boss is on the pillars and was hit. (Will be after mark crit is complete)
            if (IsCurrentBehaviour((int)AIToadController.Behaviour.Defensive) && m_IsOnPillar == true && m_ToadBoss.m_HasBeenHit == true)
            {
                // Then set the player back to ground behaviour
                // SetAction((int)AIToadController.Action.Stagger);
                SetNextAction((int)AIToadController.Action.Jump);
                SetBehaviour((int)AIToadController.Behaviour.Aggressive);

                // Get the pillar to decend
                m_Pillars[m_PillarToGoTo - 2].GetComponent<Animator>().SetBool("B_IsStairsDown", false);
            }

            // If the toad reaches the next health threshhold
            if (m_CurrentHealth <= m_NextHealthThreshhold)
            {
                m_NextHealthThreshhold -= (m_MaxHealth / 4);
                m_GoToPillar = true;
                SetNextAction((int)AIToadController.Action.Stomp);
                SetBehaviour((int)AIToadController.Behaviour.Defensive);

                // Get the pillar to rise
                m_Pillars[m_PillarToGoTo - 1].GetComponent<Animator>().SetBool("B_IsStairsDown", true);
            }
        }

        // If the boss hit the player
        if (m_HitPlayer == true)
        {
            // Set hit to false so it only will be true for one frame
            m_HitPlayer = false;
        }

        // Reset whether or not the toad has been hit the toad was hit
        if (m_ToadBoss.m_HasBeenHit == true)
        {
            m_ToadBoss.m_HasBeenHit = false;
        }

    }

    public float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, Services.GameManager.Player.transform.position);
    }

    public void CalculateJumpID()
    {
        // if the toad is on the ground
        if (IsCurrentBehaviour((int)AIToadController.Behaviour.Aggressive))
        {
            // Set a high distance so the while loop will work
            //float Distance = 0f;

            // Save the ID to compare
            int IDCheck = m_JumpID;

            m_JumpID = 3;

        }
        // if the toad is on the pillars
        else if (IsCurrentBehaviour((int)AIToadController.Behaviour.Defensive))
        {
            m_JumpID = 0;

            if (m_PillarToGoTo == 1)
            {
                m_JumpID = 0;
                m_PillarToGoTo += 1;
            }
            else if (m_PillarToGoTo == 2)
            {
                m_JumpID = 1;
                m_PillarToGoTo += 1;
            }
            else if (m_PillarToGoTo == 3)
            {
                m_JumpID = 2;
                m_PillarToGoTo = 1;
            }

            //// Save the ID to compare
            //int IDCheck = m_JumpID;

            //// Get an ID that isin't the current ID
            //while (IDCheck == m_JumpID)
            //{
            //    // Get a random number between for the pillar jump positions. There are 3 pillar positions
            //    m_JumpID = Random.Range(0, 3);
            //}
        }
    }

    public void DebugSwitchEnemyAction(int a_EnemyAction)
    {
        if (a_EnemyAction == 0)
        {
            SetAction((int)Action.None);
        }
        if (a_EnemyAction == 1)
        {
            SetAction((int)Action.ProjectileAttack);
        }
        if (a_EnemyAction == 2)
        {
            SetAction((int)Action.ProjectileMines);
        }
        if (a_EnemyAction == 3)
        {
            SetAction((int)Action.TongueWhip);
        }
        if (a_EnemyAction == 4)
        {
            SetAction((int)Action.Jump);
        }
        if (a_EnemyAction == 5)
        {
            SetAction((int)Action.Charge);
        }
        if (a_EnemyAction == 6)
        {
            SetAction((int)Action.SpinHead);
        }
        if (a_EnemyAction == 7)
        {
            SetAction((int)Action.Stomp);
        }
        if (a_EnemyAction == 8)
        {
            SetAction((int)Action.Turn);
        }

        SetCurrentActionAsDecidedAction();

        // If the AI isint on, turn it on
        if (m_AiActive == false)
        {
            m_AiActive = true;
            SetBehaviour((int)AIToadController.Behaviour.Aggressive);
            //SetCurrentActionAsDecidedAction();
        }
    }

    public bool CanSeePlayer()
    {
        bool CanSee = false;

        // Get the player watcher to look at the Player
        m_PlayerWatcher.transform.LookAt(Services.GameManager.Player.gameObject.transform);

        float angle = Quaternion.Angle(transform.rotation, m_PlayerWatcher.transform.rotation);

        if (angle <= 50 || angle >= 310)
        {
            CanSee = true;
        }

        return CanSee;
    }

    public void EnableColliders(bool enable)
    {
        m_BodyCollider.enabled = enable;
        m_ClimbBlocker.enabled = enable;
    }

    public void PushPlayer(float force, float radius)
    {
        // Push the player away
        Services.GameManager.Player.MovementController.PushPlayer(transform.position, force);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (IsCurrentAction((int)AIToadController.Action.Charge))
        {
                m_HitPlayer = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (IsCurrentAction((int)AIToadController.Action.None))
        {
            m_HitPlayer = true;
        }
    }

    public void Reset()
    {
        CurrentBehaviour.OnActionFinished();
        SetAction(((int)AIToadController.Action.None));
        SetDecidedAction(((int)AIToadController.Action.None));
        SetBehaviour(((int)AIToadController.Behaviour.Passive));

        m_MakeDecision = false;
        m_AiActive = false;
        EnableColliders(true);

        m_NumberOfAttacks = 0;
        m_HitPlayer = false;
        m_GoToPillar = false;

        m_ToadBoss.TargetForComboFront.SetActive(true);
        m_ToadBoss.TargetForComboFront.GetComponent<ComboTarget>().Reset();
        m_ToadBoss.TargetForComboLeft.SetActive(true);
        m_ToadBoss.TargetForComboLeft.GetComponent<ComboTarget>().Reset();
        m_ToadBoss.TargetForComboRight.SetActive(true);
        m_ToadBoss.TargetForComboRight.GetComponent<ComboTarget>().Reset();

        m_ToadBoss.m_Marked = false;

        transform.position = m_JumpPosition[3].transform.position;
        m_ToadBoss.m_MaxHealth = 500.0f;
        m_ToadBoss.SetCurrentHealthAsMaxHealth();
        m_MaxHealth = Constants.ToadMaxHealth;
        m_CurrentHealth = m_MaxHealth;
        transform.rotation = initalRotaton;
    }

    private void ShowDebugStuff()
    {
      // m_BossActionText.text = CurrentAction.ToString();
      //m_BossDecidedActionText.text = CurrentBehaviour.ToString();
       //m_PlayerDistanceText.text = GetDistanceToPlayer().ToString();
    }
}