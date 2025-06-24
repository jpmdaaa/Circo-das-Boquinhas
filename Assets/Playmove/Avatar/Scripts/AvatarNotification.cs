using System;
using System.IO;
using System.Text;
using Playmove.Avatars.API;
using Playmove.Core;
using Playmove.Core.Storages;
using UnityEngine;

namespace Playmove.Avatars
{
    public class AvatarNotification : MonoBehaviour
    {
        private static AvatarNotification _instance;

        public static AvatarNotification Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<AvatarNotification>();
                return _instance;
            }
        }

        public bool IsNotifying { get; private set; }
        public static bool ChekedSlots { get; private set; }

        public void CheckAvatarSlots()
        {
            var fileTimePath = Path.Combine(AvatarAPI.RootAvatarPath, "avatar_modified_time.txt");
            AvatarAPI.GetSlots(true, result =>
            {
                if (result.HasError)
                {
                    Notify("Organize os heróis!");
                    return;
                }

                ChekedSlots = true;

                if (GameSettings.HasAI && GameSettings.TotalSlots != result.Data.Count)
                {
                    Notify("Não temos heróis suficientes, quer jogar contra vilões?");
                }
                else if (result.Data.Count != GameSettings.TotalSlots || result.Data.Count < GameSettings.MinSlots)
                {
                    Notify("Organize os heróis!");
                }
                else
                {
                    if (File.Exists(fileTimePath))
                        Storage.ReadFile(fileTimePath, onRead =>
                        {
                            var modifiedTime = DateTime.Parse(Encoding.UTF8.GetString(onRead.Data));
                            var passedTime = DateTime.Now - modifiedTime;
                            if (passedTime.Minutes >= 15)
                            {
                                Notify("A mesma liga de heróis está jogando?");
                                Storage.WriteFile(fileTimePath, Encoding.UTF8.GetBytes(DateTime.Now.ToString()), true,
                                    null);
                            }
                        });
                    else
                        Storage.WriteFile(fileTimePath, Encoding.UTF8.GetBytes(DateTime.Now.ToString()), true, null);
                }
            });
        }

        private void Notify(string message)
        {
            if (IsNotifying) return;
            IsNotifying = true;

            Debug.Log(message);
        }
    }
}