using Service;
using System.Collections.Generic;

namespace Sequence
{
    public class SequenceRegistry : SequenceService
    {
        public List<BlockingSequence> BlockingSequences { get; private set;}

        public SequenceRegistry()
        {
            BlockingSequences = new List<BlockingSequence>();
        }

        public void AddBlockingSequence(BlockingSequence sequence)
        {
            BlockingSequences.Add(sequence);
        }

        public void ResetBlockingSequences()
        {
            BlockingSequences.Clear();
        }
    }
}
