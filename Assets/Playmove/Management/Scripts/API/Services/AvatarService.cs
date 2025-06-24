using System;
using System.Collections.Generic;
using System.Linq;
using Playmove.Avatars.API;
using Playmove.Avatars.API.Models;
using Playmove.Core;
using Playmove.Core.API;
using Playmove.Core.Storages;
using UnityEngine;

namespace Playmove.Management.API.Services
{
    /// <summary>
    ///     Responsible to access APIs for Avatar this should only be used by Playmovers
    /// </summary>
    public class AvatarService : Avatars.API.Services.AvatarService
    {
        public void GetDuplicatedPlayers(string player, AsyncCallback<List<Player>> completed)
        {
            WebRequestWrapper.Instance.Get("/Alunos/GetAllByName",
                new Dictionary<string, string> { { "nome", player } },
                result =>
                {
                    var parsedResult = ParseVmsJson(result);
                    completed?.Invoke(parsedResult);
                });
        }

        /// <summary>
        ///     Add a new player/avatar with default settings
        /// </summary>
        /// <param name="completed">Callback with the representation of the Player or error</param>
        public void AddDefaultPlayer(Sprite thumbnail, AsyncCallback<Player> completed)
        {
            AddDefaultPlayer(string.Empty, thumbnail, completed);
        }

        /// <summary>
        ///     Add a new player/avatar with default settings
        /// </summary>
        /// <param name="name">Player name</param>
        /// <param name="classes">All classes that this player belongs to</param>
        /// <param name="completed">Callback with the representation of the Player or error</param>
        public void AddDefaultPlayer(string name, Sprite thumbnail, AsyncCallback<Player> completed)
        {
            var player = new Player
            {
                Name = name,
                GUID = Guid.NewGuid().ToString(),
                Elements = AvatarAPI.Categories.DefaultElements.ToList()
            };
            completed?.Invoke(new AsyncResult<Player>(player, string.Empty));
        }

        public void UpdatePlayer(Player player, Sprite thumbnail, AsyncCallback<bool> completed)
        {
            UpdatePlayer(player, _ =>
            {
                if (thumbnail != null)
                    Storage.SaveSprite(player.ThumbnailPath, thumbnail, true,
                        result => completed?.Invoke(new AsyncResult<bool>(true, string.Empty)));
                else
                    completed?.Invoke(new AsyncResult<bool>(true, string.Empty));
            });
        }

        /// <summary>
        ///     Update the specified player
        /// </summary>
        /// <param name="player">Player to be updated</param>
        /// <param name="completed">Callback containing the result of operation or error</param>
        public void UpdatePlayer(Player player, AsyncCallback<bool> completed)
        {
            // TODO: Parse json using the InterfaceContractResolver and ElementConverter
            AvatarAPI.GetPlayer(player.GUID, result =>
            {
                // -----
                if (string.IsNullOrEmpty(player.GUID))
                    player.GUID = Guid.NewGuid().ToString();
                // -----
                var thumbnailPath = $"{AvatarAPI.RootAvatarPath}/Files/Thumbnails/{player.GUID}.png";
                if (player.ThumbnailPath != thumbnailPath)
                    player.ThumbnailPath = thumbnailPath;

                if (result.Data == null)
                    ManagementAPI.Player.Add(player, playerAdded =>
                    {
                        player = playerAdded.Data;
                        completed?.Invoke(new AsyncResult<bool>(true, string.Empty));
                    });
                else
                    ManagementAPI.Player.Update(player,
                        saved => { completed?.Invoke(new AsyncResult<bool>(saved.Data, string.Empty)); });
            });
        }
    }
}