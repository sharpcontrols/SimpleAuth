using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpControls.SimpleAuth.Interfaces
{
    public interface IAuthenticable
    {
        protected string[] Ranks { get; set; }
        protected string[] AllowedActions { get; set; }
        protected bool Authenticated { get; set; }

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
            List<string> ranks = [..Ranks];
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
