using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using API.Dtos;
using API.Middleware;
using API.Helpers;

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


        //GET ALL FREELANCERS & filter
        //from query accept query string and bind with FreelancerSpecParams object named specParams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FreelancerResponseDto>>> GetAll([FromQuery] FreelancerSpecParams specParams, [FromQuery] PaginationParams pageParams)  
        {
            var spec = new FreelancerWithFilterSpec(specParams, pageParams); //creates FreelancerWithFilterSpec for data query

            var totalItems = await _repository.CountAsync(spec);
            var freelancers = await _repository.GetAllAsync(spec); //call the method in freelancer repo

            var data = _mapper.Map<IReadOnlyList<FreelancerResponseDto>>(freelancers);
            var response = new Pagination<FreelancerResponseDto>(pageParams.PageIndex, pageParams.PageSize, totalItems, data);
            return Ok(response);
        }

        //GET FREELANCER BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<FreelancerResponseDto>> GetById(int id)
        {
            var freelancer = await _repository.GetByIdAsync(id);
            if (freelancer == null)  return BadRequest("User does not exists");

            var response = _mapper.Map<FreelancerResponseDto>(freelancer);
            return Ok(response);
        }


        //GET FREELANCER BY WILD SEARCH
        //[HttpGet("search")]
        //public async Task<ActionResult<IEnumerable<Freelancer>>> Search([FromQuery] string query)
        //{
        //    var results = await _repository.SearchAsync(query);
        //    return Ok(results);
        //}

        //CREATE FREELANCER
        [HttpPost]
        public async Task<ActionResult> Create(CreateFreelancerDto dto)
        {
            //check username if exists
            var usernames = await _repository.GetByUsernameAsync(dto.Username);
            if (usernames != null) return BadRequest("User already exists");

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
            if (freelancer == null) return BadRequest("User does not exist.");

            // Check for duplicate username
            var userWithSameUsername = await _repository.GetByUsernameAsync(updatedDto.Username);
            if (userWithSameUsername != null && userWithSameUsername.Id != id)
                return BadRequest("User already exists");

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
            if (existing == null) return BadRequest("User does not exist.");

            await _repository.DeleteAsync(id);
            return NoContent();
        }

        //UPDATE ARCHIVE STATUS
        [HttpPatch("{id}/archive")]
        public async Task<ActionResult> Archive(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return BadRequest("User does not exist.");

            await _repository.ArchiveAsync(id);
            return NoContent();
        }

        //UPDATE ARCHIVE STATUS
        [HttpPatch("{id}/unarchive")]
        public async Task<ActionResult> Unarchive(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return BadRequest("User does not exist.");

            await _repository.UnarchiveAsync(id);
            return NoContent();
        }

    }
}
