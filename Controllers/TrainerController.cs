﻿using Demo_01.DAL.Entities;
using Demo_01.DAL.Repositories;
using Demo_01.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using System.Net;

namespace Demo_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerController : ControllerBase
    {
        private TrainerRepository _TrainerRepository;
        public TrainerController(TrainerRepository trainerRepository)
        {
            _TrainerRepository = trainerRepository;
            // 1 (mauvaise) façon d'injecter notre TrainerRepository
            //_TrainerRepository = new TrainerRepository();
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Trainer>))]
        public IActionResult GetTrainers()
        {
            IEnumerable<Trainer> trainers = _TrainerRepository.GetAll();
            return Ok(trainers);

            // Demo avant qu'on mette la liste étou
            //return Ok(new { message = "Hello les .Net de Technobel" });
            //OU
            //return StatusCode(StatusCodes.Status200OK, "Hello les .Net de Technobel");
        }

        [HttpGet("{trainerId:int:min(1)}")]
        [ProducesResponseType(200, Type = typeof(Trainer))]
        [ProducesResponseType(404)]
        public IActionResult GetTrainer(int trainerId)
        {
            Trainer? trainer = _TrainerRepository.GetById(trainerId);
            if(trainer is null)
            {
                return NotFound();
            }
            return Ok(trainer);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Trainer))]
        public IActionResult AddTrainer(TrainerDTO trainer)
        {
            Trainer trainerToAdd = new Trainer
            {
                LastName = trainer.LastName,
                FirstName = trainer.FirstName,
                BirthDate = trainer.BirthDate
            };

            Trainer trainerAdded = _TrainerRepository.Create(trainerToAdd);

            // Renvoie d'une 201
            return CreatedAtAction
                (
                    nameof(GetTrainer), //string : nom de l'action à mettre dans le location
                    new { trainerId = trainerAdded.Id }, //objet : contenant tous les paramètres dont la route a besoin (ici notre GetTrainer a besoin de trainerId)
                    trainerAdded //objet : représente l'objet qui vient d'être crée (qui sera dans le body de la reponse)
                );
        }

        [HttpDelete("{trainerId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTrainer(int trainerId)
        {
            if(_TrainerRepository.Delete(trainerId))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("{trainerId:int}")]
        [ProducesResponseType(501)]
        public IActionResult UpdateTrainer(int trainerId, TrainerDTO trainer)
        {
            return StatusCode(StatusCodes.Status501NotImplemented, new { reason = "Flemme" } );
        }
    }
}
