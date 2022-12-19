using Company.Constants;
using Company.NewApp.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Company.NewApp
{
    public class AppManager : UnitySingleton<AppManager>
    {
        public void Init() 
        {

        }

        public bool LoadLevel(int level)
        {
            LevelEntityModel levelEntityModel = DataModelManager.Instance.GetDataModel<LevelEntityModel>();
            if (levelEntityModel.GetLevelEntity(level) != null)
            {
                AppMemento.CurrentLevel = level;
                LoadScene(Defines.Name.SCENE_CORE_NAME);
                return true;
            }
            return false;
        }

        public void LoadScene(string name) 
        {
            SceneManager.LoadScene(name);
        }

        public void ExitApp()
        {
            Application.Quit();
        }
    }
}
