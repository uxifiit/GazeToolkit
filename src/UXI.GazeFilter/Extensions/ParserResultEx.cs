using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace FixationFilter.Extensions
{
    [Obsolete]
    static class ParserResultEx
    {
        public static ParserResult<object> WithParsedExactly<T>(this ParserResult<object> result, Action<T> action)
        {
            return WithParsedExactly(result, typeof(T), o => action.Invoke((T)o));
        }

        public static ParserResult<object> WithParsedExactly(this ParserResult<object> result, Type optionsType, Action<object> action)
        {
            return result.WithParsed<object>(o =>
            {
                var resultType = o.GetType();

                if (resultType.Equals(optionsType))
                {
                    action.Invoke(o);
                }
            });
        }
    }

    [Obsolete]
    static class ParserEx
    {
        private static Delegate WrapToTypedAction(Action<object> action, Type actionArgType)
        {
            ParameterExpression parameter = Expression.Parameter(actionArgType, "result");
            Expression converted = Expression.Convert(parameter, typeof(object));
            Expression instance = Expression.Constant(action.Target);
            Expression call = Expression.Call(instance, action.Method, converted);

            return Expression.Lambda(typeof(Action<>).MakeGenericType(actionArgType), call, parameter).Compile();
        }

        internal static void ParseArguments(this Parser parser, IEnumerable<string> args, Type optionsType, Action<object> withParsed, Action<IEnumerable<Error>> withNotParsed)
        {
            var withParsedOptions = WrapToTypedAction(withParsed, optionsType);

            parser.CallGenericMethod<object>(nameof(Parser.ParseArguments), optionsType, args)
                  .CallGenericExtensionMethod<object>(typeof(ParserResultExtensions), nameof(ParserResultExtensions.WithParsed), optionsType, withParsedOptions)
                  .CallGenericExtensionMethod<object>(typeof(ParserResultExtensions), nameof(ParserResultExtensions.WithNotParsed), optionsType, withNotParsed);
        }


        private static object InvokeGenericMethod(object source, Type sourceType, string methodName, Type[] typeArgs, object[] parameters)
        {
            MethodInfo method = sourceType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                                          .Where(m => m.Name == methodName)
                                          .Where(m => m.GetGenericArguments().Count() == typeArgs.Length)
                                          .FirstOrDefault();

            if (method == null)
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            MethodInfo generic = method.MakeGenericMethod(typeArgs);

            return generic.Invoke(source, parameters);
        }


        //public static object InvokeGenericMethod(Type sourceType, string methodName, Type[] typeArgs, object[] parameters)
        //{
        //    return InvokeGenericMethod(null, sourceType, methodName, typeArgs, parameters); 
        //}

        //private static object InvokeGenericMethod(object source, string methodName, Type[] typeArgs, object[] parameters)
        //{
        //    return InvokeGenericMethod(source, source.GetType(), methodName, typeArgs, parameters);
        //}


        public static void CallGenericMethod(this object source, string methodName, Type[] typeArgs, params object[] parameters)
        {
            InvokeGenericMethod(source, source.GetType(), methodName, typeArgs, parameters);
        }


        public static void CallGenericExtensionMethod(this object target, Type methodSourceType, string methodName, Type[] typeArgs, params object[] parameters)
        {
            InvokeGenericMethod(null, methodSourceType, methodName, typeArgs, parameters?.Prepend(target).ToArray() ?? new object[] { target });
        }


        public static void CallGenericMethod(this object source, string methodName, Type typeArg, params object[] parameters)
        {
            CallGenericMethod(source, methodName, new Type[] { typeArg }, parameters);
        }


        public static void CallGenericExtensionMethod(this object target, Type methodSourceType, string methodName, Type typeArg, params object[] parameters)
        {
            CallGenericExtensionMethod(target, methodSourceType, methodName, new Type[] { typeArg }, parameters);
        }


        public static TResult CallGenericMethod<TResult>(this object source, string methodName, Type[] typeArgs, params object[] parameters)
        {
            return (TResult)InvokeGenericMethod(source, source.GetType(), methodName, typeArgs, parameters);
        }


        public static TResult CallGenericExtensionMethod<TResult>(this object target, Type methodSourceType, string methodName, Type[] typeArgs, params object[] parameters)
        {
            return (TResult)InvokeGenericMethod(null, methodSourceType, methodName, typeArgs, parameters?.Prepend(target).ToArray() ?? new object[] { target });
        }


        public static TResult CallGenericExtensionMethod<TResult>(this object target, Type methodSourceType, string methodName, Type typeArg, params object[] parameters)
        {
            return CallGenericExtensionMethod<TResult>(target, methodSourceType, methodName, new Type[] { typeArg }, parameters);
        }


        public static TResult CallGenericMethod<TResult>(this object source, string methodName, Type typeArg, params object[] parameters)
        {
            return CallGenericMethod<TResult>(source, methodName, new Type[] { typeArg }, parameters);
        }


        private static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T item)
        {
            yield return item;

            foreach (var element in source)
            {
                yield return element;
            }
        }
    }
}
