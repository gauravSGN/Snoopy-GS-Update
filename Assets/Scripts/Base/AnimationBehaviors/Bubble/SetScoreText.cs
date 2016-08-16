using UnityEngine;

public class SetScoreText : StateMachineBehaviour
{
    private int previousScore = -1;
    private BubbleScore bubbleScore;
    private TextMesh textMesh;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (bubbleScore == null)
        {
            var gameObject = animator.gameObject;
            var parent = gameObject.transform.parent;

            textMesh = gameObject.GetComponent<TextMesh>();
            bubbleScore = parent.GetComponent<BubbleScore>();

            gameObject.transform.SetParent(null, true);
        }

        if (bubbleScore.Score != previousScore)
        {
            textMesh.text = bubbleScore.Score.ToString();
            previousScore = bubbleScore.Score;
        }
    }
}
