using System.Windows.Input;

namespace CodeSnippets.Views
{
    class AddNewSnippetViewModel : ViewModelBase
    {
        public ICommand Confirm_Click { get; set; }

        public ICommand Cancel_Click { get; set; }

        private AddModel _result;
        public AddModel Result
        {
            get => _result;
            set => Set(ref _result, value);
        }
    }

    class AddModel : ViewModelBase
    {
        private string _topic;
        public string Topic
        {
            get => _topic;
            set => Set(ref _topic, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        private string _content;
        public string Content
        {
            get => _content;
            set => Set(ref _content, value);
        }
    }
}
