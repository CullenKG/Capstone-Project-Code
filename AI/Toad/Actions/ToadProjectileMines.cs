using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadProjectileMines : AIAction
{
    bool m_Mined;
    public ToadProjectileMines(AIController aAIController) : base(aAIController)
    {

    }

    // Use this for initialization
    public override void Start()
    {
        ((AIToadController)m_AIController).m_Animator.SetTrigger("Toad_Spit");

        // Play wheeze sfx
        Services.AudioManager.PlayOdellSFX(OdellSFX.Odell_Projectile_Fire);
        m_Mined = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        if(m_Mined == true)
        {
            // Attack is finished
            ((AIToadController)m_AIController).CurrentActionFinished();
        }

        if (m_Mined == false)
        {

            // Play wheeze sfx
            Services.AudioManager.PlayOdellSFX(OdellSFX.Odell_Projectile_Fire);

            ////Debug.Log("AiSystemWorksFor the jump attack?");
            for (int i = 0; i < ((AIToadController)m_AIController).m_ProjectilesMineList.Count; i++)
            {
                //Checking if the Projectile is Dead or not

                // TODO: store components instead of gameobjects
                //Setting the Projectile to be alive and it to active
                if (((AIToadController)m_AIController).m_ProjectilesMineList[i] != null)
                {
                    ((AIToadController)m_AIController).m_ProjectilesMineList[i].SetProjectileToAlive();

                    //Giving the projectile a reference to the current position of the player aswell as the place it will be spawning
                    ((AIToadController)m_AIController).m_ProjectilesMineList[i].SetUpProjectile(((AIToadController)m_AIController).m_ProjectileSpawnPoint_Reference.transform.position, ((AIToadController)m_AIController).m_MinePositions[0].transform.position,
                        ((AIToadController)m_AIController).m_MinePositions[i + 1].transform.position);
                }
                //if (i == ((AIToadController)m_AIController).m_ProjectilesMineList.Count)
                //{
                    // Attack is finished
                    //((AIToadController)m_AIController).CurrentActionFinished();
                //}
            }

            // Attack is finished
            //((AIToadController)m_AIController).CurrentActionFinished();

            m_Mined = true;
        }

    }
}