using System.IO.Abstractions;

namespace PicklesDoc.Pickles.TestFrameworks
{
    public interface ISingleResultLoader
    {
        SingleTestRunBase Load(IFileInfo fileInfo);
    }
}