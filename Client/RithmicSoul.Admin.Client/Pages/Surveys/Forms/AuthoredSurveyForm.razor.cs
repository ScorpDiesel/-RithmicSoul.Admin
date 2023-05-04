

using Microsoft.AspNetCore.Components;

namespace RithmicSoul.Admin.Client.Pages.Surveys.Forms;

public partial class AuthoredSurveyFormBase : ComponentBase
{



    [Inject]
    NavigationManager Navigation { get; set; }

    protected void Cancel()
    {
        Navigation.NavigateTo("/Survey");
    }

    protected void Save()
    {
        Navigation.NavigateTo("/Survey");
    }
}