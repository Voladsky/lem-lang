using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Lem
{
    internal class LexerBase
    {
        protected string code;
        protected int line = 1;
        protected int column = 0;
        protected int cur = 0;
        protected int start = 0;
        protected bool atEoln = false;
        protected Position CurrentPosition() => new Position(line, column);
        protected bool IsAtEnd() => cur >= code.Length;
        public char NextChar()
        {
            if (atEoln)
            {
                atEoln = false;
                ++line;
                column = 0;
            }
            var res = PeekChar();
            if (res == '\n')
                atEoln = true;
            if (res != '\0')
            {
                ++cur;
                ++column;
            }
            return res;
        }
        protected bool IsMatch(char expected)
        {
            var res = PeekChar() == expected;
            if (res)
                NextChar();
            return res;
        }
        protected char PeekChar() => IsAtEnd() ? (char)0 : code[cur];
        protected char PeekNextChar()
        {
            var pos = cur + 1;
            return pos > code.Length ? (char)0 : code[pos];
        }
        protected bool IsAlpha(char c) => Regex.IsMatch(c.ToString(), @"[A-za-z]");
        protected bool IsAlphaNumeric(char c) => IsAlpha(c) || char.IsDigit(c);
        public string[] Lines => code.Split('\n');
        public LexerBase(string code) => this.code = code;
    }
}
