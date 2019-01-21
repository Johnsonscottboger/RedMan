using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedMan.Extensions
{
    /// <summary>
    /// 定义<see cref="DateTime"/>实例的扩展方法
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime - start).TotalSeconds);
        }
    }
}
