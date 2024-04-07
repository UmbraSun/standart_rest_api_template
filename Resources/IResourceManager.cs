using System.Globalization;

namespace Resources
{
    public interface IResourceManager
    {
        void SetCulture(CultureInfo language);

        CultureInfo CurrentCulture { get; }
    }
}
