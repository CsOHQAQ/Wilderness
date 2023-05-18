using System;

namespace SaveFramework
{
    public interface IMainDataManager
    {
        /// <summary>
        /// 加载存档
        /// </summary>
        /// <returns>是否加载成功</returns>
        bool LoadFrom();
        /// <summary>
        /// 保存存档
        /// </summary>
        void SaveTo();
    }
}