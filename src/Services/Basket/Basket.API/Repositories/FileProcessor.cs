using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileProcessor.Entities;
using FileProcessor.Helper;
using FileProcessor.Queues;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace FileProcessor.Repositories.Interfaces
{
    public class FileProcessorClass : IFileProcessorRepository
    {
        private readonly IDistributedCache _redisCache;
        private IOptions<EventBusSettings> _appConfig;

        public FileProcessorClass(IDistributedCache cache, IOptions<EventBusSettings> appConfig)
        {
            _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
            _appConfig = appConfig;
        }
        public async Task<Dictionary<string, FileModel>> GetFiles(string FileLocation, string TargetFileExt)
        {
            ////Read From DB In Future...
            //var files = await _redisCache.GetStringAsync(FileLocation,TargetFileExt);
            //

            var files = new Dictionary<string, FileModel>();

            string ConstProducerName = "File Process Service 1";
            List<FileModel> DummyFiles = await giveMeSomeDummyFiles();

            if (DummyFiles.Count == 0)
                return null;

            foreach (var item in DummyFiles)
            {
                if (item.TargetFileExt == TargetFileExt)
                {
                    files.Add(ConstProducerName + " - " + item.FileName, item);
                };
            }
            sendFilesToQueue(files);
            return files;
        }
        private void sendFilesToQueue(Dictionary<string, FileModel> data)
        {
            Publisher publisher = new Publisher(_appConfig);
            publisher.ConnectMQ(data);
        }
        public async Task<List<FileModel>> giveMeSomeDummyFiles()
        {
            List<FileModel> files = new List<FileModel>()
            {
               new FileModel
               {
                   FileLocation = "MyData Location",
                   FileName = "English Book",
                   TargetFileExt = ".txt"
               },
               new FileModel
               {
                   FileLocation = "MyData2 Location",
                   FileName = "Arbic Book",
                   TargetFileExt = ".txt"
               },
               new FileModel
               {
                   FileLocation = "MyData3 Location",
                   FileName = "Language Book",
                   TargetFileExt = ".pdf"
               },
               new FileModel
               {
                   FileLocation = "MyData4 Location",
                   FileName = "Cartoon Book",
                   TargetFileExt = ".csv"
               },
               new FileModel
               {
                   FileLocation = "MyData5 Location",
                   FileName = "Some other Book",
                   TargetFileExt = ".txt"
               },
               new FileModel
               {
                   FileLocation = "MyData6 Location",
                   FileName = "Computer Science Book",
                   TargetFileExt = ".txt"
               },
               new FileModel
               {
                   FileLocation = "MyData7 Location",
                   FileName = "One More Book",
                   TargetFileExt = ".pdf"
               }
            };
            return files;
        }
    }
}
