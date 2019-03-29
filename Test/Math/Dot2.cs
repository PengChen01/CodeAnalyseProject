using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Product2.Test.Math
{
    #region 元素
    public class Dot2
    {
        public Dot2(double x, double y)
        {
            X = x;
            Y = y;
        }
        public Dot2()
        {
            X = 0;
            Y = 0;
        }
        public Dot2(Point point)
        {
            X = point.X;
            Y = point.Y;
        }
        /// <summary>
        /// 横坐标
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// 纵坐标
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// 幅角，弧度制
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public double Argument { 
            get{return System.Math.Atan2(Y,X);} 
            set{
                X = Length * System.Math.Cos(value);
                Y = Length * System.Math.Sin(value); 
            } 
        }
        /// <summary>
        /// 到坐标原点的距离
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public double Length {
            get { return System.Math.Sqrt(X * X + Y * Y); }
            set{
                X = value * System.Math.Cos(Argument);
                Y = value * System.Math.Sin(Argument);
            } 
        }
        public static Dot2 operator +(Dot2 A, Dot2 B)
        {
            return new Dot2(A.X + B.X, A.Y + B.Y);
        }
        public static Dot2 operator -(Dot2 A, Dot2 B)
        {
            return new Dot2(A.X - B.X, A.Y - B.Y);
        }
        public static double operator *(Dot2 A, Dot2 B)
        {
            return A.X*B.X+A.Y*B.Y;
        }
        public Point toPoint()
        {
            return new Point(X, Y);
        }
        public Dotx toDotx()
        {
            return new Dotx(this);
        }
        public Dotx toDotx(DrawType type)
        {
            return new Dotx(this,type);
        }
    }
    class Vector
    {
        /// <summary>
        /// 创建单位向量
        /// </summary>
        public Vector()
        {
            From = new Dot2(0, 0);
            To = new Dot2(1, 0);
        }
        /// <summary>
        /// 根据两个点生成向量
        /// </summary>
        public Vector(Dot2 from,Dot2 to)
        {
            From = from;
            To = to;
        }
        /// <summary>
        /// 将一个点转为矢径
        /// </summary>
        /// <param name="D"></param>
        public Vector(Dot2 D)
        {
            From = new Dot2(0, 0);
            To = D;
        }
        public Dot2 From{get;set;}
        public Dot2 To { get; set; }
        public double X
        {
            get
            {
                return To.X - From.X;
            }
        }
        public double Y
        {
            get
            {
                return To.Y - From.Y;
            }
        }
        /// <summary>
        /// 获取向量的长度
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Length(From,To);
            }
        }
        /// <summary>
        /// 获取向量的角度
        /// </summary>
        public double Angle
        {
            get
            {
                return System.Math.Atan2(To.Y - From.Y, To.X - From.X);
            }
        }
    }
    #endregion

    #region 图集合
   public enum Type
    {
        Append,
        Insert,
        Delete,
        Read,
        Modify
    }
    /// <summary>
    /// 记录操作类型
    /// </summary>
   public class Operate
    {
        public Type type;
        public int index;
        //[System.Xml.Serialization.XmlIgnore]
        public Dot2 dot;
        public Operate(Type type, Dot2 dot, int index)
        {
            this.type = type;
            this.dot = dot;
            this.index = index;
        }
        public Operate()
        {

        }
    }
   public enum DrawType
   {
       normal,
       Emphasize,
       Error,
       Ignore,
       Info
   }
   public class Dotx
   {
       /// <summary>
       /// 点类型
       /// </summary>
       public DrawType type{get;set;}

       /// <summary>
       /// 横坐标
       /// </summary>
       public double X { get; set; }
       /// <summary>
       /// 纵坐标
       /// </summary>
       public double Y { get; set; }
       public Dot2 toDot2()
       {
           return new Dot2(this.X, this.Y);
       }
       public Dotx(Dot2 dot,DrawType type)
       {
           this.X = dot.X;
           this.Y = dot.Y;
           this.type = type;
       }
       public Dotx(Dot2 dot)
       {
           this.X = dot.X;
           this.Y = dot.Y;
           this.type = DrawType.normal;
       }
       public Dotx()
       {

       }
   }
    class Border
    {
        List<Dotx> Dots = new List<Dotx>();
        List<Operate> Operate = new List<Operate>();
        #region 边界范围
        public List<Operate> getOperateLog()
        {
            return this.Operate;
        }
        public Operate fetchLog()
        {
            return this.Operate[this.Operate.Count - 1];
        }
        public int DotCount
        {
            get
            {
                return Dots.Count;
            }
        }
        /// <summary>
        /// 返回边界的第i个点，最后一个点与初始点连接
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        //public Dot2 get(int index)
        //{
        //    //Operate.Add(new Operate(Type.Read, Dots[index], index));
        //    if (index ==Dots.Count)
        //    {
        //        return Dots[0];
        //    }
        //    else
        //    {
        //        return Dots[index];
        //    }
        //}

        public Dot2 get(int index)
        {
            index +=Dots.Count;
            return Dots[index %= Dots.Count].toDot2();
        }
        public DrawType getDotType(int index)
        {
            index += Dots.Count;
            return Dots[index %= Dots.Count].type;
        }
        public void put(Dot2 dot)
        {
            Math.putMinLengthDotToBorder(dot, this);
            //Math.putDotToBorder(dot, this);
            //getNearestVector
        }
        public void Insert(Dot2 dot, int index)
        {
            if (index==Dots.Count)
            {
                Add(dot);
            }
            else
            {
                Operate.Add(new Operate(Type.Insert, dot, index));
                Dots.Insert(index, dot.toDotx());
            }
        }
        public void Insert(Dot2 dot, int index,DrawType type)
        {
            if (index == Dots.Count)
            {
                Operate.Add(new Operate(Type.Append, dot, index));
                Dots.Add(dot.toDotx(type));
            }
            else
            {
                Operate.Add(new Operate(Type.Insert, dot, index));
                Dots.Insert(index, dot.toDotx(type));
            }
        }
        public void Add(Dot2 dot)
        {
            Operate.Add(new Operate(Type.Append, dot, Dots.Count));
            Dots.Add(dot.toDotx());
        }
        public void Modify(Dot2 dot, int index)
        {
            Dots[index].X = dot.X;
            Dots[index].Y = dot.Y;
            Operate.Add(new Operate(Type.Modify, dot, index));
        }
        public void Delete(int index)
        {
            Operate.Add(new Operate(Type.Delete,get(index), index));
            Dots.RemoveAt(index);
        }
        /// <summary>
        /// 获取边界向量最后一个向量为初始点和结束点相连
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector getVecter(int index)
        {
            return new Vector(this.get(index), this.get(index + 1));
        }
        #endregion
        public Border(List<Dot2> dots)
        {
            Dot2 indot;
            bool isIntersect=false;
            for (int i = 0; i < dots.Count; i++)
            {
                if (i >= 3)
                {
                    for (int j = 0; j < i - 2; j++)
                    {
                        if (Math.isIntersect(new Vector(dots[i], dots[i - 1]), new Vector(dots[j], dots[j + 1]), out indot))
                        {
                            if (indot != null)
                            {
                                //draw.Lines(new List<Dot2>() { dot, Dots[i - 1], Dots[j], Dots[j + 1] });
                                //dotss.Add(indot);
                            }
                            isIntersect = true;
                            break;
                        }
                    }
                    if (isIntersect == true)
                    {
                        isIntersect = false;
                        continue;
                    }
                    else
                    {
                        Add(dots[i]);
                    }
                }
                else
                {
                    Add(dots[i]);
                }
            }
        }


        /// <summary>
        /// 随机产生一个无交叉的边界
        /// </summary>
        /// <param name="number">3</param>
        public Border(int number,int maxWidth,int maxHeight ,out  List<Dot2> dotss,Draw.Draw draw)
        {
            dotss = new List<Dot2>();
            bool isIntersect = false;
            Random rand = new Random();
            Dot2 indot=null;
            Dot2 dot = Math.CreateRandomDot(rand, maxWidth, maxHeight);
            int i=0;
            Dot2 tempdot;
            while (Dots.Count<number)
            {
                tempdot = Math.CreateRandomDot(rand, dot, 100, maxWidth, maxHeight);
                if (i<=2)
                {
                    Add(tempdot);
                    dot = tempdot;
                     i++;
                }
                else
                {
                    for (int j = 0; j < i - 2; j++)
                    {


                        if (Math.isIntersect(new Vector(tempdot, get(i - 1)), new Vector(get(j), get(j + 1)), out indot))
                        {
                            if (indot != null)
                            {
                                //draw.Lines(new List<Dot2>() { dot, Dots[i - 1], Dots[j], Dots[j + 1] });
                                dotss.Add(indot);
                            }
                            isIntersect = true;
                            break;
                        }
                    }
                    if (isIntersect == true)
                    {
                        isIntersect = false;
                        continue;
                    }
                    else
                    {
                        Add(tempdot);
                        dot = tempdot;
                        i++;
                    }
                }
            }
        }

        public Border(int number, int maxWidth, int maxHeight) 
        {
            Random rand = new Random();
            Dot2 dot;
                //tempdot = Math.CreateRandomDot(rand, dot, 100, maxWidth, maxHeight);
            for (int i = 0; i < number; i++)
            {
                dot = Math.CreateRandomDot(rand, maxWidth, maxHeight);
                this.put(dot);
            }
        }
    }
    class VNGraph
    {
        public List<Dot2> Dots;
        public List<Vector> Vecs;
        public void putDot(Dot2 dot)
        {
            if (Dots.Contains(dot))
            {
                return;
            }
            Dots.Add(dot);
        }
        /// <summary>
        /// 通过VN图边界实例化一个VN图
        /// </summary>
        /// <param name="border"></param>
        public VNGraph(List<Dot2> border)
        {
            
        }
    }
    #endregion
    /// <summary>
    /// 自定义数据处理
    /// </summary>
    class Math
    {
        /// <summary>
        /// 获取两个点的长度
        /// </summary>
        /// <returns></returns>
        public static double Length(Dot2 D1,Dot2 D2)
        {
            return System.Math.Sqrt(System.Math.Pow((D1.X - D2.X), 2) + System.Math.Pow((D1.Y - D2.Y), 2));
        }
        /// <summary>
        /// 产生一个随机点
        /// </summary>
        /// <param name="number"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public static Dot2 CreateRandomDot(Random rand, int maxWidth, int maxHeight)
        {
            return new Dot2(rand.NextDouble() * maxWidth, rand.NextDouble() * maxHeight);
        }
        public static Dot2 CreateRandomDot(Random rand,Dot2 dot, double Radius,int maxWidth, int maxHeight)
        {
            Dot2 returndot;
            //Dot2 nulls;
            double radius = Radius + Radius * 1 * (rand.NextDouble() - 0.5);
            //Vector top = new Vector(new Dot2(maxWidth,0));
            //Vector left = new Vector(new Dot2(0,maxHeight));
            //Vector buttom = new Vector(new Dot2(0,maxHeight),new Dot2(maxWidth,maxHeight));
            //Vector right = new Vector(new Dot2(maxWidth,0),new Dot2(maxWidth,maxHeight));
            Border border = new Border(new List<Dot2>() { new Dot2(0, 0), new Dot2(maxWidth, 0), new Dot2(maxWidth, maxHeight), new Dot2(0, maxHeight) });
            double theta = (rand.NextDouble() - 0.5) * 2 * System.Math.PI;
            returndot = dot + new Dot2(radius * System.Math.Cos(theta), radius * System.Math.Sin(theta));
            while (!Math.isInside(returndot, border)
                //isIntersect(new Vector(dot, returndot), top, out nulls) || isIntersect(new Vector(dot, returndot), left, out nulls) || isIntersect(new Vector(dot, returndot), buttom, out nulls) || isIntersect(new Vector(dot, returndot), right, out nulls)
                )
            {
                radius = Radius + Radius * 1 * (rand.NextDouble() - 0.5);
                theta = (rand.NextDouble() - 0.5) *2* System.Math.PI ;
                returndot = dot + new Dot2(radius * System.Math.Cos(theta), radius * System.Math.Sin(theta));
            }
            return returndot;
        }
        public static List<Dot2> CreateRandomDots(int number ,int maxWidth,int maxHeight)
        {
            List<Dot2> dots = new List<Dot2>(number);
            Random rand = new Random();
            for (int i = 0; i < number; i++)
			{
                dots.Add(new Dot2(rand.NextDouble() * maxWidth, rand.NextDouble() * maxHeight));
			}
            return dots;
        }
        public List<Dot2> Transport(List<Dot2> DotSet, Vector vec)
        {
            for (int i = 0; i < DotSet.Count; i++)
            {
                DotSet[i].X = DotSet[i].X + vec.To.X - vec.From.X;
                DotSet[i].Y = DotSet[i].Y + vec.To.Y - vec.From.Y;
            }
            return DotSet;
        }
        /// <summary>
        /// 返回离点集最近的点
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="DotSet"></param>
        /// <returns></returns>
        public static Dot2 getNearestDot(Dot2 origin, List<Dot2> DotSet)
        {
            if (DotSet.Count == 0)
            {
                return null;
            }
            int j = 0;
            double lengh = Math.Length(origin, DotSet[j]), temp;
            for (int i = 1; i < DotSet.Count; i++)
            {
                temp = Math.Length(origin, DotSet[i]);
                if (temp < lengh)
                {
                    lengh = temp;
                    j = i;
                }
            }
            return DotSet[j];
        }
        /// <summary>
        /// 返回离点集最远的点
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="DotSet"></param>
        /// <returns></returns>
        public static Dot2 getFastestDot(Dot2 origin, List<Dot2> DotSet)
        {
            int j = 0;
            double lengh = 0, temp;
            for (int i = 0; i < DotSet.Count; i++)
            {
                temp = Math.Length(origin, DotSet[i]);
                if (temp > lengh)
                {
                    lengh = temp;
                    j = i;
                }
            }
            return DotSet[j];
        }
        /// <summary>
        /// 获取离边界最近的一条边（边可以不再边界上）
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="border"></param>
        /// <returns></returns>
        public static Vector getNearestVector(Dot2 origin,Border border)
        {
            if (border.DotCount < 2)
            {
                return null;
            }
            int j1 = 0,j2 = 1;
            double lengh1 = Math.Length(origin, border.get(j1)),lengh2 = Math.Length(origin, border.get(j2)), temp;
            if (lengh1>lengh2)
            {
                lengh1 = lengh1 + lengh2;
                lengh2 = lengh1 - lengh2;
                lengh1 = lengh1 - lengh2;
            }
            for (int i = 1; i < border.DotCount; i++)
            {
                temp = Math.Length(origin, border.get(i));
                if (temp <= lengh1)
                {
                    lengh2 = lengh1;
                    lengh1 = temp;
                    j2 = j1;
                    j1 = i;
                }
                else if (temp > lengh1 && temp <= lengh2)
                {
                    lengh2 = temp;
                    j2 = i;
                }
            }
            return new Vector(border.get(j1),border.get(j2));
        }
        /// <summary>
        /// 将增加长度最小的点插入
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="border"></param>
        public static void putMinLengthDotToBorder(Dot2 origin, Border border)
        {
            double temp, lengh = double.MaxValue;
            if (border.DotCount <= 2)
            {
                border.Add(origin);
                return;
            }
            int j=0;
            for (int i = 0; i < border.DotCount; i++)
            {
                temp = Math.Length(origin, border.get(i)) + Math.Length(origin, border.get(i + 1)) - border.getVecter(i).Length;
                if (temp < lengh)
                {
                    lengh = temp;
                    j = i;
                }
            }
            border.Insert(origin, j + 1, DrawType.Ignore);
        }
        /// <summary>
        /// 将一个点插入到边界中，形成凸面体
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="border"></param>
        /// <returns></returns>
        public static void putDotToBorder(Dot2 origin, Border border)
        {
            Dot2 dot;
            int i,j = 0, count = border.DotCount, tempj;
            if (count <= 2)
            {
                border.Add(origin);
                return;
            }
            double temp,lengh = double.MaxValue;

            int isIntersects = 0;
            for (i = 0; i < count; i++)
            {
                temp = Math.Length(origin, border.get(i));
                if (temp < lengh)
                {
                    lengh = temp;
                    j = i;
                }
            }

            //判断是否相交
            tempj = count + j;
            for (i = 0; i < count; i++)
            {
                if (i == tempj % count || i == (tempj - 1) % count)
                {
                    continue;
                }
                if (isIntersect(new Vector(origin, border.get(tempj)), border.getVecter(i), out dot))
                {
                    isIntersects = 1;
                    break;
                }
            }
            if (isIntersects == 1)
            {
                isIntersects = 4;
                lengh = double.MaxValue;
                //采用面积最小方式
                for (i = 0; i < border.DotCount; i++)
                {
                    temp = Math.Area(origin, border.get(i), border.get((i + 1)));
                    if (temp < lengh)
                    {
                        lengh = temp;
                        j = i;
                    }
                }
                //判断是否相交,若相交，取最近的相交的线段插入
                temp = double.MaxValue;
                tempj = count + j;
                for ( i= 0; i < count; i++)
                {
                    if (i == tempj % count || i == (tempj - 1) % count)
                    {
                        continue;
                    }
                    if (isIntersect(new Vector(origin, border.get(tempj)), border.getVecter(i), out dot))
                    {
                        temp = Math.Length(origin, dot);
                        if (temp < lengh)
                        {
                            isIntersects = 5;
                            lengh = temp;
                            j = i;
                        }
                    }
                }
                //if (isIntersects == 5)
                //{
                    
                //}
                //tempj = count + i+1;
                //for (i = 0; i < count; i++)
                //{
                //    if (i == tempj % count || i == (tempj - 1) % count)
                //    {
                //        continue;
                //    }
                //    if (isIntersect(new Vector(origin, border.get(tempj)), border.getVecter(i), out dot))
                //    {
                //        isIntersects = 5;
                //        break;
                //    }
                //}

            }
            else
            {
                tempj = count + j - 1;
                for (i = 0; i < count; i++)
                {
                    if (i == tempj % count || i == (tempj - 1) % count)
                    {
                        continue;
                    }
                    if (isIntersect(new Vector(origin, border.get(tempj)), border.getVecter(i), out dot))
                    {
                        isIntersects = 1;
                        break;
                    }
                }
                tempj = count + j + 1;
                for ( i = 0; i < count; i++)
                {
                    if (i == tempj % count || i == (tempj - 1) % count)
                    {
                        continue;
                    }
                    if (isIntersect(new Vector(origin, border.get(tempj)), border.getVecter(i), out dot))
                    {
                        if (isIntersects == 1)
                        {
                            isIntersects = 2;
                        }
                        else
                        {
                            isIntersects = 3;
                        }
                        break;
                    }
                }
            }

            
            switch (isIntersects)
            {
                case 0://两线段都无交点
                    if (Math.Length(origin, border.get(j - 1)) + border.getVecter(j).Length > Math.Length(origin, border.get(j + 1)) + border.getVecter(j-1).Length)
                    {
                        border.Insert(origin, j+1, DrawType.normal);
                    }
                    else
                    {
                        border.Insert(origin, j, DrawType.normal);
                    }
                    break;
                case 1://仅左线段相交
                    border.Insert(origin, j + 1, DrawType.Ignore);
                    break;
                case 2://左右线段都相交
                    break;
                case 3://仅右线段相交
                    border.Insert(origin, j, DrawType.Ignore);
                    break;
                case 4://采用面积最小方式,无相交
                    border.Insert(origin, j+1, DrawType.Ignore);
                    break;
                case 5://采用面积最小方式,有相交,插入相交线段
                    border.Insert(origin, j+1 , DrawType.Ignore);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 判断两条边是否相交
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// /// <param name="des">双方容差距离</param>
        /// <returns></returns>
        public static bool isIntersect(Vector A, Vector B, out Dot2 interdot,double des = 10)
        {
            interdot = null;
            //第一步：盒子判断
            double amin, amax, bmin, bmax;
            if (A.From.X > A.To.X)
            {
                amin = A.To.X;
                amax = A.From.X;
            }
            else
            {
                amin = A.From.X;
                amax = A.To.X;
            }
            if (B.From.X > B.To.X)
            {
                bmin = B.To.X;
                bmax = B.From.X;
            }
            else
            {
                bmin = B.From.X;
                bmax = B.To.X;
            }
            if (amax - bmin < des || amin - bmax > des)
            {
                return false;
            }

            if (A.From.Y > A.To.Y)
            {
                amin = A.To.Y;
                amax = A.From.Y;
            }
            else
            {
                amin = A.From.Y;
                amax = A.To.Y;
            }
            if (B.From.Y > B.To.Y)
            {
                bmin = B.To.Y;
                bmax = B.From.Y;
            }
            else
            {
                bmin = B.From.Y;
                bmax = B.To.Y;
            }
            if (amax - bmin < des || amin - bmax > des)
            {
                return false;
            }
            //第二步同向判断
            if (cross(A,new Vector(A.From,B.From))*cross(A,new Vector(A.From,B.To))>0)
            {
                return false;
            }

            //第三步位置判断
            interdot = new Dot2(((A.To.Y * A.From.X - A.To.X * A.From.Y) * (B.To.X - B.From.X) - (B.To.Y * B.From.X - B.To.X * B.From.Y) * (A.To.X - A.From.X)) / ((B.To.X - B.From.X) * (A.To.Y - A.From.Y) - (B.To.Y - B.From.Y) * (A.To.X - A.From.X)), 
                -((B.To.Y * B.From.X - B.To.X * B.From.Y) * (A.To.Y - A.From.Y) - (B.To.Y - B.From.Y) * (A.To.Y * A.From.X - A.To.X * A.From.Y)) / ((B.To.X - B.From.X) * (A.To.Y - A.From.Y) - (B.To.Y - B.From.Y) * (A.To.X - A.From.X)));
           
            if (dot(new Vector(interdot,A.From),new Vector(interdot,A.To))>0)
            {
                if (Length(interdot,A.From)>des ||Length(interdot,A.To)>des)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 判断一个线段是否与闭合边界相交
        /// </summary>
        /// <param name="A"></param>
        /// <param name="border"></param>
        /// <returns></returns>
        public static bool isIntersect(Vector A, Border border)
        {
            Dot2 dot;
            for (int i = 0; i < border.DotCount; i++)
            {
                if (isIntersect(A, border.getVecter(i), out dot))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 两个向量叉乘的数值
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static double cross(Vector A,Vector B)
        {
            return A.X*B.Y-A.Y*B.X;
        }
        /// <summary>
        /// 两个向量点乘
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static double dot(Vector A, Vector B)
        {
            return A.X * B.X + A.Y * B.Y;
        }
        /// <summary>
        /// 判断点是否在边界内
        /// </summary>
        /// <param name="dot"></param>
        /// <param name="border"></param>
        /// <returns></returns>
        public static bool isInside(Dot2 dot, Border border)
        {
            int num = 0;
            //double minx, maxx;
            for (int i = 0; i < border.DotCount-1; i++)
            {
                if (dot.X <=(System.Math.Max( border.get(i).X,border.get(i+1).X))&&(border.get(i).Y-dot.Y)*(border.get(i+1).Y-dot.Y)<0)
                {
                    if (dot.X > System.Math.Min(border.get(i).X, border.get(i + 1).X))
                    {
                        if ((border.get(i).X*border.get(i+1).Y-border.get(i).Y*border.get(i+1).X+dot.Y*(border.get(i+1).X-border.get(i).X))/(border.get(i+1).Y-border.get(i).Y)>dot.X)
                        {
                            num++;
                        }
                    }
                    else
                    {
                        num++;
                    }
                }
            }
            if (num%2==1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 计算三角形的面积
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static double Area(Dot2 A, Dot2 B, Dot2 C)
        {
            return 0.5 * System.Math.Abs(A.X * B.Y + B.X * C.Y + A.Y * C.X - A.X * C.Y - A.Y * B.X - B.Y * C.X);
        }
    }
}
