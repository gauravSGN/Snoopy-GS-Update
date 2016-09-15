using Sequence;
using System.Collections.Generic;

namespace Service
{
    public interface SequenceService : SharedService
    {
        List<BlockingSequence> BlockingSequences { get; }

        void AddBlockingSequence(BlockingSequence sequence);
        void ResetBlockingSequences();
    }
}
