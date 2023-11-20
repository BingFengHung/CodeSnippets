namespace CodeSnippets.Views
{
    public class TreeNode
    {
        public string Name { get; set; }

        public object Children { get; set; }

        public TreeNode()
        {
            //Children = new ObservableCollection<TreeNode>();
        }
    }
}
