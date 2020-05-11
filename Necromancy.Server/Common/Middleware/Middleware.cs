namespace Necromancy.Server.Common.Middleware
{
    public abstract class Middleware<T, TReq, TRes> : IMiddleware<T, TReq, TRes>
    {
        protected Middleware()
        {
        }

        public abstract void Handle(T client, TReq message, TRes response, MiddlewareDelegate<T, TReq, TRes> next);
    }
}
