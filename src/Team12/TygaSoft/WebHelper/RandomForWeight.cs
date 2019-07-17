using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.WebHelper
{
    /// <summary>
    /// 权重概率：百分比概率控制
    /// </summary>
    public class RandomForWeight
    {
        GLBfb[] _arrGLbfb = null;
        public RandomForWeight(GLBfb[] arrGLbfb)
        {
            _arrGLbfb = arrGLbfb;
            BubbleSortDX(arrGLbfb);
            InitGLBfb(arrGLbfb);
        }

        /// <summary>
        /// 获取按权重概率计算的随机号
        /// </summary>
        /// <returns></returns>
        public string GetGLNumber()
        {
            string rt = "";

            try
            {
                int seed = GetRandomSeed();
                Random rd = new Random(seed);
                int rdNum = rd.Next(1, 101);

                int index = GetGlbfbIndex(_arrGLbfb, rdNum);
                if (_arrGLbfb[index].SjsList.Count <= 1)
                {
                    rt = _arrGLbfb[index].SjsList[0];
                }
                else
                {
                    int seed2 = GetRandomSeed();
                    Random rd2 = new Random(seed2);
                    int start = 0;
                    int end = _arrGLbfb[index].SjsList.Count;
                    int rdN = rd2.Next(start, end);

                    rt = _arrGLbfb[index].SjsList[rdN];

                }
            }
            catch (Exception ex)
            {
                rt = "";

            }

            return rt;
        }

        //生成随机种子
        private int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// 从小到大排序
        /// </summary>
        /// <param name="arrGLbfb"></param>
        private void BubbleSort(GLBfb[] arrGLbfb)
        {
            int i, j; //交换标志 \
            GLBfb temp = new GLBfb();
            bool exchange;
            for (i = 0; i < arrGLbfb.Length; i++) //最多做R.Length-1趟排序 
            {
                exchange = false; //本趟排序开始前，交换标志应为假
                for (j = arrGLbfb.Length - 2; j >= i; j--)
                {
                    if (arrGLbfb[j + 1].Bfb < arrGLbfb[j].Bfb)　//交换条件
                    {
                        temp = arrGLbfb[j + 1];
                        arrGLbfb[j + 1] = arrGLbfb[j];
                        arrGLbfb[j] = temp;
                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }
                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 从大到小排序
        /// </summary>
        /// <param name="arrGLbfb"></param>
        private void BubbleSortDX(GLBfb[] arrGLbfb)
        {
            int i, j; //交换标志 \
            GLBfb temp = new GLBfb();
            bool exchange;
            for (i = 0; i < arrGLbfb.Length; i++) //最多做R.Length-1趟排序 
            {
                exchange = false; //本趟排序开始前，交换标志应为假
                for (j = arrGLbfb.Length - 2; j >= i; j--)
                {
                    if (arrGLbfb[j + 1].Bfb > arrGLbfb[j].Bfb)　//交换条件
                    {
                        temp = arrGLbfb[j];
                        arrGLbfb[j] = arrGLbfb[j + 1];
                        arrGLbfb[j + 1] = temp;
                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }
                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 初始化GLBfb数组，初始化好各个摇奖概率
        /// </summary>
        /// <param name="arrGLbfb"></param>
        private void InitGLBfb(GLBfb[] arrGLbfb)
        {
            int tp;

            arrGLbfb[0].StartNumber = 100 - arrGLbfb[0].Bfb + 1;
            arrGLbfb[0].EndNumber = 100;

            tp = arrGLbfb[0].StartNumber - 1;

            for (int i = 1; i < arrGLbfb.Length; i++)
            {
                arrGLbfb[i].StartNumber = tp - arrGLbfb[i].Bfb + 1;
                arrGLbfb[i].EndNumber = tp;

                tp = arrGLbfb[i].StartNumber - 1;
            }
        }

        /// <summary>
        /// 根据随机数获取概率区间的索引
        /// </summary>
        /// <param name="arrGLbfb"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private int GetGlbfbIndex(GLBfb[] arrGLbfb, int key)
        {
            int index = 1;

            for (int i = 0; i < arrGLbfb.Length; i++)
            {
                if (key >= arrGLbfb[i].StartNumber && key <= arrGLbfb[i].EndNumber)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

    }
}
