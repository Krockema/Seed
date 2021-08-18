
namespace Seed.Cli;
public class Mappings : Dictionary<string, string>
{
    public Mappings()
    {
        this.Add("-mc", "MaterialConfig");
        this.Add("-rc", "ResourceConfig");
        this.Add("-out", "OutputPath");
        this.Add("-q", "Quiet");
    }

}
