using Playmove.Core;
using Playmove.Core.API;
using Playmove.Core.API.Models;

namespace Playmove.Management.API.Services
{
    /// <summary>
    ///     Responsible to access APIs for Classroom this should only be used by Playmovers
    /// </summary>
    public class ClassroomService : Core.API.Services.ClassroomService
    {
        /// <summary>
        ///     Add new classroom to playtable
        /// </summary>
        /// <param name="classe">Classroom to be added</param>
        /// <param name="completed">Callback containing the representation of the Classroom or error</param>
        public void Add(Classroom classe, AsyncCallback<Classroom> completed)
        {
            WebRequestWrapper.Instance.Post("/Turmas/Add", classe.GetVmJson(),
                result => completed?.Invoke(ParseVmJson(result)));
        }

        /// <summary>
        ///     Update the specified classroom
        /// </summary>
        /// <param name="classe">Classroom to be updated</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void Update(Classroom classe, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Post("/Turmas/Update", classe.GetVmJson(),
                result => completed?.Invoke(SimpleResult(result)));
        }

        /// <summary>
        ///     Delete the specified classroom
        /// </summary>
        /// <param name="classe">Classroom to be deleted</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void Delete(Classroom classe, AsyncCallback<bool> completed)
        {
            Delete(classe.Id, completed);
        }

        /// <summary>
        ///     Delete the specified classroom
        /// </summary>
        /// <param name="id">Id of classroom to be deleted</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void Delete(long id, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Post("/Turmas/Delete", id.ToString(),
                result => completed?.Invoke(SimpleResult(result)));
        }
    }
}