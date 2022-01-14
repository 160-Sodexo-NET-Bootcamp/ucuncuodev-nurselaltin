using AutoMapper;
using Entity.Concrete;
using GarbageCollectSystemAPI.ViewModels;
using GarbageCollectSystemAPI.ViewModels.Operations;

namespace GarbageCollectSystemAPI.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Add configuration
            //>> Vehicle
            CreateMap<VehicleModel, Vehicle>();
            CreateMap<Vehicle, VehicleModel>();
            CreateMap<Vehicle, GetAllVehicleModel>();
            CreateMap<Vehicle, UpdateVehicleModel>();
            CreateMap<UpdateVehicleModel, Vehicle>();

            //>> Container
            CreateMap<ContainerModel, Container>();
            CreateMap<Container, ContainerModel>();
            CreateMap<Container, GetAllContainerModel>();
            CreateMap<Container, UpdateContainerModel>();
            CreateMap<UpdateContainerModel, Container>();
        }
    }
}
