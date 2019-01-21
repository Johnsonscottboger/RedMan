using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedMan.Extensions
{
    /// <summary>
    /// 定义<see cref="object"/>实例的扩展方法
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 对指定实例运用指定的方法
        /// </summary>
        /// <typeparam name="T">指定的实例类型</typeparam>
        /// <param name="obj">指定的实例对象</param>
        /// <param name="block">指定对实例运用的方法</param>
        /// <returns>调用的实例</returns>
        public static T Apply<T>(this T obj, Action<T> block)
        {
            block?.Invoke(obj);
            return obj;
        }
    }
}
