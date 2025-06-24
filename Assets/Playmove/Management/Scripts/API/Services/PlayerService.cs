using System;
using System.Collections.Generic;
using System.Linq;
using Playmove.Avatars.API.Models;
using Playmove.Avatars.API.Vms;
using Playmove.Core;
using Playmove.Core.API;
using Playmove.Core.API.Models;
using Playmove.Core.API.Services;

namespace Playmove.Management.API.Services
{
    /// <summary>
    ///     Responsible to access APIs for Player this should only be used by Playmovers
    /// </summary>
    public class PlayerService : Service<Player, AlunoVm>
    {
        /// <summary>
        ///     Get a player
        /// </summary>
        /// <param name="id">Id of the player</param>
        /// <param name="completed">Callback containing the player requested or error</param>
        public void Get(long id, AsyncCallback<Player> completed)
        {
            WebRequestWrapper.Instance.Get("/Alunos/Get", new Dictionary<string, string> { { "id", id.ToString() } },
                result => completed?.Invoke(ParseVmJson(result)));
        }

        /// <summary>
        ///     Get a player
        /// </summary>
        /// <param name="name">Name of the player</param>
        /// <param name="completed">Callback containing the player requested or error</param>
        public void Get(string name, AsyncCallback<Player> completed)
        {
            WebRequestWrapper.Instance.Get("/Alunos/Get", new Dictionary<string, string> { { "nome", name } },
                result => completed?.Invoke(ParseVmJson(result)));
        }

        /// <summary>
        ///     Get all players from playtable including players without classroom
        /// </summary>
        /// <param name="completed">Callback containing a list with all players or error</param>
        public void GetAll(AsyncCallback<List<Player>> completed)
        {
            WebRequestWrapper.Instance.Get("/Alunos/GetAll",
                result =>
                {
                    var parsedResult = ParseVmsJson(result);
                    if (!parsedResult.HasError)
                        parsedResult.Data = parsedResult.Data.Where(player => !player.Deleted).ToList();
                    completed?.Invoke(parsedResult);
                });
        }

        /// <summary>
        ///     Get all players from playtable at a specific classroom
        /// </summary>
        /// <param name="classId">Classroom id from where the players are</param>
        /// <param name="completed">Callback containing a list with all players or error</param>
        public void GetAllInClassroom(long classId, AsyncCallback<List<Player>> completed)
        {
            WebRequestWrapper.Instance.Get("/Alunos/GetAll",
                new Dictionary<string, string>
                {
                    { "turmaId", classId.ToString() }, { "semTurma", "False" }
                },
                result =>
                {
                    var parsedResult = ParseVmsJson(result);
                    if (!parsedResult.HasError)
                        parsedResult.Data = parsedResult.Data.Where(player => !player.Deleted).ToList();
                    completed?.Invoke(parsedResult);
                });
        }

        /// <summary>
        ///     Get all players without classroom from playtable
        /// </summary>
        /// <param name="completed">Callback containing a list with all players or error</param>
        public void GetAllWithoutClassroom(AsyncCallback<List<Player>> completed)
        {
            WebRequestWrapper.Instance.Get("/Alunos/GetAllByNoClass",
                new Dictionary<string, string> { { "semTumra", "True" } },
                result =>
                {
                    var parsedResult = ParseVmsJson(result);
                    if (!parsedResult.HasError)
                        parsedResult.Data = parsedResult.Data.Where(player => !player.Deleted).ToList();
                    completed?.Invoke(parsedResult);
                });
        }

        /// <summary>
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="completed"></param>
        public void GetAllInClassroomPlusWithoutClassroom(long classId, AsyncCallback<List<Player>> completed)
        {
            WebRequestWrapper.Instance.Get("/Alunos/GetAll",
                new Dictionary<string, string>
                {
                    { "turmaId", classId.ToString() }, { "semTurma", "True" }
                },
                result =>
                {
                    var parsedResult = ParseVmsJson(result);
                    if (!parsedResult.HasError)
                        parsedResult.Data = parsedResult.Data.Where(player => !player.Deleted).ToList();
                    completed?.Invoke(parsedResult);
                });
        }

        public void GetPlayersInTrash(AsyncCallback<List<Player>> completed)
        {
            WebRequestWrapper.Instance.Get("/Alunos/GetAll",
                result =>
                {
                    var parsedResult = ParseVmsJson(result);
                    if (!parsedResult.HasError)
                        parsedResult.Data = parsedResult.Data.Where(player => player.Deleted).ToList();
                    completed?.Invoke(parsedResult);
                });
        }

        /// <summary>
        ///     Add new player to playtable
        /// </summary>
        /// <param name="name">Name of the player to be added</param>
        /// <param name="completed">Callback containing the representation of the player or error</param>
        public void Add(string name, AsyncCallback<Player> completed)
        {
            Add(name, new List<Classroom>(), completed);
        }

        /// <summary>
        ///     Add new player to playtable
        /// </summary>
        /// <param name="name">Name of the player to be added</param>
        /// <param name="classroomIds">Classroom ids that this player belongs</param>
        /// <param name="completed">Callback containing the representation of the player or error</param>
        public void Add(string name, List<Classroom> classrooms, AsyncCallback<Player> completed)
        {
            Add(new Player
            {
                Name = name,
                Classrooms = classrooms
            }, completed);
        }

        /// <summary>
        ///     Add new player to playtable
        /// </summary>
        /// <param name="player">Player data to be added</param>
        /// <param name="completed">Callback containing the representation of the player or error</param>
        public void Add(Player player, AsyncCallback<Player> completed)
        {
            WebRequestWrapper.Instance.Post("/Alunos/Add", player.GetVmJson(),
                result => completed?.Invoke(ParseVmJson(result)));
        }

        /// <summary>
        ///     Update the specified player
        /// </summary>
        /// <param name="player">Player to be updated</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void Update(Player player, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Post("/Alunos/Update", player.GetVmJson(),
                result => completed?.Invoke(SimpleResult(result)));
        }

        /// <summary>
        ///     Delete the specified player
        /// </summary>
        /// <param name="player">Player to be deleted</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void Delete(Player player, AsyncCallback<bool> completed)
        {
            Delete(player.Id, completed);
        }

        /// <summary>
        ///     Delete the specified player
        /// </summary>
        /// <param name="id">Player id to be deleted</param>
        /// <param name="completed">Callback containing the result of the operation or error</param>
        public void Delete(long id, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Post("/Alunos/Delete", id.ToString(),
                result => completed?.Invoke(SimpleResult(result)));
        }


        public void Restore(long id, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Post("/Alunos/Restore", id.ToString(),
                result => completed?.Invoke(SimpleResult(result)));
        }

        [Obsolete]
        public void MigrateNames(List<Player> players, AsyncCallback<bool> completed)
        {
            WebRequestWrapper.Instance.Post("/Alunos/MigrateNames", GetVmsJson(players),
                result => completed?.Invoke(SimpleResult(result)));
        }
    }
}