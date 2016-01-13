namespace MSSQLServerAuditor.Utils
{
    using System;

    public static class MayBe
    {
        public static TResult With<TInput, TResult>(
            this TInput o,
            Func<TInput, TResult> evaluator)
        {
            if (o == null)
            {
                return default(TResult);
            }
            return evaluator(o);
        }

        public static TResult With<TInput, TResult>(
            this TInput o,
            Func<TInput, TResult> evaluator,
            TResult defaultResul)
        {
            if (o == null)
            {
                return defaultResul;
            }
            return evaluator(o);
        }
    }
}