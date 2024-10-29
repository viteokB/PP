using MerosWebApi.Core.Models;
using MerosWebApi.Persistence.Entites;
using System.Linq.Expressions;
using System.Reflection;

namespace MerosWebApi.Persistence.Helpers
{
    /// <summary>
    /// Присваивает значения свойств с одинаковым названием 
    /// из одного объекта другому.
    /// </summary>
    /// <typeparam name="TObjTo">Объект, которому надо присвоить значения свойств.</typeparam>
    /// <typeparam name="TObjFrom">Объект, с которого надо считать свойства.</typeparam>
    public class PropertyAssigner<TObjTo, TObjFrom>
    {
        private static readonly Dictionary<string, PropertyInfo> _sourceProperties = new();

        private static readonly Dictionary<string, PropertyInfo> _targetProperties = new();

        private static readonly Action<TObjFrom, TObjTo> _propertyAssigner;

        private static readonly Func<TObjFrom, TObjTo> _someMapper;


        static PropertyAssigner()
        {
            InitializePropertyDictionaries();

            _propertyAssigner = GeneratePropertyAssigner();
            _someMapper = GenerateMapperFunction();
        }

        public static void AssignPropertyValues(TObjTo objTo, TObjFrom objFrom)
        {
            _propertyAssigner(objFrom, objTo);
        }
        public static TObjTo Map(TObjFrom objFrom)
        {
            return _someMapper(objFrom);
        }

        private static void InitializePropertyDictionaries()
        {
            PopulatePropertyDictionary(typeof(TObjFrom), _sourceProperties);
            PopulatePropertyDictionary(typeof(TObjTo), _targetProperties);
        }

        private static void PopulatePropertyDictionary(Type type, 
            Dictionary<string, PropertyInfo> propertyDictionary)
        {
            foreach (var prop in type.GetProperties())
            {
                propertyDictionary[prop.Name] = prop;
            }
        }

        private static Action<TObjFrom, TObjTo> GeneratePropertyAssigner()
        {
            var sourceParam = Expression.Parameter(typeof(TObjFrom), "source");
            var targetParam = Expression.Parameter(typeof(TObjTo), "target");

            var assignments = _targetProperties
                .Select(kvp =>
                {
                    if (_sourceProperties.TryGetValue(kvp.Key, out var sourceProp))
                    {
                        var sourceAccess = Expression.Property(sourceParam, sourceProp);
                        var targetAccess = Expression.Property(targetParam, kvp.Value);
                        return Expression.Assign(targetAccess, sourceAccess);
                    }
                    return null;
                })
                .Where(assignment => assignment != null);

            var body = Expression.Block(assignments);
            return Expression.Lambda<Action<TObjFrom, TObjTo>>(body, sourceParam, targetParam).Compile();
        }

        public static Func<TObjFrom, TObjTo> GenerateMapperFunction()
        {
            var sourceParam = Expression.Parameter(typeof(TObjFrom), "source");
            var targetParam = Expression.New(typeof(TObjTo));

            var bindings = _targetProperties
                .Select(keyValuePair =>
                {
                    var key = keyValuePair.Key;
                    var targetPropInfo = keyValuePair.Value;

                    if (_sourceProperties.TryGetValue(key, out var sourcePropInfo))
                    {
                        var sourcePropertyAccess = Expression.Property(sourceParam, sourcePropInfo);
                        return Expression.Bind(targetPropInfo, sourcePropertyAccess);
                    }

                    return null;
                })
                .Where(assignment => assignment != null);

            var body = Expression.MemberInit(targetParam, bindings);

            var lambda = Expression.Lambda<Func<TObjFrom, TObjTo>>(body, sourceParam);

            return lambda.Compile();
        }
    }

    public static class PropertyAssigner
    {
        public static void AssignPropertyValues<TObjTo, TObjFrom>(TObjTo objTo, TObjFrom objFrom)
        {
            PropertyAssigner<TObjTo, TObjFrom>.AssignPropertyValues(objTo, objFrom);
        }

        public static TObjTo Map <TObjTo, TObjFrom>(TObjFrom objFrom)
        {
            return PropertyAssigner<TObjTo, TObjFrom>.Map(objFrom);
        }
    }
}
