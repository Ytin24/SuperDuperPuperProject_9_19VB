using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SuperDuperPuperProject_9_19VB {
    internal static class GetImage {
        internal static ImageSource doGetImageSourceFromResource(string psAssemblyName, string psResourceName) {
            Uri oUri = new Uri("pack://application:,,,/" + psAssemblyName + ";component/" + psResourceName, UriKind.RelativeOrAbsolute);
            return BitmapFrame.Create(oUri);
        }
    }
}
