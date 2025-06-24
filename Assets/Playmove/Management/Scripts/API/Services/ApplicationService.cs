using System.Collections.Generic;
using Playmove.Core;
using Playmove.Core.API;
using Playmove.Core.API.Services;
using Playmove.Management.API.Models;
using Playmove.Management.API.Vms;

namespace Playmove.Management.API.Services
{
    /// <summary>
    ///     Responsible to access APIs for Application
    /// </summary>
    public class ApplicationService : Service<Application, AplicativoVm>
    {
        /// <summary>
        ///     Get application data from the specified GUID
        ///     This GUID is passed by Playmove
        /// </summary>
        /// <param name="guid">Application GUID passed by Plamove</param>
        /// <param name="completed">Callback containing data from Application or error</param>
        public void Get(string guid, AsyncCallback<Application> completed)
        {
            WebRequestWrapper.Instance.Get("/Aplicativos/Get",
                new Dictionary<string, string> { { "productGuid", guid } },
                result => completed?.Invoke(ParseVmJson(result)));
        }

        public void GetAll(AsyncCallback<List<Application>> completed)
        {
            WebRequestWrapper.Instance.Get("/Aplicativos/GetAll", result => completed?.Invoke(ParseVmsJson(result)));
        }
    }
}