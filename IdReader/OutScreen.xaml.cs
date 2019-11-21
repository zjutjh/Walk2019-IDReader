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
            try
            {
                if (e != null)
                {
                    base.OnNavigatedTo(e);
                    if (e.Parameter != null)
                    {

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
                    else
                    {
                        Error.Visibility = Visibility.Visible;
                        ErrMsg.Text = "等待刷卡";
                    }
                }
            }
            catch (System.Exception)
            {

              
            }
          
           

        }
    }
}
