using IdReader.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace IdReader
{
    public sealed partial class OutScreen : Page
    {
        public OutScreen()
        {
            this.InitializeComponent();
        }
        public Member CurrentMember
        {
            get { return (Member)GetValue(CurrentMemberProperty); }
            set { SetValue(CurrentMemberProperty, value); }
        }
        public static readonly DependencyProperty CurrentMemberProperty = DependencyProperty.Register("CurrentMember", typeof(Member), typeof(OutScreen), new PropertyMetadata(new Member()));
       
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter == null)
            {
                Error.Visibility = Visibility.Visible;
                ErrMsg.Text = "等待刷卡";
                return;
            }

            CurrentMember = e.Parameter as Member;
            if (!CurrentMember.isOk)
            {
                Error.Visibility = Visibility.Visible;
                ErrMsg.Text = CurrentMember.msg;
            }
            else
            {
                Error.Visibility = Visibility.Collapsed;
            }

        }
    }
}
