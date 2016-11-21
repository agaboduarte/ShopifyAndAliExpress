using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Rules
{
    public class RulesException : Exception
    {
        public string PropertyName { get; set; }

        public RulesException(string propertyName, string message)
            : base(message)
        {
            PropertyName = propertyName;
        }

        public static void ThrowAggregateException(string propertyName, string propertyMessage)
        {
            throw new AggregateException(
                new RulesException(propertyName, propertyMessage)
            );
        }

        public static void ThrowAggregateException(string message, string propertyName, string propertyMessage)
        {
            throw new AggregateException(
                message,
                new RulesException(propertyName, propertyMessage)
            );
        }

        public static void ThrowAggregateException(IEnumerable<KeyValuePair<string, string>> messages)
        {
            throw new AggregateException(
                messages.Select(item => new RulesException(item.Key, item.Value))
            );
        }

        public static void ThrowAggregateException(string message, IEnumerable<KeyValuePair<string, string>> messages)
        {
            throw new AggregateException(
                message,
                messages.Select(item => new RulesException(item.Key, item.Value))
            );
        }

        public static void ThrowAggregateException(IEnumerable<RulesException> exceptions)
        {
            throw new AggregateException(
                exceptions
            );
        }

        public static void ThrowAggregateException(string message, IEnumerable<RulesException> exceptions)
        {
            throw new AggregateException(
                message,
                exceptions
            );
        }
    }

    public static class AggregateExceptionExtensions
    {
        public static void ThrowIfNotAnyRules(this AggregateException e)
        {
            if (!e.AnyRules())
            {
                throw e;
            }
        }

        public static bool AnyRules(this AggregateException e)
        {
            return e.InnerExceptions.OfType<RulesException>().Any();
        }

        public static bool AnyRules(this AggregateException e, string propertyName)
        {
            return e.InnerExceptions.OfType<RulesException>().Any(item => item.PropertyName == propertyName);
        }

        public static IEnumerable<RulesException> AllRules(this AggregateException e)
        {
            return e.InnerExceptions.OfType<RulesException>();
        }
    }
}
