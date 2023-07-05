using AutoMapper;
using System.Collections.Generic;

namespace AdmissionPortal_API.Utility.AutoMapper
{
    public class MapperProfile<T, C> where T : class where C : class
    {
        public C Mapper(T sourceObject)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<T, C>();
            });

            IMapper mapper = config.CreateMapper();
            T sourceViewModel = sourceObject;
            C resultEntity = mapper.Map<T, C>(sourceViewModel);

            return resultEntity;
        }
        public C Mapper(T sourceObject, C destObject)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<T, C>();
            });

            IMapper mapper = config.CreateMapper();
            T sourceViewModel = sourceObject;
            C destViewModel = destObject;
            C resultEntity = mapper.Map<T, C>(sourceViewModel, destViewModel);

            return resultEntity;
        }
        public List<C> MapToList(List<T> sourceObject)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T, C>();
            });

            IMapper mapper = config.CreateMapper();
            List<T> sourceViewModel = sourceObject;
            List<C> resultEntity = mapper.Map<List<T>, List<C>>(sourceViewModel);

            return resultEntity;
        }
    }
}

