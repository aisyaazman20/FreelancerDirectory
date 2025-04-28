using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public interface ISpecification<T> //generic spec, T = entity 
    {
        Expression<Func<T, bool>> Criteria { get; } //find T where ...
        List<Expression<Func<T, object>>> Includes { get; } //include related entities

        Expression<Func<T, object>> OrderByAscend { get; } //sort in acsending order

        Expression<Func<T, object>> OrderByDescend { get; } //sort in descending order

        //pagination
        int Take { get; } //page size
        int Skip { get; } //how many to skip
        bool IsPagingEnabled { get; }
    }
}
