using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class AIKingController : AIController
{
    public enum Behaviour
    {
        Passive,
        Offensive,
        Defensive,
        Dueling,
        Following,
        Cinematic,
        Presentation,
        Pillar
    }
    public enum Action
    {
        None,
        Walk,
        DuelMove,
        Duel,
        Dodge,
        ShieldCharge,
        Lunge,
        Blocking,
        Slashing,
        ProjectileAttack,
        Turn,
        Stagger,
        FireCircle,
    }

    // GameObjects

    public GameObject m_Prefab_Projectile_Reference;
    public GameObject m_ProjectileSpawnPoint_Reference;
    public GameObject m_FireCircle;
    public GameObject m_PlayerWatcher;
    public GameObject m_PlayerCamera;

    public GameObject m_Shield;

    // Move Positions
    public GameObject[] m_CircleMovePositions;

    public GameObject[] m_DodgeChecks;

    // Projectiles 
    public List<BasicProjectile> m_ProjectileList;

    // Triggers
    public TriggerBoxObject m_LeftPillars;
    public TriggerBoxObject m_RightPillars;
    public TriggerBoxObject m_RoomCenter;

    public TriggerBoxObject m_KingChargeTrigger;

    public BoxCollider m_KingSwordCollider;

    public KingParryCube m_KingParryCube;

    // public GameObject m_CutsceneCamera;

    string[] m_ToadAttackMoves;
    string[] EnemyActionNames = System.Enum.GetNames(typeof(Action));
    public string[] GetEnemyActionNames() { return EnemyActionNames; }

    // See if this works variables
    float m_ActTimer;

    public Animator m_Animator;

    public Player m_Player;

    // Timers
    Timer m_AttackCooldownTimer;
    Timer FireCirlceCooldownTimer;
    public Timer FireCircleLengthTimer;

    // Varriables
    Vector3 m_HealthBarSize;
    public int m_NumberOfAttacks = 0;

    public bool m_HitPlayer;
    public bool m_IsDueling;

    public float m_CurrentHealth;
    public float m_MaxHealth;

    public string m_BossName;

    // Text
    public Text m_BossText;
    // Text (used for debugging)
    public Text m_BossActionText;
    public Text m_BossDecidedActionText;
    public Text m_PlayerDistanceText;

    // Scripts
    public KingBoss m_KingBoss;

    // Components
    public NavMeshAgent m_NavMeshAgent;

    // Use this for initialization
    void Start()
    {
        // Create timers
        m_AttackCooldownTimer = Services.TimerManager.CreateTimer("KingAttackCooldownTimer", 1.5f, false);
        FireCirlceCooldownTimer = Services.TimerManager.CreateTimer("FireCirlceCooldownTimer", 30.0f, false);
        FireCircleLengthTimer = Services.TimerManager.CreateTimer("FireCircleLengthTimer", Constants.KingFireCircleLength, false);

        FireCirlceCooldownTimer.Restart();

        m_Animator = GetComponentInChildren<Animator>();

        // UI set up
        m_HealthBarSize.Set(0.65765f, 0.188084f, 0.188084f);
        m_BossName = "King dude";

        m_CurrentHealth = 75;
        m_MaxHealth = 100;
        m_KingBoss.m_ShieldUp = true;
        m_IsDueling = false;

        // Disable the fire circle
        m_FireCircle.SetActive(false);

        // Disable the king sword collider
        m_KingSwordCollider.enabled = false;

        // TODO
        Services.GameManager.RegisterKing(this);

        m_Player = Services.GameManager.Player;

        // Player Watcher Setup
        m_PlayerWatcher = GameObject.Find("KingPlayerWatcher");

        m_PlayerCamera = GameObject.Find("PlayerCamera");
        //m_CutsceneCamera = GameObject.Find("CutsceneCamera");

        // Circle Move Position initialization
        //m_CircleMovePositions[0] = GameObject.Find("MovePosition1");
        //m_CircleMovePositions[1] = GameObject.Find("MovePosition2");
        //m_CircleMovePositions[2] = GameObject.Find("MovePosition3");
        //m_CircleMovePositions[3] = GameObject.Find("MovePosition4");
        //m_CircleMovePositions[4] = GameObject.Find("MovePosition5");
        //m_CircleMovePositions[5] = GameObject.Find("MovePosition6");
        //m_CircleMovePositions[6] = GameObject.Find("MovePosition7");
        //m_CircleMovePositions[7] = GameObject.Find("MovePosition8");
        //m_CircleMovePositions[8] = GameObject.Find("MovePosition9");
        //m_CircleMovePositions[9] = GameObject.Find("MovePosition10");
        //m_CircleMovePositions[10] = GameObject.Find("MovePosition11");
        //m_CircleMovePositions[11] = GameObject.Find("MovePosition12");
        //m_CircleMovePositions[12] = GameObject.Find("MovePosition13");
        //m_CircleMovePositions[13] = GameObject.Find("MovePosition14");
        //m_CircleMovePositions[14] = GameObject.Find("MovePosition15");

        m_KingBoss = gameObject.GetComponent<KingBoss>();

        // Making the projectile pool
        for (int i = 0; i < 3; i++)
        {
            //Making a pool of projectiles to spawn
            m_ProjectileList.Add(Instantiate<BasicProjectile>(m_Prefab_Projectile_Reference.GetComponent<BasicProjectile>(), m_ProjectileSpawnPoint_Reference.transform));
            //Turning the Objects off 
            m_ProjectileList[i].gameObject.SetActive(false);
            m_ProjectileList[i].transform.parent = null;
            m_ProjectileList[i].m_HasDied = true;
        }


        m_AIBehaviours.Add((int)Behaviour.Offensive, new KingOffensiveBehaviour(this));
        m_AIBehaviours.Add((int)Behaviour.Defensive, new KingDefensiveBehaviour(this));
        m_AIBehaviours.Add((int)Behaviour.Passive, new KingPassiveBehaviour(this));
        m_AIBehaviours.Add((int)Behaviour.Cinematic, new KingCutsceneBehaviour(this));
        m_AIBehaviours.Add((int)Behaviour.Following, new KingFollowBehaviour(this));
        m_AIBehaviours.Add((int)Behaviour.Dueling, new KingDuelBehaviour(this));
        m_AIBehaviours.Add((int)Behaviour.Pillar, new KingPillarBehaviour(this));
        m_AIActions.Add((int)Action.None, new KingNone(this));
        m_AIActions.Add((int)Action.Walk, new KingWalkToPlayer(this));
        m_AIActions.Add((int)Action.Duel, new KingDuelMove(this));
        m_AIActions.Add((int)Action.ProjectileAttack, new KingProjectileAttack(this));
        m_AIActions.Add((int)Action.Slashing, new KingMeleeAttack(this));
        m_AIActions.Add((int)Action.Blocking, new KingBlock(this));
        m_AIActions.Add((int)Action.ShieldCharge, new KingShieldCharge(this));
        m_AIActions.Add((int)Action.Lunge, new KingLunge(this));
        m_AIActions.Add((int)Action.Turn, new KingTurn(this));
        m_AIActions.Add((int)Action.Dodge, new KingDodge(this));
        m_AIActions.Add((int)Action.Stagger, new KingStagger(this));
        m_AIActions.Add((int)Action.FireCircle, new KingSummonFireCircle(this));
        m_AIActions.Add((int)Action.DuelMove, new KingDuelMove(this));

        SetDecidedAction((int)AIKingController.Action.None);
        SetAction((int)AIKingController.Action.None);
        SetNextAction((int)AIKingController.Action.None);
        SetBehaviour((int)AIKingController.Behaviour.Passive);
    }

    protected override void Update()
    {
        base.Update();

        //m_NavMeshAgent.destination = m_Player.transform.position;

        if (m_KingBoss.m_ShieldUp == true)
        {
            //  m_Shield.SetActive(true);
        }
        if (m_KingBoss.m_ShieldUp == false)
        {
            // m_Shield.SetActive(false);
        }

        ShowDebugStuff();

        // Enable the AI if the plater gets close
        if (GetDistanceToPlayer() < 50.0f && m_AiActive == false)
        {
            Services.GameManager.KingTriggered = true;
            m_AiActive = true;
            SetBehaviour((int)AIKingController.Behaviour.Offensive);
            SetAction((int)AIKingController.Action.None);
            m_MakeDecision = true;
        }

        // Enable the AI with button press
        if (Input.GetKeyDown("n"))
        {
            m_AiActive = true;
            SetBehaviour((int)AIKingController.Behaviour.Passive);
            SetAction((int)AIKingController.Action.Dodge);
            //SetNextAction((int)AIKingController.Action.DuelMove);
            // m_MakeDecision = true;
            //m_Animator.SetTrigger("t_Vertical_Swing");
        }

        // If the AI is active
        if (m_AiActive == true)
        {
            // If the player enters the pillars and the current beahviour isint pillar
            //if (m_LeftPillars.m_IsPlayerinside == true || m_RightPillars.m_IsPlayerinside == true && !IsCurrentAction((int)AIKingController.Behaviour.Pillar))
            //{
            //      // Change the behaviour to pillar
            //      SetBehaviour((int)AIKingController.Behaviour.Pillar);
            //}

            // If the Duel Behaviour is true and the behaviour isin't dueling
            if (m_IsDueling == true && !IsCurrentBehaviour((int)Behaviour.Dueling))
            {
                // Set Dueling to false and restart the Fire Circle cooldown
                m_IsDueling = false;
                FireCirlceCooldownTimer.Restart();
            }

            if (IsCurrentBehaviour((int)AIKingController.Behaviour.Offensive))
            {
                // If the king is walking
                if (IsCurrentAction((int)AIKingController.Action.Walk))
                {
                    // If the boss reaches melee range
                    if (GetDistanceToPlayer() < Constants.MeleeRange)
                    {
                        // Reset the walk and finish the action so the behaviour will choose the melee attack or continue walking
                        CurrentAction.Start();
                        CurrentBehaviour.OnActionFinished();
                    }
                }
            }

            // If the player and the king are in the center of the room
            if (m_RoomCenter.m_IsKingInside == true && m_RoomCenter.m_IsPlayerinside == true && m_IsDueling == false && FireCirlceCooldownTimer.IsFinished() && IsCurrentAction((int)AIKingController.Action.Walk))
            {
                // If the player is within range to start the duel
                if (GetDistanceToPlayer() < 15.0f && GetDistanceToPlayer() > 8.0f)
                {
                    // Set the action to summon fire circle and switch behaviour to dueling
                    SetBehaviour((int)AIKingController.Behaviour.Dueling);
                    SetAction((int)AIKingController.Action.FireCircle);
                    m_IsDueling = true;
                    FireCircleLengthTimer.Restart();
                }
            }
        }

        //// If the boss hit the player
        //if (m_HitPlayer == true)
        //{
        //    // Set hit to false so it only will be true for one frame
        //    m_HitPlayer = false;
        //}
    }

    public float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, Services.GameManager.Player.transform.position);
    }

    public void DebugSwitchEnemyAction(int a_EnemyAction)
    {
        if (a_EnemyAction == 0)
        {
            SetDecidedAction((int)Action.None);
        }
        if (a_EnemyAction == 1)
        {
            SetDecidedAction((int)Action.Walk);
        }
        if (a_EnemyAction == 2)
        {
            SetDecidedAction((int)Action.Duel);
        }
        if (a_EnemyAction == 3)
        {
            SetDecidedAction((int)Action.ProjectileAttack);
        }
        if (a_EnemyAction == 4)
        {
            SetDecidedAction((int)Action.Slashing);
        }
        if (a_EnemyAction == 5)
        {
            SetDecidedAction((int)Action.Blocking);
        }
        if (a_EnemyAction == 6)
        {
            SetDecidedAction((int)Action.ShieldCharge);
        }
        if (a_EnemyAction == 7)
        {
            SetDecidedAction((int)Action.Lunge);
        }

        SetCurrentActionAsDecidedAction();

        // If the AI isint on, turn it on
        if (m_AiActive == false)
        {
            m_AiActive = true;
            SetBehaviour((int)AIKingController.Behaviour.Offensive);
        }
    }

    public void PushPlayer(float force, float radius)
    {
        // Push the player away
        Services.GameManager.Player.MovementController.PushPlayer(transform.position, force);
    }

    void OnCollisionEnter(Collision collision)
    {
        //if (IsCurrentAction((int)AIKingController.Action.ShieldCharge))
        //{
        //    m_HitPlayer = true;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    public void Reset()
    {
        m_NumberOfAttacks = 0;
        m_HitPlayer = false;

        m_MaxHealth = Constants.KingMaxHealth;
        m_CurrentHealth = m_MaxHealth;

        SetAction(((int)AIKingController.Action.None));
        SetDecidedAction(((int)AIKingController.Action.None));
        SetBehaviour(((int)AIKingController.Behaviour.Passive));
    }

    private void ShowDebugStuff()
    {
       m_BossActionText.text = CurrentAction.ToString();
       //m_BossDecidedActionText.text = DecidedAction.ToString();
       //m_PlayerDistanceText.text = GetDistanceToPlayer().ToString();
    }

    public void TurnToPlayer()
    {
        // Get the player watcher to look at the player
        m_PlayerWatcher.transform.LookAt(Services.GameManager.Player.gameObject.transform);

        // Calculate the angle between the toad and the player watcher
        var angle = Quaternion.Angle(transform.rotation, m_PlayerWatcher.transform.rotation);

        // If the angle is greater than 0
        if (angle > 0)
        {
            // Get the Quaternion and exclude the y axis
            Quaternion XYRotation = Quaternion.Euler(new Vector3(0f, transform.localEulerAngles.y, 0f));

            // Look at the player (toad rotation is still screwed so it looks bad)
            //transform.rotation = Quaternion.Lerp(XYRotation, m_PlayerWatcher.transform.rotation, Time.deltaTime * m_TurnSpeed / angle);

            transform.rotation = Quaternion.Lerp(XYRotation, m_PlayerWatcher.transform.rotation, Time.deltaTime * Constants.TurnSpeed);
        }
    }

    public bool CanSeePlayer(int VisionRange)
    {
        bool CanSee = false;

        // Get the player watcher to look at the Player
        m_PlayerWatcher.transform.LookAt(Services.GameManager.Player.gameObject.transform);

        float angle = Quaternion.Angle(transform.rotation, m_PlayerWatcher.transform.rotation);

        if (VisionRange == 3)
        {
            // 140 degrees of vision
            if (angle <= 70 || angle >= 290)
            {
                CanSee = true;
            }
        }

        if (VisionRange == 2)
        {
            // 100 degrees of vision
            if (angle <= 50 || angle >= 310)
            {
                CanSee = true;
            }
        }

        if (VisionRange == 1)
        {
            // 60 degrees of vision
            if (angle <= 30 || angle >= 330)
            {
                CanSee = true;
            }
        }

        if (VisionRange == 0)
        {
            // 40 degrees of vision
            if (angle <= 20 || angle >= 340)
            {
                CanSee = true;
            }
        }

        return CanSee;
    }
}