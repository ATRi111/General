using UnityEngine.Events;

namespace Services.Save
{
    public interface ISaveManager : IService
    {
        /// <summary>
        /// 是否需要读档；许多对象会在发出读档请求之后才会生成，所以这些对象通过NeedLoad属性判断是否要读档
        /// </summary>
        bool NeedLoad { get; set; }
        WholeSaveData RuntimeData { get; }
        /// <summary>
        /// 发出读档请求后，一些对象从RuntimeData读取数据(读取的是当前的RuntimeData,如果要直接读取硬盘上的数据，需要先调用Read)
        /// </summary>
        UnityEvent SaveRequest { get; }
        /// <summary>
        /// 发出存档请求后，一些对象将自己的数据传给RuntimeData(只是修改RuntimeData，如果要写到硬盘上，需要再调用Save)
        /// </summary>
        UnityEvent LoadRequest { get; }

        void Read(string savePath);
        void Write(string savePath);
    }
}