using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the file that contains all the constants in the code.
/// </summary>
public class Constants : MonoBehaviour
{
    #region Input Constants

    /// <summary>
    /// Default binds for keyboard and mouse.
    /// </summary>
    [NamedArray(typeof(InputAction))]
    public KeyCode[] DefaultKeyMouseBinds =
    {
        KeyCode.W,          // Move forward
        KeyCode.S,          // Move backward
        KeyCode.D,          // Move right
        KeyCode.A,          // Move left
        KeyCode.LeftShift,  // Dash
        KeyCode.Space,      // Jump
        KeyCode.Mouse0,     // Attack
        KeyCode.E,          // Fire
        KeyCode.Tab,        // Lock on
        KeyCode.T,          // Heal       
        KeyCode.F,          // Interact     
        KeyCode.Escape      // Pause
    };

    /// <summary>
    /// Defaut binds for controller.
    /// </summary>
    [NamedArray(typeof(InputAction))]
    public ControllerInput[] DefaultControllerBinds =
    {
        ControllerInput.LeftStickUp,        // Move forward
        ControllerInput.LeftStickDown,      // Move backward
        ControllerInput.LeftStickRight,     // Move right
        ControllerInput.LeftStickLeft,      // Move left                                                             
        ControllerInput.RightBumper,        // Dash
        ControllerInput.A,                  // Jump
        ControllerInput.RightTrigger,       // Attack
        ControllerInput.Y,                  // Fire
        ControllerInput.RightStickClick,    // Lock on
        ControllerInput.LeftTrigger,        // Heal       
        ControllerInput.X,                  // Interact     
        ControllerInput.Start               // Pause    
    };


    /// <summary>
    /// Binds for gamepad.
    /// </summary>
    public static readonly string[] GamepadBinds =
    {   // Add + or - for axes, add _Button for buttons.
        "Vertical+",                // Move forward
        "Vertical-",                // Move backward
        "Horizontal-",              // Move left
        "Horizontal+",              // Move right
        "RightStickClick_Button",   // Lock on
        "RightBumper_Button",       // Dash
        "A_Button",                 // Jump
        "RightTrigger+",            // Attack
        "Y_Button",                 // Fire
        "X_Button",                 // Interact
        "LeftTrigger+",             // Heal
        "Start_Button"              // Pause
    };

    /// <summary>
    /// The deadzone for the controller axes.
    /// </summary>
    public const float ControllerDeadzone = 0.2f;

    #endregion


    // Toad Constants (need renaming)
    public const int ProjectileDamage = 10;

    // Constants Variables
    public const float ToadJumpHeight = 90.0f;
    public const float ChargeDistanceBehindPlayer = 5.0f;
    public const int MaxAttacksBeforeBehaviourSwitch = 10;
    public const int MaxHeadSpins = 3;
    public const int ProjectileWillBeRepeatedAmount = 6;
    public const float TurnSpeed = 3.0f;

    // Constant Times (used to set the timers length)
    public const float ProjectileAttackTimer = 1.5f;
    public const float HeadSpinTimer = 2.5f;
    public const float HeadSpinDurationTimer = 0.75f;
    public const float ChargeDelay = 1.0f;
    public const float ChargeDuration = 1.5f;
    public const float JumpDuration = 1.5f;
    public const float HealDelay = 1.0f;
    public const float MineAttackDelay = 1.5f;
    public const float MineLifeDuration = 40.0f;
    public const float AttackCooldown = 2.5f;
    public const float PlatfromAttackCooldown = 1.0f;
    public const float TongueWhipDuration = 1.5f;
    public const float StompDurationTimer = 1.5f;
    public const float PushTimer = 0.75f;
    public const float TongueGrabDelay = 1.0f;
    public const float TonguePullDurration = 3.0f;

    // Combat Constants
    public const float ChargeDamage = 25.0f;
    public const float StompDamage = 50.0f;
    public const float TongueDamage = 15.0f;
    public const float TongueGrabDamage = 40.0f;
    public const float ToadTongueWipDamage = 15.0f;

    // Toad Health
    public const float ToadMaxHealth = 100.0f;

    // King Constants
    public const float KingMaxHealth = 200.0f;

    // Combat Constants
    public const float MeleeRange = 4.5f;
    public const float AggroChargeRange = 8.5f;

    // King timers
    public const float KingTurnTimer = 0.75f;
    public const float KingAggroDelay = 6.0f;
    public const float KingFireCircleCooldown = 20.0f;
    public const float KingFireCircleLength = 30.0f;

    //King Damage Constants
    public const float KingMeleeDamage = 25.0f;
    public const float KingLungeDamage = 35.0f;
    public const float KingSheildChargeDamage = 15.0f;

    // Other Constants
    public const float BoulderSpawnRate = 1.5f;

    public const float FireDamage = 20.0f;
}
