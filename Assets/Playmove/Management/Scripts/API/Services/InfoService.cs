using Playmove.Core;
using Playmove.Core.API;
using Playmove.Core.API.Services;
using Playmove.Core.API.Vms;
using Playmove.Management.API.Models;

namespace Playmove.Management.API.Services
{
    /// <summary>
    ///     Responsible to access APIs for Playtable Infos this should only be used by Playmovers
    /// </summary>
    public class InfoService : Service<Info, PlaytableInfoVm>
    {
        /// <summary>
        ///     Get playtable infos
        /// </summary>
        /// <param name="completed">Callback containing the playtable info or error</param>
        public void Get(AsyncCallback<Info> completed)
        {
            WebRequestWrapper.Instance.Get("/PlaytableInfos/Get",
                result => completed?.Invoke(ParseVmJson(result)));
        }

        /// <summary>
        ///     Add playtable info
        /// </summary>
        /// <param name="info">Info to be added</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void Add(Info info, AsyncCallback<Info> completed)
        {
            WebRequestWrapper.Instance.Post("/PlaytableInfos/Add", info.GetVmJson(),
                result => completed?.Invoke(ParseVmJson(result)));
        }

        /// <summary>
        ///     Update playtable info
        /// </summary>
        /// <param name="info">Info to be updated</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void Update(Info info, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Post("/PlaytableInfos/Update", info.GetVmJson(),
                result => completed?.Invoke(SimpleResult(result)));
        }

        /// <summary>
        ///     Delete playtable info
        /// </summary>
        /// <param name="info">Info to be deleted</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        private void Delete(Info info, AsyncCallback<bool> completed)
        {
            Delete(info.Id, completed);
        }

        /// <summary>
        ///     Delete playtable info
        /// </summary>
        /// <param name="id">Info id to be deleted</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        private void Delete(long id, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Post("/PlaytableInfos/Delete", id.ToString(),
                result => completed?.Invoke(SimpleResult(result)));
        }
    }
}