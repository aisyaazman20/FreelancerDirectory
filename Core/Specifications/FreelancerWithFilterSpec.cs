using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class FreelancerWithFilterSpec: BaseSpecification<Freelancer>
    {
        public FreelancerWithFilterSpec(FreelancerSpecParams specParams, PaginationParams pageParams)
            : base(f =>  //calls the constructor in base spec (criteria)
                (string.IsNullOrEmpty(specParams.Search) || 
                f.Username.ToLower().Contains(specParams.Search.ToLower()) || 
                f.Email.ToLower().Contains(specParams.Search.ToLower()) ||
                f.PhoneNumber.ToLower().Contains(specParams.Search.ToLower())) &&
                (string.IsNullOrEmpty(specParams.Skill) || f.Skills.Any(s => s.Name.ToLower().Contains(specParams.Skill.ToLower()))) &&
                (string.IsNullOrEmpty(specParams.Hobby) || f.Hobbies.Any(h => h.Name.ToLower().Contains(specParams.Hobby.ToLower()))) &&
                !f.IsArchived )
        {
            AddInclude(f => f.Skills);   //call method in base spec
            AddInclude(x => x.Hobbies);  //call method in base spec

            ApplyPaging((pageParams.PageIndex - 1) * pageParams.PageSize, pageParams.PageSize);

        }
    }
}
