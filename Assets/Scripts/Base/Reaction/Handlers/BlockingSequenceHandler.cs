using Service;
using System.Collections;

namespace Reaction
{
    [ReactionHandlerAttribute(ReactionPriority.PostPowerUpAnimation)]
    [ReactionHandlerAttribute(ReactionPriority.PostPopAnimation)]
    [ReactionHandlerAttribute(ReactionPriority.PostGenericPopAnimation)]
    [ReactionHandlerAttribute(ReactionPriority.PostChainPopAnimation)]
    public class BlockingSequenceHandler : BaseReactionHandler<BubbleReactionEvent>
    {
        private int active = 0;

        override public int Count { get { return active; } }

        override public IEnumerator HandleActions()
        {
            active = 0;
            var sequenceService = GlobalState.Instance.Services.Get<SequenceService>();
            var index = 0;

            while (index < sequenceService.BlockingSequences.Count)
            {
                yield return null;

                for (; index < sequenceService.BlockingSequences.Count; index++)
                {
                    if (sequenceService.BlockingSequences[index].Blocking)
                    {
                        break;
                    }
                }
            }

            sequenceService.ResetBlockingSequences();
        }

        override protected void OnReactionEvent(BubbleReactionEvent gameEvent)
        {
            if (gameEvent.priority == priority)
            {
                active++;
            }
        }
    }
}
