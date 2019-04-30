using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneToad : AIController
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
        Turn
    }

    public GameObject m_PlayerWatcher;

    public GameObject m_PlayerCamera;
    public GameObject m_CutsceneCamera;

    // Jump Positions
    public GameObject[] m_JumpPosition;


    string[] m_ToadAttackMoves;
    string[] EnemyActionNames = System.Enum.GetNames(typeof(Action));
    public string[] GetEnemyActionNames() { return EnemyActionNames; }

    public GameObject m_ProjectileSpawnPoint_Reference;

    GameObject m_Prefab_Projectile_Reference;

    public GameObject m_Prefab_Projectile_Reference_TL;
    public GameObject m_Prefab_Projectile_Reference_TR;
    public GameObject m_Prefab_Projectile_Reference_HOR;
    public GameObject m_Prefab_Projectile_Reference_VERT;

    public List<BasicProjectile> m_ProjectilesAttackList;

    Timer m_AttackCooldownTimer;
    public Animator m_Animator;

    // Varriables
    public int m_JumpID;
    public int m_NumberOfJumps;

    // Text (some are used for debugging)
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

        // Player Watcher Setup
        m_PlayerWatcher = GameObject.Find("PlayerWatcher");

        m_PlayerCamera = GameObject.Find("PlayerCamera");

        m_ToadBoss = gameObject.GetComponent<ToadBoss>();

        // Jump Position initialization
        //m_JumpPosition[1] = GameObject.Find("GroundJumpPosition1");

        m_JumpID = -1;
        m_NumberOfJumps = m_JumpPosition.Length;

        m_AiActive = true;

        m_AIActions.Add(((int)Action.None), new ToadNone(this));
        m_AIActions.Add(((int)Action.Jump), new CutSceneJump(this));

        m_AIBehaviours.Add((int)Behaviour.Cinematic, new ToadCutsceneBehaviour(this));

        SetBehaviour((int)AIToadController.Behaviour.Cinematic);
        SetAction((int)AIToadController.Action.Jump);
 
    }

    protected override void Update()
    {
        base.Update();

       
    }

    public float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, Services.GameManager.Player.transform.position);
    }

    public void CalculateJumpID()
    {
        // If the toad should go to the pillar   
        if (m_JumpID < m_NumberOfJumps - 1)
        {
            m_JumpID++;
        }
    }
}