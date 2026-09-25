using System.Text;

using MessageLoop.Common.Models.LongRun;

namespace MessageLoop.Node.Code.Extensions
{
    public static class StringExtensions
    {
        extension (string str)
        {
            public string BuildDataTree(IEnumerable<LongRunResult> results)
            {
                if (!results.Any())
                {
                    return str;
                }

                var root = new StringBuilder();
                foreach (var result in results)
                {
                    var tree = result.Data.ToString().AsSpan();
                    var segments = tree.Split(Environment.NewLine);
                    foreach (var range in segments)
                    {
                        var word = tree[range];
                        if (!word.IsEmpty)
                        {
                            root.Append(str).Append("-->").Append(tree[range]).AppendLine();
                        }
                    }
                }
                return root.ToString();
            }
        }
    }
}
