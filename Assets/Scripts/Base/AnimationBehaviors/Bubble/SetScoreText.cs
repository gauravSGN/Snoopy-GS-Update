using Service;
using Registry;
using UnityEngine;
using UnityEngine.UI;

public class SetScoreText : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var gameObject = animator.gameObject;
        var parent = gameObject.transform.parent;
        var bubbleScore = parent.GetComponent<BubbleScore>();

        var color = bubbleScore.Model.definition.BaseColor;
        gameObject.GetComponent<Outline>().effectColor = color;
        gameObject.GetComponent<Shadow>().effectColor = color;

        gameObject.GetComponent<Text>().text = bubbleScore.Score.ToString();

        gameObject.GetComponent<MoveToRegisteredCanvas>().MoveToCanvas();
    }
}
