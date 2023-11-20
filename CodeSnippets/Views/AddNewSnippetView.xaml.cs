using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CodeSnippets.Views
{
    /// <summary>
    /// AddNewSnippetView.xaml 的互動邏輯
    /// </summary>
    public partial class AddNewSnippetView : UserControl
    {
        public AddNewSnippetView()
        {
            InitializeComponent();

            ViewModel = this.user.DataContext as AddNewSnippetViewModel;
        }

        internal AddNewSnippetViewModel ViewModel
        {
            get => user.DataContext as AddNewSnippetViewModel;
            set => user.DataContext = value;
        }


        public ICommand ConfirmCommand
        {
            get { return (ICommand)GetValue(ConfirmCommandProperty); }
            set { SetValue(ConfirmCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConfirmCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConfirmCommandProperty =
            DependencyProperty.Register("ConfirmCommand", typeof(ICommand), typeof(AddNewSnippetView), new PropertyMetadata(null, OnConfirmCommandPropertyChanged));

        private static void OnConfirmCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as AddNewSnippetView;
            view.ViewModel.Confirm_Click = (ICommand)e.NewValue;
        }

        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CancelCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register("CancelCommand", typeof(ICommand), typeof(AddNewSnippetView), new PropertyMetadata(null, OnCancelCommandPropertyChanged));

        private static void OnCancelCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as AddNewSnippetView;
            view.ViewModel.Cancel_Click = (ICommand)e.NewValue;
        }



        public object ConfirmCommandParameter
        {
            get { return (object)GetValue(ConfirmCommandParameterProperty); }
            set { SetValue(ConfirmCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConfirmCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConfirmCommandParameterProperty =
            DependencyProperty.Register("ConfirmCommandParameter", typeof(object), typeof(AddNewSnippetView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnConfirmCommandParameterPropertyChanged));

        private static void OnConfirmCommandParameterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as AddNewSnippetView;
            view.ViewModel.Result = (AddModel)e.NewValue;
        }
    }
}
