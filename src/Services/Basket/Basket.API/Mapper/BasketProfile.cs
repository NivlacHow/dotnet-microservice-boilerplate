using AutoMapper;
using EventBus.Messages.Events;
using FileProcessor.Entities;

namespace FileProcessor.Mapper
{
    public class FileProfile : Profile
	{
		public FileProfile()
		{
			CreateMap<FileModel, FileEvent>().ReverseMap();
		}
	}
}
