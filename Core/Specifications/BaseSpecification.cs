using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public abstract class BaseSpecification<T>:ISpecification<T>
    {
        //constructor
        public BaseSpecification()
        {
        }
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; protected set; }  //where ...
        public List<Expression<Func<T, object>>> Includes { get; } = new();  // include multiple related entities


        public Expression<Func<T, object>> OrderByAscend { get; private set; }
        public Expression<Func<T, object>> OrderByDescend { get; private set; }

        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; }

        //HELPER METHODS - definition
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        protected void AddOrderByAscend(Expression<Func<T, object>> orderByAscExpression)
        {
            OrderByAscend = orderByAscExpression;
        }

        protected void AddOrderByDescend(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescend = orderByDescExpression;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

    }
}
