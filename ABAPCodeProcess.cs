using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Product.Code
{
    class ABAPCodeProcess
    {
        public static string[] Keywords;
        const string keywordstring = "ABAP-SOURCE ABBREVIATED ABS ABSTRACT ACCEPT ACCEPTING ACCORDING ACTIVATION ACTUAL ADD ADD-CORRESPONDING ADJACENT " +
"AFTER ALIAS ALIASES ALIGN ALL ALLOCATE ALPHA ANALYSIS ANALYZER AND ANY APPEND APPENDAGE APPENDING APPLICATION ARCHIVE " +
"AREA ARITHMETIC AS ASCENDING ASSERT ASSIGN ASSIGNED ASSIGNING ASSOCIATION ASYNCHRONOUS AT ATTRIBUTES AUTHORITY AUTHORITY-CHECK AVG " +
"BACK BACKGROUND BACKUP BACKWARD BADI BASE BEFORE BEGIN BETWEEN BIG BINARY BIT BIT-AND BIT-NOT BIT-OR BIT-XOR BLACK BLANK BLANKS " +
"BLOB BLOCK BLOCKS BLUE BOUND BOUNDARIES BOUNDS BOXED BREAK-POINT BT BUFFER BY BYPASSING BYTE BYTE-CA BYTE-CN " +
"BYTE-CO BYTE-CS BYTE-NA BYTE-NS BYTE-ORDER CA CALL CALLING CASE CAST CASTING CATCH CEIL CENTER CENTERED CHAIN " +
"CHAIN-INPUT CHAIN-REQUEST CHANGE CHANGING CHANNELS CHAR CHAR-TO-HEX CHARACTER CHECK CHECKBOX CIRCULAR CLASS CLASS-CODING " +
"CLASS-DATA CLASS-EVENTS CLASS-METHODS CLASS-POOL CLEANUP CLEAR CLIENT CLNT CLOB CLOCK CLOSE CN CO COALESCE CODE CODING " +
"COL_BACKGROUND COL_GROUP COL_HEADING COL_KEY COL_NEGATIVE COL_NORMAL COL_POSITIVE COL_TOTAL COLLECT COLOR COLUMN " +
"COLUMNS COMMENT COMMENTS COMMIT COMMON COMMUNICATION COMPARING COMPONENT COMPONENTS COMPRESSION COMPUTE CONCAT CONCATENATE COND " +
"CONDENSE CONDITION CONNECTION CONSTANTS CONTEXT CONTEXTS CONTINUE CONTROL CONTROLS CONV CONVERSION " +
"CONVERT COPIES COPY CORRESPONDING COUNT COUNTRY COVER CP CPI CREATE CREATING CRITICAL CS CUKY CURR CURRENCY " +
"CURRENT CURSOR CURSOR-SELECTION CUSTOMER CUSTOMER-FUNCTION DANGEROUS DATA DATABASE DATAINFO DATASET DATE " +
"DATS  DAYLIGHT DD/MM/YY DD/MM/YYYY DDMMYY DEALLOCATE DEC DECIMALS DECLARATIONS DEEP DEFAULT " +
"DEFERRED DEFINE DEFINING DEFINITION DELETE DELETING DEMAND DEPARTMENT DESCENDING DESCRIBE DESTINATION DETAIL " +
"DIALOG DIRECTORY DISPLAY DISPLAY-MODE DISTANCE DISTINCT DIV DIVIDE DIVIDE-CORRESPONDING DO " +
"DUMMY DUPLICATE DUPLICATES DURATION DURING DYNAMIC DYNPRO E EDIT EDITOR-CALL ELSE ELSEIF " +
"EMPTY ENABLED ENABLING ENCODING END END-ENHANCEMENT-SECTION END-LINES END-OF-DEFINITION END-OF-FILE END-OF-PAGE " +
"END-OF-SELECTION END-TEST-INJECTION END-TEST-SEAM ENDAT ENDCASE ENDCATCH ENDCHAIN ENDCLASS ENDDO ENDENHANCEMENT " +
"ENDEXEC ENDFORM ENDFUNCTION ENDIAN ENDIF ENDING ENDINTERFACE ENDLOOP ENDMETHOD ENDMODULE ENDON ENDPROVIDE " +
"ENDSELECT ENDTRY ENDWHILE ENDWITH ENGINEERING ENHANCEMENT ENHANCEMENT-POINT ENHANCEMENT-SECTION ENHANCEMENTS " +
"ENTRIES ENTRY ENVIRONMENT EQ EQUIV ERRORS ESCAPE ESCAPING EVENT EVENTS EXACT EXCEPT EXCEPTION EXCEPTION-TABLE " +
"EXCEPTIONS EXCLUDE EXCLUDING EXEC EXISTS EXIT EXIT-COMMAND EXPAND EXPANDING EXPIRATION EXPLICIT EXPONENT EXPORT " +
"EXPORTING EXTENDED EXTENSION EXTRACT FAIL FETCH FIELD FIELD-GROUPS FIELD-SYMBOL FIELD-SYMBOLS FIELDS FILE FILTER " +
"FILTER-TABLE FILTERS FINAL FIND FIRST FIRST-LINE FIXED-POINT FKEQ FKGE FLOOR FLTP FLUSH FONT FOR FORM FORMAT " +
"FORWARD FOUND FRAME FRAMES FREE FRIENDS FROM FUNCTION FUNCTION-POOL FUNCTIONALITY FURTHER GAPS GE GENERATE GET GIVING " +
"GKEQ GKGE GLOBAL GREEN GROUP GROUPS GT HANDLE HANDLER HARMLESS HASHED HAVING HDB HEAD-LINES HEADER HEADERS HEADING " +
"HELP-ID HELP-REQUEST HIDE HIGH HINT HOLD HOTSPOT I ICON ID IDENTIFICATION IDENTIFIER IDS IF IGNORE IGNORING IMMEDIATELY " +
"IMPLEMENTATION IMPLEMENTATIONS IMPLEMENTED IMPLICIT IMPORT IMPORTING IN INACTIVE INCL INCLUDE INCLUDES INCLUDING " +
"INCREMENT INDEX INDEX-LINE INFOTYPES INHERITING INIT INITIAL INITIALIZATION INNER INPUT INSERT INSTANCE INSTANCES " +
"INT1 INT2 INT4 INT8 INTENSIFIED INTERFACE INTERFACE-POOL INTERFACES INTERNAL INTERVALS INTO INVERSE INVERTED-DATE IS " +
"ISO JOB JOIN KEEP KEEPING KERNEL KEY KEYS KEYWORDS KIND LANG LANGUAGE LAST LATE LAYOUT LE LEADING LEAVE LEFT LEFT-JUSTIFIED " +
"LEFTPLUS LEFTSPACE LEGACY LENGTH LET LEVEL LEVELS LIKE LINE LINE-COUNT LINE-SELECTION LINE-SIZE LINEFEED LINES LIST " +
"LIST-PROCESSING LISTBOX LITTLE LLANG LOAD LOAD-OF-PROGRAM LOB LOCAL LOCALE LOCATOR LOG-POINT LOGFILE LOGICAL LONG LOOP LOW " +
"LOWER LPAD LPI LT LTRIM M MAIL MAIN MAPPING MARGIN MARK MASK MATCH MATCHCODE MAX MAXIMUM MEDIUM MEMBERS MEMORY MESH MESSAGE " +
"MESSAGE-ID MESSAGES MESSAGING METHOD METHODS MIN MINIMUM MM/DD/YY MM/DD/YYYY MMDDYY MOD MODE MODIF MODIFIER MODIFY MODULE " +
"MOVE MOVE-CORRESPONDING MULTIPLY MULTIPLY-CORRESPONDING NA NAME NAMETAB NATIVE NB NE NESTED NESTING NEW NEW-LINE NEW-PAGE " +
"NEW-SECTION NEXT NO NO-DISPLAY NO-EXTENSION NO-GAP NO-GAPS NO-GROUPING NO-HEADING NO-SCROLLING NO-SIGN NO-TITLE NO-TOPOFPAGE " +
"NO-ZERO NODE NODES NON-UNICODE NON-UNIQUE NOT NP NS NULL NUMBER NUMC O OBJECT OBJECTS OBLIGATORY OCCURRENCE OCCURRENCES OCCURS " +
"OF OFF OFFSET ON ONLY OPEN OPTION OPTIONAL OPTIONS OR ORDER OTHER OTHERS OUT OUTER OUTPUT OUTPUT-LENGTH OVERFLOW OVERLAY PACK " +
"PACKAGE PAD PADDING PAGE PAGES PARAMETER PARAMETER-TABLE PARAMETERS PART PARTIALLY PATTERN PERCENTAGE PERFORM PERFORMING PERSON " +
"PF PF-STATUS PINK PLACES POOL POS_HIGH POS_LOW POSITION PRAGMAS PRECOMPILED PREFERRED PRESERVING PRIMARY PRINT PRINT-CONTROL " +
"PRIORITY PRIVATE PROCEDURE PROCESS PROGRAM PROPERTY PROTECTED PROVIDE PUBLIC PUSH PUSHBUTTON PUT QUAN QUEUE-ONLY QUICKINFO " +
"RADIOBUTTON RAISE RAISING RANGE RANGES RAW READ READ-ONLY READER RECEIVE RECEIVED RECEIVER RECEIVING RED REDEFINITION REDUCE " +
"REDUCED REF REFERENCE REFRESH REGEX REJECT REMOTE RENAMING REPLACE REPLACEMENT REPLACING REPORT REQUEST REQUESTED RESERVE RESET " +
"RESOLUTION RESPECTING RESPONSIBLE RESULT RESULTS RESUMABLE RESUME RETRY RETURN RETURNCODE RETURNING RIGHT RIGHT-JUSTIFIED " +
"RIGHTPLUS RIGHTSPACE RISK RMC_COMMUNICATION_FAILURE RMC_INVALID_STATUS RMC_SYSTEM_FAILURE ROLLBACK ROUND ROWS RTRIM RUN SAP " +
"SAP-SPOOL SAVING SCALE_PRESERVING SCALE_PRESERVING_SCIENTIFIC SCAN SCIENTIFIC SCIENTIFIC_WITH_LEADING_ZERO SCREEN SCROLL " +
"SCROLL-BOUNDARY SCROLLING SEARCH SECONDARY SECONDS SECTION SELECT SELECT-OPTIONS SELECTION SELECTION-SCREEN SELECTION-SET " +
"SELECTION-SETS SELECTION-TABLE SELECTIONS SEND SEPARATE SEPARATED SET SHARED SHIFT SHORT SHORTDUMP-ID SIGN SIGN_AS_POSTFIX " +
"SIMPLE SINGLE SIZE SKIP SKIPPING SMART SOME SORT SORTABLE SORTED SOURCE SPACE SPECIFIED SPLIT SPOOL SPOTS SQL SQLSCRIPT SSTRING " +
"STABLE STAMP STANDARD START-OF-SELECTION STARTING STATE STATEMENT STATEMENTS STATIC STATICS STATUSINFO STEP-LOOP STOP STRUCTURE " +
"STRUCTURES STYLE SUBKEY SUBMATCHES SUBMIT SUBROUTINE SUBSCREEN SUBSTRING SUBTRACT SUBTRACT-CORRESPONDING SUFFIX SUM SUMMARY " +
"SUMMING SUPPLIED SUPPLY SUPPRESS SWITCH SWITCHSTATES SYMBOL SYNCPOINTS SYNTAX SYNTAX-CHECK SYNTAX-TRACE SYSTEM-CALL " +
"SYSTEM-EXCEPTIONS SYSTEM-EXIT TAB TABBED TABLE TABLES TABLEVIEW TABSTRIP TARGET TASK TASKS TEST TEST-INJECTION TEST-SEAM TESTING " +
"TEXT TEXTPOOL THEN THROW TIME TIMES TIMESTAMP TIMEZONE TIMS TITLE TITLE-LINES TITLEBAR ?TO TO TOKENIZATION TOKENS TOP-LINES " +
"TOP-OF-PAGE TRACE-FILE TRACE-TABLE TRAILING TRANSACTION TRANSFER TRANSFORMATION TRANSLATE TRANSPORTING TRMAC TRUNCATE TRUNCATION " +
"TRY TYPE TYPE-POOL TYPE-POOLS TYPES ULINE UNASSIGN UNDER UNICODE UNION UNIQUE UNIT UNIX UNPACK UNTIL UNWIND UP UPDATE UPPER USER " +
"USER-COMMAND USING UTF VALID VALUE VALUE-REQUEST VALUES VARY VARYING VERIFICATION-MESSAGE VERSION VIA VISIBLE WAIT WHEN " +
"WHERE WHILE WIDTH WINDOW WINDOWS WITH WITH-HEADING WITH-TITLE WITHOUT WORD WORK WRITE WRITER XML XSD YELLOW YES YYMMDD Z ZERO ZONE";
        //static ColorProcess{}
        public enum CodeType{
            /// <summary>
            /// 字符串,如 'sdas'
            /// </summary>
            String,
            /// <summary>
            /// 以*开头的注释
            /// </summary>
            Comment,
            /// <summary>
            /// 关键字
            /// </summary>
            Keyword,
            /// <summary>
            /// 数字
            /// </summary>
            Number,
            /// <summary>
            /// 以"开头的注释
            /// </summary>
            Hint,
            /// <summary>
            /// 一般变量，字段,表,后面考虑到代码的链接和跳转，需要给一个path地址属性
            /// </summary>
            Variable,
            /// <summary>
            /// 操作符+-*/ 可能后面会遇到->这种转义的语法，可能逻辑流要改动
            /// </summary>
            Operator,
            /// <summary>
            /// 圆括号（）用处不大先这样放着，以后有时间来改
            /// </summary>
            Punctuation,
            /// <summary>
            /// 空格，要考虑到代码缩进对应几个空格
            /// </summary>
            Blank,
            /// <summary>
            /// 语法错误，即使代码语法有误，也要展示出来，其文本为红色，并且有下波浪线
            /// </summary>
            Error,
        }

        public struct LineElement
        {
            public CodeType type; //展示样式
            public string content; //展示的文本内容
            public int start;      //该代码片段在原代码的位置，为后续代码快速索引定位做准备
            public int length;    //代码片段的长度，没什么用
            public string path;     //代码可能包含的链接，比如一些字段或者表，函数，子例程，当单击该标签的时候，根据该地址跳转到另外一个地方去；如果是错误代码，也可作为浮动提示文本的内容信息
        }
        //List<LineElement> Line;
        public class CodeLine
        {
            public string lineCode;
            public List<LineElement> elements;
            public CodeLine()
            {
                lineCode = "";
                elements = new List<LineElement>();
            }
        }
        public List<CodeLine> CodeList;

        string[] codestring;
        public ABAPCodeProcess()
        {
            Keywords = keywordstring.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
        public ABAPCodeProcess(string code)
        {
            Keywords = keywordstring.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            code = code.Replace("\t", "  ");
            this.codestring = code.Split(new string[]{"\r\n"},StringSplitOptions.None);
            convertcodetoList();
        }
        public ABAPCodeProcess(string[] code)
        {
            Keywords = keywordstring.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            this.codestring = code;
        }

        public enum CodeNodeStyle
        {
            /// <summary>
            /// 字母A~Z a~z 包括%
            /// </summary>
            A,
            /// <summary>
            /// 数字0~9
            /// </summary>
            D,
            /// <summary>
            /// 小数点.语句结束标识
            /// </summary>
            Dot,
            /// <summary>
            /// 逗号，
            /// </summary>
            Comma,
            /// <summary>
            /// 单引号'
            /// </summary>
            SingleQuote,
            /// <summary>
            /// 双引号"
            /// </summary>
            DoubleQuote,
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
            /// 制表符，转换为两个空格
            /// </summary>
            Tab,
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
            /// 左圆括号(
            /// </summary>
            LP,
            /// <summary>
            /// 右圆括号)
            /// </summary>
            GP,
            /// <summary>
            /// ?
            /// </summary>
            Question,
            /// <summary>
            /// +
            /// </summary>
            Add,
            /// <summary>
            /// -
            /// </summary>
            Minus,
            /// <summary>
            ///*
            /// </summary>
            Multiply,
            /// <summary>
            /// 除号/
            /// </summary>
            Bias,
            /// <summary>
            /// 感叹号！注释<!--...-->【现在不用】
            /// </summary>
            Exclam,
            /// <summary>
            /// 其它字符,如中文
            /// </summary>
            Others,
        }
        private CodeNodeStyle getNodeStyle(char node)
        {
            CodeNodeStyle nodeStyle;
            if (node >= 'A' && node <= 'Z' || node >= 'a' && node <= 'z'||node=='%')
            {
                nodeStyle = CodeNodeStyle.A;
            }
            else if (node >= '0' && node <= '9')
            {
                nodeStyle = CodeNodeStyle.D;
            }
            else if (node == '.')
            {
                nodeStyle = CodeNodeStyle.Dot;
            }
            else if (node == ',')
            {
                nodeStyle = CodeNodeStyle.Comma;
            }
            else if (node == '\'')
            {
                nodeStyle = CodeNodeStyle.SingleQuote;
            }
            else if (node == '"')
            {
                nodeStyle = CodeNodeStyle.DoubleQuote;
            }
            else if (node == '/')
            {
                nodeStyle = CodeNodeStyle.Bias;
            }
            else if (node == '=')
            {
                nodeStyle = CodeNodeStyle.Equal;
            }
            else if (node == ':')
            {
                nodeStyle = CodeNodeStyle.Colon;
            }
            else if (node == ' ')
            {
                nodeStyle = CodeNodeStyle.Space;
            }
            else if (node == '\t')
            {
                nodeStyle = CodeNodeStyle.Tab;
            }
            else if (node == '\r' || node == '\n')
            {
                nodeStyle = CodeNodeStyle.ESC;
            }
            else if (node == '>')
            {
                nodeStyle = CodeNodeStyle.GT;
            }
            else if (node == '<')
            {
                nodeStyle = CodeNodeStyle.LT;
            }
            else if (node == '(')
            {
                nodeStyle = CodeNodeStyle.LP;
            }
            else if (node==')')
            {
                nodeStyle = CodeNodeStyle.GP;
            }
            else if (node == '?')
            {
                nodeStyle = CodeNodeStyle.Question;
            }
            else if (node == '-')
            {
                nodeStyle = CodeNodeStyle.Minus;
            }
            else if (node == '!')
            {
                nodeStyle = CodeNodeStyle.Exclam;
            }
            else
            {
                nodeStyle = CodeNodeStyle.Others;
            }
            return nodeStyle;
        }
        public enum CodeMode
        {
            /// <summary>
            /// 0:第一个字符之前的状态,还未开始搜索
            /// </summary>
            Init,
            /// <summary>
            /// 2:处于单词中
            /// </summary>
            InKey,
            /// <summary>
            /// 处于数字中
            /// </summary>
            InNum,
            /// <summary>
            /// 3:退出单词【没用了，可能要删掉】
            /// </summary>
            EndKey,
            /// <summary>
            ///进入空格之中 
            /// </summary>
            InSpace,
            /// <summary>
            /// 1:正确进入双引号模式
            /// </summary>
            EnterD,
            /// <summary>
            /// 2:正确进入单引号模式
            /// </summary>
            EnterS,
            /// <summary>
            /// 2:正确退出单引号模式
            /// </summary>
            OutS,
            /// <summary>
            /// 进入标点符号/ ＜ ＞ （） = + -【以后要细分】
            /// </summary>
            EnterTrunc,
            /// <summary>
            /// 20:异常
            /// </summary>
            Error,
        }
        public void convertcodetoList()
        {
            CodeList = new List<CodeLine>();
            LineElement lineElement;
            CodeLine codeLine;
            int searchIndex = 0;
            foreach (string item in codestring)
            {
                searchIndex = 0;
                codeLine = new CodeLine();
                codeLine.lineCode = item;
                if (String.IsNullOrEmpty(item))
                {
                    lineElement = new LineElement();
                    lineElement.content = "";
                    lineElement.length = 0;
                    lineElement.path = "";
                    lineElement.start = 0;
                    lineElement.type = CodeType.Blank;
                    codeLine.elements.Add(lineElement);
                    CodeList.Add(codeLine);
                    continue;
                }
                if (item[searchIndex].Equals('*'))
                {
                    lineElement = new LineElement();
                    lineElement.content = item;
                    lineElement.length = item.Length;
                    lineElement.path = "";
                    lineElement.start = 0;
                    lineElement.type = CodeType.Comment;
                    codeLine.elements.Add(lineElement);
                    CodeList.Add(codeLine);
                    continue;
                }
                CodeMode currentMode = CodeMode.Init;

                int KeyStart = 0;//当前搜索位置
                for (int i = 0; i < item.Length; i++)
                {
                    switch (getNodeStyle(item[i]))
                    {
                        case CodeNodeStyle.A:
                        case CodeNodeStyle.Others:
                            switch (currentMode)
	                        {
                                case CodeMode.Init:
                                    currentMode = CodeMode.InKey;
                                    KeyStart = i;
                                    break;
                                case CodeMode.EnterTrunc:
                                    currentMode = CodeMode.InKey;
                                    break;
                                case CodeMode.InKey:
                                    break;
                                case CodeMode.InNum:
                                    //变量不能以字母开头
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.InSpace:
                                    currentMode = CodeMode.InKey;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart,i-KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = CodeType.Blank;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.EndKey:
                                    currentMode = CodeMode.InKey;
                                    KeyStart = i;
                                    break;
                                case CodeMode.EnterD:
                                    break;
                                case CodeMode.EnterS:
                                    currentMode = CodeMode.EnterS;
                                    break;
                                case CodeMode.OutS:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.Error:
                                    break;
                                default:
                                    break;
	                        }
                            break;
                        case CodeNodeStyle.D:
                            switch (currentMode)
                            {
                                case CodeMode.Init:
                                    currentMode = CodeMode.InNum;
                                    KeyStart = i;
                                    break;
                                case CodeMode.InKey:
                                    break;
                                case CodeMode.EnterTrunc:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.InNum:
                                    //变量不能以字母开头
                                    currentMode = CodeMode.InNum;
                                    break;
                                case CodeMode.InSpace:
                                    currentMode = CodeMode.InNum;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart, i - KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = CodeType.Blank;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.EndKey:
                                    currentMode = CodeMode.InNum;
                                    break;
                                case CodeMode.EnterD:
                                    break;
                                case CodeMode.EnterS:
                                    currentMode = CodeMode.EnterS;
                                    break;
                                case CodeMode.OutS:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.Error:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case CodeNodeStyle.Dot:
                        case CodeNodeStyle.Comma:
                        case CodeNodeStyle.LP:
                        case CodeNodeStyle.GP:
                            switch (currentMode)
                            {
                                case CodeMode.Init:
                                    currentMode = CodeMode.EnterTrunc;
                                    break;
                                case CodeMode.InKey:
                                    currentMode = CodeMode.EnterTrunc;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart,i-KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = isKeyword(lineElement.content)? CodeType.Keyword:CodeType.Variable;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.InNum:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.InSpace:
                                    currentMode = CodeMode.EnterTrunc;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart, i - KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = CodeType.Blank;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.EndKey:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.EnterD:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.EnterS:
                                    currentMode = CodeMode.EnterS;
                                    break;
                                case CodeMode.OutS:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.EnterTrunc:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.Error:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case CodeNodeStyle.SingleQuote:
                            switch (currentMode)
                            {
                                case CodeMode.Init:
                                    currentMode = CodeMode.EnterS;
                                    break;
                                case CodeMode.EnterTrunc:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.InKey:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.InNum:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.InSpace:
                                    currentMode = CodeMode.EnterS;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart, i - KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = CodeType.Blank;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.EndKey:
                                    currentMode = CodeMode.EnterS;
                                    break;
                                case CodeMode.EnterD:
                                    break;
                                case CodeMode.EnterS:
                                    currentMode = CodeMode.OutS;
                                    break;
                                case CodeMode.OutS:
                                    currentMode = CodeMode.EnterS;
                                    break;
                                case CodeMode.Error:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case CodeNodeStyle.DoubleQuote:
                            switch (currentMode)
                            {
                                case CodeMode.Init:
                                case CodeMode.EndKey://直接让本行结束，提高效率
                                    currentMode = CodeMode.EnterD;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(i);
                                    lineElement.length = item.Length-i;;
                                    lineElement.path = "";
                                    lineElement.start = i;
                                    lineElement.type = CodeType.Hint;
                                    codeLine.elements.Add(lineElement);
                                    i = item.Length;
                                    break;
                                case CodeMode.InSpace:
                                    currentMode = CodeMode.EnterD;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart, i - KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = CodeType.Blank;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.EnterTrunc:
                                    currentMode = CodeMode.EnterD;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart, i - KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = CodeType.Punctuation;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.InKey:
                                    currentMode = CodeMode.EnterD;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart, i - KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = isKeyword(lineElement.content) ? CodeType.Keyword : CodeType.Variable;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.InNum:
                                    currentMode = CodeMode.EnterD;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart, i - KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = CodeType.Number;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.EnterD:
                                    currentMode = CodeMode.EnterD;
                                    break;
                                case CodeMode.EnterS:
                                    currentMode = CodeMode.EnterS;
                                    break;
                                case CodeMode.OutS:
                                    currentMode = CodeMode.EnterD;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart, i - KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = CodeType.String;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.Error:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case CodeNodeStyle.Bias:
                            switch (currentMode)
                            {
                                case CodeMode.Init:
                                    currentMode = CodeMode.EnterTrunc;
                                    KeyStart = i;
                                    break;
                                case CodeMode.InKey:
                                    break;
                                case CodeMode.EnterTrunc:
                                    currentMode = CodeMode.InKey;
                                    break;
                                case CodeMode.InNum:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.InSpace:
                                    currentMode = CodeMode.EnterTrunc;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart, i - KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = CodeType.Blank;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.EndKey:
                                    currentMode = CodeMode.EnterTrunc;
                                    KeyStart = i;
                                    break;
                                case CodeMode.EnterD:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.EnterS:
                                    currentMode = CodeMode.EnterS;
                                    break;
                                case CodeMode.OutS:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.Error:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case CodeNodeStyle.Equal:
                        case CodeNodeStyle.Minus:
                        case CodeNodeStyle.Exclam:
                            switch (currentMode)
                            {
                                case CodeMode.Init:
                                    currentMode = CodeMode.EnterTrunc;
                                    KeyStart = i;
                                    break;
                                case CodeMode.InKey:
                                    currentMode = CodeMode.InKey;
                                    break;
                                case CodeMode.InNum:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.InSpace:
                                    currentMode = CodeMode.EnterTrunc;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart, i - KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = CodeType.Blank;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.EndKey:
                                    currentMode = CodeMode.EnterTrunc;
                                    KeyStart = i;
                                    break;
                                case CodeMode.EnterD:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.EnterS:
                                    currentMode = CodeMode.EnterS;
                                    break;
                                case CodeMode.OutS:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.EnterTrunc:
                                    currentMode = CodeMode.EnterTrunc;
                                    break;
                                case CodeMode.Error:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case CodeNodeStyle.Colon:
                        case CodeNodeStyle.Question:
                            switch (currentMode)
                            {
                                case CodeMode.Init:
                                    currentMode = CodeMode.EnterTrunc;
                                    KeyStart = i;
                                    break;
                                case CodeMode.InKey:
                                    currentMode = CodeMode.EnterTrunc;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart,i-KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = isKeyword(lineElement.content) ? CodeType.Keyword : CodeType.Variable;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.InNum:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.InSpace:
                                    currentMode = CodeMode.EnterTrunc;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart, i - KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = CodeType.Blank;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.EndKey:
                                    currentMode = CodeMode.EnterTrunc;
                                    KeyStart = i;
                                    break;
                                case CodeMode.EnterD:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.EnterS:
                                    currentMode = CodeMode.EnterS;
                                    break;
                                case CodeMode.OutS:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.EnterTrunc:
                                    currentMode = CodeMode.EnterTrunc;
                                    break;
                                case CodeMode.Error:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case CodeNodeStyle.Tab:

                            break;
                        case CodeNodeStyle.ESC://如果没有分行
                            break;
                        case CodeNodeStyle.Space:
                            switch (currentMode)
                            {
                                case CodeMode.Init:
                                    currentMode = CodeMode.InSpace;
                                    KeyStart = i;
                                    break;
                                case CodeMode.InKey:
                                    currentMode = CodeMode.InSpace;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart, i - KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = isKeyword(lineElement.content) ? CodeType.Keyword : CodeType.Variable;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.InNum:
                                    currentMode = CodeMode.InSpace;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart, i - KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = CodeType.Number;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.InSpace:
                                    currentMode = CodeMode.InSpace;
                                    break;
                                case CodeMode.EndKey:
                                    currentMode = CodeMode.InSpace;
                                    KeyStart = i;
                                    break;
                                case CodeMode.EnterD:
                                    currentMode = CodeMode.EnterD;
                                    break;
                                case CodeMode.EnterS:
                                    currentMode = CodeMode.EnterS;
                                    break;
                                case CodeMode.OutS:
                                    currentMode = CodeMode.InSpace;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart, i - KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = CodeType.String;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.EnterTrunc:
                                    currentMode = CodeMode.InSpace;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart, i - KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = CodeType.Operator;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.Error:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case CodeNodeStyle.LT:
                        case CodeNodeStyle.GT:
                            switch (currentMode)
                            {
                                case CodeMode.Init:
                                    currentMode = CodeMode.EnterTrunc;
                                    KeyStart = i;
                                    break;
                                case CodeMode.InKey:
                                    currentMode = CodeMode.InKey;
                                    break;
                                case CodeMode.InNum:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.InSpace:
                                    currentMode = CodeMode.EnterTrunc;
                                    lineElement = new LineElement();
                                    lineElement.content = item.Substring(KeyStart, i - KeyStart);
                                    lineElement.length = i - KeyStart;
                                    lineElement.path = "";
                                    lineElement.start = KeyStart;
                                    lineElement.type = CodeType.Blank;
                                    codeLine.elements.Add(lineElement);
                                    KeyStart = i;
                                    break;
                                case CodeMode.EndKey:
                                    currentMode = CodeMode.EnterTrunc;
                                    KeyStart = i;
                                    break;
                                case CodeMode.EnterD:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.EnterS:
                                    currentMode = CodeMode.EnterS;
                                    break;
                                case CodeMode.OutS:
                                    currentMode = CodeMode.Error;
                                    break;
                                case CodeMode.EnterTrunc:
                                    currentMode = CodeMode.EnterTrunc;
                                    break;
                                case CodeMode.Error:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }

                //结束段处理
                if (currentMode != CodeMode.EndKey)
                {
                    lineElement = new LineElement();
                    lineElement.content = item.Substring(KeyStart, item.Length - KeyStart);
                    lineElement.length = item.Length - KeyStart;
                    lineElement.path = "";
                    lineElement.start = KeyStart;
                    switch (currentMode)
                    {
                        case CodeMode.Init:
                            break;
                        case CodeMode.InKey:
                            lineElement.type = isKeyword(lineElement.content) ? CodeType.Keyword : CodeType.Variable;
                            break;
                        case CodeMode.InNum:
                            lineElement.type = CodeType.Number;
                            break;
                        case CodeMode.EndKey:
                            //lineElement.type = CodeType.Blank;
                            break;
                        case CodeMode.EnterD:
                            lineElement.type = CodeType.Hint;
                            break;
                        case CodeMode.EnterS:
                            //lineElement.type = CodeType.Hint;
                            break;
                        case CodeMode.OutS:
                            //lineElement.type = CodeType.;
                            break;
                        case CodeMode.EnterTrunc:
                            lineElement.type = CodeType.Punctuation;
                            break;
                        case CodeMode.Error:
                            lineElement.type = CodeType.Error;
                            break;
                        default:
                            break;
                    }
                    //lineElement.type = isKeyword();
                    codeLine.elements.Add(lineElement);
                    currentMode = CodeMode.EndKey;
                }
                CodeList.Add(codeLine);
            }
        } 
        private bool isKeyword(string keyword)
        {
            return Keywords.Any(s => s.Equals(keyword.ToUpper()));
        }
    }
}
