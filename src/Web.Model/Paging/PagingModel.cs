using RedMan.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedMan.Model.Paging
{
    public class PagingModel<T>
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        public List<T> ModelList { get; set; }

        /// <summary>
        /// 分页信息
        /// </summary>
        public PagingInfo PagingInfo { get; set; }
    }
}
