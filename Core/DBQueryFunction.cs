using System.Linq.Expressions;

namespace Core;

public class DBQueryFunction
{
    public static Expression<Func<T, bool>> BuildSearchFilter<T>(string searchQuery, string[] fields)
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        Expression? combinedExpression = null;

        foreach (var field in fields)
        {
            var fieldParts = field.Split('.');
            Expression propertyExpression = parameter;
            foreach (var part in fieldParts)
            {
                propertyExpression = Expression.Property(propertyExpression, part);
            }

            var containsMethod = Expression.Call(
                propertyExpression,
                typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                Expression.Constant(searchQuery)
            );

            if (combinedExpression == null)
            {
                combinedExpression = containsMethod;
            }
            else
            {
                combinedExpression =
                    Expression.OrElse(combinedExpression, containsMethod); // OR with existing conditions
            }
        }

        return Expression.Lambda<Func<T, bool>>(combinedExpression!, parameter); // Return the combined expression
    }
}