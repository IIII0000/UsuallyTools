using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tools.Helper
{
    /// <summary>
    /// 对象池:提供创建/回收/清除的功能
    /// </summary>
    public class ObjectPool : MonoSingleton<ObjectPool>
    {
        private Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();
        private ObjectPool() { }
        /// <summary>
        /// 创建对象放入池中
        /// </summary>
        /// <param name="name">预制对象名</param>
        /// <param name="go">预制物体</param>
        /// <param name="pos">位置</param>
        /// <param name="rota">旋转</param>
        /// <returns>池中引用</returns>
        public GameObject CreateObject(string name, GameObject go, Vector3 pos, Quaternion rota)
        {
            var temp = FindUsable(name);
            if (temp != null)
            {
                temp.transform.position = pos;
                temp.transform.rotation = rota;
                temp.SetActive(true);
            }
            else
            {
                temp = Instantiate(go, pos, rota) as GameObject;
                Add(name, temp);
            }
            temp.transform.parent = transform;
            return temp;
        }
        private void Add(string name, GameObject temp)
        {
            if (!pool.ContainsKey(name))
            {
                pool.Add(name, new List<GameObject>());
            }
            pool[name].Add(temp);
        }
        private GameObject FindUsable(string name)
        {
            if (!pool.ContainsKey(name)) return null;
            return pool[name].Find(go => !go.activeSelf);
        }
        /// <summary>
        /// 清除池中单个对象列表
        /// </summary>
        /// <param name="name">对象名</param>
        public void Clear(string name)
        {
            if (pool.ContainsKey(name))
            {
                for (int i = 0; i < pool[name].Count; i++)
                {
                    Destroy(pool[name][i]);
                }
                pool.Remove(name);
            }
        }
        /// <summary>
        /// 清空池中所有对象
        /// </summary>
        public void ClearAll()
        {
            var keys = new List<string>(pool.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                Clear(keys[i]);
            }
        }
        /// <summary>
        /// 回收对象到池中
        /// 提供延时回收和实时回收，默认实时回收
        /// </summary>
        /// <param name="go">需要回收的对象</param>
        /// <param name="delay">延迟时间</param>
        public void CollectObject(GameObject go)
        {
            go.SetActive(false);
        }
        public void CollectObject(GameObject go, float delay = 0)
        {
            StartCoroutine(DelayCollect(go, delay));
        }
        private IEnumerator DelayCollect(GameObject go, float delay)
        {
            yield return new WaitForSeconds(delay);
            go.SetActive(false);
        }
    }
}