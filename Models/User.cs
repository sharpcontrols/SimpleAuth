using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpControls.SimpleAuth.Models
{
    public class User
    {
        public string Name { get; set; } = "User";
        public string Token { get; set; } = "";

        public string[] Ranks { get; private set; } = [];
        public string[] AllowedActions { get; private set; } = [];
        public bool Authenticated { get; private set; } = false;

        /// <summary>
        /// Login as Guest
        /// </summary>
        public User()
        {
            Ranks = ["guest"];
            Authenticated = true;
        }

        /// <summary>
        /// Login with name and password
        /// </summary>
        /// <param name="name">Name of the user</param>
        /// <param name="password">Password of the user</param>
        /// <param name="encrypted">Is the password already encrypted?</param>
        public User(string name, string password, bool encrypted = true)
        {
            //Encrypt the password
            if (!encrypted)
            {
                throw new NotImplementedException("Encryption not implemented, implement it yourself!");
            }
            Login(name, password);
        }

        private void Login(string name, string password)
        {
            
        }

        public void SetAuthenticated(bool authenticated)
        {
            Authenticated = authenticated;
        }

        public void AddAllowedAction(string action)
        {
            List<string> actions = [.. AllowedActions, action];
            AllowedActions = [.. actions];
        }

        public void RemoveAllowedAction(string action)
        {
            List<string> actions = [.. AllowedActions];
            actions.Remove(action);
            AllowedActions = [.. actions];
        }

        public void AddRank(string rank)
        {
            List<string> ranks = [.. Ranks, rank];
            Ranks = [.. ranks];
        }

        public void RemoveRank(string rank)
        {
            List<string> ranks = [.. Ranks];
            ranks.Remove(rank);
            Ranks = [.. ranks];
        }

        public bool IsAuthenticated()
        {
            return Authenticated;
        }

        public bool IsNotAuthenticated()
        {
            return !IsAuthenticated();
        }

        public bool IsAuthenticatedAs(string rank)
        {

            return Authenticated && Ranks.Contains(rank);
        }

        public bool IsNotAuthenticatedAs(string rank)
        {
            return !IsAuthenticatedAs(rank);
        }

        public bool IsAuthenticatedFor(string action)
        {
            return Authenticated && AllowedActions.Contains(action);
        }

        public bool IsNotAuthenticatedFor(string action)
        {
            return !IsAuthenticatedFor(action);
        }
    }
}
