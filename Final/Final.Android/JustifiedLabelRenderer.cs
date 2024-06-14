using Android.Text;
using Final;
using Final.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(JustifiedLabel), typeof(JustifiedLabelRenderer))]
namespace Final.Droid
{
    public class JustifiedLabelRenderer : LabelRenderer
    {
        public JustifiedLabelRenderer(Android.Content.Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Control.JustificationMode = JustificationMode.InterWord;
            }
        }
    }
}
