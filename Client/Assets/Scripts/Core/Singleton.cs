using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 生成单利对象
/// </summary>
public static class Singleton{

    private static Dictionary<Type, object> singleIns = new Dictionary<Type, object>();
    
    /// <summary>
    /// 从字典表里面获取一个单利实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetInstance<T>() where T : class {
        Type type = typeof(T);
        if(! singleIns.ContainsKey(type)){
            T t = Activator.CreateInstance<T>();
            singleIns.Add(type, t);
        }
        return (T)singleIns[type];
    }
}
