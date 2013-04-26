namespace Mvc.ViewModels
{
    public class TopicViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int RecordCount { get; set; }
        public byte[] Timestamp { get; set; }
    }
}