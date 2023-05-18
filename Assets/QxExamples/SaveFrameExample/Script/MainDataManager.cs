using QxFramework.Core;

namespace SaveFramework
{
    public class MainDataManager : LogicModuleBase, IMainDataManager
    {
        private SaveData _saveData;

        public SaveData SaveData
        {
            get
            {
                //因为每次读取时都会重新生成一份数据，之前获取到的数据可能是过时的，获取存档数据的时候请务必从QXData里直接获取
                _saveData = QXData.Instance.Get<SaveData>();
                return _saveData;
            }
        }

        public override void Init()
        {
            base.Init();
            //注册数据
            if (!RegisterData(out _saveData))
            {
                //如果数据不存在，就初始化一份
                InitSaveData();
            }
        }

        private void InitSaveData()
        {
            _saveData = new SaveData();
        }

        private string ReadFromCSV()
        {
            return QXData.Instance.TableAgent.GetString("Test", "1", "Value");
        }

        public bool LoadFrom()
        {
            return QXData.Instance.LoadFromFile("SaveFramework.json");
        }

        public void SaveTo()
        {
            QXData.Instance.SaveToFile("SaveFramework.json");
        }

    }
    //待添加的存档数据
    public class SaveData : GameDataBase
    {
        
    }
}