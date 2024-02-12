using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SharpControls.SimpleAuth.Models
{
    public class User
    {
        public static string ApiHost { get; set; } = "";
        public static string AdminRank { get; set; } = "admin";
        public string Name { get; set; } = "Guest";
        public string Token { get; set; } = "";

        public string[] Ranks { get; private set; } = [];
        public string[] AllowedActions { get; private set; } = [];
        public bool Authenticated { get; private set; } = false;
        public event EventHandler? AuthenticatedChanged;

        /// <summary>
        /// Login as Guest
        /// </summary>
        public User()
        {
            Authenticated = true;
            AuthenticatedChanged?.Invoke(this, new EventArgs());
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

        private async void Login(string name, string password)
        {
            Dictionary<string, string> headers = [];
            headers.Add("Accept", "application/json");
            var connection = new APIHandler.REST.RestConnection(headers, ApiHost);
            var content = new APIHandler.RequestContent();
            content.AddNode("name", name);
            content.AddNode("password", password);
            var response = await connection.SendPost("login", content);
            var loginStatus = response.Value<string>("status");
            if (loginStatus == "ok")
            {
                Name = name;
                Ranks = response.Value<string[]>("ranks") ?? ([]);
                AllowedActions = response.Value<string[]>("allowed_actions") ?? ([]);
                Token = response.Value<string>("token") ?? "";
                Authenticated = true;
                AuthenticatedChanged?.Invoke(this, new EventArgs());
            }
            else if(loginStatus == "wrong_credentials")
            {
                Authenticated = false;
                AuthenticatedChanged?.Invoke(this, new EventArgs());
            }
            else if(loginStatus == "error")
            {
                var loginStatusText = response.Value<string>("statusText") ?? "";
                throw new Exception("Server Error: " + loginStatusText);
            }
        }

        public bool IsGuest()
        {
            return Ranks.Length == 0;
        }

        public bool IsAdmin()
        {
            return Ranks.Contains(AdminRank);
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
