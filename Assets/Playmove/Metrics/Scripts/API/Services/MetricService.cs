using System.Collections.Generic;
using Playmove.Core;
using Playmove.Core.API;
using Playmove.Core.API.Services;
using Playmove.Metrics.API.Models;
using Playmove.Metrics.API.Vms;
using UnityEngine;

namespace Playmove.Metrics.API.Services
{
    /// <summary>
    ///     Responsible to handle APIs for Metrics.
    ///     This class is called when Session, Mathes and Events occurs
    /// </summary>
    public class MetricService
    {
        public void StartSession(AsyncCallback<Session> completed)
        {
            WebRequestWrapper.Instance.Get("/Metricas/StartSession",
                new Dictionary<string, string> { { "GameGUID", GameSettings.GUID } },
                result => { completed?.Invoke(Service<Session, SessaoVm>.ParseVmJson(result)); });
        }

        public void EndSession(long SessionID, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Post("/Metricas/EndSession", SessionID.ToString(), result =>
            {
                if (result.HasError)
                {
                    Debug.LogError("Erro ao encerrar Sessão");
                    return;
                }

                completed?.Invoke(new AsyncResult<bool>(true, string.Empty));
            });
        }

        public void StartMatch(Match match, AsyncCallback<long> completed)
        {
            WebRequestWrapper.Instance.Post("/Metricas/StartMatch", match.GetVmJson(), result =>
            {
                if (result.HasError)
                {
                    Debug.LogError("Erro ao iniciar Match");
                    return;
                }

                completed?.Invoke(new AsyncResult<long>(long.Parse(result.Data.text), string.Empty));
            });
        }

        public void SaveMatch(Match match, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Post("/Metricas/EndMatch", match.GetVmJson(), result =>
            {
                if (result.HasError)
                {
                    Debug.LogError("Erro ao iniciar Match");
                    return;
                }

                completed?.Invoke(new AsyncResult<bool>(true, string.Empty));
            });
        }
    }
}