using Playmove.Core;
using Playmove.Core.API.Models;
using Playmove.Core.API.Vms;

namespace Playmove.Management.API.Services
{
    public class PrefsService : Core.API.Services.PrefsService
    {
        public void GetUpdateAutomaticClassrooms(AsyncCallback<Pref> completed)
        {
            Get("atualizacaoAutomaticaDeTurmas", completed);
        }

        public void GetShowClassroomInGame(AsyncCallback<Pref> completed)
        {
            Get("turmasHabilitadas", completed);
        }

        public void SetUpdateAutomaticClassrooms(bool value, AsyncCallback<bool> completed)
        {
            Set("atualizacaoAutomaticaDeTurmas", ValorTipo.Bool, value.ToString(), completed);
        }

        public void SetClassroomsInGame(bool value, AsyncCallback<bool> completed)
        {
            Set("turmasHabilitadas", ValorTipo.Bool, value.ToString(), completed);
        }
    }
}