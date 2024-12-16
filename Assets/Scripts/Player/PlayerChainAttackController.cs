using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChainAttackController : MonoBehaviour
{
    Player player;

    [Header("Settings")]
    public float chainAttackTime = 10f;
    public float chainAttackSpeed = 5;
    public float stoppingDistance = 1;
    public float attackSpeedMultiplier = 0.25f;
    public float attackRadius = 20;
    
    public List<Character> potentialTargets = new List<Character>();

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void PerformChainAttack()
    {
        if (player.performingAction) return;
        
        Collider[] targets = Physics.OverlapSphere(player.transform.position, attackRadius, LayerMaskManager.instance.characterLayer);

        foreach (var target in targets)
        {
            Character targetCharacter = target.GetComponent<Character>();
            
            if (targetCharacter == null) continue;
            
            if (targetCharacter.CompareTag("Player")) continue;
            
            potentialTargets.Add(targetCharacter);
        }

        if (potentialTargets.Count != 0)
        {
            foreach (Character character in potentialTargets)
            {
                character.characterAnimatorManager.IsMoving = false;
            }
            StartCoroutine(AttackTargets());
        }
    } 
    
    private IEnumerator AttackTargets()
    {
        float currentAttackSpeed = attackSpeedMultiplier;
        
        player.canMove = false;
        player.canRotate = false;
        
        foreach (var potentialTarget in potentialTargets)
        {
            if (potentialTarget == null) continue;
            
            if (potentialTarget.isDead) continue;
            
            Vector3 directionToTarget = potentialTarget.transform.position - player.transform.position;
            directionToTarget.y = 0;
            directionToTarget.Normalize();
            
            RotateTowardsTarget(potentialTarget);
            //Vector3 stopPosition = potentialTarget.transform.position - (directionToTarget * stoppingDistance);

            player.playerAnimatorManager.SetAnimatorParameters(0, 0);
            player.playerAnimatorManager.ChainDashDone = false;
            player.playerAnimatorManager.PlayDashFwdBegin();
            yield return null;

            while (Vector3.Distance(player.transform.position, potentialTarget.transform.position) > stoppingDistance)
            {
                Vector3 movement = chainAttackSpeed * Time.deltaTime * directionToTarget;
                player.controller.Move(movement);
                
                yield return null;
            }
            
            while (!potentialTarget.isDead)
            {
                player.playerAnimatorManager.PlayAttackAction(
                    player.playerInventoryManager.currentRightHandWeapon.liteAttackAction, true);
                
                RotateTowardsTarget(potentialTarget);
                yield return null;

                var animState = player.animator.GetNextAnimatorStateInfo(1);
                float timeElapsed = 0;

                while (timeElapsed <= animState.length)
                {
                    timeElapsed += Time.deltaTime;

                    Time.timeScale = Mathf.Clamp(timeElapsed / animState.length + currentAttackSpeed, 0.5f, 2.0f);
                    Time.fixedDeltaTime = 0.02f * Time.timeScale;

                    yield return null;
                }
                
                currentAttackSpeed *= 1.5f;
                Time.timeScale = 1;
            }

            /*float timeElapsed = 0f;
            while (timeElapsed < chainAttackTime)
            {
                timeElapsed += Time.deltaTime;
                
                Debug.Log("chain attack time : " + timeElapsed);
                
                if (potentialTarget.isDead) break;
                
                yield return null;
            }*/
            
            yield return null;
        }
        
        player.playerAnimatorManager.ChainDashDone = true;
        potentialTargets.Clear();
    }

    private void RotateTowardsTarget(Character potentialTarget)
    {
        Vector3 directionToTarget = potentialTarget.transform.position - player.transform.position;
        directionToTarget.y = 0;
        directionToTarget.Normalize();
            
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        player.transform.rotation = targetRotation;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}