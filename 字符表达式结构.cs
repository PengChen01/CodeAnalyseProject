using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Product2.Code.Calculate
{
    /**<summary>表达式的拆分类型的优先级,其中（）认为是一个最高的优先级算符，作用是可以改变计算的优先级</summary>*/
    public enum Priority
    {
        //var s = 1&&2>3+4*4^3|2
        /// <summary>
        /// 赋值运算，优先级最低，最后运算
        /// </summary>
        Assign,
        /// <summary>
        /// 布尔运算运算可分离（&&,||,!）
        /// </summary>
        Boolean,
        /// <summary>
        ///比较运算符可分离 （>,<,==,>=,<=,）
        /// </summary>
        Compare,

        /// <summary>
        /// (+-)加法和减法，遵循从左到右运算顺序
        /// 如1+（2+3）*4-5,可拆分为子表达式:(+)1，(+)(2+3)*4，(-)5
        /// </summary>
        Sum,
        /// <summary>
        /// (*/%)乘法,除法,取余运算,遵循从左到右运算顺序
        /// 如-2/(2+3)*(-2)/0可拆分为: (-)2,(/)(2+3),(+)(-2)
        /// </summary>
        Multiply,
        /// <summary>
        /// (^)幂运算可分离
        /// 如（1+2）^(2*3)^(-0) 可拆分为 (+)（1+2）,(+)(2*3),(+)(-9)
        /// </summary>
        Power,
        /// <summary>
        /// 按位与或运算可分离（&,|,~）
        /// </summary>
        AndOr,
        /// <summary>
        /// 括号/函数表达式优先级最高（）、XX（）
        /// </summary>
        Bracket
        
    }
    /// <summary>
    /// 前置符号，计算方法
    /// </summary>
    public enum Symbol
    {
        /// <summary>
        /// 正号，加法,无处理
        /// </summary>
        None,
        /// <summary>
        /// 负号,相反数
        /// </summary>
        Opposite,
        /// <summary>
        /// 除法，倒数
        /// </summary>
        Divide,
        /// <summary>
        /// 求余
        /// </summary>
        Remainder,
        /// <summary>
        /// 求正弦值
        /// </summary>
        Sin,
        /// <summary>
        /// 求正弦值
        /// </summary>
        Cos,
        /// <summary>
        /// 对数运算（二元算符）
        /// </summary>
        Log,
        /// <summary>
        /// 赋值运算，第一个参数为输出变量名
        /// </summary>
        Assign
    }
    public enum Type
    {
        /// <summary>
        /// 布尔类型
        /// </summary>
        Bool,
        /// <summary>
        /// 整型
        /// </summary>
        Int,
        /// <summary>
        /// 浮点数
        /// </summary>
        Double,
        /// <summary>
        /// 字符串
        /// </summary>
        String,
        /// <summary>
        /// 向量
        /// </summary>
        Vecter,
        /// <summary>
        /// 矩阵
        /// </summary>
        Matrix
    }
    public class Expression
    {
        /// <summary>
        /// 表达式直接可拆分的类型,优先级,
        /// </summary>
        public Priority priority;
        /// <summary>
        /// 表达式经过拆分类型拆分后的子表达式集
        /// </summary>
        public List<Expression> subExpression;
        /// <summary>
        /// 前置符号
        /// </summary>
        public Symbol symbol;
        /// <summary>
        /// 表达式的计算结果的数据类型
        /// </summary>
        public Type type;
        /// <summary>
        /// 表达式的计算结果的值
        /// </summary>
        public object result;
        public Expression()
        {
            priority = Priority.Assign;
            subExpression = new List<Expression>();
            symbol = Symbol.None;
            type = Type.String;
        }
        public Expression(Priority priority, Symbol symbol, Type type, object result)
        {
            this.priority = priority;
            this.symbol = symbol;
            this.type = type;
            this.result = result;
        }
    }
    class ExpressProcess
    {
        public static Expression getExpress(string expressstr)
        {
            Expression express = new Expression();
            return express;
        }
        
        /// <summary>
        /// 第一层，优先级；第二层，数据类型；第三层，计算方式
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        public static object calculateExpress(Expression express)
        {
            switch (express.priority)
            {
                case Priority.Assign:
                    switch (express.symbol)
                    {
                        case Symbol.None:
                            return express.result;
                        case Symbol.Opposite:
                            switch (express.type)
                            {
                                case Type.Bool:
                                    return !(bool)express.result;
                                case Type.Int:
                                    return -(int)express.result;
                                case Type.Double:
                                    return - Convert.ToDouble(express.result);
                                case Type.String:
                                    break;
                                case Type.Vecter:
                                    break;
                                case Type.Matrix:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case Symbol.Divide:
                            switch (express.type)
                            {
                                case Type.Bool:
                                    //异常
                                    throw(new Exception("布尔类型无法做除法"));
                                case Type.Int:
                                case Type.Double:
                                    if (Convert.ToDouble(express.result) == 0)
                                    {
                                        //异常
                                        throw (new Exception("分母不能为0"));
                                    }
                                    return 1.0/ Convert.ToDouble(express.result);
                                case Type.String:
                                    break;
                                case Type.Vecter:
                                    break;
                                case Type.Matrix:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case Symbol.Remainder://依赖项，无法算出
                            break;
                        case Symbol.Sin:
                            return Math.Sin(Convert.ToDouble(express.result));
                        case Symbol.Cos:
                            return Math.Cos(Convert.ToDouble(express.result));
                        case Symbol.Log:
                            return Math.Log10(Convert.ToDouble(express.result));
                        case Symbol.Assign:
                            break;
                        default:
                            break;
                    }
                    return express.result;
                case Priority.Boolean:
                    break;
                case Priority.Compare:
                    break;
                case Priority.Sum:
                    switch (express.type)
                    {
                        case Type.Bool:
                            break;
                        case Type.Int:
                            int s = 0;
                            for (int i = 0; i < express.subExpression.Count; i++)
                            {
                                s = s + (int)calculateExpress(express.subExpression[i]);
                            }
                            return s;
                        case Type.Double:
                            double dou = 0;
                            for (int i = 0; i < express.subExpression.Count; i++)
                            {
                                dou = dou +  Convert.ToDouble( calculateExpress(express.subExpression[i]));
                            }
                            return dou;
                        case Type.String:
                            string str = "";
                            for (int i = 0; i < express.subExpression.Count; i++)
                            {
                                str = str + calculateExpress(express.subExpression[i]);
                            }
                            return str;
                        case Type.Vecter:
                            break;
                        case Type.Matrix:
                            break;
                        default:
                            break;
                    }
                    for (int i = 0; i < express.subExpression.Count; i++)
                    {

                    }
                    break;
                case Priority.Multiply://乘法，除法，求余处于同一优先级
                    switch (express.type)
                    {
                        case Type.Bool:
                            break;
                        case Type.Int:
                            break;
                        case Type.Double:
                            double s=1,subresult=0;
                            for (int i = 0; i < express.subExpression.Count; i++)
                            {
                                subresult = Convert.ToDouble( calculateExpress(express.subExpression[i]));
                                switch (express.subExpression[i].symbol)
                                {
                                    case Symbol.Remainder:
                                        s = Math.Floor(s / subresult) * subresult;
                                        break;
                                    default:
                                        s = s * subresult;
                                        break;
                                }
                            }
                            return s;
                        case Type.String:
                            break;
                        case Type.Vecter:
                            break;
                        case Type.Matrix:
                            break;
                        default:
                            break;
                    }
                    break;
                case Priority.Power:
                    break;
                case Priority.AndOr:
                    break;
                case Priority.Bracket:
                    
                    switch (express.symbol)
                    {
                        case Symbol.None:
                            break;
                        case Symbol.Opposite:
                            break;
                        case Symbol.Divide:
                            double s = Convert.ToDouble(calculateExpress(express.subExpression[0]));
                            if (s == 0)
                                throw (new Exception("分母不能为0"));
                            else
                                return 1.0 / s;
                        case Symbol.Remainder:
                            break;
                        case Symbol.Sin:
                            return Math.Sin( Convert.ToDouble(calculateExpress(express.subExpression[0])));
                        case Symbol.Cos:
                            return Math.Cos(Convert.ToDouble(calculateExpress(express.subExpression[0])));
                        case Symbol.Log:
                            if (express.subExpression.Count==1)
                                return Math.Log10(Convert.ToDouble(calculateExpress(express.subExpression[0])));
                            else if (express.subExpression.Count==2)
                            {
                                return Math.Log(Convert.ToDouble(calculateExpress(express.subExpression[0])), Convert.ToDouble(calculateExpress(express.subExpression[1])));
                            }
                            else
                            {
                                throw (new Exception("对数函数的参数数量有误"));
                            }
                        case Symbol.Assign:
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return express.result;
        }
    }
}
