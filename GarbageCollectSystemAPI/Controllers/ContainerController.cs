using AutoMapper;
using DataAccess.UnityOfWork;
using Entity.Concrete;
using GarbageCollectSystemAPI.ViewModels;
using GarbageCollectSystemAPI.ViewModels.Operations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GarbageCollectSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerController : ControllerBase
    {
        private readonly IUnityOfWork _unityOfWork;
        private readonly IMapper _mapper;

        public ContainerController(IUnityOfWork unityOfWork, IMapper mapper)
        {
            _unityOfWork = unityOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all containers.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            //Get containers
            var containers = _unityOfWork.ContainerRepository.GetAll();

            //Do mapping then return view model
            List<GetAllContainerModel> containerVm = new List<GetAllContainerModel>();

            if (containers.Count() > 0)
            {
                containerVm = _mapper.Map<List<GetAllContainerModel>>(containers);
            }
            return Ok(containerVm);
        }

        /// <summary>
        /// Get containers of vehicle
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetVehicleContainers")]
        public IActionResult GetAll([FromQuery] int vehicleID)
        {
            //Get containers of vehicle
            var containers = _unityOfWork.ContainerRepository.GetAll(x => x.VehicleID == vehicleID);

            //Do mapping then return view model
            List<ContainerModel> containerVm = new List<ContainerModel>();

            if (containers.Count() > 0)
            {
                containerVm = _mapper.Map<List<ContainerModel>>(containers);
            }

            return Ok(containerVm);
        }

        /// <summary>
        /// Add new container.
        /// </summary>
        /// <param name="model">The model type is  Container  model.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] ContainerModel model)
        {
            //Control the new model
            if (model is null)
                return BadRequest("Please , input the container information.You can not send empty container model.");

            //Is container exist?
            var container = _unityOfWork.ContainerRepository.Get(x => x.Latitude == model.Latitude && x.Longitude == model.Longitude);
            if (container is not null)
                return BadRequest("Container already added.Please , add another container.");

            //Do mapping operation then add new container
            container = _mapper.Map<Container>(model);
            _unityOfWork.ContainerRepository.Add(container, true);

            //Do mapping then return view model
            container = _unityOfWork.ContainerRepository.Get(x => x.Latitude == model.Latitude && x.Longitude == model.Longitude);
            ContainerModel containerVm = _mapper.Map<ContainerModel>(container);

            return Ok(containerVm);
        }

        /// <summary>
        /// Get model with FromBody.
        /// </summary>
        /// <param name="model">The model type is  Container model.</param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put([FromBody] UpdateContainerModel model)
        {
            //Control the new model
            if (model is null)
                return BadRequest("Please , input the container information.You can not send empty container model.");

            //Do mapping operation then update container
            Container container = _mapper.Map<Container>(model);
            _unityOfWork.ContainerRepository.Update(container, true);

            //Do mapping then return view model
            UpdateContainerModel containerVm = _mapper.Map<UpdateContainerModel>(container);

            return Ok(containerVm);
        }

        /// <summary>
        /// Delete container of id. Get the id value with FromRoute
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //Control the id 
            if (id == 0)
                return BadRequest("Please,send valid container id.");

            //Is container exist?
            var container = _unityOfWork.ContainerRepository.GetByID(id);
            if (container is null)
                return BadRequest("Container is not exist.");

            //Remove container
            _unityOfWork.ContainerRepository.Delete(id, true);

            //Get container List
            var containers = _unityOfWork.VehicleRepository.GetAll();

            //Do mapping then return view model
            List<ContainerModel> containerVm = new List<ContainerModel>();
            if (containers.Count() > 0)
            {
                containerVm = _mapper.Map<List<ContainerModel>>(containers);
            }

            return Ok(containerVm);
        }
    }
}
