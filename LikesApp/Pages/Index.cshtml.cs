using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LikesApp.Pages;

[IgnoreAntiforgeryToken]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public class IndexData
    {
        public string Token { get; set; }
    }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync([FromBody] IndexData request, CancellationToken cancellationToken)
    {
        var httpClient = new HttpClient();
        using var formData = new MultipartFormDataContent
        {
            { new StringContent(request.Token), "token" }
        };
        var response = await httpClient.PostAsync("https://api.dtf.ru/v3.4/auth/refresh", formData, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<DtfTokenResponse>(json);
            return new JsonResult(new TokenResponse { Token = responseData.Data.AccessToken, Expires = responseData.Data.AccessExpTimestamp });
        }

        return new JsonResult(new TokenResponse { Token = null });
    }
}
