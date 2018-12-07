using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Product
{
    class ColorProcess
    {
        /// <summary>
        /// 返回指定深浅的一种随机颜色
        /// </summary>
        /// <param name="index">颜色的中心值</param>
        /// <param name="radius">变化的幅度</param>
        /// <returns></returns>
        public static Color getRandColor(byte index,byte radius)
        {
            Random rand = new Random();
            int[] rgb = new int[3];

            int maxrgbindex =rand.Next(3);
            rgb[maxrgbindex]=255;

            int secrgbindex =rand.Next(2);
            int boolindex = getroundnumber(3,maxrgbindex,secrgbindex+1);
            rgb[boolindex]=rand.Next(2)*255;

            int minrgbindex = getroundnumber(3,maxrgbindex,getroundnumber(2,secrgbindex,1)+1);
            rgb[minrgbindex]=0;

            //调用参数信息
            radius = (byte)Math.Abs(radius);
            if (radius>index)
	        {
		         radius = index;
	        }
            rgb[minrgbindex] = (byte)(index + rand.Next(-1 * radius, radius));
            if (rgb[boolindex]==0)
	        {
                rgb[boolindex] = (byte)(index + rand.Next(-1 * radius, radius));
	        }
            Color color = Color.FromArgb(rgb[0], rgb[1], rgb[2]);
            return color;
        }
        /// <summary>
        /// 将数N回归到0~N-1之间
        /// </summary>
        /// <param name="N"></param>
        /// <param name="index"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static int getroundnumber(int N,int index,int offset)
        {
            return (index + offset) % N;
        }
    }
}
