using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell.Interop;

namespace FirstToolWin
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("4c08165e-74d2-479d-8d97-5c96535b2c72")]
    public class FirstToolWindow : ToolWindowPane
    {
        public FirstToolWindowControl control;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstToolWindow"/> class.
        /// </summary>
        public FirstToolWindow() : base(null)
        {
            this.Caption = "FirstToolWindow";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            //this.Content = new FirstToolWindowControl();

            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;

            control = new FirstToolWindowControl();
            base.Content = control;

            this.ToolBar = new CommandID(new Guid(FirstToolWindowCommand.guidFirstToolWindowPackageCmdSet),
                FirstToolWindowCommand.ToolbarID);
            this.ToolBarLocation = (int)VSTWT_LOCATION.VSTWT_TOP;
        }
    }
}
