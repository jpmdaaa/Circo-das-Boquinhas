using Playmove.Management.API.Services;

namespace Playmove.Management
{
    /// <summary>
    ///     APIs more specific to app Gerenciador de Conteúdo should only be used by Playmovers
    /// </summary>
    public static class ManagementAPI
    {
        private static ApplicationService _application;

        private static InfoService _info;

        private static PrefsService _prefs;

        private static ThemeService _theme;

        private static ClassroomService _classroom;

        private static GradeService _grade;

        private static PlayerService _player;

        private static AvatarService _avatar;

        private static PlaytableFileService _file;

        /// <summary>
        ///     Service to access Playtable Info
        /// </summary>
        public static ApplicationService Application
        {
            get
            {
                if (_application == null)
                    _application = new ApplicationService();
                return _application;
            }
        }

        /// <summary>
        ///     Service to access Playtable Info
        /// </summary>
        public static InfoService Info
        {
            get
            {
                if (_info == null)
                    _info = new InfoService();
                return _info;
            }
        }

        /// <summary>
        ///     Service to access Playtable Setups
        /// </summary>
        public static PrefsService Prefs
        {
            get
            {
                if (_prefs == null)
                    _prefs = new PrefsService();
                return _prefs;
            }
        }

        /// <summary>
        ///     Service to access Theme
        /// </summary>
        public static ThemeService Theme
        {
            get
            {
                if (_theme == null)
                    _theme = new ThemeService();
                return _theme;
            }
        }

        /// <summary>
        ///     Service to access Classroom
        /// </summary>
        public static ClassroomService Classroom
        {
            get
            {
                if (_classroom == null)
                    _classroom = new ClassroomService();
                return _classroom;
            }
        }

        /// <summary>
        ///     Service to access Grade
        /// </summary>
        public static GradeService Grade
        {
            get
            {
                if (_grade == null)
                    _grade = new GradeService();
                return _grade;
            }
        }

        /// <summary>
        ///     Service to access Player
        /// </summary>
        public static PlayerService Player
        {
            get
            {
                if (_player == null)
                    _player = new PlayerService();
                return _player;
            }
        }

        /// <summary>
        ///     Service to access Avatar
        /// </summary>
        public static AvatarService Avatar
        {
            get
            {
                if (_avatar == null)
                    _avatar = new AvatarService();
                return _avatar;
            }
        }

        /// <summary>
        ///     Service to access File
        /// </summary>
        public static PlaytableFileService File
        {
            get
            {
                if (_file == null)
                    _file = new PlaytableFileService();
                return _file;
            }
        }
    }
}