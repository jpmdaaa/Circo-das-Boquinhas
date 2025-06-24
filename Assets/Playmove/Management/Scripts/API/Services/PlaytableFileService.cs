using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Playmove.Core;
using Playmove.Core.API;
using Playmove.Core.API.Models;
using Playmove.Core.API.Vms;
using Playmove.Core.Storages;
using Playmove.Management.API.Models;
using UnityEngine;

namespace Playmove.Management.API.Services
{
    /// <summary>
    ///     Responsible to access APIs for Files this should only be used by Playmovers
    /// </summary>
    public class PlaytableFileService : Core.API.Services.PlaytableFileService
    {
        public void GetFiles(AsyncCallback<List<PlaytableFile>> completed)
        {
            WebRequestWrapper.Instance.Get("/ArquivoAplicativos/GetAll",
                result =>
                {
                    var parsedResult = ParseVmsJson(result);
                    if (!parsedResult.HasError)
                        parsedResult.Data = parsedResult.Data.Where(file => !file.Deleted).ToList();
                    completed?.Invoke(parsedResult);
                });
        }

        public void GetFiles(long applicationId, string language, AsyncCallback<List<PlaytableFile>> completed)
        {
            GetFiles(applicationId, result =>
            {
                if (result.HasError)
                {
                    completed?.Invoke(result);
                    return;
                }

                completed?.Invoke(new AsyncResult<List<PlaytableFile>>(
                    result.Data.Where(file => !file.Deleted && file.Language.ToLower() == language.ToLower()).ToList(),
                    string.Empty));
            });
        }

        public void GetFiles(long applicationId, AsyncCallback<List<PlaytableFile>> completed)
        {
            WebRequestWrapper.Instance.Get("/ArquivoAplicativos/GetAll",
                new Dictionary<string, string> { { "aplicativoId", applicationId.ToString() } },
                result =>
                {
                    var parsedResult = ParseVmsJson(result);
                    if (!parsedResult.HasError)
                        parsedResult.Data = parsedResult.Data.Where(file => !file.Deleted).ToList();
                    completed?.Invoke(parsedResult);
                });
        }

        /// <summary>
        ///     Get a list of all files that the playtable has from all apps
        /// </summary>
        /// <param name="filter">Filter that you may want to use</param>
        /// <param name="completed">Callback containing a list with all files or error</param>
        public void GetFilesFiltered(FilesFilter filter, AsyncCallback<List<PlaytableFile>> completed)
        {
            GetFilesGrouped(filter, result =>
            {
                if (result.HasError)
                {
                    completed?.Invoke(new AsyncResult<List<PlaytableFile>>(null, result.Error));
                    return;
                }

                var files = new List<PlaytableFile>();
                foreach (var group in result.Data)
                    files.AddRange(group.Value);
                completed?.Invoke(new AsyncResult<List<PlaytableFile>>(files, string.Empty));
            });
        }

        /// <summary>
        ///     Get all files that the playtable has from all apps grouped together by the filter properties
        /// </summary>
        /// <param name="filter">Filter that you may want to use, this will also group the files</param>
        /// <param name="completed">Callback containing a list with all files or error</param>
        public void GetFilesGrouped(FilesFilter filter,
            AsyncCallback<Dictionary<string, List<PlaytableFile>>> completed)
        {
            var parameters = new Dictionary<string, string>
            {
                { "AplicativoId", filter.ApplicationId.GetValueOrDefault(-1).ToString() },
                { "AlunoId", filter.PlayerId.GetValueOrDefault(-1).ToString() },
                { "HistoricoId", filter.HistoricId.GetValueOrDefault(-1).ToString() },
                { "TurmaId", filter.ClassroomId.GetValueOrDefault(-1).ToString() },
                { "AgrupamentoId", filter.GroupingId.GetValueOrDefault(-1).ToString() }
            };
            WebRequestWrapper.Instance.Get("/ArquivoAplicativos/GetAllByFilter", parameters, result =>
            {
                if (result.HasError)
                {
                    completed?.Invoke(new AsyncResult<Dictionary<string, List<PlaytableFile>>>(null, result.Error));
                    return;
                }

                var files = new Dictionary<string, List<PlaytableFile>>();
                try
                {
                    var filesGroupedVm =
                        JsonConvert.DeserializeObject<Dictionary<string, List<ArquivoAplicativoVm>>>(result.Data.text);
                    foreach (var filesGroup in filesGroupedVm)
                        files.Add(filesGroup.Key,
                            filesGroup.Value.Select(file => new PlaytableFile(file)).Where(file => !file.Deleted)
                                .ToList());

                    completed?.Invoke(new AsyncResult<Dictionary<string, List<PlaytableFile>>>(files, string.Empty));
                }
                catch (Exception e)
                {
                    completed?.Invoke(new AsyncResult<Dictionary<string, List<PlaytableFile>>>(null, e.ToString()));
                }
            });
        }

