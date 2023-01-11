using Microsoft.AspNetCore.Components;

namespace ZionetCompetition.Services
{
    public class ErrorService
    {
        private readonly NavigationManager _navigationManager;
        public ErrorService(NavigationManager navigationManager) 
        {
            _navigationManager = navigationManager;
        }
        public void Redirect(string error)
        {
            Console.WriteLine(error);
            switch (error)
            {
                case string s when s.Contains(Errors.Errors.General):
                    GeneralErr(Errors.Errors.General);
                    break;

                case string s when s.Contains(Errors.Errors.ConnectionError):
                    GeneralErr(Errors.Errors.ConnectionError);
                    break;

                case string s when s.Contains(Errors.Errors.BadRequest):
                    GeneralErr(Errors.Errors.BadRequest);
                    break;

                case string s when s.Contains(Errors.Errors.ItemNotFound):
                    NotFoundPage(Errors.Errors.ItemNotFound);
                    break;

                case string s when s.Contains(Errors.Errors.ConflictData):
                    GeneralErr(Errors.Errors.ConflictData);
                    break;

                case string s when s.Contains(Errors.Errors.NotAuthorizedOnClient):
                    Unauthorized(Errors.Errors.NotAuthorizedOnClient);
                    break;

                case string s when s.Contains(Errors.Errors.NotAuthorizedOnServer):
                    Unauthorized(Errors.Errors.NotAuthorizedOnServer);
                    break;

                default:
                    GeneralErr(error); 
                    break;

            }
        }

        private void Unauthorized(string errorMessage)
        {
            _navigationManager.NavigateTo($"/401/{errorMessage}");
        }

        private void NotFoundPage(string errorMessage)
        {
           // errorMessage = "asdf";
            _navigationManager.NavigateTo($"/404/{errorMessage}");
        }

        //private void GeneralErr(string errorMessage)
        //{
        //    _navigationManager.NavigateTo($"/500/{errorMessage}");
        //}

        private void GeneralErr(string errorMessage)
        {
            _navigationManager.NavigateTo($"/500/{errorMessage}");
        }
    }
}
