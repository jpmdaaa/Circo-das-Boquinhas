using System.Collections.Generic;
using Playmove.Core;
using Playmove.Core.API;
using Playmove.Core.API.Services;
using Playmove.Management.API.Models;
using Playmove.Management.API.Vms;

namespace Playmove.Management.API.Services
{
    /// <summary>
    ///     Responsible to access APIs for Grades this should only be used by Playmovers
    /// </summary>
    public class GradeService : Service<Grade, SerieVm>
    {
        public void GetAll(AsyncCallback<List<Grade>> completed)
        {
            GetAll(GameSettings.Language, completed);
        }

        public void GetAll(string language, AsyncCallback<List<Grade>> completed)
        {
            WebRequestWrapper.Instance.Get("/Series/GetAllByLocalization",
                new Dictionary<string, string> { { "localizacao", language } },
                result => completed?.Invoke(ParseVmsJson(result)));
        }

        public void Get(long id, AsyncCallback<Grade> completed)
        {
            WebRequestWrapper.Instance.Get("/Series/Get",
                new Dictionary<string, string> { { "id", id.ToString() } },
                result => completed?.Invoke(ParseVmJson(result)));
        }
    }
}