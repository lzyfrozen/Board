using Newtonsoft.Json;
using System;
using System.Data;
using System.Text;

namespace Board.ToolKits
{
    public static class JsonExtend
    {
        /// <summary>
        /// Json字符串转换为对象
        /// </summary>
        /// <param name="jsonString">Josn字符串</param>
        /// <returns>T</returns>
        public static T? ToObject<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static string DatableToJson(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                //如果查询到的数据为空则返回标记ok:false
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();

            sb.Append("[");
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("{");
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    sb.AppendFormat("\"{0}\":\"{1}\",", dr.Table.Columns[i].ColumnName.Replace("\"", "\\\"").Replace("\'", "\\\'"), ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                }
                sb.Remove(sb.ToString().LastIndexOf(','), 1);
                sb.Append("},");
            }

            sb.Remove(sb.ToString().LastIndexOf(','), 1);
            sb.Append("],");
            return sb.ToString();
        }

        /// <summary>
        /// 将object转换成为string
        /// </summary>
        /// <param name="ob">obj对象</param>
        /// <returns></returns>
        public static string? ObjToStr(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }
    }
}