        /// <summary>
        ///     Get a dictionary with all dropdowns for all files in playtable from all apps
        /// </summary>
        /// <param name="filter">Filter that you may want to use</param>
        /// <param name="completed">Callback containing a dictionary with all dropdowns or error</param>
        public void GetFilesFilteredDropdown(FilesFilter filter,
            AsyncCallback<Dictionary<string, List<DropdownItem>>> completed)
        {
            var parameters = new Dictionary<string, string>
            {
                { "AplicativoId", filter.ApplicationId.GetValueOrDefault(-1).ToString() },
                { "AlunoId", filter.PlayerId.GetValueOrDefault(-1).ToString() },
                { "HistoricoId", filter.HistoricId.GetValueOrDefault(-1).ToString() },
                { "TurmaId", filter.ClassroomId.GetValueOrDefault(-1).ToString() },
                { "AgrupamentoId", filter.GroupingId.GetValueOrDefault(-1).ToString() }
            };
            WebRequestWrapper.Instance.Get("/ArquivoAplicativos/GetAllDropdowns", parameters, result =>
            {
                if (result.HasError)
                {
                    completed?.Invoke(new AsyncResult<Dictionary<string, List<DropdownItem>>>(null, result.Error));
                    return;
                }

                var dropdowns = new Dictionary<string, List<DropdownItem>>();
                try
                {
                    var dropdownsVms = JsonConvert.DeserializeObject<JObject>(result.Data.text);
                    foreach (var entryVm in dropdownsVms)
                    {
                        dropdowns.Add(entryVm.Key, new List<DropdownItem>());
                        foreach (var item in entryVm.Value)
                        {
                            var name = item.Value<string>("Nome") ?? item.Value<string>("NomeApp");
                            if (entryVm.Key == "Historico")
                                name = item.Value<string>("Descricao");
                            else if (entryVm.Key == "Agrupamento")
                                name = item.Value<string>("Agrupamento");

                            if (!string.IsNullOrEmpty(name) &&
                                dropdowns[entryVm.Key].Find(drop => drop.Name == name) == null)
                                dropdowns[entryVm.Key].Add(new DropdownItem
                                {
                                    Id = item.Value<long>("Id"),
                                    Name = name
                                });
                        }
                    }

                    completed?.Invoke(new AsyncResult<Dictionary<string, List<DropdownItem>>>(dropdowns, string.Empty));
                }
                catch (Exception e)
                {
                    completed?.Invoke(new AsyncResult<Dictionary<string, List<DropdownItem>>>(null, e.ToString()));
                }
            });
        }

        public void Add(Sprite sprite, PlaytableFile file, AsyncCallback<PlaytableFile> completed)
        {
            Add(sprite.texture, file, completed);
        }

        public void Add(Texture2D texture, PlaytableFile file, AsyncCallback<PlaytableFile> completed)
        {
            if (string.IsNullOrEmpty(file.Extension))
                file.Extension = ".png";
            Add(texture.EncodeToPNG(), file, completed);
        }

        public void Add(byte[] fileBytes, PlaytableFile file, AsyncCallback<PlaytableFile> completed)
        {
            if (file.Size == 0)
                file.Size = fileBytes.Length;
            WebRequestWrapper.Instance.Post("/ArquivoAplicativos/Add", file.GetVmJson(),
                result =>
                {
                    var resultCallback = ParseVmJson(result);
                    if (resultCallback.HasError)
                    {
                        completed?.Invoke(resultCallback);
                        return;
                    }

                    if (!resultCallback.Data.Exists)
                        Storage.WriteFile(resultCallback.Data.FullPath, fileBytes, true,
                            resultFile =>
                            {
                                if (resultFile.HasError)
                                    completed?.Invoke(new AsyncResult<PlaytableFile>(resultCallback.Data,
                                        resultFile.Error));
                                else
                                    completed?.Invoke(resultCallback);
                            }
                        );
                    else
                        completed?.Invoke(resultCallback);
                }
            );
        }

        public void Add(PlaytableFile file, AsyncCallback<PlaytableFile> completed)
        {
            WebRequestWrapper.Instance.Post("/ArquivoAplicativos/Add", file.GetVmJson(),
                result => completed?.Invoke(ParseVmJson(result)));
        }

        /// <summary>
        ///     Delete the file permanently from playtable
        /// </summary>
        /// <param name="file">File to be deleted</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void DeletePermanently(PlaytableFile file, AsyncCallback<bool> completed)
        {
            DeletePermanently(file.Id, completed);
        }

        /// <summary>
        ///     Delete the file permanently from playtable
        /// </summary>
        /// <param name="id">Id of file to be deleted</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void DeletePermanently(long id, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Post("/ArquivoAplicativos/DeleteWithoutLogicExclusion", id.ToString(),
                result => completed?.Invoke(SimpleResult(result)));
        }

        /// <summary>
        ///     Delete the raw file
        /// </summary>
        /// <param name="file">File to be deleted</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        [Obsolete]
        public void DeleteRawFile(RawFile file, AsyncCallback<bool> completed)
        {
            DeleteRawFile(file.Id, completed);
        }

        /// <summary>
        ///     Delete the raw file
        /// </summary>
        /// <param name="id">Id of file to be deleted</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        [Obsolete]
        public void DeleteRawFile(long id, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Post("/Arquivos/Delete", id.ToString(),
                result => completed?.Invoke(SimpleResult(result)));
        }

        public void GetPendrivePath(PlaytableFile file, AsyncCallback<string> completed)
        {
            WebRequestWrapper.Instance.Get("/Arquivos/GetPendrivePath",
                new Dictionary<string, string> { { "id", file.RawFile.Id.ToString() } },
                result => completed?.Invoke(ParseJson<string>(result)));
        }
    }
}