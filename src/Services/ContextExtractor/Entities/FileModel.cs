using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileContextExtractor.Entities
{
    public class FileModel
    {
        public string FileName { get; set; }
        public string FileLocation { get; set; }
        public string TargetFileExt { get; set; }
    }
    // Just the conversion to store the JSON that we are receiving from Queue....

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Value
    {
        public string FileName { get; set; }
        public string FileLocation { get; set; }
        public string TargetFileExt { get; set; }
    }

    public class Message
    {
        public string Key { get; set; }
        public Value Value { get; set; }
    }

    public class Root
    {
        public string Name { get; set; }
        public Message Message { get; set; }
    }
}
