using Resources.Words;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace Resources
{
    public class ResourceManager : IResourceManager, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public ResourceManager() 
        {
            SetCulture(new CultureInfo("en-US"));
        }

        public void SetCulture(CultureInfo language)
        {
            Thread.CurrentThread.CurrentUICulture = language;
            Resource.Culture = language;
            Invalidate();
        }

        public CultureInfo CurrentCulture => Resource.Culture;

        public void Invalidate()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
