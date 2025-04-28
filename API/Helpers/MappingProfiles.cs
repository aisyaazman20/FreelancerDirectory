using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //input
            CreateMap<CreateFreelancerDto, Freelancer>(); 
            CreateMap<SkillDto, Skill>();
            CreateMap<HobbyDto, Hobby>();

            //output
            CreateMap<Freelancer, FreelancerResponseDto>() //for output, freelancer to dto
               .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills.Select(s => s.Name)))
               .ForMember(dest => dest.Hobbies, opt => opt.MapFrom(src => src.Hobbies.Select(h => h.Name)));
        }
    }
}
