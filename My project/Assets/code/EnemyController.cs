using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
   public enum EnemyStates { Idle, Chase, Attack, Hit, Death }
   public EnemyStates state = EnemyStates.Idle;
   public NavMeshAgent nav;
    public Animator anim;
   public float chaseDistance = 50;
   public float attackDistance = 2;

   public float stunDuration = 1;
   public float deathDuration = 5;
   public float attackDuration = 5;

   public GameObject hitParticle;

   private bool locked = false;
   private PlayerController player;
   private float distance = 1000;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void Update()
    {
        if (locked) return;
        distance = Vector3.Distance(transform.position, player.transform.position);
        switch (state)
        {
           case EnemyStates.Idle:
                IdleUpdate();
                break;
           case EnemyStates.Chase:
                ChaseUpdate();
                break;
           case EnemyStates.Attack:
                AttackUpdate();
                break;
           case EnemyStates.Hit:
                HitUpdate();
                break;         
        } 
    }

    void IdleUpdate()
    {
      if (distance < chaseDistance)
      {
        if(distance < attackDistance)
        {
            EnterAttack();
        }
        else
        {
            nav.isStopped = false;
            state = EnemyStates.Chase;
            anim.SetBool("IsRunning", true);
        }
      }
      else
      {
        nav.isStopped = true;
      }
    }

    void ChaseUpdate()
    {
        Debug.Log(distance);
      if (distance < attackDistance)
        {
            Debug.Log("enter attack");
            EnterAttack();
      }
      else if(distance >= chaseDistance)
      {
        state = EnemyStates.Idle;
        anim.SetBool("IsRunning", false);
        nav.isStopped = true;
        }
      else
      {
        nav.isStopped = false;
        nav.SetDestination(player.transform.position);
      }
    }

    void AttackUpdate()
    {
         if (distance >= attackDistance)
         {
            if (distance >= chaseDistance)
            {
                nav.isStopped = true;
                state = EnemyStates.Idle;
                anim.SetBool("IsRunning", false);
            }
            else
            {
              state = EnemyStates.Chase;
                anim.SetBool("IsRunning", true);
                nav.isStopped = false;
            }
         }
         else
         {
            EnterAttack();
         }
    }

    void HitUpdate()
    {

    }

     public void GetHit()
     {
        locked = true;
        Instantiate(hitParticle, transform);
        nav.isStopped = true;
        CancelInvoke("DealDamage");
        CancelInvoke("Unlock");

        anim.SetTrigger("Hit");

        state = EnemyStates.Hit;
        Invoke("Unlock", stunDuration);
     }
 
     void Unlock()
    {
        locked = false;
    }

    void EnterAttack()
    {
        anim.SetTrigger("Attack");
       state = EnemyStates.Attack;
       locked = true;
       nav.isStopped = true;
       CancelInvoke("DealDamage");
       CancelInvoke("Unlock");
       Invoke("DealDamage", 0.5f);
       Invoke("Unlock", attackDuration);
        anim.SetBool("IsRunning", false);
    }

    void DealDamage()
     {
       Collider[] hitcolliders = Physics.OverlapSphere(transform.position, attackDistance);
       foreach (var target in hitcolliders)
       {
           if(target.CompareTag("Player"))
           {
              player.GetHit(1, Vector3.MoveTowards(target.transform.position, transform.position, 0.5f));
              break;
           }
       }
     }
}