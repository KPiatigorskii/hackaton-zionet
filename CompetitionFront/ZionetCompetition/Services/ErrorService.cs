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

                case string s when s.Contains(Errors.Errors.NotAuthorized):
                    Forbidden(Errors.Errors.NotAuthorized);
                    break;

                case string s when s.Contains(Errors.Errors.NotAuthorizedByEmail):
                    Unauthorized(Errors.Errors.NotAuthorizedByEmail);
                    break;

                case string s when s.Contains(Errors.Errors.Forbidden):
                    Forbidden(Errors.Errors.Forbidden);
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

        private void Forbidden(string errorMessage)
        {
            _navigationManager.NavigateTo($"/403/{errorMessage}");
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
