using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Product2.LoginSAP
{
    class AnalyseXML
    {
        SAPLoginWindow aplicationView;
        string content;
        public AnalyseXML(string content, SAPLoginWindow aplicationView)
        {
            this.content = content;
            this.aplicationView = aplicationView;
        }
        public AnalyseXML(string content, int start, int length)
        {
            this.content = content.Substring(start, length);
        }
        public AnalyseXML(string content)
        {
            this.content = content;
        }
        public enum XMLNodeStyle
        {
            /// <summary>
            /// 字母A~Z a~z
            /// </summary>
            A, 
            /// <summary>
            /// 数字0~9
            /// </summary>
            D, 
            /// <summary>
            /// 小数点.
            /// </summary>
            Dot,
            /// <summary>
            /// 单引号'
            /// </summary>
            SingleQuote,
            /// <summary>
            /// 双引号"
            /// </summary>
            DoubleQuote,
            /// <summary>
            /// 斜线/
            /// </summary>
            Bias,
            /// <summary>
            /// 等于=
            /// </summary>
            Equal,
            /// <summary>
            /// 冒号:
            /// </summary>
            Colon,
            /// <summary>
            /// 空格 space
            /// </summary>
            Space,
            /// <summary>
            /// 转义字符\r \n ..
            /// </summary>
            ESC,
            /// <summary>
            /// 开始标签符号&lt;
            /// </summary>
            LT,
            /// <summary>
            /// 结束标签符号&gt;
            /// </summary>
            GT,
            /// <summary>
            /// ?
            /// </summary>
            Question,
            /// <summary>
            /// -
            /// </summary>
            Minus,
            /// <summary>
            /// 感叹号！注释<!--...-->
            /// </summary>
            Exclam,
            /// <summary>
            /// 其它字符,如中文
            /// </summary>
            Others,
        }
        public enum XMLMode
        {
            /// <summary>
            /// 0:未碰到任何非空字符 
            /// </summary>
            Init,
            /// <summary>
            /// 1:第一次碰到&lt;之后
            /// </summary>
            SentenceStart,
            /// <summary>
            /// 2:处于开始标签文本
            /// </summary>
            InStartTagText,
            /// <summary>
            /// 3:开始标签结束
            /// </summary>
            StartTagEnd,
            /// <summary>
            /// 4:结束标签开始
            /// </summary>
            EndTagStartInit,
            /// <summary>
            /// 5:结束标签遇到第一个&lt;/
            /// </summary>
            EndTagStart,
            /// <summary>
            /// 6:开始标签文本结束
            /// </summary>
            StartTagTextEnd,
            /// <summary>
            /// 7:处于内容文本中   ***
            /// </summary>
            InContent,
            /// <summary>
            /// 8:处于结束标签文本
            /// </summary>
            InEndTagText,
            /// <summary>
            /// 9:处于属性键名文本中
            /// 属性名可以包含：
            /// </summary>
            InAttrName,
            /// <summary>
            /// 10:句子结束
            /// </summary>
            SentenceEnd,
            /// <summary>
            /// 11:结束标签文本结束
            /// </summary>
            EndTagTextEnd,
            /// <summary>
            /// 12:属性键名结束
            /// </summary>
            AttrNameEnd,
            /// <summary>
            /// 13:属性键值文本中
            /// </summary>
            InAttrValue,
            /// <summary>
            /// 14:属性键值结束
            /// </summary>
            AttrValueEnd,
            /// <summary>
            /// 15:开始标签文本结束后遇到/,或结束标签之前的?如 &lt;? xxx ?&gt;
            /// </summary>
            SentenceEndInit,
            /// <summary>
            /// 16:【无用】属性值以‘开头
            /// </summary>
            AttrValueString1Init,
            /// <summary>
            /// 17:【无用】属性值以‘开头，遇到/
            /// </summary>
            AttrValueString1,
            /// <summary>
            /// 18:【无用】属性值以“开头
            /// </summary>
            AttrValueString2Init,
            /// <summary>
            /// 19:【无用】属性值以“开头，遇到/
            /// </summary>
            AttrValueString2,
            /// <summary>
            /// 20:异常
            /// </summary>
            Error,
            /// <summary>
            /// 21:注释用&lt;之后出现！
            /// </summary>
            CommentInitStart,
            /// <summary>
            /// 22:注释用&lt;!之后出现-
            /// </summary>
            CommentInit ,
            /// <summary>
            /// 23:注释用&lt;!-之后出现-
            /// </summary>
            InComment,
            /// <summary>
            /// 24:注释后出现-
            /// </summary>
            CommentEndInitStart,
            /// <summary>
            /// 25:注释后出现--
            /// </summary>
            CommentEndInit,
            /// <summary>
            /// 26:【无用】注释后出现-->
            /// </summary>
            CommentEnd
        }

        public enum StringMode
        {
            /// <summary>
            /// 初始化状态
            /// </summary>
            Init,
            /// <summary>
            /// 1:正确进入双引号模式
            /// </summary>
            EnterD,
            /// <summary>
            /// 2:正确进入单引号模式
            /// </summary>
            EnterS,
            /// <summary>
            /// 2:【无用】正确退出双引号模式
            /// </summary>
            OutD,
            /// <summary>
            /// 2:【无用】正确退出单引号模式
            /// </summary>
            OutS,
        }
        public class tagNest
        {
            public string tagName;
            public Dictionary<string, object> Attribute;
            public List<tagNest> subtag;
            public string content;
        }
        List<tagNest> Ntag;
        public class tagList
        {
            public int Stack;
            public int parentKey;
            public int Key;
            public string Content;
            //public int Index;
            public int sentenceStart;
            //public int sentenceEndStart;//结束标签开始位置
            public string tagName;
            public Dictionary<string, object> Attribute;
            public string message;
            public tagList()
            {
                Attribute = new Dictionary<string, object>();
                tagName = "";
                message = "";
            }
        }
        static List<tagList> Ltag;
        static tagList tag;
        public static void getStruct(string content)
        {
            Ltag = new List<tagList>();
            //tagSet tag = new tagSet();
            tag=new tagList();
            
            int parentKey= -1;
            int Stack=0;
            int Key=0;
            int sentenceStart = -1;
            int sentenceEndStart = -1;
            int contentStart = -1;
            int contentEnd = 0;
            string keyName = "";
            object keyValue; 
            tag.parentKey = parentKey;
            tag.Stack = Stack;
            tag.Key = Key;
            Ltag.Add(tag);
            int TagStart = -1;
            XMLMode currentMode = XMLMode.Init;
            StringMode stringMode = StringMode.Init;
            for (int i = 0; i < content.Length && currentMode != XMLMode.Error; i++)
            {
                #region DeBug
                if (i%30==0)
                {
                    if (true)
                    {
                        
                    }
                }
                if (i==2500)
                {
                    
                    if (true)
                    {
                        
                    }
                }
                #endregion
                switch (getNodeStyle(content[i]))
                {
                    case XMLNodeStyle.A:
                        switch (currentMode)
                        {
                            case XMLMode.Init:
                                tag.message = "以字母开始";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceStart:
                                TagStart = i;
                                parentKey = tag.Key;
                                Stack++;
                                Key++;
                                tag = new tagList();
                                tag.parentKey = parentKey;
                                tag.Stack = Stack;
                                tag.Key = Key;
                                tag.sentenceStart = sentenceStart;
                                sentenceStart = -1;
                                Ltag.Add(tag);
                                currentMode = XMLMode.InStartTagText;
                                break;
                            case XMLMode.InStartTagText:
                                currentMode = XMLMode.InStartTagText;
                                break;
                            case XMLMode.StartTagEnd:
                                currentMode = XMLMode.InContent;
                                contentStart = i;
                                break;
                            case XMLMode.EndTagStartInit:
                                TagStart = i;
                                parentKey = tag.Key;
                                Stack++;
                                Key++;
                                tag = new tagList();
                                tag.parentKey = parentKey;
                                tag.Stack = Stack;
                                tag.Key = Key;
                                tag.sentenceStart = sentenceEndStart;
                                sentenceEndStart = -1;
                                Ltag.Add(tag);
                                currentMode = XMLMode.InStartTagText;
                                break;
                            case XMLMode.EndTagStart:
                                TagStart = i;
                                currentMode = XMLMode.InEndTagText;
                                break;
                            case XMLMode.StartTagTextEnd:
                                TagStart = i;
                                currentMode = XMLMode.InAttrName;
                                break;
                            case XMLMode.InContent:
                                currentMode = XMLMode.InContent;
                                break;
                            case XMLMode.InEndTagText:
                                currentMode = XMLMode.InEndTagText;
                                break;
                            case XMLMode.InAttrName:
                                currentMode = XMLMode.InAttrName;
                                break;
                            case XMLMode.SentenceEnd:
                                tag.message = "不会出现的状态";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagTextEnd:
                                tag.message = "结束标签后出现两个文本";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrNameEnd:
                                TagStart = i;
                                currentMode = XMLMode.InAttrValue;
                                break;
                            case XMLMode.InAttrValue:
                                currentMode = XMLMode.InAttrValue;
                                break;
                            case XMLMode.AttrValueEnd:
                                TagStart = i;
                                currentMode = XMLMode.InAttrName;
                                break;
                            case XMLMode.SentenceEndInit:
                                tag.message = "开始标签结束符/后不能包含文本";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrValueString1Init:
                                break;
                            case XMLMode.AttrValueString1:
                                break;
                            case XMLMode.AttrValueString2Init:
                                break;
                            case XMLMode.AttrValueString2:
                                break;
                            case XMLMode.Error:
                                break;
                            default:
                                break;
                        }
                        break;
                    case XMLNodeStyle.D:
                        switch (currentMode)
                        {
                            case XMLMode.Init:
                                tag.message = "以数字开始";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceStart:
                                tag.message = "开始标签<之后直接出现数字";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InStartTagText:
                                currentMode = XMLMode.InStartTagText;
                                break;
                            case XMLMode.StartTagEnd:
                                currentMode = XMLMode.InContent;
                                contentStart = i;
                                break;
                            case XMLMode.EndTagStartInit:
                                tag.message = "结束标签<之后直接出现数字";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagStart:
                                tag.message = "结束标签</之后直接出现数字";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.StartTagTextEnd:
                                tag.message = "属性名以数字开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InContent:
                                currentMode = XMLMode.InContent;
                                break;
                            case XMLMode.InEndTagText:
                                currentMode = XMLMode.InEndTagText;
                                break;
                            case XMLMode.InAttrName:
                                currentMode = XMLMode.InAttrName;
                                break;
                            case XMLMode.SentenceEnd:
                                tag.message = "不会出现的状态";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagTextEnd:
                                tag.message = "结束标签后出现数值";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrNameEnd:
                                TagStart = i;
                                currentMode = XMLMode.InAttrValue;
                                break;
                            case XMLMode.InAttrValue:
                                currentMode = XMLMode.InAttrValue;
                                break;
                            case XMLMode.AttrValueEnd:
                                tag.message = "属性名以数字开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceEndInit:
                                tag.message = "开始标签结束符/后不能包含数字";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrValueString1Init:
                                break;
                            case XMLMode.AttrValueString1:
                                break;
                            case XMLMode.AttrValueString2Init:
                                break;
                            case XMLMode.AttrValueString2:
                                break;
                            case XMLMode.Error:
                                break;
                            default:
                                break;
                        }
                        break;
                    case XMLNodeStyle.Dot:
                        switch (currentMode)
                        {
                            case XMLMode.Init:
                                tag.message = "以小数点开始";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceStart:
                                tag.message = "开始标签<之后直接出现小数点";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InStartTagText:
                                currentMode = XMLMode.InStartTagText;
                                break;
                            case XMLMode.StartTagEnd:
                                currentMode = XMLMode.InContent;
                                contentStart = i;
                                break;
                            case XMLMode.EndTagStartInit:
                                tag.message = "结束标签<之后直接出现小数点";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagStart:
                                tag.message = "结束标签</之后直接出现小数点";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.StartTagTextEnd:
                                TagStart = i;
                                currentMode = XMLMode.InAttrName;
                                break;
                            case XMLMode.InContent:
                                currentMode = XMLMode.InContent;
                                break;
                            case XMLMode.InEndTagText:
                                currentMode = XMLMode.InEndTagText;
                                break;
                            case XMLMode.InAttrName:
                                currentMode = XMLMode.InAttrName;
                                break;
                            case XMLMode.SentenceEnd:
                                tag.message = "不会出现的状态";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagTextEnd:
                                tag.message = "结束标签后出现小数点";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrNameEnd:
                                TagStart = i;
                                currentMode = XMLMode.InAttrValue;
                                break;
                            case XMLMode.InAttrValue:
                                currentMode = XMLMode.InAttrValue;
                                break;
                            case XMLMode.AttrValueEnd:
                                TagStart = i;
                                currentMode = XMLMode.InAttrName;
                                break;
                            case XMLMode.SentenceEndInit:
                                tag.message = "开始标签结束符/后不能包含小数点";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrValueString1Init:
                                break;
                            case XMLMode.AttrValueString1:
                                break;
                            case XMLMode.AttrValueString2Init:
                                break;
                            case XMLMode.AttrValueString2:
                                break;
                            case XMLMode.Error:
                                break;
                            default:
                                break;
                        }
                        break;
                    case XMLNodeStyle.SingleQuote:
                        switch (currentMode)
                        {
                            case XMLMode.Init:
                                tag.message = "以单引号开始";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceStart:
                                tag.message = "开始标签<直接出现单引号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InStartTagText:
                                tag.message = "开始标签中包含单引号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.StartTagEnd:
                                //switch (stringMode)
                                //{
                                //    case StringMode.Init:
                                //        stringMode = StringMode.Init;
                                //        break;
                                //    case StringMode.EnterD:
                                //        stringMode = StringMode.Init;
                                //        break;
                                //    case StringMode.EnterS:
                                //        stringMode = StringMode.Init;
                                //        break;
                                //    case StringMode.OutD:
                                //        stringMode = StringMode.Init;
                                //        break;
                                //    case StringMode.OutS:
                                //        stringMode = StringMode.Init;
                                //        break;
                                //    default:
                                //        break;
                                //}
                                currentMode = XMLMode.InContent;
                                contentStart = i;
                                break;
                            case XMLMode.EndTagStartInit:
                                tag.message = "结束标签<之后直接出现单引号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagStart:
                                tag.message = "结束标签</之后直接出现单引号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.StartTagTextEnd:
                                tag.message = "属性名以单引号开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InContent:
                                currentMode = XMLMode.InContent;
                                break;
                            case XMLMode.InEndTagText:
                                tag.message = "结束标签文本包含单引号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InAttrName:
                                tag.message = "属性名文本包含单引号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceEnd:
                                tag.message = "不会出现的状态";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagTextEnd:
                                tag.message = "结束标签后出现单引号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrNameEnd:
                                switch (stringMode)
                                {
                                    case StringMode.Init:
                                        stringMode = StringMode.EnterS;
                                        break;
                                    case StringMode.EnterD:
                                        stringMode = StringMode.EnterD;
                                        break;
                                    case StringMode.EnterS:
                                        stringMode = StringMode.Init;
                                        break;
                                    case StringMode.OutD:
                                        stringMode = StringMode.EnterS;
                                        break;
                                    case StringMode.OutS:
                                        stringMode = StringMode.EnterS;
                                        break;
                                    default:
                                        break;
                                }
                                currentMode = XMLMode.InAttrValue;
                                break;
                            case XMLMode.InAttrValue:
                                switch (stringMode)
                                {
                                    case StringMode.Init:
                                        tag.message = "非字符串属性值包含单引号";
                                        currentMode = XMLMode.Error;
                                        stringMode = StringMode.Init;
                                        break;
                                    case StringMode.EnterD:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterD;
                                        break;
                                    case StringMode.EnterS:
                                        keyValue = content.Substring(TagStart, i - TagStart+1);
                                        tag.Attribute.Add(keyName, keyValue);
                                        keyName = "";
                                        keyValue = null;
                                        currentMode = XMLMode.AttrValueEnd;
                                        stringMode = StringMode.Init;
                                        break;
                                    case StringMode.OutD:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    case StringMode.OutS:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case XMLMode.AttrValueEnd:
                                tag.message = "第一个属性名后“开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceEndInit:
                                tag.message = "开始标签结束符/后不能包含单引号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrValueString1Init:
                                break;
                            case XMLMode.AttrValueString1:
                                break;
                            case XMLMode.AttrValueString2Init:
                                break;
                            case XMLMode.AttrValueString2:
                                break;
                            case XMLMode.Error:
                                break;
                            default:
                                break;
                        }
                        break;
                    case XMLNodeStyle.DoubleQuote:
                        switch (currentMode)
                        {
                            case XMLMode.Init:
                                tag.message = "以双引号开始";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceStart:
                                tag.message = "开始标签<直接出现双引号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InStartTagText:
                                tag.message = "开始标签包含双引号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.StartTagEnd:
                                //switch (stringMode)
                                //{
                                //    case StringMode.Init:
                                //        stringMode = StringMode.Init;
                                //        break;
                                //    case StringMode.EnterD:
                                //        stringMode = StringMode.Init;
                                //        break;
                                //    case StringMode.EnterS:
                                //        stringMode = StringMode.Init;
                                //        break;
                                //    case StringMode.OutD:
                                //        stringMode = StringMode.Init;
                                //        break;
                                //    case StringMode.OutS:
                                //        stringMode = StringMode.Init;
                                //        break;
                                //    default:
                                //        break;
                                //}
                                currentMode = XMLMode.InContent;
                                contentStart = i;
                                break;
                            case XMLMode.EndTagStartInit:
                                tag.message = "结束标签<之后直接出现双引号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagStart:
                                tag.message = "结束标签</之后直接出现双引号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.StartTagTextEnd:
                                tag.message = "属性名以双引号开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InContent:
                                //switch (stringMode)
                                //{
                                //    case StringMode.Init:
                                //        stringMode = StringMode.EnterS;
                                //        break;
                                //    case StringMode.EnterD:
                                //        stringMode = StringMode.EnterD;
                                //        break;
                                //    case StringMode.EnterS:
                                //        stringMode = StringMode.OutS;
                                //        break;
                                //    case StringMode.OutD:
                                //        stringMode = StringMode.EnterS;
                                //        break;
                                //    case StringMode.OutS:
                                //        stringMode = StringMode.EnterS;
                                //        break;
                                //    default:
                                //        break;
                                //}
                                currentMode = XMLMode.InContent;
                                break;
                            case XMLMode.InEndTagText:
                                tag.message = "结束标签文本包含双引号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InAttrName:
                                tag.message = "属性名文本包含双引号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceEnd:
                                tag.message = "不会出现的状态";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagTextEnd:
                                tag.message = "结束标签后出现双引号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrNameEnd:
                                switch (stringMode)
                                {
                                    case StringMode.Init:
                                        stringMode = StringMode.EnterD;
                                        break;
                                    case StringMode.EnterD:
                                        stringMode = StringMode.OutD;
                                        break;
                                    case StringMode.EnterS:
                                        stringMode = StringMode.EnterS;
                                        break;
                                    case StringMode.OutD:
                                        stringMode = StringMode.EnterD;
                                        break;
                                    case StringMode.OutS:
                                        stringMode = StringMode.EnterD;
                                        break;
                                    default:
                                        break;
                                }
                                TagStart = i;
                                currentMode = XMLMode.InAttrValue;
                                break;
                            case XMLMode.InAttrValue:
                                switch (stringMode)
                                {
                                    case StringMode.Init:
                                        tag.message = "非字符串属性值包含双引号";
                                        currentMode = XMLMode.Error;
                                        stringMode = StringMode.Init;
                                        break;
                                    case StringMode.EnterD:
                                        keyValue = content.Substring(TagStart, i - TagStart+1);
                                        tag.Attribute.Add(keyName, keyValue);
                                        keyName = "";
                                        keyValue = null;
                                        currentMode = XMLMode.AttrValueEnd;
                                        stringMode = StringMode.Init;
                                        break;
                                    case StringMode.EnterS:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterS;
                                        break;
                                    case StringMode.OutD:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    case StringMode.OutS:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case XMLMode.AttrValueEnd:
                                tag.message = "第二个属性名以双引号开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceEndInit:
                                tag.message = "开始标签结束符/后不能包含双引号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrValueString1Init:
                                break;
                            case XMLMode.AttrValueString1:
                                break;
                            case XMLMode.AttrValueString2Init:
                                break;
                            case XMLMode.AttrValueString2:
                                break;
                            case XMLMode.Error:
                                break;
                            default:
                                break;
                        }
                        break;
                    case XMLNodeStyle.Bias:
                        switch (currentMode)
                        {
                            case XMLMode.Init:
                                tag.message = "以斜线开始";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceStart:
                                currentMode = XMLMode.EndTagStart;
                                break;
                            case XMLMode.InStartTagText:
                                tag.message = "标签名包含/";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.StartTagEnd:
                                currentMode = XMLMode.InContent;
                                contentStart = i;
                                break;
                            case XMLMode.EndTagStartInit:
                                currentMode = XMLMode.EndTagStart;
                                break;
                            case XMLMode.EndTagStart:
                                tag.message = "结束标签</之后又出现/";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.StartTagTextEnd:
                                currentMode = XMLMode.SentenceEndInit;
                                break;
                            case XMLMode.InContent:
                                currentMode = XMLMode.InContent;
                                break;
                            case XMLMode.InEndTagText:
                                tag.message = "结束标签文本后又出现/";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InAttrName:
                                tag.message = "属性名文本包含/";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceEnd:
                                tag.message = "不会出现的状态";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagTextEnd:
                                tag.message = "结束标签后出现/";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrNameEnd:
                                 tag.message = "属性值不以/开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InAttrValue:
                                switch (stringMode)
                                {
                                    case StringMode.Init:
                                        tag.message = "非字符串属性值包含/";
                                        currentMode = XMLMode.Error;
                                        stringMode = StringMode.Init;
                                        break;
                                    case StringMode.EnterD:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterD;
                                        break;
                                    case StringMode.EnterS:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterS;
                                        break;
                                    case StringMode.OutD:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    case StringMode.OutS:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case XMLMode.AttrValueEnd:
                                currentMode = XMLMode.SentenceEndInit;
                                break;
                            case XMLMode.SentenceEndInit:
                                tag.message = "开始标签结束符/后不能再包含/";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrValueString1Init:
                                break;
                            case XMLMode.AttrValueString1:
                                break;
                            case XMLMode.AttrValueString2Init:
                                break;
                            case XMLMode.AttrValueString2:
                                break;
                            case XMLMode.Error:
                                break;
                            default:
                                break;
                        }
                        break;
                    case XMLNodeStyle.Equal:
                        switch (currentMode)
                        {
                            case XMLMode.Init:
                                tag.message = "以等于号开始";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceStart:
                                tag.message = "开始标签异常<=";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InStartTagText:
                                tag.message = "开始标签名包含等于号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.StartTagEnd:
                                currentMode = XMLMode.InContent;
                                contentStart = i;
                                break;
                            case XMLMode.EndTagStartInit:
                                tag.message = "结束标签<后直接出现=";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagStart:
                                tag.message = "结束标签</后直接出现=";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.StartTagTextEnd:
                                tag.message = "属性名以=开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InContent:
                                currentMode = XMLMode.InContent;
                                break;
                            case XMLMode.InEndTagText:
                                tag.message = "结束标签文本包含=";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InAttrName:
                                //tag.message = "属性名文本包含=";
                                keyName = content.Substring(TagStart, i - TagStart);
                                TagStart = -1;
                                currentMode = XMLMode.AttrNameEnd;
                                break;
                            case XMLMode.SentenceEnd:
                                tag.message = "不会出现的状态";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagTextEnd:
                                tag.message = "结束标签后出现=";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrNameEnd:
                                tag.message = "属性值不能以=开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InAttrValue:
                                switch (stringMode)
                                {
                                    case StringMode.Init:
                                        tag.message = "非字符串属性值包含=";
                                        currentMode = XMLMode.Error;
                                        stringMode = StringMode.Init;
                                        break;
                                    case StringMode.EnterD:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterD;
                                        break;
                                    case StringMode.EnterS:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterS;
                                        break;
                                    case StringMode.OutD:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    case StringMode.OutS:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case XMLMode.AttrValueEnd:
                                tag.message = "第二个属性名以=开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceEndInit:
                                tag.message = "开始标签结束符/后不能包含=";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrValueString1Init:
                                break;
                            case XMLMode.AttrValueString1:
                                break;
                            case XMLMode.AttrValueString2Init:
                                break;
                            case XMLMode.AttrValueString2:
                                break;
                            case XMLMode.Error:
                                break;
                            default:
                                break;
                        }
                        break;
                    case XMLNodeStyle.Colon:
                        switch (currentMode)
                        {
                            case XMLMode.Init:
                                tag.message = "以冒号开始";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceStart:
                                tag.message = "开始标签异常<:";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InStartTagText:
                                tag.message = "开始标签名包含：";
                                currentMode = XMLMode.InStartTagText;
                                break;
                            case XMLMode.StartTagEnd:
                                currentMode = XMLMode.InContent;
                                contentStart = i;
                                break;
                            case XMLMode.EndTagStartInit:
                                tag.message = "结束标签<后直接出现：";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagStart:
                                tag.message = "结束标签</后直接出现：";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.StartTagTextEnd:
                                tag.message = "属性名以：开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InContent:
                                currentMode = XMLMode.InContent;
                                break;
                            case XMLMode.InEndTagText:
                                tag.message = "结束标签文本包含：";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InAttrName:
                                //属性名可以包含：
                                //keyName = content.Substring(TagStart, i - TagStart);
                                //TagStart = -1;
                                //currentMode = XMLMode.AttrNameEnd;
                                currentMode = XMLMode.InAttrName;
                                break;
                            case XMLMode.SentenceEnd:
                                tag.message = "不会出现的状态";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagTextEnd:
                                tag.message = "结束标签后出现冒号";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrNameEnd:
                                tag.message = "属性值不能以：开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InAttrValue:
                                switch (stringMode)
                                {
                                    case StringMode.Init:
                                        tag.message = "非字符串属性值包含：";
                                        currentMode = XMLMode.Error;
                                        stringMode = StringMode.Init;
                                        break;
                                    case StringMode.EnterD:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterD;
                                        break;
                                    case StringMode.EnterS:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterS;
                                        break;
                                    case StringMode.OutD:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    case StringMode.OutS:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case XMLMode.AttrValueEnd:
                                tag.message = "非首个属性名以：开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceEndInit:
                                tag.message = "开始标签结束符/后不能包含：";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrValueString1Init:
                                break;
                            case XMLMode.AttrValueString1:
                                break;
                            case XMLMode.AttrValueString2Init:
                                break;
                            case XMLMode.AttrValueString2:
                                break;
                            case XMLMode.Error:
                                break;
                            default:
                                break;
                        }
                        break;
                    case XMLNodeStyle.Space:
                        switch (currentMode)
                        {
                            case XMLMode.Init:
                                currentMode = XMLMode.Init;
                                break;
                            case XMLMode.SentenceStart:
                                currentMode = XMLMode.SentenceStart;
                                break;
                            case XMLMode.InStartTagText:
                                tag.tagName = content.Substring(TagStart, i - TagStart);
                                TagStart = -1;
                                currentMode = XMLMode.StartTagTextEnd;
                                break;
                            case XMLMode.StartTagEnd:
                                currentMode = XMLMode.StartTagEnd;
                                break;
                            case XMLMode.EndTagStartInit:
                                currentMode = XMLMode.EndTagStartInit;
                                break;
                            case XMLMode.EndTagStart:
                                currentMode = XMLMode.EndTagStart;
                                break;
                            case XMLMode.StartTagTextEnd:
                                currentMode = XMLMode.StartTagTextEnd;
                                break;
                            case XMLMode.InContent:
                                currentMode = XMLMode.InContent;
                                break;
                            case XMLMode.InEndTagText:
                                
                                if (tag.tagName==content.Substring(TagStart,i-TagStart))
                                {
                                    currentMode = XMLMode.EndTagTextEnd;
                                }
                                else
                                {
                                    tag.message = "开始标签和结束标签不一致";
                                    currentMode = XMLMode.Error;
                                }
                                break;
                            case XMLMode.InAttrName:
                                keyName = content.Substring(TagStart, i - TagStart);
                                TagStart = -1;
                                currentMode = XMLMode.AttrNameEnd;
                                break;
                            case XMLMode.SentenceEnd:
                                tag.message = "不会出现的状态";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagTextEnd:
                                currentMode = XMLMode.EndTagTextEnd;
                                break;
                            case XMLMode.AttrNameEnd:
                                currentMode = XMLMode.AttrNameEnd;
                                break;
                            case XMLMode.InAttrValue:
                                switch (stringMode)
                                {
                                    case StringMode.Init:
                                        keyValue = content.Substring(TagStart, i - TagStart);
                                        tag.Attribute.Add(keyName, keyValue);
                                        keyName = "";
                                        keyValue = null;
                                        currentMode = XMLMode.AttrValueEnd;
                                        stringMode = StringMode.Init;
                                        break;
                                    case StringMode.EnterD:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterD;
                                        break;
                                    case StringMode.EnterS:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterS;
                                        break;
                                    case StringMode.OutD:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    case StringMode.OutS:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case XMLMode.AttrValueEnd:
                                currentMode = XMLMode.AttrValueEnd;
                                break;
                            case XMLMode.SentenceEndInit:
                                currentMode = XMLMode.SentenceEndInit;
                                break;
                            case XMLMode.AttrValueString1Init:
                                break;
                            case XMLMode.AttrValueString1:
                                break;
                            case XMLMode.AttrValueString2Init:
                                break;
                            case XMLMode.AttrValueString2:
                                break;
                            case XMLMode.Error:
                                break;
                            default:
                                break;
                        }
                        break;
                    case XMLNodeStyle.ESC:
                        switch (currentMode)
                        {
                            case XMLMode.Init:
                                currentMode = XMLMode.Init;
                                break;
                            case XMLMode.SentenceStart:
                                currentMode = XMLMode.SentenceStart;
                                break;
                            case XMLMode.InStartTagText:
                                tag.tagName = content.Substring(TagStart, i - TagStart);
                                TagStart = -1;
                                currentMode = XMLMode.StartTagTextEnd;
                                break;
                            case XMLMode.StartTagEnd:
                                currentMode = XMLMode.StartTagEnd;
                                break;
                            case XMLMode.EndTagStartInit:
                                currentMode = XMLMode.EndTagStartInit;
                                break;
                            case XMLMode.EndTagStart:
                                currentMode = XMLMode.EndTagStart;
                                break;
                            case XMLMode.StartTagTextEnd:
                                currentMode = XMLMode.StartTagTextEnd;
                                break;
                            case XMLMode.InContent:
                                currentMode = XMLMode.InContent;
                                break;
                            case XMLMode.InEndTagText:
                                if (tag.tagName == content.Substring(TagStart, i - TagStart))
                                {
                                    currentMode = XMLMode.EndTagTextEnd;
                                }
                                else
                                {
                                    tag.message = "开始标签和结束标签不一致";
                                    currentMode = XMLMode.Error;
                                }
                                break;
                            case XMLMode.InAttrName:
                                keyName = content.Substring(TagStart, i - TagStart);
                                TagStart = -1;
                                currentMode = XMLMode.AttrNameEnd;
                                break;
                            case XMLMode.SentenceEnd:
                                tag.message = "不会出现的状态";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagTextEnd:
                                currentMode = XMLMode.EndTagTextEnd;
                                break;
                            case XMLMode.AttrNameEnd:
                                currentMode = XMLMode.AttrNameEnd;
                                break;
                            case XMLMode.InAttrValue:
                                switch (stringMode)
                                {
                                    case StringMode.Init:
                                        keyValue = content.Substring(TagStart, i - TagStart);
                                        tag.Attribute.Add(keyName, keyValue);
                                        keyName = "";
                                        keyValue = null;
                                        currentMode = XMLMode.AttrValueEnd;
                                        stringMode = StringMode.Init;
                                        break;
                                    case StringMode.EnterD:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterD;
                                        break;
                                    case StringMode.EnterS:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterS;
                                        break;
                                    case StringMode.OutD:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    case StringMode.OutS:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case XMLMode.AttrValueEnd:
                                currentMode = XMLMode.AttrValueEnd;
                                break;
                            case XMLMode.SentenceEndInit:
                                currentMode = XMLMode.SentenceEndInit;
                                break;
                            case XMLMode.AttrValueString1Init:
                                break;
                            case XMLMode.AttrValueString1:
                                break;
                            case XMLMode.AttrValueString2Init:
                                break;
                            case XMLMode.AttrValueString2:
                                break;
                            case XMLMode.Error:
                                break;
                            default:
                                break;
                        }
                        break;
                    case XMLNodeStyle.LT:
                        switch (currentMode)
                        {
                            case XMLMode.Init:
                                sentenceStart = i;
                                currentMode = XMLMode.SentenceStart;
                                break;
                            case XMLMode.SentenceStart:
                                tag.message = "开始标签连续两个<";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InStartTagText:
                                tag.message = "开始标签名包含<";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.StartTagEnd:
                                sentenceEndStart = i;
                                currentMode = XMLMode.EndTagStartInit;
                                break;
                            case XMLMode.EndTagStartInit:
                                tag.message = "结束标签连续两个<";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagStart:
                                tag.message = "异常结束标签</<";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.StartTagTextEnd:
                                tag.message = "异常属性名以<开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InContent:
                                sentenceEndStart = i;
                                if (contentStart > 0)
                                {
                                    tag.Content = content.Substring(contentStart, i - contentStart);
                                    contentStart = -1;
                                }
                                contentEnd = i;
                                currentMode = XMLMode.EndTagStartInit;
                                break;
                            case XMLMode.InEndTagText:
                                tag.message = "异常结束标签文本后包含<";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InAttrName:
                                tag.message = "异常属性名包含<";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceEnd:
                                tag.message = "不会出现的状态";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagTextEnd:
                                tag.message = "结束标签后出现<";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrNameEnd:
                                tag.message = "属性值不以<开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InAttrValue:
                                switch (stringMode)
                                {
                                    case StringMode.Init:
                                        tag.message = "非字符串属性值不能包含<";
                                        currentMode = XMLMode.Error;
                                        stringMode = StringMode.Init;
                                        break;
                                    case StringMode.EnterD:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterD;
                                        break;
                                    case StringMode.EnterS:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterS;
                                        break;
                                    case StringMode.OutD:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    case StringMode.OutS:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case XMLMode.AttrValueEnd:
                                tag.message = "异常非首个属性名以<开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceEndInit:
                                tag.message = "开始标签结束符/后不能包含<";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrValueString1Init:
                                break;
                            case XMLMode.AttrValueString1:
                                break;
                            case XMLMode.AttrValueString2Init:
                                break;
                            case XMLMode.AttrValueString2:
                                break;
                            case XMLMode.Error:
                                break;
                            default:
                                break;
                        }
                        break;
                    case XMLNodeStyle.GT:
                        switch (currentMode)
                        {
                            case XMLMode.Init:
                                tag.message = "以>开始";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceStart:
                                tag.message = "开始标签异常<>";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InStartTagText:
                                tag.tagName = content.Substring(TagStart, i - TagStart);
                                currentMode = XMLMode.StartTagEnd;
                                break;
                            case XMLMode.StartTagEnd:
                                 tag.message = "开始标签异常>>";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagStartInit:
                                tag.message = "结束标签异常<>";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagStart:
                                tag.message = "结束标签异常</>";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.StartTagTextEnd:
                                currentMode = XMLMode.StartTagEnd;
                                break;
                            case XMLMode.InContent:
                                //switch (stringMode)
                                //{
                                //    case StringMode.Init:
                                //        stringMode = StringMode.EnterS;
                                //        tag.message = "内容出现>";
                                //        currentMode = XMLMode.Error;
                                //        break;
                                //    case StringMode.EnterD:
                                //        stringMode = StringMode.EnterD;
                                //        currentMode = XMLMode.InContent;
                                //        break;
                                //    case StringMode.EnterS:
                                //        stringMode = StringMode.OutS;
                                //        currentMode = XMLMode.InContent;
                                //        break;
                                //    case StringMode.OutD:
                                //        stringMode = StringMode.EnterS;
                                //        tag.message = "内容出现>";
                                //        currentMode = XMLMode.Error;
                                //        break;
                                //    case StringMode.OutS:
                                //        stringMode = StringMode.EnterS;
                                //        tag.message = "内容出现>";
                                //        currentMode = XMLMode.Error;
                                //        break;
                                //    default:
                                //        break;
                                //}
                                currentMode = XMLMode.InContent;
                                break;
                            case XMLMode.InEndTagText:
                                //结束.
                                if (tag.tagName == content.Substring(TagStart,i-TagStart))
                                {
                                    TagStart = -1;
                                    parentKey = tag.parentKey;
                                    Stack--;
                                    tag = Ltag.Find(s=>s.Key.Equals(parentKey));
                                    currentMode = XMLMode.Init;
                                }
                                else
                                {
                                    tag.message = "开始标签和结束标签数量不一致";
                                    currentMode = XMLMode.Error;
                                }
                                if (Stack<0)
                                {
                                    tag.message = "结束标签多余开始标签";
                                    currentMode = XMLMode.Error;
                                }
                                break;
                            case XMLMode.InAttrName:
                                tag.message = "异常，<>中两个标签";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceEnd:
                                tag.message = "不会出现的状态";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagTextEnd:
                                if (Stack<0)
                                {
                                    tag.message = "结束标签多余开始标签";
                                    currentMode = XMLMode.Error;
                                }
                                else
                                {
                                    TagStart = -1;
                                    parentKey = tag.parentKey;
                                    Stack--;
                                    tag = Ltag.Find(s => s.Key.Equals(parentKey));
                                    currentMode = XMLMode.Init;
                                }
                                break;
                            case XMLMode.AttrNameEnd:
                                tag.message = "属性值缺失";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InAttrValue:
                                switch (stringMode)
                                {
                                    case StringMode.Init:
                                        keyValue = content.Substring(TagStart, i - TagStart);
                                        tag.Attribute.Add(keyName, keyValue);
                                        keyName = "";
                                        keyValue = null;
                                        TagStart = -1;
                                        currentMode = XMLMode.StartTagEnd;
                                        stringMode = StringMode.Init;
                                        break;
                                    case StringMode.EnterD:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterD;
                                        break;
                                    case StringMode.EnterS:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterS;
                                        break;
                                    case StringMode.OutD:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    case StringMode.OutS:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case XMLMode.AttrValueEnd:
                                currentMode = XMLMode.StartTagEnd;
                                break;
                            case XMLMode.SentenceEndInit:
                                if (Stack < 0)
                                {
                                    tag.message = "结束标签多余开始标签";
                                    currentMode = XMLMode.Error;
                                }
                                else
                                {
                                    TagStart = -1;
                                    parentKey = tag.parentKey;
                                    Stack--;
                                    tag = Ltag.Find(s => s.Key.Equals(parentKey));
                                    currentMode = XMLMode.Init;
                                }
                                break;
                            case XMLMode.AttrValueString1Init:
                                break;
                            case XMLMode.AttrValueString1:
                                break;
                            case XMLMode.AttrValueString2Init:
                                break;
                            case XMLMode.AttrValueString2:
                                break;
                            case XMLMode.Error:
                                break;
                            case XMLMode.CommentInitStart:
                                tag.message = "不支持语法<!>";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.CommentInit:
                                tag.message = "不支持语法<!->";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InComment:
                                tag.message = "不支持语法<!-->";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.CommentEndInitStart:
                                tag.message = "不支持语法<!-...->";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.CommentEndInit:
                                currentMode = XMLMode.Init;
                                break;
                            case XMLMode.CommentEnd:
                                break;
                            default:
                                break;
                        }
                        break;
                    case XMLNodeStyle.Question:
                        switch (currentMode)
                        {
                            case XMLMode.Init:
                                currentMode = XMLMode.Init;
                                break;
                            case XMLMode.SentenceStart:
                                currentMode = XMLMode.SentenceStart;
                                break;
                            case XMLMode.InStartTagText:
                                tag.message = "开始标签文本包含问号";
                                    currentMode = XMLMode.Error;
                                break;
                            case XMLMode.StartTagEnd:
                                currentMode = XMLMode.StartTagEnd;
                                break;
                            case XMLMode.EndTagStartInit:
                                currentMode = XMLMode.EndTagStartInit;
                                break;
                            case XMLMode.EndTagStart:
                                currentMode = XMLMode.EndTagStart;
                                break;
                            case XMLMode.StartTagTextEnd:
                                currentMode = XMLMode.SentenceEndInit;
                                break;
                            case XMLMode.InContent:
                                currentMode = XMLMode.InContent;
                                break;
                            case XMLMode.InEndTagText:
                                currentMode = XMLMode.InEndTagText;
                                break;
                            case XMLMode.InAttrName:
                                currentMode = XMLMode.InAttrName;
                                break;
                            case XMLMode.SentenceEnd:
                                break;
                            case XMLMode.EndTagTextEnd:
                                break;
                            case XMLMode.AttrNameEnd:
                                break;
                            case XMLMode.InAttrValue:
                                switch (stringMode)
                                {
                                    case StringMode.Init:
                                        keyValue = content.Substring(TagStart, i - TagStart);
                                        tag.Attribute.Add(keyName, keyValue);
                                        keyName = "";
                                        keyValue = null;
                                        TagStart = -1;
                                        currentMode = XMLMode.SentenceEndInit;
                                        stringMode = StringMode.Init;
                                        break;
                                    case StringMode.EnterD:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterD;
                                        break;
                                    case StringMode.EnterS:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterS;
                                        break;
                                    case StringMode.OutD:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    case StringMode.OutS:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case XMLMode.AttrValueEnd:
                                switch (stringMode)
                                {
                                    case StringMode.Init:
                                        currentMode = XMLMode.SentenceEndInit;
                                        stringMode = StringMode.Init;
                                        break;
                                    case StringMode.EnterD:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterD;
                                        break;
                                    case StringMode.EnterS:
                                        currentMode = XMLMode.InAttrValue;
                                        stringMode = StringMode.EnterS;
                                        break;
                                    case StringMode.OutD:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    case StringMode.OutS:
                                        tag.message = "不可能出现的状态";
                                        currentMode = XMLMode.Error;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case XMLMode.SentenceEndInit:
                                break;
                            case XMLMode.AttrValueString1Init:
                                break;
                            case XMLMode.AttrValueString1:
                                break;
                            case XMLMode.AttrValueString2Init:
                                break;
                            case XMLMode.AttrValueString2:
                                break;
                            case XMLMode.Error:
                                break;
                            default:
                                break;
                        }
                        break;
                    case XMLNodeStyle.Minus:
                        switch (currentMode)
                        {
                            case XMLMode.Init:
                                tag.message = "以-开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceStart:
                                tag.message = "不支持语法<-";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.InStartTagText:
                                break;
                            case XMLMode.StartTagEnd:
                                break;
                            case XMLMode.EndTagStartInit:
                                break;
                            case XMLMode.EndTagStart:
                                break;
                            case XMLMode.StartTagTextEnd:
                                break;
                            case XMLMode.InContent:
                                break;
                            case XMLMode.InEndTagText:
                                break;
                            case XMLMode.InAttrName:
                                break;
                            case XMLMode.SentenceEnd:
                                break;
                            case XMLMode.EndTagTextEnd:
                                break;
                            case XMLMode.AttrNameEnd:
                                break;
                            case XMLMode.InAttrValue:
                                break;
                            case XMLMode.AttrValueEnd:
                                break;
                            case XMLMode.SentenceEndInit:
                                break;
                            case XMLMode.AttrValueString1Init:
                                break;
                            case XMLMode.AttrValueString1:
                                break;
                            case XMLMode.AttrValueString2Init:
                                break;
                            case XMLMode.AttrValueString2:
                                break;
                            case XMLMode.Error:
                                break;
                            case XMLMode.CommentInitStart:
                                currentMode = XMLMode.CommentInit;
                                break;
                            case XMLMode.CommentInit:
                                currentMode = XMLMode.InComment;
                                break;
                            case XMLMode.InComment:
                                currentMode = XMLMode.CommentEndInitStart;
                                break;
                            case XMLMode.CommentEndInitStart:
                                currentMode = XMLMode.CommentEndInit;
                                break;
                            case XMLMode.CommentEndInit:
                                currentMode = XMLMode.CommentEndInit;
                                break;
                            case XMLMode.CommentEnd:
                                break;
                            default:
                                break;
                        }
                        break;
                    case XMLNodeStyle.Exclam:
                        switch (currentMode)
                        {
                            case XMLMode.Init:
                                tag.message = "以感叹号开头";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceStart:
                                currentMode = XMLMode.CommentInitStart;
                                break;
                            case XMLMode.InStartTagText:
                                break;
                            case XMLMode.StartTagEnd:
                                break;
                            case XMLMode.EndTagStartInit:
                                break;
                            case XMLMode.EndTagStart:
                                break;
                            case XMLMode.StartTagTextEnd:
                                break;
                            case XMLMode.InContent:
                                break;
                            case XMLMode.InEndTagText:
                                break;
                            case XMLMode.InAttrName:
                                break;
                            case XMLMode.SentenceEnd:
                                break;
                            case XMLMode.EndTagTextEnd:
                                break;
                            case XMLMode.AttrNameEnd:
                                break;
                            case XMLMode.InAttrValue:
                                break;
                            case XMLMode.AttrValueEnd:
                                break;
                            case XMLMode.SentenceEndInit:
                                break;
                            case XMLMode.AttrValueString1Init:
                                break;
                            case XMLMode.AttrValueString1:
                                break;
                            case XMLMode.AttrValueString2Init:
                                break;
                            case XMLMode.AttrValueString2:
                                break;
                            case XMLMode.Error:
                                break;
                            case XMLMode.CommentInitStart:
                                break;
                            case XMLMode.CommentInit:
                                break;
                            case XMLMode.InComment:
                                break;
                            case XMLMode.CommentEndInitStart:
                                break;
                            case XMLMode.CommentEndInit:
                                break;
                            case XMLMode.CommentEnd:
                                break;
                            default:
                                break;
                        }
                        break;
                    case XMLNodeStyle.Others:
                        switch (currentMode)
                        {
                            case XMLMode.Init:
                                tag.message = "以其它字符开始";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.SentenceStart:
                                TagStart = i;
                                parentKey = tag.Key;
                                Stack++;
                                Key++;
                                tag = new tagList();
                                tag.parentKey = parentKey;
                                tag.Stack = Stack;
                                tag.Key = Key;
                                Ltag.Add(tag);
                                currentMode = XMLMode.InStartTagText;
                                break;
                            case XMLMode.InStartTagText:
                                currentMode = XMLMode.InStartTagText;
                                break;
                            case XMLMode.StartTagEnd:
                                currentMode = XMLMode.InContent;
                                contentStart = i;
                                break;
                            case XMLMode.EndTagStartInit:
                                TagStart = i;
                                parentKey = tag.Key;
                                Stack++;
                                Key++;
                                tag = new tagList();
                                tag.parentKey = parentKey;
                                tag.Stack = Stack;
                                tag.Key = Key;
                                Ltag.Add(tag);
                                currentMode = XMLMode.InStartTagText;
                                break;
                            case XMLMode.EndTagStart:
                                TagStart = i;
                                currentMode = XMLMode.InEndTagText;
                                break;
                            case XMLMode.StartTagTextEnd:
                                TagStart = i;
                                currentMode = XMLMode.InAttrName;
                                break;
                            case XMLMode.InContent:
                                currentMode = XMLMode.InContent;
                                break;
                            case XMLMode.InEndTagText:
                                currentMode = XMLMode.InEndTagText;
                                break;
                            case XMLMode.InAttrName:
                                currentMode = XMLMode.InAttrName;
                                break;
                            case XMLMode.SentenceEnd:
                                tag.message = "不会出现的状态";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.EndTagTextEnd:
                                tag.message = "结束标签后出现其他字符";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrNameEnd:
                                TagStart = i;
                                currentMode = XMLMode.InAttrValue;
                                break;
                            case XMLMode.InAttrValue:
                                currentMode = XMLMode.InAttrValue;
                                break;
                            case XMLMode.AttrValueEnd:
                                TagStart = i;
                                currentMode = XMLMode.InAttrName;
                                break;
                            case XMLMode.SentenceEndInit:
                                tag.message = "开始标签结束符之后不能包含其他字符";
                                currentMode = XMLMode.Error;
                                break;
                            case XMLMode.AttrValueString1Init:
                                break;
                            case XMLMode.AttrValueString1:
                                break;
                            case XMLMode.AttrValueString2Init:
                                break;
                            case XMLMode.AttrValueString2:
                                break;
                            case XMLMode.Error:
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (currentMode == XMLMode.Error)
            {
                MessageBox.Show(tag.message);
                return;
            }
        }

        private static List<tagNest> getNest(List<tagList> ltag)
        {
            List<tagNest> ntag = new List<tagNest>();
            tagNest subtag;
            for (int i = 0; i < ltag.Count; i++)
            {
                subtag = new tagNest();
                subtag.tagName = ltag[i].tagName;
                subtag.Attribute = ltag[i].Attribute;
                subtag.subtag = getNest(Ltag.FindAll(s => s.parentKey.Equals(ltag[i].Key)));
                subtag.content = ltag[i].Content;
                ntag.Add(subtag);
            }
            return ntag;
        }
        public static List<tagList> getListXML(string xml)
        {
            getStruct(xml);
            return Ltag;
        }
        public static List<tagNest> getNestXML(string xml) 
        {
            getStruct(xml);
            return getNest(new List<tagList>() { Ltag[0] });
        }
        public void plain2nest()
        {
            Ntag = getNest(new List<tagList>() { Ltag[0] });
        }
        
        private static XMLNodeStyle getNodeStyle( char node)
        {
            XMLNodeStyle nodeStyle;
            if (node>='A'&&node<='Z'||node>='a'&&node<='z')
            {
                nodeStyle = XMLNodeStyle.A;
            }
            else if (node>='0'&&node<='9')
            {
                nodeStyle = XMLNodeStyle.D;
            }
            else if (node =='.')
            {
                nodeStyle = XMLNodeStyle.Dot;
            }
            else if (node =='\'')
            {
                nodeStyle = XMLNodeStyle.SingleQuote;
            }
            else if (node=='"')
            {
                nodeStyle = XMLNodeStyle.DoubleQuote;
            }
            else if (node == '/')
            {
                nodeStyle = XMLNodeStyle.Bias;
            }
            else if (node == '=')
            {
                nodeStyle = XMLNodeStyle.Equal;
            }
            else if (node ==':')
            {
                nodeStyle = XMLNodeStyle.Colon;
            }
            else if (node==' '|| node=='\0')
            {
                nodeStyle = XMLNodeStyle.Space;
            }
            else if (node == '\r' || node == '\n' || node == '\t')
            {
                nodeStyle = XMLNodeStyle.ESC;
            }
            else if (node == '>')
            {
                nodeStyle = XMLNodeStyle.GT;
            }
            else if (node == '<')
            {
                nodeStyle = XMLNodeStyle.LT;
            }
            else if ( node == '?')
            {
                nodeStyle = XMLNodeStyle.Question;
            }
            else if (node =='-')
            {
                nodeStyle = XMLNodeStyle.Minus;
            }
            else if (node == '!')
            {
                nodeStyle = XMLNodeStyle.Exclam;
            }
            else
            {
                nodeStyle = XMLNodeStyle.Others;
            }
            return nodeStyle;
        }
        /// <summary>
        /// 初始化时用
        /// </summary>
        /// <param name="aplicationView"></param>
        public void addToAplicationViewTree()
        {
            getStruct(this.content);
            plain2nest();
            TreeNode node;
            TreeNode subNode;
            List<tagNest> Nodes = Ntag[0].subtag[1].subtag[1].subtag;

            for (int i = 0; i < Nodes.Count; i++)
            {
                node = aplicationView.treeView1.Nodes.Add(Nodes[i].tagName);
                node.SelectedImageIndex = node.ImageIndex = 2; //设置选中后的图片和选中前的一致
                for (int j = 0; j < Nodes[i].subtag.Count; j++)
                {
                    Nodes[i].subtag[j].tagName = Nodes[i].subtag[j].Attribute["name"].ToString().Replace("\"","");
                    subNode = node.Nodes.Add(Nodes[i].subtag[j].tagName);
                    subNode.SelectedImageIndex = subNode.ImageIndex = 3;

                }
            }
        }
        /// <summary>
        /// 点击Tree Node时调用
        /// </summary>
        /// <param name="aplicationView.listView1">要展示的地方，提前已经创建好了Header</param>
        /// <param name="p">Node节点名称</param>
        internal void addToAplicationViewList(TreeNode treeNode)
        {
            Dictionary<string, string> detail;
            if (treeNode.Level == 0)
            {
                if (treeNode.IsExpanded)
                {
                    treeNode.Collapse();
                    treeNode.ImageIndex = 2;
                    treeNode.SelectedImageIndex = 2;
                }
                else
                {
                    treeNode.Expand();
                    treeNode.ImageIndex = 1;
                    treeNode.SelectedImageIndex = 1;
                }
            }else if (treeNode.Level == 1)
            {
                aplicationView.listView1.Items.Clear();
                tagNest nodes = Ntag[0].subtag[1].subtag[1].subtag.Find(s => s.tagName.Equals(treeNode.Parent.Text)).subtag.Find(j => j.tagName.Equals(treeNode.Text));
                aplicationView.listView1.BeginUpdate();  //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度  
                switch (treeNode.Parent.Text)
                {
                    case "Favorites":
                        aplicationView.nodeStyle = SAPLoginWindow.NodeStyle.Favorites;
                        break;
                    case "Shortcuts":
                        aplicationView.nodeStyle = SAPLoginWindow.NodeStyle.Shortcuts;
                        //tagNest nodes = Ntag[0].subtag[1].subtag[1].subtag.Find(s=>s.Attribute.First(q=>q.Value == ""))
                        aplicationView.listView1.Columns.Clear();
                        aplicationView.listView1.Columns.Add("名称", 120, HorizontalAlignment.Left);
                        aplicationView.listView1.Columns.Add("描述", 150, HorizontalAlignment.Left);
                        aplicationView.listView1.Columns.Add("系统", 60, HorizontalAlignment.Left);
                        aplicationView.listView1.Columns.Add("集团", 60, HorizontalAlignment.Left);
                        aplicationView.listView1.Columns.Add("账号", 100, HorizontalAlignment.Left);
                        aplicationView.listView1.Columns.Add("事务代码", 60, HorizontalAlignment.Left);

                        for (int i = 0; i < nodes.subtag.Count; i++)
                        {
                            nodes.subtag[i].tagName = nodes.subtag[i].Attribute["name"].ToString().Replace("\"", "");
                            ListViewItem lvi = new ListViewItem();
                            lvi.ImageIndex = 4;     //通过与imageList绑定，显示imageList中第i项图标    
                            lvi.Text = nodes.subtag[i].tagName;
                            detail = aplicationView.listViewDetailShortcuts.First((s) => s["Label"] == lvi.Text);
                            lvi.SubItems.Add(detail["-desc"]);
                            lvi.SubItems.Add(detail["-sid"]);
                            lvi.SubItems.Add(detail["-clt"]);
                            lvi.SubItems.Add(detail["-u"]);
                            lvi.SubItems.Add(detail["-cmd"]);

                            aplicationView.listView1.Items.Add(lvi);
                        }
                        break;
                    case "Connections":
                        aplicationView.nodeStyle = SAPLoginWindow.NodeStyle.Connections;
                        aplicationView.listView1.Columns.Clear();
                        aplicationView.listView1.Columns.Add("名称", 180, HorizontalAlignment.Left);
                        aplicationView.listView1.Columns.Add("服务器地址", 180, HorizontalAlignment.Left);
                        aplicationView.listView1.Columns.Add("系统编号", 60, HorizontalAlignment.Left);
                        aplicationView.listView1.Columns.Add("系统标识", 60, HorizontalAlignment.Left);

                        for (int i = 0; i < nodes.subtag.Count; i++)
                        {
                            nodes.subtag[i].tagName = nodes.subtag[i].Attribute["name"].ToString().Replace("\"", "");
                            ListViewItem lvi = new ListViewItem();
                            lvi.ImageIndex = 4;     //通过与imageList绑定，显示imageList中第i项图标    
                            lvi.Text = nodes.subtag[i].tagName;
                            detail = aplicationView.listViewDetailConnections.First((s) => s["Description"] == lvi.Text);
                            lvi.SubItems.Add(detail["Server"]);
                            lvi.SubItems.Add(detail["Database"]);
                            lvi.SubItems.Add(detail["MSSysName"]);

                            aplicationView.listView1.Items.Add(lvi);
                        }
                        break;
                    default:
                        break;
                }
                aplicationView.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。
            }

        }
    }
}
