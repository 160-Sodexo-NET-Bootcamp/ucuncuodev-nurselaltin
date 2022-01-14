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
    public class VehicleController : ControllerBase
    {
        private readonly IUnityOfWork _unityOfWork;
        private readonly IMapper _mapper;
        public VehicleController(IUnityOfWork unityOfWork, IMapper mapper = null)
        {
            _unityOfWork = unityOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Get vehicle by id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetByID")]
        public IActionResult GetByID([FromQuery] int id)
        {
            //Control the id 
            if (id == 0)
                return BadRequest("Please,send valid vehicle id.");

            //Get vehicle
            var vehicle = _unityOfWork.VehicleRepository.GetByID(id);

            //Do mapping then return view model
            VehicleModel vehicleVm = new VehicleModel();
            if (vehicle is not null)
            {
                vehicleVm = _mapper.Map<VehicleModel>(vehicle);
            }

            return Ok(vehicleVm);
        }

        /// <summary>
        /// Get all vehicles.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            //Get vehicles
            var vehicles = _unityOfWork.VehicleRepository.GetAll();

            //Do mapping then return view model
            List<GetAllVehicleModel> vehicleVm = new List<GetAllVehicleModel>();

            if (vehicles.Count() > 0)
            {
                vehicleVm = _mapper.Map<List<GetAllVehicleModel>>(vehicles);
            }

            return Ok(vehicleVm);
        }

        /// <summary>
        /// Add new vehicle.
        /// </summary>
        /// <param name="model">The model type is  Vehicle  model.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] VehicleModel model)
        {
            //Control the new model
            if (model is null)
                return BadRequest("Please , input the vehicle information.You can not send empty vehicle model.");

            //Is Vehicle exist?
            var vehicle = _unityOfWork.VehicleRepository.Get(x => x.VehicleName.ToLower() == model.VehicleName.ToLower());
            if (vehicle is not null)
                return BadRequest("Vehicle already added.Please , add another vehicle.");
            
            //Do mapping operation then add new vehicle
            vehicle = _mapper.Map<Vehicle>(model);
            _unityOfWork.VehicleRepository.Add(vehicle, true);

            //Do mapping then return view model
            vehicle = _unityOfWork.VehicleRepository.Get(x => x.VehicleName.ToLower() == vehicle.VehicleName.ToLower());
            VehicleModel vehicleVm = _mapper.Map<VehicleModel>(vehicle);

            return Ok(vehicleVm);
        }

        /// <summary>
        /// Get  model with FromBody.
        /// </summary>
        /// <param name="model">The model type is  Vehicle model.</param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put([FromBody] VehicleModel model)
        {
            //Control the new model
            if (model is null)
                return BadRequest("Please , input the vehicle information.You can not send empty vehicle model.");

            //Do mapping operation then update vehicle
            Vehicle vehicle = _mapper.Map<Vehicle>(model);
            _unityOfWork.VehicleRepository.Update(vehicle, true);
          
            //Do mapping then return view model
            UpdateVehicleModel vehicleVm = _mapper.Map<UpdateVehicleModel>(vehicle);

            return Ok(vehicleVm);
        }

        /// <summary>
        /// Delete vehicle of id. Get the id value with FromRoute
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //Control the id 
            if (id == 0)
                return BadRequest("Please,send valid vehicle id.");

            //Is vehicle exist?
            var vehicle = _unityOfWork.VehicleRepository.GetByID(id); 
            if (vehicle is null)
                return BadRequest("Vehicle is not exist.");
        
            //Remove vehicle
            _unityOfWork.VehicleRepository.Delete(vehicle.VehicleID, true);

            //Get updated vehicles List
            var vehicles = _unityOfWork.VehicleRepository.GetAll();

            //Do mapping then return view model
            List<VehicleModel> vehicleVm = new List<VehicleModel>();
            if (vehicles.Count() > 0)
            {
                vehicleVm = _mapper.Map<List<VehicleModel>>(vehicles);
            }

            return Ok(vehicleVm);
        }
    }
}
