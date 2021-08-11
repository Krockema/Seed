using Seed.Data;
using System.Reflection.Metadata;

namespace Seed.Generator.TraverseActions
{
    public interface ITraverseAction
{
        void Do(MaterialEdge edge, params object[] options);
    }
}
