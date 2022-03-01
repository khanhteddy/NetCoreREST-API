using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Dto;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controller
{
    //api commands
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
           _repository = repository;
           _mapper = mapper;
        }
        //Get api/command
        [HttpGet]
        public ActionResult <IEnumerable<Command>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();
            return Ok(_mapper.Map<IEnumerable<Command>>(commandItems));
        }
        //Get api/command/{id}
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult <CommandReadDto> GetCommandById(int id)
        {
            var commandItems = _repository.GetCommandById(id);
            if(commandItems != null)
            {
                return Ok(_mapper.Map<CommandReadDto>(commandItems));
            }
            return NotFound();
            
        }
        [HttpPost]
        public ActionResult <CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();

            var commandReadDto =  _mapper.Map<CommandReadDto>(commandModel);
            //return Ok(commandReadDto);
            return CreatedAtRoute(nameof(GetCommandById), new {Id = commandReadDto.Id}, commandReadDto);
        }
   }
}