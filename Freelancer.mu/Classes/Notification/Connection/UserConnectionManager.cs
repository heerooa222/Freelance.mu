using System.Collections.Generic;
using System.Linq;

namespace Freelancer.mu.Classes.Notification.Connection
{
    public class UserConnectionManager : IUserConnectionManager
    {
        private static Dictionary<string, List<string>> _userConnectionMap = new();
        private static string _userConnectionMapLocker = string.Empty;
        
        public void KeepUserConnection(string userId, string connectionId)
        {
            lock (_userConnectionMapLocker)
            {
                if (!_userConnectionMap.ContainsKey(userId))
                {
                    _userConnectionMap[userId] = new List<string>();
                }
                _userConnectionMap[userId].Add(connectionId);
            }
        }

        public void RemoveUserConnection(string connectionId)
        {
            lock (_userConnectionMapLocker)
            {
                foreach (var userId in _userConnectionMap.Keys.Where(userId => _userConnectionMap.ContainsKey(userId)).Where(userId => _userConnectionMap[userId].Contains(connectionId)))
                {
                    _userConnectionMap[userId].Remove(connectionId);
                    break;
                }
            }
        }

        public List<string> GetUserConnections(string userId)
        {
            var conn = new List<string>();
            lock (_userConnectionMapLocker)
            {
                conn = _userConnectionMap[userId];
            }
            return conn;
        }

        public List<string> GetAllConnection()
        {
            var conn = new List<string>();
            lock (_userConnectionMapLocker)
            {
                foreach(var dictionary in _userConnectionMap)
                {
                    conn.AddRange(dictionary.Value.ToList());
                }
            }
            return conn;
        }
    }
}