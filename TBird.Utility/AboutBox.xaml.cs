namespace TBird.Utility
{
#if !NETFX_CORE
    using System.Diagnostics.CodeAnalysis;
#endif
    using System.Windows;
    using System.Windows.Input;

    using TBird.Utility.ViewModels;

    /// <summary>
    /// Interaction logic for AboutBox.xaml
    /// </summary>
#if !NETFX_CORE
    [ExcludeFromCodeCoverage]
#endif
    public partial class AboutBox : Window
    {
        public AboutBox()
        {
            this.InitializeComponent();
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (s, e) => this.Close()));
            this.DataContext = new AboutBoxViewModel();
        }
    }
}
