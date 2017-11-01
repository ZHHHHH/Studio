using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 传智Studio.Common
{
    public class PageBarHelper
    {
        //分页
        public static string GetPagaBar(int pageIndex, int pageCount)
        {
            if (pageCount == 1)
            {
                return string.Empty;
            }
            int start = pageIndex - 5;//计算起始位置.要求页面上显示10个数字页码.
            if (start < 1)
            {
                start = 1;
            }
            int end = start + 9;//计算终止位置.
            if (end > pageCount)
            {
                end = pageCount;
                //重新计算一下Start值.
                start = end - 9 < 1 ? 1 : end - 9;
            }
            StringBuilder sb = new StringBuilder();
            //上一页
            if (pageIndex > 1)
            {
                sb.AppendFormat("<a href='?pageIndex={0}' class='myPageBar'>上一页</a>", pageIndex - 1);
            }
            //遍历10个a标签
            for (int i = start; i <= end; i++)
            {
                if (i == pageIndex)
                {
                    sb.Append(i);
                }
                else
                {
                    sb.AppendFormat("<a href='?pageIndex={0}' class='myPageBar'>{0}</a>", i);
                }
            }
            //下一页
            if (pageIndex < pageCount)
            {
                sb.AppendFormat("<a href='?pageIndex={0}' class='myPageBar'>下一页</a>", pageIndex + 1);
            }

            return sb.ToString();
        }
    }
}
