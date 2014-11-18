using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Delegates
{
    public class Delegates
    {
        private delegate void SetTextDelegate(TextBox text_box, string text);
        private delegate int GetTrackBarValueDelegate(TrackBar trackBar);
        private delegate void SetEnabledDelegate(Control controller, bool enabled);

        private SetTextDelegate SetTextCallback;
        private GetTrackBarValueDelegate GetTrackBarValueCallback;
        private SetEnabledDelegate SetEnabledCallback;

        public Delegates()
        {
            SetTextCallback = this.SetText;
            GetTrackBarValueCallback = this.GetTrackBarValue;
            SetEnabledCallback = this.SetEnabled;
        }

        /***************************************************************************************
         * SET & GET User-Interface controls methods - DONT USE THEM! (hence the "private")
         ***************************************************************************************/
        private void SetText(TextBox text_box, string text)
        {
            text_box.Text = text;
        }

        private int GetTrackBarValue(TrackBar trackBar)
        {
            return trackBar.Value;
        }

        private void SetEnabled(Control controller, bool enabled)
        {
            controller.Enabled = enabled;
        }

        /***************************************************************************************
         * SET & GET User-Interface controls methods - USE ONLY THEM!
         ***************************************************************************************/
        public void SetControlText(System.Windows.Forms.Form form, Control control, string text)
        {
            if (control == null)
            {
                return;
            }
            else if (control.InvokeRequired)
            {
                form.BeginInvoke(SetTextCallback, new object[] { control, text });
            }
            else
            {
                control.Text = text;
            }
        }

        public void SetControlEnabled(System.Windows.Forms.Form form, Control controller, bool enabled)
        {
            if (controller == null)
            {
                return;
            }
            if (controller.InvokeRequired)
            {
                form.BeginInvoke(SetEnabledCallback, new object[] { controller, enabled });
            }
            else
            {
                controller.Enabled = enabled;
            }
        }
    }
}
