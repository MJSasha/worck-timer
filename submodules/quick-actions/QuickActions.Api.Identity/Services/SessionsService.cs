using Microsoft.AspNetCore.Http;
using QuickActions.Common.Data;

namespace QuickActions.Api.Identity.Services
{
    public class SessionsService<T>
    {
        protected Dictionary<string, Session<T>> sessions = new();

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string keyName;
        private readonly int sessionLifeTime;
        private readonly Func<Session<T>, string[], bool> rolesChecker;

        public SessionsService(IHttpContextAccessor httpContextAccessor, string keyName, int sessionLifeTime, Func<Session<T>, string[], bool> rolesChecker)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.keyName = keyName;
            this.sessionLifeTime = sessionLifeTime;
            this.rolesChecker = rolesChecker;
        }

        public string CreateSession(Session<T> session)
        {
            var key = Guid.NewGuid().ToString();

            lock (sessions)
            {
                sessions.Add(key, session);
            }
            return key;
        }

        public Session<T> ReadSession(string key)
        {
            DeleteExpiredSessions();
            if (string.IsNullOrWhiteSpace(key)) return default;

            lock (sessions)
            {
                return sessions.GetValueOrDefault(key);
            }
        }

        public Session<T> ReadSession(HttpContext context = null)
        {
            context ??= httpContextAccessor.HttpContext;
            var key = GetKey(context);
            return ReadSession(key);
        }

        public void DeleteSession(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return;

            lock (sessions)
            {
                sessions.Remove(key);
            }
        }

        public void DeleteSession(HttpContext context = null)
        {
            context ??= httpContextAccessor.HttpContext;
            var key = GetKey(context);
            DeleteSession(key);
        }

        public bool CheckAccess(string[] roleNames = null)
        {
            var session = ReadSession();
            if (session == null) return false;
            if (roleNames != null && roleNames.Any()) return rolesChecker?.Invoke(session, roleNames) ?? true;
            else return true;
        }

        private string GetKey(HttpContext context)
        {
            return context.Request.Headers["Authorization"];
        }

        private void DeleteExpiredSessions()
        {
            lock (sessions)
            {
                var sessionsKeysToDelete = sessions.Where(s => s.Value.CreatedAt <= DateTime.UtcNow.AddMinutes(-sessionLifeTime)).Select(s => s.Key).ToList();
                sessionsKeysToDelete.ForEach(str => sessions.Remove(str));
            }
        }
    }
}