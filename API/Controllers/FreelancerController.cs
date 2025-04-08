using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using API.Dtos;

namespace API.Controllers
{
    public class FreelancersController : BaseController
    {
        //constructor
        private readonly IFreelancerRepository _repository;
        private readonly IMapper _mapper;

        public FreelancersController(IFreelancerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        //GET ALL FREELANCERS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Freelancer>>> GetAll([FromQuery] string? search, [FromQuery] string? skill, [FromQuery] string? hobby)
        {
            var freelancers = await _repository.GetAllAsync(search, skill, hobby);
            var response = _mapper.Map<IEnumerable<FreelancerResponseDto>>(freelancers);
            return Ok(response);
        }

        //GET FREELANCER BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<FreelancerResponseDto>> GetById(int id)
        {
            var freelancer = await _repository.GetByIdAsync(id);
            if (freelancer == null) return NotFound("User does not exists.");

            var response = _mapper.Map<FreelancerResponseDto>(freelancer);
            return Ok(response);
        }


        //GET FREELANCER BY WILD SEARCH
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Freelancer>>> Search([FromQuery] string query)
        {
            var results = await _repository.SearchAsync(query);
            return Ok(results);
        }


        //CREATE FREELANCER
        [HttpPost]
        public async Task<ActionResult> Create(CreateFreelancerDto dto)
        {
            //check username if exists
            var usernames = await _repository.GetByUsernameAsync(dto.Username);
            if (usernames != null) return BadRequest("Username already exists");

            //check email if exists
            var emails = await _repository.GetByEmailAsync(dto.Email);
            if (emails != null) return BadRequest("Email already exists");

            //create
            var freelancer = _mapper.Map<Freelancer>(dto);
            await _repository.AddAsync(freelancer);

            var response = _mapper.Map<FreelancerResponseDto>(freelancer);
            return CreatedAtAction(nameof(GetById), new { id = freelancer.Id }, response);
        }

        //UPDATE FREELANCER
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CreateFreelancerDto updatedDto)
        {
            // Check if the user exists
            var freelancer = await _repository.GetByIdAsync(id);
            if (freelancer == null) return NotFound("User does not exist.");

            // Check for duplicate username
            var userWithSameUsername = await _repository.GetByUsernameAsync(updatedDto.Username);
            if (userWithSameUsername != null && userWithSameUsername.Id != id)
                return BadRequest("Username already exists");

            // Check for duplicate email
            var userWithSameEmail = await _repository.GetByEmailAsync(updatedDto.Email);
            if (userWithSameEmail != null && userWithSameEmail.Id != id)
                return BadRequest("Email already exists");

            _mapper.Map(updatedDto, freelancer);

            await _repository.UpdateAsync(freelancer);
            return NoContent();
        }

        //DELETE FREELANCER
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound("User does not exists.");

            await _repository.DeleteAsync(id);
            return NoContent();
        }

        //UPDATE ARCHIVE STATUS
        [HttpPatch("{id}/archive")]
        public async Task<ActionResult> Archive(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound("User does not exists.");

            await _repository.ArchiveAsync(id);
            return NoContent();
        }

        //UPDATE ARCHIVE STATUS
        [HttpPatch("{id}/unarchive")]
        public async Task<ActionResult> Unarchive(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound("User does not exists.");

            await _repository.UnarchiveAsync(id);
            return NoContent();
        }
    }
}
