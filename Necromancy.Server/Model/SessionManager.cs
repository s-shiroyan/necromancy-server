using System.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;
using Necromancy.Server.Common;

namespace Necromancy.Server.Model
{
    public class SessionManager
    {
        private readonly Dictionary<string, Session> _sessions;
        private object _lock;

        public SessionManager()
        {
            _sessions = new Dictionary<string, Session>();
            _lock = new object();
        }

        public string NewSessionKey()
        {
            string sessionKey = Util.GenerateSessionKey(16);
            return sessionKey;
        }

        public Session GetSession(string sessionKey)
        {
            lock (_lock)
            {
                if (_sessions.ContainsKey(sessionKey))
                {
                    return _sessions[sessionKey];
                }
            }

            return null;
        }

        public List<Session> GetSessions()
        {
            List<Session> sessions;
            lock (_lock)
            {
                sessions = new List<Session>(_sessions.Values);
            }

            return sessions;
        }

        public Session FetchSession(string sessionKey)
        {
            lock (_lock)
            {
                if (_sessions.ContainsKey(sessionKey))
                {
                    Session session = _sessions[sessionKey];
                    _sessions.Remove(sessionKey);
                    return session;
                }
            }

            return null;
        }

        public Session GetSession(int accountId)
        {
            lock (_lock)
            {
                foreach (Session session in _sessions.Values)
                {
                    if (session.Account.Id == accountId)
                    {
                        return session;
                    }
                }
            }

            return null;
        }

        public void StoreSession(Session session)
        {
            lock (_lock)
            {
                if (_sessions.ContainsKey(session.Key))
                {
                    _sessions[session.Key] = session;
                }
                else
                {
                    _sessions.Add(session.Key, session);
                }
            }
        }

        public Session DeleteSession(string sessionKey)
        {
            Session session = null;
            lock (_lock)
            {
                if (_sessions.ContainsKey(sessionKey))
                {
                    session = _sessions[sessionKey];
                    _sessions.Remove(sessionKey);
                }
            }

            return session;
        }
    }
}