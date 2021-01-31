using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class MrSassyController : Interactable
{
    public bool debugTestAccept;
    public SpriteRenderer RequestItemRef;
    public Item currentRequestItem;

    public SpriteRenderer feedingItem;

    private PlayableDirector director;
    public PlayableAsset showQuestItem;
    public PlayableAsset hideQuestItem;
    public PlayableAsset feedMrSassy;

    public RuntimeAnimatorController animatorController;

    private Animator animator;
    [HideInInspector] public bool showHint;
    [HideInInspector] public bool EatingItem;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorController;
        director = GetComponent<PlayableDirector>();
        RollUpNewRequest();
    }

    public void RollUpNewRequest()
    {
        currentRequestItem = new Item(ItemListManager.pickRandomItemFromGroup());
        RequestItemRef.sprite = currentRequestItem.stats.icon;
    }

    public void FixedUpdate()
    {
        float distance = Vector3.Distance(PlayerController.instance.getFocusObject().transform.position, this.transform.position);
        if(distance < 2.0f)
        {
            if (showHint == false && EatingItem == false)
            {
                showQuest();
            }
        }
        else
        {
            if (showHint == true)
            {
                hideQuest();
            }
        }
    }

    public void showQuest()
    {
        animator.StopPlayback();
        animator.runtimeAnimatorController = null;
        showHint = true;
        director.Play(showQuestItem);
    }
    public void hideQuest()
    {
        animator.StopPlayback();
        animator.runtimeAnimatorController = null;
        showHint = false;
        director.Play(hideQuestItem);
    }

    public void FinishedAnimation()
    {
        animator.runtimeAnimatorController = animatorController;
        if (EatingItem == true)
        {
            EatingItem = false;
            showHint = false;
            RewardPlayer(currentRequestItem.stats.questReward);
            RollUpNewRequest();
            
        }
        PlayerController.instance.LockedCharacter = false;
    }

    public override void Interact()
    {
        if (showHint && EatingItem == false)
        {
            feedingItem.sprite = RequestItemRef.sprite;
            EatingItem = true;
            PlayerController.instance.LockedCharacter = true;
            animator.StopPlayback();
            animator.runtimeAnimatorController = null;
            director.Play(feedMrSassy);
        }
    }
    public void RewardPlayer(string questReward)
    {
        switch (questReward)
        {
            case "health+":
                PlayerController.instance.characterHealth.TotalHealth += 1;
                PlayerController.instance.characterHealth.currentHealth += 1;
                GameHandler.instance.CreateUpgradePopUp("Health +");
                break;
            case "damage+":
                PlayerController.instance.attacker.AttackDamage += 1;
                GameHandler.instance.CreateUpgradePopUp("Claws +");
                break;
            case "fullHeal":
                PlayerController.instance.characterHealth.currentHealth = PlayerController.instance.characterHealth.TotalHealth;
                GameHandler.instance.CreateUpgradePopUp("Fully healed");
                break;
        }

        PlayerController.instance.attacker.HealthUpdated.Invoke(); //updateUI
    }
}
