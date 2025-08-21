using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Pages.Users
{
    /// <summary>
    /// Handles the User management page 
    /// This class is for loading users, create new users, edit users that already exist and for deleting users through the API
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<User> Users { get; set; } = new();

        [BindProperty]
        public User FormUser { get; set; } = new();

        [BindProperty]
        public string ActionMode { get; set; } = "Create";  

        public async Task OnGetAsync()
        {
            await LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var data = await client.GetFromJsonAsync<List<User>>("users");
            if (data != null) Users = data;
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");

            if (ActionMode == "Create")
            {
                await client.PostAsJsonAsync("users", FormUser);
            }
            else if (ActionMode == "Edit")
            {
                await client.PutAsJsonAsync($"users/{FormUser.UserName}", FormUser);
            }

            return RedirectToPage();  
        }
        /// <summary>
        /// Deletes a user based on their username using the API
        /// </summary>
        /// <param name="username">Username of the user to delete</param>
        /// <returns>The page reloads after deleting a user</returns>
        public async Task<IActionResult> OnPostDeleteAsync(string username)
        {
            var client = _httpClientFactory.CreateClient("Api");
            await client.DeleteAsync($"users/{username}");
            return RedirectToPage();
        }

        /// <summary>
        /// Loads the selected user's data into the form 
        /// </summary>
        /// <param name="username"> Username of the user to edit </param>
        /// <returns> Returns the Page with the user's data already filled in the form </returns>
        public async Task<IActionResult> OnPostEditAsync(string username)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var user = await client.GetFromJsonAsync<User>($"users/{username}");
            if (user != null)
            {
                FormUser = user;
                ActionMode = "Edit";
            }
            await LoadUsersAsync();
            return Page();
        }
    }
}
