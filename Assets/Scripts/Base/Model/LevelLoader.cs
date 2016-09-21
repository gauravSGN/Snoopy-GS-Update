using Goal;
using Model;
using PowerUps;
using UnityEngine;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private BubbleFactory bubbleFactory;

    [SerializeField]
    private PowerUpController powerUpController;

    [SerializeField]
    private BubbleQueueType bubbleQueueType;

    [SerializeField]
    private BubblePlacer placer;

    private LevelConfiguration configuration;

    public LevelData LevelData { get; private set; }
    public BubbleQueueType BubbleQueueType { get { return bubbleQueueType; } }
    public LevelConfiguration Configuration { get { return configuration; } }

    public void LoadLevel(string levelData)
    {
        LevelData = JsonUtility.FromJson<LevelData>(levelData);

        configuration = new LevelConfiguration(LevelData);
        bubbleFactory.Configuration = configuration;

        CreateLevel(LevelData);
        powerUpController.Setup(LevelData.PowerUpFills);
    }

    private void CreateLevel(LevelData level)
    {
        var bubbleMap = new Dictionary<int, GameObject>();

        foreach (var bubble in level.Bubbles)
        {
            bubbleMap[bubble.Key] = placer.PlaceBubble(bubble);
            bubble.model = bubbleMap[bubble.Key].GetComponent<BubbleModelBehaviour>().Model;

            if (!configuration.Counts.ContainsKey(bubble.model.type))
            {
                configuration.Counts[bubble.model.type] = 0;
            }

            configuration.Counts[bubble.model.type]++;
        }

        AttachBubbles(bubbleMap);
        ComputeRootDistances(bubbleMap.Values);

        foreach (var pair in bubbleMap)
        {
            pair.Value.GetComponent<BubbleModelBehaviour>().Model.SortNeighbors();
        }

        CreateGoals(level);
    }

    private static void CreateGoals(LevelData level)
    {
        level.goals = LevelGoalFactory.GetGoalsForLevel(level);

        foreach (var goal in level.goals)
        {
            GlobalState.EventService.Dispatch(new GoalCreatedEvent(goal));
        }
    }

    private void AttachBubbles(Dictionary<int, GameObject> bubbleMap)
    {
        var neighbors = new int[6];
        var ceilingBubbleCount = GlobalState.Instance.Config.bubbles.numPerRow + 1;
        var ceilingBubbleMap = new Dictionary<int, GameObject>();

        for (int ceilingX = 0; ceilingX < ceilingBubbleCount; ceilingX++)
        {
            var ceilingData = new BubbleData(ceilingX - 1, -1, BubbleType.Ceiling);
            ceilingBubbleMap[ceilingX - 1] = placer.PlaceBubble(ceilingData);
        }

        foreach (var pair in bubbleMap)
        {
            int x = pair.Key & 0xf;
            int y = pair.Key >> 4;
            var attachments = pair.Value.GetComponent<BubbleAttachments>();

            GetNeighbors(x, y, neighbors);

            foreach (var neighbor in neighbors)
            {
                if (neighbor >= 0 && bubbleMap.ContainsKey(neighbor))
                {
                    attachments.Attach(bubbleMap[neighbor]);
                }
            }

            if (y == 0)
            {
                attachments.Attach(ceilingBubbleMap[x - 1]);
                attachments.Attach(ceilingBubbleMap[x]);
            }
        }

        ComputeRootDistances(ceilingBubbleMap.Values);
    }

    private void ComputeRootDistances(IEnumerable<GameObject> bubbles)
    {
        foreach (var bubble in bubbles)
        {
            var model = bubble.GetComponent<BubbleModelBehaviour>().Model;

            if (model.IsRoot)
            {
                model.DistanceFromRoot = 0;
                model.PropagateRootDistance();
            }
        }
    }

    private void GetNeighbors(int x, int y, int[] neighbors)
    {
        var offset = (y & 1) * 2 - 1;

        neighbors[0] = BubbleData.GetKey(x + offset, y - 1);
        neighbors[1] = BubbleData.GetKey(x, y - 1);
        neighbors[2] = BubbleData.GetKey(x - 1, y);
        neighbors[3] = BubbleData.GetKey(x + 1, y);
        neighbors[4] = BubbleData.GetKey(x + offset, y + 1);
        neighbors[5] = BubbleData.GetKey(x, y + 1);
    }
}
