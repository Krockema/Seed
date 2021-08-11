using Seed.Data;

namespace Seed.Generator.TraverseActions
{
    public class TraverseActionOutputString : ITraverseAction
    {
        public void Do(MaterialEdge edge, params object[] options)
        {
            var intend = "".PadLeft(edge.To.InitialLevel * 2 , '-');
            System.Diagnostics.Debug.WriteLine(intend + $"> {edge.From.InitialLevel} {edge.From.Guid}");
        }
    }
}
