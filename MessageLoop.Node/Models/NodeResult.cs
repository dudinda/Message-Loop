using MessageLoop.Common.Models.LongRun;

namespace MessageLoop.Node.Models
{
    public class NodeResult
    {
        public object Result { get; set; }
        public List<LongRunResult> ChildResults { get; set; }
    }
}
