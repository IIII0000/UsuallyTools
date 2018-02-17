using System;
using System.Collections.Generic;
namespace Tools.Helper
{
    public delegate Tkey SelectHandle<T, Tkey>(T t);
    public delegate bool FindHandle<T>(T t);
    /// <summary>
    /// 数组助手类
    /// 排序:升序/降序
    /// 获取:最大/小值，
    /// 查找:单个/所有满足条件的对象
    /// 选择:数组中的某类成员形成单独的对象
    /// </summary>
    public static class ArrayHelper
    {
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <typeparam name="Tkey">数据类型属性</typeparam>
        /// <param name="array">数据类型的对象数组</param>
        /// <param name="handler">选择委托</param>
        /// <param name="isReverse">是否反转，默认升序排列</param>
        public static void OrderBy<T, Tkey>(T[] array, SelectHandle<T, Tkey> handler, bool isReverse = false) where Tkey : IComparable<Tkey>
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (handler(array[i]).CompareTo(handler(array[j])) > 0)
                    {
                        var temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }
            if (isReverse)
                Array.Reverse(array);
        }
        /// <summary>
        /// 获取数组最大最小值对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <typeparam name="Tkey">数据类型属性</typeparam>
        /// <param name="array">数据类型对象数组</param>
        /// <param name="handler">选择委托</param>
        /// <param name="isMin">是否最小值，默认最大值</param>
        /// <returns></returns>
        public static T GetBy<T, Tkey>(T[] array, SelectHandle<T, Tkey> handler, bool isMin = false) where Tkey : IComparable<Tkey>
        {
            T max = array[0];
            int parable = isMin ? 1 : -1;
            for (int i = 1; i < array.Length; i++)
            {
                if (handler(max).CompareTo(handler(array[i])) == parable)
                {
                    max = array[i];
                }
            }
            return max;
        }
        /// <summary>
        /// 查找单个对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数据类型的对象数组</param>
        /// <param name="handler">查找委托</param>
        /// <returns></returns>
        public static T Find<T>(T[] array, FindHandle<T> handler)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (handler(array[i]))
                {
                    return array[i];
                }
            }
            return default(T);
        }
        /// <summary>
        /// 查找满足条件所有对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数据类型对象数组</param>
        /// <param name="handler">查找委托</param>
        /// <returns></returns>
        public static T[] FindAll<T>(T[] array, FindHandle<T> handler)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < array.Length; i++)
            {
                if (handler(array[i]))
                {
                    list.Add(array[i]);
                }
            }
            return list.ToArray();
        }
        /// <summary>
        /// 选择数组中的某类成员形成单独的数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <typeparam name="Tkey">数据类型的某个成员类型</typeparam>
        /// <param name="array">数据类型的对象数组</param>
        /// <param name="handler">选择委托</param>
        /// <returns></returns>
        public static Tkey[] Select<T, Tkey>(T[] array, SelectHandle<T, Tkey> handler)
        {
            Tkey[] arr = new Tkey[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                arr[i] = handler(array[i]);
            }
            return arr;
        }
    }
}
