using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;

namespace hzwu.DAL.ExpressionExtend
{
    public static class ExpressionHelper
    {
        public static string ToWhere<T>(this Expression<Func<T, bool>> expression, out List<SqlParameter> sqlParameters)
        {
            CustomExpressionVisitor visitor = new CustomExpressionVisitor();
            visitor.Visit(expression);
            string where = visitor.GetWhere(out sqlParameters);
            return where;
        }

        //public static string ToWhereWithParameter<T>(this Expression<Func<T, bool>> expression, out List<SqlParameter> sqlParameters)
        //{
        //    CustomExpressionVisitor visitor = new CustomExpressionVisitor();
        //    visitor.Visit(expression);
        //    string where = visitor.GetWhere(out sqlParameters);
        //    return where;
        //}
    }
}
