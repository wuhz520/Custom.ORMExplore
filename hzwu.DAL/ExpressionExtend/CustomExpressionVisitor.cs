using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using hzwu.Framework.SqlMapping;

namespace hzwu.DAL.ExpressionExtend
{
    public class CustomExpressionVisitor : ExpressionVisitor
    {
        private Stack<string> ConditionStack = new Stack<string>();
        //id>10    id>@Id    sqlparameter(@id,10)
        private List<SqlParameter> _SqlParameterList = new List<SqlParameter>();
        private object _TempValue = null;


        public string GetWhere(out List<SqlParameter> sqlParameters)
        {
            string where = string.Concat(this.ConditionStack.ToArray());
            this.ConditionStack.Clear();
            sqlParameters = _SqlParameterList;
            return where;
        }


        public override Expression Visit(Expression node)
        {
            Console.WriteLine($"Visit入口：{node.NodeType} {node.Type} {node.ToString()}");
            return base.Visit(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Console.WriteLine($"VisitBinary：{node.NodeType} {node.Type} {node.ToString()}");
            this.ConditionStack.Push(" ) ");
            base.Visit(node.Right);
            this.ConditionStack.Push(node.NodeType.ToSqlOperator());
            base.Visit(node.Left);
            this.ConditionStack.Push(" ( ");
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            Console.WriteLine($"VisitConstant：{node.NodeType} {node.Type} {node.ToString()}");
            //this.ConditionStack.Push($"'{node.Value.ToString()}'");
            this._TempValue = node.Value;
            //栈里面不要值，要的是@PropertyName,但是从后往前，先有值再有属性--但是二者是连续的
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            Console.WriteLine($"VisitMember：{node.NodeType} {node.Type} {node.ToString()}");
            //this.ConditionStack.Push($"{node.Member.GetMappingName()}");
            if (node.Expression is ConstantExpression)
            {
                var value1 = this.InvokeValue(node);
                var value2 = this.ReflectionValue(node);
                //this.ConditionStack.Push($"'{value1}'");
                this._TempValue = value1;
            }
            else
            {
                //this.ConditionStack.Push($"{node.Member.Name}");
                //this.ConditionStack.Push($"{node.Member.GetMappingName()}");//映射数据
                if (this._TempValue != null)
                {
                    string name = node.Member.GetMappingName();
                    string paraName = $"@{name}{this._SqlParameterList.Count}";
                    string sOperator = this.ConditionStack.Pop();
                    this.ConditionStack.Push(paraName);
                    this.ConditionStack.Push(sOperator);
                    this.ConditionStack.Push(name);

                    var tempValue = this._TempValue;
                    this._SqlParameterList.Add(new SqlParameter(paraName, tempValue));
                    this._TempValue = null;
                }
            }
            return node;
        }

        private object InvokeValue(MemberExpression member)
        {
            var objExp = Expression.Convert(member, typeof(object));//struct需要
            return Expression.Lambda<Func<object>>(objExp).Compile().Invoke();
        }

        private object ReflectionValue(MemberExpression member)
        {
            var obj = (member.Expression as ConstantExpression).Value;
            return (member.Member as FieldInfo).GetValue(obj);
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m == null) throw new ArgumentNullException("MethodCallExpression");

            this.Visit(m.Arguments[0]);
            string format;
            switch (m.Method.Name)
            {
                case "StartsWith":
                    format = "({0} LIKE {1}+'%')";
                    break;

                case "Contains":
                    format = "({0} LIKE '%'+{1}+'%')";
                    break;

                case "EndsWith":
                    format = "({0} LIKE '%'+{1})";
                    break;

                default:
                    throw new NotSupportedException(m.NodeType + " is not supported!");
            }
            this.ConditionStack.Push(format);
            this.Visit(m.Object);
            string left = this.ConditionStack.Pop();
            format = this.ConditionStack.Pop();
            string right = this.ConditionStack.Pop();
            this.ConditionStack.Push(String.Format(format, left, right));

            return m;
        }
    }
}
