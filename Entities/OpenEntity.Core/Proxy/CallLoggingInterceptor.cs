using System;
using System.Diagnostics;
using Castle.Core.Interceptor;

namespace Centro.OpenEntity.Proxy
{
#if DEBUG
    [Serializable]
    public class CallLoggingInterceptor :
        IInterceptor
        , IHasCount
    {
        private int indention;
        public void Intercept(IInvocation invocation)
        {
            try
            {
                indention++;
                Count++;
                Trace.WriteLine(string.Format("{0}Intercepting: {1}", new string('\t', indention), invocation.Method.Name));
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Exception in method {0}{1}{2}", invocation.Method.Name,
                                                Environment.NewLine, ex.Message));
                throw;
            }
            finally
            {
                indention--;
            }
        }

        public int Count { get; private set; }
    }
#endif
}
