using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace CodeSnippets.Views
{
    class SnippetTreeViewModel : ViewModelBase
    {
        string _dirFullPath;
        string _fileFullPath;
        private List<SnippetTopic> _topics;

        public SnippetTreeViewModel()
        {
            Initial();

            _topics = GetSnippets();

            Nodes = new ObservableCollection<TreeNode>();
            foreach (var topic in _topics)
            {
                var node = new TreeNode();
                node.Name = topic.Title;

                var child = new ObservableCollection<TreeNode>();
                for (var i = 0; i < topic.Topic.Count; i++)
                {
                    child.Add(new TreeNode
                    {
                        Name = topic.Topic[i].Title,
                        Children = new ObservableCollection<TreeNode>
                        {
                            new TreeNode
                            {
                                Name = topic.Topic[i].Title,
                                Children = topic.Topic[i].Content, //new ObservableCollection<TreeNode>{
                                    //new TreeNode {
                                        //Name=topic.Topic[i].Content
                                    //}
                                //}
                            }
                    }
                    });
                }

                node.Children = child;

                Nodes.Add(node);
            }


            AddModel = new AddModel();
        }

        public ObservableCollection<TreeNode> Nodes { get; set; }

        private AddModel _addModel;
        public AddModel AddModel
        {
            get => _addModel;
            set => Set(ref _addModel, value);
        }

        private bool _isAddPanelVsb = false;
        public bool IsAddPanelVsb
        {
            get => _isAddPanelVsb;
            set => Set(ref _isAddPanelVsb, value);
        }

        private void InsertSnippetCode(string code)
        {
            IVsTextManager textManager = (IVsTextManager)ServiceProvider.GlobalProvider.GetService(typeof(SVsTextManager));
            textManager.GetActiveView(1, null, out IVsTextView vsTextView);

            // 轉換 IVsTextView 為 IWpfTextView
            IWpfTextView wpfTextView = GetWpfTextView(vsTextView);
            if (wpfTextView == null)
            {
                return;
            }

            // 使用 IWpfTextView 來獲取光標位置
            SnapshotPoint caretPosition = wpfTextView.Caret.Position.BufferPosition;

            // 在光標位置插入文本
            using (var edit = wpfTextView.TextBuffer.CreateEdit())
            {
                edit.Insert(caretPosition, code);
                edit.Apply();
            }
        }

        private IWpfTextView GetWpfTextView(IVsTextView vsTextView)
        {
            var userData = vsTextView as IVsUserData;
            if (userData == null)
            {
                return null;
            }

            Guid guidWpfTextViewHost = Microsoft.VisualStudio.Editor.DefGuidList.guidIWpfTextViewHost;
            userData.GetData(ref guidWpfTextViewHost, out object wpfTextViewHost);
            return (wpfTextViewHost as IWpfTextViewHost)?.TextView;
        }

        public RelayCommand InsertContent_Click => new RelayCommand(x =>
        {
            var collections = x as CodeSnippets.Views.TreeNode;

            var node = collections.Children as ObservableCollection<TreeNode>;
            var code = node[0].Children as string;

            InsertSnippetCode(code);
        });

        public RelayCommand DeleteContent_Click => new RelayCommand(x =>
        {

        });

        public RelayCommand AddNewSnippet_Click => new RelayCommand(x =>
        {
            IsAddPanelVsb = true;
        });

        public RelayCommand AddConfirm_Click => new RelayCommand(x =>
        {

            AddModel model = AddModel;

            var list = ConvertNodeToList();

            var find = list.Where(s => s.Title == model.Topic).FirstOrDefault();

            if (find != null)
            {


                find.Topic.Add(new Snippet
                {
                    Title = model.Topic,
                    Content = model.Content,
                });
            }
            else
            {
                list.Add(new SnippetTopic
                {
                    Title = model.Topic,
                    Topic = new List<Snippet>
                {
                    new Snippet{ Content = model.Content },
                }
                });
            }

            Save(list);

            IsAddPanelVsb = false;
        });

        public RelayCommand AddCancel_Click => new RelayCommand(x =>
        {
            IsAddPanelVsb = false;
        });

        private void Initial()
        {
            string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dir = "CodeSnippets";

            _dirFullPath = Path.Combine(rootPath, dir);

            if (!Directory.Exists(_dirFullPath))
                Directory.CreateDirectory(_dirFullPath);

            string filename = "snippet.json";

            _fileFullPath = Path.Combine(_dirFullPath, filename);

            if (!File.Exists(_fileFullPath))
                File.Create(_fileFullPath).Close();
        }

        private List<SnippetTopic> GetSnippets()
        {
            List<SnippetTopic> topics = new List<SnippetTopic>();
            try
            {
                // 讀取 JSON 檔案內容
                string jsonData = File.ReadAllText(_fileFullPath);

                // 反序列化 JSON 到類
                var list = JsonConvert.DeserializeObject<List<SnippetTopic>>(jsonData);

                if (list != null) topics = list;

                return topics;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading or parsing JSON file: " + ex.Message);
            }

            return new List<SnippetTopic>();
        }

        private List<SnippetTopic> ConvertNodeToList()
        {
            List<SnippetTopic> list = new List<SnippetTopic>();
            foreach (var node in Nodes)
            {
                var topic = new SnippetTopic();
                topic.Title = node.Name;
                topic.Topic = new List<Snippet>();

                foreach (var n in node.Children as ObservableCollection<TreeNode>)
                {
                    topic.Topic.Add(new Snippet
                    {
                        Title = n.Name,
                        Content = (string)((ObservableCollection<TreeNode>)n.Children)[0].Children
                    });
                }

                list.Add(topic);
            }

            return list;
        }

        private void Save(List<SnippetTopic> topics)
        {
            string jsonString = JsonConvert.SerializeObject(topics, Formatting.Indented);

            try
            {
                File.WriteAllText(_fileFullPath, jsonString);
                Console.WriteLine("JSON file written successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing JSON file: " + ex.Message);
            }
        }
    }

    class SnippetTopic
    {
        public string Title { get; set; }

        public List<Snippet> Topic { get; set; }
    }

    class Snippet
    {
        public string Title { get; set; }

        public string Content { get; set; }
    }
}
