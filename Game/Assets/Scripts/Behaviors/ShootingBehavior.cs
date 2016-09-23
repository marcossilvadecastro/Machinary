using UnityEngine;
using System.Collections;

public class ShootingBehavior : StateMachineBehaviour {

    private bool shooting = false;
    public Vector2 minMaxWait = new Vector2(0.1f, 0.2f);

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FighterBase controller = animator.GetComponentInParent<FighterBase>();
        if (controller != null && !shooting) {
            shooting = true;
            controller.AnimationShootingIsPlaying = shooting;
            controller.StartCoroutine(Shoot(controller));
            controller.IsAttaking = true;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FighterBase controller = animator.GetComponentInParent<FighterBase>();
        shooting = false;
        controller.AnimationShootingIsPlaying = shooting;
        controller.IsAttaking = false;
    }

    IEnumerator Shoot(FighterBase controller)
    {
        while (shooting)
        {
            controller.SendMessage("Attack");
            float time = Random.Range(minMaxWait.x, minMaxWait.y);
            yield return new WaitForSeconds(time);
        }
    }
    
}
 