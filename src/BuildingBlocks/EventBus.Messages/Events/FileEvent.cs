namespace EventBus.Messages.Events
{
    public class FileEvent : IntegrationBaseEvent
    {
        public string FileName { get; set; }
        public string FileLocation { get; set; }
        public string TargetFileExt { get; set; }
    }
}
