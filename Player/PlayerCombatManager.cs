using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCombatManager : MonoBehaviour
{
    [SerializeField] private GameObject m_CrosshairCanvas;

    [SerializeField] private AttackDirection m_TargetAttackDirection = AttackDirection.Null;

    public AttackDirection TargetAttackDirection
    {
        get
        {
            return m_TargetAttackDirection;
        }

        private set
        {
            m_TargetAttackDirection = value;
        }
    }

    [SerializeField] private AttackDirection m_CurrentAttackDirection = AttackDirection.Null;

    public AttackDirection CurrentAttackDirection
    {
        get
        {
            return m_CurrentAttackDirection;
        }

        private set
        {
            m_CurrentAttackDirection = value;
        }
    }

    [SerializeField] private Image[] m_CrosshairImages = new Image[(int)AttackDirection._NumberOfDirections];

    [SerializeField] private bool m_InCombatMode;

    private Player m_Player;
    float MouseAngle = 0;


    //////////////

    // Gets the toadboss




    public bool TutorialUseProjectile = false;

    GameObject PlayerSlashZone;

    public GameObject m_FocusSpawnPoint;
    public GameObject m_PrefabFocusProjectile;
    public GameObject m_DeathCanvas;

    public GameObject m_TeleportPoint1;
    public GameObject m_TeleportPoint2;
    public GameObject m_TeleportPoint3;
    public GameObject m_TeleportPoint4;

    public Tutorial m_Tutorial;
    // Audio variables
    private int m_SwordSwingCount;

    void Start()
    {
        m_Player = Services.GameManager.Player;

        m_CrosshairCanvas = GameObject.Find("/Canvas/Canvas_Crosshair/Crosshair");

        for (int i = 0; i < (int)AttackDirection._NumberOfDirections; i++)
        {
            m_CrosshairImages[i] = m_CrosshairCanvas.transform.GetChild(i).GetComponent<Image>();
           
        }

        PlayerSlashZone = GameObject.Find("/Player/PlayerSlashZone");

        m_SwordSwingCount = 0;
    }

    // Update is called once per frame
    void Update()
    {


        // Disable the sword collider if an attack animation is not running.
        if (m_Player.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            DisableSwordCollider();
        }
        else // Otherwise enable it
        {
            EnableSwordCollider();
        }

        if(!m_InCombatMode)
        {
            m_CrosshairCanvas.SetActive(false);
        }
        else
        {
            m_CrosshairCanvas.SetActive(true);
        }

        


        // If the player is pressing the attack button
        if (Services.InputManager.GetAction(InputAction.Attack))
        {
            //if (m_Player.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            //{
            // Disable camera input
            m_Player.CameraController.SetInputEnabled(false);

            // Set the current attack to null
            CurrentAttackDirection = AttackDirection.Null;

            // We are now in combat mode
            m_InCombatMode = true;
            //}
        }
        else // The player released the attack button
        {

            //MouseAngle = 0;
            // If the player is in combat mode and has a target direction
            if (m_InCombatMode && TargetAttackDirection != AttackDirection.Null)
            {
                if (m_Player.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    // Set the animator to play the animation
                    m_Player.Animator.SetTrigger(TargetAttackDirection.ToString());

                    // Set the current attack to the target attack
                    CurrentAttackDirection = TargetAttackDirection;

                    // Use some stamina
                    //m_Player.m_CurrentStamina -= m_Player.SizeOfStaminaChunks;

                    // Multiple sword swing sfx functionality
                    m_SwordSwingCount++;

                    if (m_SwordSwingCount == 1)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Sword_Slash_1);
                    }
                    if (m_SwordSwingCount == 2)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Sword_Slash_2);
                    }
                    if (m_SwordSwingCount == 3)
                    {
                        Services.AudioManager.PlayPlayerSFX(PlayerSFX.Sword_Slash_3);
                        m_SwordSwingCount = 0;
                    }
                }
            }

            // Enable camera input
            m_Player.CameraController.SetInputEnabled(true);

            // No longer in combat mode
            m_InCombatMode = false;

            // Reset taret direction to null
            TargetAttackDirection = AttackDirection.Null;
        }


        // Reset all the crosshair images' color and scale
        ResetCrosshairImages();

        Vector2 lookInput = Services.InputManager.GetLookInput();

        if (Services.InputManager.CurrentInputType == InputType.Keyboard)
        {
            if (lookInput.sqrMagnitude < 0.5f)
            {
                lookInput = Vector2.zero;
            }
            else
            {
                lookInput.Normalize();
            }
        }

        HandleLookInput(lookInput);

        // Set the currently selected direction's crosshair to show as selected
        if (TargetAttackDirection != AttackDirection.Null)
        {
            m_CrosshairImages[(int)TargetAttackDirection].color = Color.blue;
            m_CrosshairImages[(int)TargetAttackDirection].transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
        }
    }

    private void ResetCrosshairImages()
    {
        foreach (Image image in m_CrosshairImages)
        {
            image.color = new Color(1, 1, 1, 0.078f);
            image.transform.localScale = Vector3.one;
        }
    }

    private void HandleLookInput(Vector2 lookInput)
    {
        if (m_InCombatMode)
        {
            Vector2 centerScreen = new Vector2(Screen.width / 2, Screen.height / 2);


            if (lookInput.magnitude >= 0.9999f && Services.InputManager.CurrentInputType == InputType.Keyboard)
            {
                MouseAngle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg;

                if (MouseAngle < 0)
                {
                    MouseAngle = 360 + MouseAngle;
                }
                MouseAngle = 360 - MouseAngle;

            }
            else if (lookInput.magnitude >= 0.5f && Services.InputManager.CurrentInputType == InputType.Controller)
            {

                MouseAngle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg;
        
                if (MouseAngle < 0)
                {
                    MouseAngle = 360 + MouseAngle;
                }
                MouseAngle = 360 - MouseAngle;
            }

           // 2.5 first then 7.5
            if (MouseAngle >= 20.0f && MouseAngle <= 70.0f)
            {
                TargetAttackDirection = AttackDirection.SouthEast;
            }
            else if (MouseAngle > 70.0f && MouseAngle < 110.0f)
            {
                TargetAttackDirection = AttackDirection.South;
            }
            else if (MouseAngle >= 110.0f && MouseAngle <= 160.0f)
            {
                TargetAttackDirection = AttackDirection.SouthWest;
            }
            else if (MouseAngle > 160.0f && MouseAngle < 200.0f)
            {
                TargetAttackDirection = AttackDirection.West;
            }
            else if (MouseAngle >= 200.0f && MouseAngle <= 250.0f)
            {
                TargetAttackDirection = AttackDirection.NorthWest;
            }
            else if (MouseAngle > 250.0f && MouseAngle < 290.0f)
            {
                TargetAttackDirection = AttackDirection.North;
            }
            else if (MouseAngle >= 290.0f && MouseAngle <= 340.0f)
            {
                TargetAttackDirection = AttackDirection.NorthEast;
            }
            else if (MouseAngle > 340.0f || MouseAngle < 20.0f && MouseAngle != 0)
            {
                TargetAttackDirection = AttackDirection.East;
            }




            //float minLook = usingMouse ? 0.3f : 0.5f;

            //if (lookInput.x >= minLook && lookInput.y >= minLook)
            //{
            //    TargetAttackDirection = AttackDirection.NorthEast;
            //}
            //else if (lookInput.x <= -minLook && lookInput.y >= minLook)
            //{
            //    TargetAttackDirection = AttackDirection.NorthWest;
            //}
            //else if (lookInput.x >= minLook && lookInput.y <= -minLook)
            //{
            //    TargetAttackDirection = AttackDirection.SouthEast;
            //}
            //else if (lookInput.x <= -minLook && lookInput.y <= -minLook)
            //{
            //    TargetAttackDirection = AttackDirection.SouthWest;
            //}
            //else if (lookInput.y >= minLook)
            //{
            //    TargetAttackDirection = AttackDirection.North;
            //}
            //else if (lookInput.y <= -minLook)
            //{
            //    TargetAttackDirection = AttackDirection.South;
            //}
            //else if (lookInput.x >= minLook)
            //{
            //    TargetAttackDirection = AttackDirection.East;
            //}
            //else if (lookInput.x <= -minLook)
            //{
            //    TargetAttackDirection = AttackDirection.West;
            //}
        }
    }

    public void TeleportToPosition1()
    {
        gameObject.transform.position = m_TeleportPoint1.gameObject.transform.position;
    }
    public void TeleportToPosition2()
    {
        gameObject.transform.position = m_TeleportPoint2.gameObject.transform.position;
    }
    public void TeleportToPosition3()
    {
        gameObject.transform.position = m_TeleportPoint3.gameObject.transform.position;
    }

    public void TeleportToPosition4()
    {
        gameObject.transform.position = m_TeleportPoint4.gameObject.transform.position;
    }

    public void EnableSwordCollider()
    {
        PlayerSlashZone.GetComponent<CapsuleCollider>().enabled = true;
    }

    public void DisableSwordCollider()
    {
        PlayerSlashZone.GetComponent<CapsuleCollider>().enabled = false;
    }
}
