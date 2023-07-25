using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lem
{
    internal static class TokenConsts
    {
        public static TokenType[] ArithmeticOperations = { TokenType.Plus, TokenType.Minus, TokenType.Multiply, TokenType.Divide };
        public static TokenType[] CompareOperations = { TokenType.Equal, TokenType.Less, TokenType.LessEqual, TokenType.Greater, TokenType.GreaterEqual, TokenType.NotEqual };
        public static TokenType[] LogicalOperations = { TokenType.tkAnd, TokenType.tkOr, TokenType.tkNot };
        public static Dictionary<string, TokenType> Keywords = new Dictionary<string, TokenType>()
        {
            {"int", TokenType.Id},
            {"double", TokenType.Id},
            {"bool", TokenType.Id},
            {"string", TokenType.Id},
            {"function", TokenType.Id},
            {"True", TokenType.tkTrue},
            {"False", TokenType.tkFalse},
            {"if", TokenType.tkIf},
            {"then", TokenType.tkThen},
            {"else", TokenType.tkElse},
            {"while", TokenType.tkWhile},
            {"for", TokenType.tkFor},
            {"do", TokenType.tkDo}
        };
    }
}
