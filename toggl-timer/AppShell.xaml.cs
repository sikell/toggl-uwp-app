using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace toggl_timer
{
    public sealed partial class AppShell : Page
    {
        public AppShell()
        {
            InitializeComponent();
        }

        public void SetContentFrame(Frame frame)
        {
            RootSplitView.Content = frame;
        }

        public void SetMenuPaneContent(UIElement content)
        {
            RootSplitView.Pane = content;
        }
    }
}
