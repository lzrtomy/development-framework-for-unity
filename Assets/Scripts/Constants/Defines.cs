using Company.NewApp.Entities;
using System.Collections.Generic;

namespace Company.Constants
{
    public class Defines
    {
        /// <summary>
        /// 标签定义
        /// </summary>
        public class Tags
        {

        }

        /// <summary>
        /// 层级定义
        /// </summary>
        public class Layers
        {

        }

        /// <summary>
        /// 格式定义
        /// </summary>
        public class Format
        {
            public const string Concat2 = "{0}{1}";
            public const string Concat3 = "{0}{1}{2}";
            public const string UnderLine = "{0}_{1}";
            public const string UnderLine2 = "{0}_{1}_{2}";
            public const string ProcessFormat = "{0}/{1}";
            public const string AttrFormat = "{0}+{1}";
            public const string LevelId = "{0}_{1}_{2}";
            public const string DAY_HOUR_MIN = "{0}d {1}h {2}m";
            public const string DAY_HOUR = "{0}d {1}h";
            public const string HOUR_MINUTE = "{0}h {1}m";
            public const string HOUR_MINUTE_SECOND = "{0}h {1}m {2}s";
            public const string MINUTE_SECOND = "{0}m {1}s";
            public const string COUNT_DOWN_TIME = "{0}:{1}:{2}";
        }

        /// <summary>
        /// 文本定义
        /// </summary>
        public class Text
        {
            public const string BEST_HTTP_NOT_AVALIABLE = "The plugin - Best Http v2 - is not avaliable. Install it and add the macro definition \"BEST_HTTP\" to Player Settings.";
        }

        /// <summary>
        /// 路径定义
        /// </summary>
        public class Path
        {
            public const string RES_PREFAB = "Prefabs/";
            public const string RES_PREFAB_UI = "Prefabs/UI/";
            public const string RES_EFFECT = "Eff/";
            public const string RES_SPRITE = "Sprites/";
            public const string RES_GUID = "Prefabs/UI/Guid/";
            public const string RES_SOUNDS = "Sounds/";

            public const string CSV_HOTKEY = "CSV/Hotkey.csv";
        }

        /// <summary>
        /// Url定义
        /// </summary>
        public class Url
        {

        }

        /// <summary>
        /// 名称定义
        /// </summary>
        public class Name
        {
            //入口场景名
            public static string SCENE_MAIN_NAME = "Main";
            //核心业务场景名
            public static string SCENE_CORE_NAME = "CoreBusiness";
        }

        /// <summary>
        /// 数值定义
        /// </summary>
        public class Val
        {
            public const long MINUTE = 60;
            public const long HOUR = 3600;
            public const long DAY = 86400;
        }

        /// <summary>
        /// 广告Placement定义
        /// </summary>
        public class AD
        {

        }

        /// <summary>
        /// Facebook打点事件
        /// </summary>
        public class FBEvent
        {
#if UNITY_ANDROID

#elif UNITY_IOS

#endif
        }

        /// <summary>
        /// Adjust打点事件
        /// </summary>
        public class AdjustEvent
        {
#if UNITY_ANDROID

#elif UNITY_IOS

#endif
        }

        /// <summary>
        /// Firebase打点事件
        /// </summary>
        public class FirebaseEvent
        {

        }

        /// <summary>
        /// Firebase打点事件参数名称
        /// </summary>
        public class FirebaseParamName
        {

        }

        /// <summary>
        /// Firebase云控参数名称
        /// </summary>
        public class RemoteConfig
        {
#if UNITY_ANDROID

#elif UNITY_IOS

#endif
        }
    }
}
