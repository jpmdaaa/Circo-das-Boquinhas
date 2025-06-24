using System;
using System.Collections.Generic;
using Playmove.Core;
using Playmove.Core.API;
using Playmove.Core.API.Models;

namespace Playmove.Management.API.Services
{
    /// <summary>
    ///     Responsible to access APIs for Themes this should only be used by Playmovers
    /// </summary>
    public class ThemeService : Core.API.Services.ThemeService
    {
        /// <summary>
        ///     Get all themes for the specified app
        /// </summary>
        /// <param name="applicationId">Application id that you want the themes from</param>
        /// <param name="completed">Callback containing a list with all themes or error</param>
        public void GetAll(long applicationId, AsyncCallback<List<Theme>> completed)
        {
            WebRequestWrapper.Instance.Get("/GrupoArquivos/GetAllByApp",
                new Dictionary<string, string> { { "aplicativoId", applicationId.ToString() } },
                result => completed?.Invoke(ParseVmsJson(result)));
        }

        /// <summary>
        ///     Get all themes for the specified app and filtered by language
        /// </summary>
        /// <param name="applicationId">Application id that you want the themes from</param>
        /// <param name="language">Language that the themes should be</param>
        /// <param name="completed">Callback containing a list with all themes or error</param>
        public void GetAll(long applicationId, string language, AsyncCallback<List<Theme>> completed)
        {
            WebRequestWrapper.Instance.Get("/GrupoArquivos/GetAllByAppAndLocalization",
                new Dictionary<string, string>
                {
                    { "aplicativoId", applicationId.ToString() }, { "localizacao", language }
                },
                result => completed?.Invoke(ParseVmsJson(result)));
        }

        /// <summary>
        ///     Get only all factory themes for the specified app
        /// </summary>
        /// <param name="applicationId">Application id that you want the themes from</param>
        /// <param name="completed">Callback containing a list with all themes or error</param>
        public void GetFactoryThemes(long applicationId, AsyncCallback<List<Theme>> completed)
        {
            WebRequestWrapper.Instance.Get("/GrupoArquivos/GetAllByAppAndConfig",
                new Dictionary<string, string>
                {
                    { "aplicativoId", applicationId.ToString() }, { "configuracaoFabrica", "True" }
                },
                result => completed?.Invoke(ParseVmsJson(result)));
        }

        /// <summary>
        ///     Get only all factory themes for the specified app filtered by language
        /// </summary>
        /// <param name="applicationId">Application id that you want the themes from</param>
        /// <param name="language">Language that the themes should be</param>
        /// <param name="completed">Callback containing a list with all themes or error</param>
        public void GetFactoryThemes(long applicationId, string language, AsyncCallback<List<Theme>> completed)
        {
            WebRequestWrapper.Instance.Get("/GrupoArquivos/GetAllByAppAndConfigAndLocalization",
                new Dictionary<string, string>
                {
                    { "aplicativoId", applicationId.ToString() },
                    { "configuracaoFabrica", "True" },
                    { "localizacao", language }
                },
                result => completed?.Invoke(ParseVmsJson(result)));
        }

        /// <summary>
        ///     Get all themes that are on trash
        /// </summary>
        /// <param name="completed">Callback containing a list with all themes or error</param>
        public void GetThemesInTrash(AsyncCallback<List<Theme>> completed)
        {
            WebRequestWrapper.Instance.Get("/GrupoArquivos/GetAllFromTrash",
                new Dictionary<string, string> { { "lixeira", "True" } },
                result => completed?.Invoke(ParseVmsJson(result)));
        }

        /// <summary>
        ///     Add new theme to playtable
        /// </summary>
        /// <param name="theme">Theme to be added</param>
        /// <param name="completed">Callback containing the representation of the theme or error</param>
        public void Add(Theme theme, AsyncCallback<Theme> completed)
        {
            WebRequestWrapper.Instance.Post("/GrupoArquivos/Add", theme.GetVmJson(),
                result => completed?.Invoke(ParseVmJson(result)));
        }

        /// <summary>
        ///     Update the specified theme
        /// </summary>
        /// <param name="theme">Theme to be updated</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void Update(Theme theme, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Post("/GrupoArquivos/Update", theme.GetVmJson(),
                result => completed?.Invoke(SimpleResult(result)));
        }

        /// <summary>
        ///     Delete the specified theme
        /// </summary>
        /// <param name="theme">Theme to be deleted</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void Delete(Theme theme, AsyncCallback<bool> completed)
        {
            Delete(theme.Id, completed);
        }

        /// <summary>
        ///     Delete the specified theme
        /// </summary>
        /// <param name="id">Theme id to be deleted</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void Delete(long id, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Post("/GrupoArquivos/Delete", id.ToString(),
                result => completed?.Invoke(SimpleResult(result)));
        }

        /// <summary>
        ///     Delete the theme entirely from the playtable this can not be undone
        /// </summary>
        /// <param name="theme">Theme to be deleted</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void DeleteWithoutLogicExclusion(Theme theme, AsyncCallback<bool> completed)
        {
            DeleteWithoutLogicExclusion(theme.Id, completed);
        }

        /// <summary>
        ///     Delete the theme entirely from the playtable this can not be undone
        /// </summary>
        /// <param name="id">Theme id to be deleted</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void DeleteWithoutLogicExclusion(long id, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Post("/GrupoArquivos/DeleteWithoutLogicExclusion", id.ToString(),
                result => completed?.Invoke(SimpleResult(result)));
        }

        /// <summary>
        ///     Remove specified theme from trash and restore it
        /// </summary>
        /// <param name="theme">Theme to be restored</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void RemoveFromTrash(Theme theme, AsyncCallback<bool> completed)
        {
            RemoveFromTrash(theme.Id, completed);
        }

        /// <summary>
        ///     Remove specified theme from trash and restore it
        /// </summary>
        /// <param name="id">Theme id to be restored</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void RemoveFromTrash(long id, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Post("/GrupoArquivos/RemoveFromTrash", id.ToString(),
                result => completed?.Invoke(SimpleResult(result)));
        }

        /// <summary>
        ///     Set visibility to specified theme
        /// </summary>
        /// <param name="theme">Theme to set visibility</param>
        /// <param name="visibility">Is visible or not</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void SetVisibility(Theme theme, bool visibility, AsyncCallback<bool> completed)
        {
            theme.IsVisible = visibility;
            SetVisibility(theme.Id, visibility, completed);
        }

        /// <summary>
        ///     Set visibility to specified theme
        /// </summary>
        /// <param name="id">Theme id to set visibility</param>
        /// <param name="visibility">Is visible or not</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void SetVisibility(long id, bool visibility, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Get("/GrupoArquivos/SetVisibility",
                new Dictionary<string, string> { { "id", id.ToString() }, { "visibilidade", visibility.ToString() } },
                result => completed?.Invoke(SimpleResult(result)));
        }

        [Obsolete]
        public void SetFactoryVisibility(long applicationId, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Get("/GrupoArquivos/SetFactoryVisibility",
                new Dictionary<string, string> { { "aplicativoId", applicationId.ToString() } },
                result => completed?.Invoke(SimpleResult(result)));
        }
    }
}