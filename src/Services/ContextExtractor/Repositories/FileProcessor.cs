using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileContextExtractor.Entities;
using FileContextExtractor.Helper;
using FileContextExtractor.Queues;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace FileContextExtractor.Repositories.Interfaces
{
    public class FileProcessorClass 
    {
        private readonly IDistributedCache _redisCache;
        private IOptions<EventBusSettings> _appConfig;

        public FileProcessorClass(IDistributedCache cache, IOptions<EventBusSettings> appConfig)
        {

        }
        //public async Task<Dictionary<string, FileModel>> GetFiles(string FileLocation, string TargetFileExt)
        //{
        //    ////Read From DB In Future...
        //    //var files = await _redisCache.GetStringAsync(FileLocation,TargetFileExt);
        //    //

        //    var files = new Dictionary<string, FileModel>();

        //    string ConstProducerName = "File Process Service 1";
        //    List<FileModel> DummyFiles = await giveMeSomeDummyFiles();

        //    if (DummyFiles.Count == 0)
        //        return null;

        //    foreach (var item in DummyFiles)
        //    {

        //        if (item.TargetFileExt == TargetFileExt)
        //        {
        //            files.Add(
        //                    ConstProducerName+" - "+item.FileName,
        //                        new FileModel
        //                        {
        //                            FileLocation = item.FileLocation,
        //                            FileName = item.FileName,
        //                            TargetFileExt = item.TargetFileExt
        //                        }
        //                  );
        //            };
                
        //    }
        //    sendFilesToQueue(files);
        //    return files;
        //}
        //private void sendFilesToQueue(Dictionary<string, FileModel> data)
        //{
        //    Publisher publisher = new Publisher(_appConfig);
        //    publisher.ConnectMQ(data);
        //}
        //public async Task<List<FileModel>> giveMeSomeDummyFiles()
        //{
        //    List<FileModel> files = new List<FileModel>()
        //    {
        //       new FileModel
        //       {
        //           FileLocation = "MyData Location",
        //           FileName = "FileName From Location",
        //           TargetFileExt = ".txt"
        //       },
        //       new FileModel
        //       {
        //           FileLocation = "MyData2 Location",
        //           FileName = "FileName2 From Location",
        //           TargetFileExt = ".txt"
        //       },
        //       new FileModel
        //       {
        //           FileLocation = "MyData3 Location",
        //           FileName = "FileName3 From Location",
        //           TargetFileExt = ".pdf"
        //       },
        //       new FileModel
        //       {
        //           FileLocation = "MyData4 Location",
        //           FileName = "FileName4 From Location",
        //           TargetFileExt = ".csv"
        //       },
        //       new FileModel
        //       {
        //           FileLocation = "MyData5 Location",
        //           FileName = "FileName5 From Location",
        //           TargetFileExt = ".txt"
        //       },
        //       new FileModel
        //       {
        //           FileLocation = "MyData6 Location",
        //           FileName = "FileName6 From Location",
        //           TargetFileExt = ".txt"
        //       },
        //       new FileModel
        //       {
        //           FileLocation = "MyData7 Location",
        //           FileName = "FileName7 From Location",
        //           TargetFileExt = ".pdf"
        //       }
        //    };
        //    return files;
        //}
    
    }
}
