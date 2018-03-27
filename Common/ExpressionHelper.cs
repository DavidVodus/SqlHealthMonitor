using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common
{
    /// <summary>
    /// Create Expression from various inputs
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// Compose Expression from list of constants ,where relationship is defined by predicate, and join them with OR
        /// Sql provider interpret it as "SELECT FROM something WHERE 
        /// ((1 =[SwitchId]) AND (N'test' = [SwitchIp]) AND (N'o' = [HostName])) OR ((2 = [SwitchId])"
        /// </summary>
        /// <typeparam name="TSource">Type of entities,stored in storage,they are used for querying</typeparam>
        /// <typeparam name="TConstant">Properties must be equal in name to TSource,can be different type then TSource</typeparam>
        /// <param name="predicate"> (x, y) => (x.SwitchId == y.SwitchId && x.SwitchIp == y.SwitchIp);</param>
        /// <param name="constants">List() { new TConstant { SwitchId = 1, SwitchIp = "test", HostName = "o" }</param>
        /// <returns></returns>
        public static Expression<Func<TSource, bool>> WherePredicateFromArray<TSource, TConstant>
          (Expression<Func<TSource, TConstant, bool>> predicate,
          List<TConstant> constants)
        {
            List<Expression> sourcesWithConstantBody = new List<Expression>();
            foreach (var constant in constants)
            {
                Expression predicateBinary = ReplaceRightWithConstant(predicate.Body, constant);
                sourcesWithConstantBody.Add(predicateBinary);

            }
            //join all sets of Expression with OR
            Expression predicateBody = JoinExpressions(sourcesWithConstantBody);
            //make Expression base only on first parrameter which belongs to TSource
            return Expression.Lambda<Func<TSource, bool>>(predicateBody, new ParameterExpression[]
            { predicate.Parameters.First() });

        }

        private static Expression JoinExpressions(List<Expression> rightExpressions, Expression leftExpression = null)
        {
            if (leftExpression == null)
            {
                leftExpression = rightExpressions.First();
                rightExpressions.Remove(rightExpressions.First());
                if (rightExpressions.Count <= 0)
                    return leftExpression;
            }
            Expression left = leftExpression;
            Expression right = rightExpressions.First();
            Expression body = Expression.Or(left, right);
            rightExpressions.Remove(rightExpressions.First());
            //if this is the last expression,return final expression
            if (rightExpressions.Count <= 0)
                return body;
            //else join next source constant Expression in list with this one
            body = JoinExpressions(rightExpressions, body);
            return body;
        }

        /// <summary>
        /// Replace right side of Expression tree with constant from list TConst equal in name to property TSource  
        /// </summary>
        /// <typeparam name="TConstant"></typeparam>
        /// <param name="input"></param>
        /// <param name="constant"></param>
        /// <returns></returns>
        private static Expression ReplaceRightWithConstant<TConstant>(Expression input, TConstant constant)
        {
            // c=constant,s=source
            //left(s.a == c.a && s.b == c.b)right(s.c ==c.c)                            baseBinary
            //                                   /                                         /\
            //                                  /                               baseBinary     baseBinary -> (s.c ==c.c) 
            //                                 /(s.a == c.a && s.b == c.b) -->       /\             /\
            //                                /                                                    /  \
            //  left(x.SwitchId  )right(y.SwitchId)                                left side with      right side of TConstant 
            //                                                                     TSource property    property name,it is replaced
            //                                                                     name                 by value from TConst 

            Expression result = null;
            var baseBinary = input as BinaryExpression;
            if (baseBinary != null)
            {
                BinaryExpression inputBinary = baseBinary;
                //if expression is final type(end of the expression tree) with constant 
                //,replace right side of constant property name with constant value equal to property name of TSource
                if (inputBinary.Right.NodeType == ExpressionType.MemberAccess)
                {
                    var constantPropertyName = ((MemberExpression)inputBinary.Right).Member.Name;
                    var constantValue = constant.GetType().GetProperty(constantPropertyName).GetValue(constant);
                    Expression left = inputBinary.Left;
                    Expression right = Expression.Constant(constantValue);
                    result = Expression.MakeBinary(inputBinary.NodeType, left, right, inputBinary.IsLiftedToNull, inputBinary.Method);

                }
                //not final,traverse in expression tree
                else
                {
                    Expression constantValueExpression = null;
                    //BinaryExpression expr = rootBinary;
                    //traverse in right way
                    constantValueExpression = ReplaceRightWithConstant(baseBinary.Right, constant);
                    //join left side with right side which contain constant value
                    inputBinary = Expression.MakeBinary(inputBinary.NodeType, inputBinary.Left, constantValueExpression, inputBinary.IsLiftedToNull, inputBinary.Method);
                    //traverse in left way
                    constantValueExpression = ReplaceRightWithConstant(baseBinary.Left, constant);
                    result = Expression.MakeBinary(inputBinary.NodeType, constantValueExpression, inputBinary.Right, inputBinary.IsLiftedToNull, inputBinary.Method);
                }
            }
            return result;

        }
    }
}
