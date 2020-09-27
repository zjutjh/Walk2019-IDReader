using IdReader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace IdReader
{
    public sealed partial class GroupReader : UserControl
    {
        public GroupReader()
        {
            this.InitializeComponent();
        }

        public delegate void GroupReceivedEventHandler(object sender, System.EventArgs e);
        public event GroupReceivedEventHandler GroupReceived;


        public Group CurrentGroup
        {
            get { return (Group)GetValue(CurrentGroupProperty); }
            set { SetValue(CurrentGroupProperty, value); }
        }

        public static readonly DependencyProperty CurrentGroupProperty =
            DependencyProperty.Register("CurrentGroup", typeof(Group), typeof(GroupReader), new PropertyMetadata(new Group()));


        public Group LoadGroup(IDCard iDCard)
        {
            CurrentGroup = new Member(iDCard).GroupInfo;
            return CurrentGroup;
        }
        public Group LoadGroup(string idNumber)
        {
            CurrentGroup = new Member(idNumber).GroupInfo;
            return CurrentGroup;
        }

        public Group LoadGroup(Group group)
        {
            if (group != null)
                CurrentGroup = group;
            else
                CurrentGroup = new Group();
            return CurrentGroup;
        }
    }
}
