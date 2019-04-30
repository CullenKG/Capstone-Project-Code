using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneProjectile : AIAction
{

    int m_AmountOfTimesRepeated;
    int m_ProjectileWillBeRepeatedAmount;

    private Timer m_ProjectileAttackTimer;
    public CutSceneProjectile(CutSceneToad aAIController) : base(aAIController)
    {
        m_ProjectileAttackTimer = Services.TimerManager.CreateTimer("TutorialProjectile", Constants.ProjectileAttackTimer, false);
    }

    // Use this for initialization
    public override void Start()
    {
        ////Debug.Log("AiSystemWorksFor the jump attack?");
        m_AmountOfTimesRepeated = 0;
        m_ProjectileWillBeRepeatedAmount = 2;
        m_ProjectileAttackTimer.StartTimer();

        // Play wheeze sfx
        Services.AudioManager.PlayOdellSFX(OdellSFX.Odell_Projectile_Wheeze);
    }

    // Update is called once per frame
    public override void Update()
    {
        // Turn to the player
        // ((AIToadController)m_AIController).TurnToPlayer();

        if (m_AmountOfTimesRepeated <= m_ProjectileWillBeRepeatedAmount)
        {
            //TODO: use timer class
            //When the boss timer is 0 Do the Attack
            if (m_ProjectileAttackTimer.IsFinished())
            {
                ((CutSceneToad)m_AIController).m_Animator.SetTrigger("Toad_Spit");
                //Looping through all the projectiles in the list
                for (int i = 0; i < ((CutSceneToad)m_AIController).m_ProjectilesAttackList.Count; i++)
                {
                    int RanNum = Random.Range(0, ((CutSceneToad)m_AIController).m_ProjectilesAttackList.Count);

                    if (((CutSceneToad)m_AIController).m_ProjectilesAttackList[RanNum].GetHasDied() == true)
                    {
                        //Checking if the Projectile is Dead or not
                        if (((CutSceneToad)m_AIController).m_ProjectilesAttackList[RanNum].GetComponent<BasicProjectile>().m_hasDiedBefore == false)
                        {
                            //Setting the Projectile to be alive and it to active
                            ((CutSceneToad)m_AIController).m_ProjectilesAttackList[RanNum].SetProjectileToAlive();

                            //// Calculate the direction vector
                            //Vector3 dir = Services.GameManager.Player.transform.position - ((AIToadController)m_AIController).transform.position;
                            //dir.y -= 1.0f;
                            //dir.Normalize();

                            //Giving the projectile a reference to the current position of the player aswell as the place it will be spawning
                            ((CutSceneToad)m_AIController).m_ProjectilesAttackList[RanNum].SetUpProjectile(Services.GameManager.Player.gameObject.transform.position, ((CutSceneToad)m_AIController).m_ProjectileSpawnPoint_Reference.gameObject.transform.position, Services.GameManager.Player.gameObject);

                            //Increasing the amount of times this attack can be repeated before switching to a different attack
                            m_AmountOfTimesRepeated++;

                            // Play spit sfx
                            Services.AudioManager.PlayOdellSFX(OdellSFX.Odell_Projectile_Fire);

                            //Giving the ToadBossTimer the ProjectileTimer amount
                            m_ProjectileAttackTimer.Restart();

                            // If the toad can't see the player
                            //if (((CutSceneToad)m_AIController).CanSeePlayer == false)
                            //{
                            //    // End the action early
                            //    ((CutSceneToad)m_AIController).CurrentActionFinished();
                            //}
                            // If not repeat
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            // Attack is finished
            ((AIToadController)m_AIController).CurrentActionFinished();


        }
    }
}