using System;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    Player m_Player;
    CharacterController m_CharacterController;
    float m_MoveSpeed;
    float m_DodgeSpeed;
    float m_maxSpeed;

    public Vector3 m_Velocity = Vector3.zero;
    Vector3 MoveDirection;
    Vector3 Gravityfix;

    bool canJump;
    bool IsJumping;
    bool DodgeInput;
    public bool TutorialDash = false;
    public Vector3 OutsideForce = Vector3.zero;
    float mass = 3.0f;

    Timer DodgeTimer;
    public float m_JumpAmmount;
    Timer m_jumpTimer;
    float JumpLerp = 0;
    float m_fallLerp = 0;
    float m_Gravity;
    bool JumpInput;
    public bool m_playerMovementDissabled { get; set; }

    // Walk SFX variables
    float m_FootstepSFXDelay = 0.5f;
    float m_FootstepDelayCountdown;
    int m_FootstepCount;
    public bool InWater;
    bool isGrounded;
  

    void Awake()
    {
        m_Player = GetComponent<Player>();
        m_CharacterController = GetComponent<CharacterController>();

        DodgeTimer = Services.TimerManager.CreateTimer("DodgeTimer", 0.3f, false);
        m_jumpTimer = Services.TimerManager.CreateTimer("JumpTimer", 0.25f, false);

        m_maxSpeed = 7.0f;
        m_DodgeSpeed = 14.0f;
        m_JumpAmmount = 7.0f;
        m_MoveSpeed = m_maxSpeed;
        m_playerMovementDissabled = false;

       // transform.localEulerAngles = new Vector3(0.0f, 270.0f, 0.0f);

        canJump = false;
        IsJumping = false;

        // Initializing walk SFX variables
        m_FootstepDelayCountdown = 0;
        m_FootstepCount = 0;
        InWater = false;
    }
    void OnEnable()
    {
        GameManager.OnPause += OnPause;
        GameManager.OnResume += OnResume;
    }
    void OnDisable()
    {
        GameManager.OnPause -= OnPause;
        GameManager.OnResume -= OnResume;
    }
    // Update is called once per frame
    void Update()
    {
        float horizontalMovement = Services.InputManager.GetAxisRawValue(InputAxis.MoveHorizontal);
        float frontwardMovement = Services.InputManager.GetAxisRawValue(InputAxis.MoveFrontward);

        //Get any special movement 
        JumpInput = Services.InputManager.GetAction(InputAction.Jump);
        DodgeInput = Services.InputManager.GetActionDown(InputAction.Dash);


        MoveDirection = (horizontalMovement * transform.right + frontwardMovement * transform.forward).normalized;


        if (DodgeInput && DodgeTimer.IsRunning() == false && MoveDirection != Vector3.zero)
        {
            if (m_Player.m_CurrentStamina >= m_Player.SizeOfStaminaChunks)
            {
                m_Player.m_CurrentStamina -= m_Player.SizeOfStaminaChunks;
                m_MoveSpeed = m_DodgeSpeed;
                TutorialDash = true;
                DodgeTimer.Restart();
            }
        }

        if (DodgeTimer.IsFinished())
        {
            m_MoveSpeed = m_maxSpeed;
        }

        // Walk SFX logic
        if (m_CharacterController.velocity != Vector3.zero && m_CharacterController.isGrounded)
        {          
            m_FootstepDelayCountdown -= Time.fixedDeltaTime;

            if (m_FootstepCount > 10)
                m_FootstepCount = 0;

            if (m_FootstepDelayCountdown <= 0)
            {
                if (InWater == false)
                {
                    if (m_FootstepCount == 0)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Groud_1);
                    }
                    if (m_FootstepCount == 1)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Groud_2);
                    }
                    if (m_FootstepCount == 2)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Groud_3);
                    }
                    if (m_FootstepCount == 3)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Groud_4);
                    }
                    if (m_FootstepCount == 4)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Groud_5);
                    }
                    if (m_FootstepCount == 5)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Groud_6);
                    }
                    if (m_FootstepCount == 6)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Groud_7);
                    }
                    if (m_FootstepCount == 7)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Groud_8);
                    }
                    if (m_FootstepCount == 8)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Groud_9);
                    }
                    if (m_FootstepCount == 9)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Groud_10);
                    }
                }
                else if (InWater == true)
                {
                    if (m_FootstepCount == 0)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Water_1);
                    }
                    if (m_FootstepCount == 1)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Water_2);
                    }
                    if (m_FootstepCount == 2)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Water_3);
                    }
                    if (m_FootstepCount == 3)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Water_4);
                    }
                    if (m_FootstepCount == 4)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Water_5);
                    }
                    if (m_FootstepCount == 5)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Water_6);
                    }
                    if (m_FootstepCount == 6)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Water_7);
                    }
                    if (m_FootstepCount == 7)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Water_8);
                    }
                    if (m_FootstepCount == 8)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Water_9);
                    }
                    if (m_FootstepCount == 9)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Player_Footsteps_Water_10);
                    }
                }
                m_FootstepCount++;
                m_FootstepDelayCountdown = m_FootstepSFXDelay;

            }
        }     
    }

    void FixedUpdate()
    {
        if (m_playerMovementDissabled == false)
        {           
            m_Velocity = (MoveDirection * m_MoveSpeed);

            isGrounded = m_CharacterController.isGrounded;

            if (!isGrounded || IsJumping)
            {
                if (m_jumpTimer.IsRunning())
                {
                    JumpLerp += Time.deltaTime;
                    m_Velocity.y += Mathf.Lerp(m_JumpAmmount, 0.0f, JumpLerp / 0.25f);                 
                }
                else
                {
                    //only apply gravity when in the air when not jumping
                    IsJumping = false;
                    m_fallLerp += Time.deltaTime;
                    m_Velocity.y -= m_fallLerp * m_JumpAmmount*1.3f;                  
                }
            }

            isGrounded = m_CharacterController.isGrounded;

            //If we are grounded update accordinly
            if (isGrounded)
            {
                canJump = true;
                m_Velocity.y -= m_CharacterController.stepOffset / Time.deltaTime;
              
                if (JumpInput && m_jumpTimer.IsRunning() == false && canJump)
                {
                    m_jumpTimer.Restart();
                    m_Velocity.y = 0.0f;
                    m_Velocity.y += m_JumpAmmount;
                    IsJumping = true;
                    canJump = false;
                }
                JumpLerp = 0;
                m_fallLerp = 0;
            }

            if (OutsideForce.magnitude > 0.2F)
            {
                m_CharacterController.Move(OutsideForce * Time.deltaTime);
            }
            else
            {
                OutsideForce = Vector3.zero;
            }

            // consumes the impact energy each cycle:
            OutsideForce = Vector3.Lerp(OutsideForce, Vector3.zero, 5 * Time.deltaTime);

            //After all calculations move player
            m_CharacterController.Move(m_Velocity * Time.deltaTime);  
        }
    }

    //this function handles outside forces and applies them to the player
    public void PushPlayer(Vector3 dir, float force)
    {   
        dir.Normalize();
        if (dir.y < 0)
        {
            dir.y = -dir.y;
        }
        OutsideForce += dir.normalized * force / mass;
    }

    void OnPause()
    {
        GetComponent<FPSCameraController>().enabled = false;
        GetComponent<PlayerCombatManager>().enabled = false;
    }

    void OnResume()
    {
        GetComponent<FPSCameraController>().enabled = true;
        GetComponent<PlayerCombatManager>().enabled = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Water")
        {
            InWater = true;
        }
        else
        {
            InWater = false;
        }

        if(hit.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }

        if(hit.gameObject.tag == "Fire")
        {
            Services.GameManager.Player.MovementController.PushPlayer((hit.transform.position), 15.0f);
            Services.GameManager.Player.TakeDamage(Constants.FireDamage);
        }
    }
}
