using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingProjectileAttack : AIAction
{
    int m_AmountOfTimesRepeated;
    int m_ProjectileWillBeRepeatedAmount;

    private Timer KingProjectileAttackTimer;

    public KingProjectileAttack(AIController aAIController) : base(aAIController)
    {
        // Make a new constant for king
        KingProjectileAttackTimer = Services.TimerManager.CreateTimer("m_ProjectileAttackTimer", Constants.ProjectileAttackTimer, false);
    }

    // Use this for initialization
    public override void Start()
    {
        m_AmountOfTimesRepeated = 0;
        m_ProjectileWillBeRepeatedAmount = 0;
        KingProjectileAttackTimer.StartTimer();

        // Play sound effect
        // Services.AudioManager.PlaySFX(SFX.Odell_Projectile_Wheeze);
    }

    // Update is called once per frame
    public override void Update()
    {
        // Turn to the player
        //((AIToadController)m_AIController).TurnToPlayer();

        if (m_AmountOfTimesRepeated <= m_ProjectileWillBeRepeatedAmount)
        {
            //TODO: use timer class
            //When the boss timer is 0 Do the Attack
            if (KingProjectileAttackTimer.IsFinished())
            {
                //Looping through all the projectiles in the list
                for (int i = 0; i < ((AIKingController)m_AIController).m_ProjectileList.Count; i++)
                {
                    // TODO: Store components in list instead of gameobject
                    //Checking if the Projectile is Dead or not
                    if (((AIKingController)m_AIController).m_ProjectileList[i].GetHasDied() == true)
                    {
                        //Setting the Projectile to be alive and it to active
                        ((AIKingController)m_AIController).m_ProjectileList[i].SetProjectileToAlive();

                        //Giving the projectile a reference to the current position of the player aswell as the place it will be spawning
                        ((AIKingController)m_AIController).m_ProjectileList[i].SetUpProjectile(Services.GameManager.Player.gameObject.transform.position, ((AIKingController)m_AIController).m_ProjectileSpawnPoint_Reference.gameObject.transform.position, Services.GameManager.Player.gameObject);

                        //Increasing the amount of times this attack can be repeated before switching to a different attack
                        m_AmountOfTimesRepeated++;

                        //Giving the ToadBossTimer the ProjectileTimer amount
                        KingProjectileAttackTimer.Restart();
                        break;
                    }
                }
            }
        }
        else
        {
            // Attack is finished
            ((AIKingController)m_AIController).CurrentActionFinished();
        }
    }
}