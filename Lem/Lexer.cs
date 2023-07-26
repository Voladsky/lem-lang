using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lem
{
    public enum TokenType {
        Int, DoubleLiteral, StringLiteral,
        Id,
        Plus, Minus, Multiply, Divide, Dot,
        Semicolon, LPar, RPar, Assign, AssignPlus, AssignMinus, AssignMult, AssignDiv, LBrace, RBrace, Comma, Colon,
        Equal, Less, LessEqual, Greater, GreaterEqual, NotEqual,
        tkAnd, tkOr, tkNot,
        Eof,
        tkPrint, tkTrue, tkFalse, tkIf, tkThen, tkElse, tkWhile, tkFor, tkDo
    }
    internal class Token: TokenBase
    {
        public TokenType type;
        public Token(TokenType type, Position pos, object value = null) : base(value, pos)
        {
            this.type = type;
        }
    }
    
    internal class Lexer: LexerBase
    {
        public Lexer(string code) : base(code) { }
        private Token GetIdentifier(Position startPos)
        {
            while (IsAlphaNumeric(PeekChar()))
                NextChar();
            var val = code.Substring(start, cur - start);
            var type = TokenType.Id;
            if (TokenConsts.Keywords.ContainsKey(val))
                type = TokenConsts.Keywords[val];
            return new Token(type, startPos, val);
        }
        private Token GetString(Position startPos)
        {
            while (PeekChar() != '"' && !IsAtEnd())
                NextChar();
            NextChar(); // skip "
            var val = code.Substring(start + 1, cur - start - 2);
            return new Token(TokenType.StringLiteral, startPos, val);
        }
        private Token GetNumber(Position startPos)
        {
            while (char.IsDigit(PeekChar()))
                NextChar();
            if (PeekChar() == '.' && char.IsDigit(PeekNextChar()))
            {
                NextChar();
                while (char.IsDigit(PeekChar()))
                    NextChar();
                var val = code.Substring(start, cur - start);
                var re = double.Parse(val, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                return new Token(TokenType.DoubleLiteral, startPos, re);
            }
            else
            {
                var val = code.Substring(start, cur - start);
                return new Token(TokenType.Int, startPos, int.Parse(val));
            }
        }
        public Token NextToken()
        {
            var c = NextChar();
            var endls = new HashSet<char>() { '\n', ' ', '\t' };
            while (endls.Contains(c))
                c = NextChar();
            var pos = CurrentPosition();
            Token res = null;
            start = cur - 1;
            switch (c)
            {
                case (char)0:
                    res = new Token(TokenType.Eof, pos, "Eof");
                    break;
                case ',':
                    res = new Token(TokenType.Comma, pos);
                    break;
                case ')':
                    res = new Token(TokenType.RPar, pos);
                    break;
                case '(':
                    res = new Token(TokenType.LPar, pos);
                    break;
                case '}':
                    res = new Token(TokenType.RBrace, pos);
                    break;
                case '{':
                    res = new Token(TokenType.LBrace, pos);
                    break;
                case '+':
                    res = new Token(IsMatch('=') ? TokenType.AssignPlus : TokenType.Plus, pos);
                    break;
                case '-':
                    res = new Token(IsMatch('=') ? TokenType.AssignMinus : TokenType.Minus, pos);
                    break;
                case '*':
                    res = new Token(IsMatch('=') ? TokenType.AssignMult : TokenType.Multiply, pos);
                    break;
                case '/':
                    if (IsMatch('/'))
                    {
                        while (PeekChar() != (char)10 && !IsAtEnd())
                            NextChar();
                        break;
                    }
                    else res = new Token(IsMatch('=') ? TokenType.AssignDiv : TokenType.Divide, pos);
                    break;
                case ';':
                    res = new Token(TokenType.Semicolon, pos);
                    break;
                case '!':
                    res = new Token(IsMatch('=') ? TokenType.NotEqual : TokenType.tkNot, pos);
                    break;
                case '=':
                    res = new Token(IsMatch('=') ? TokenType.Equal : TokenType.Assign, pos);
                    break;
                case '>':
                    res = new Token(IsMatch('=') ? TokenType.GreaterEqual : TokenType.Greater, pos);
                    break;
                case '<':
                    res = new Token(IsMatch('=') ? TokenType.LessEqual : TokenType.Less, pos);
                    break;
                case '&':
                    if (IsMatch('&'))
                    {
                        res = new Token(TokenType.tkAnd, pos);
                        break;
                    }
                    else throw new Exception("Неверный символ после &");
                case '|':
                    if (IsMatch('|'))
                    {
                        res = new Token(TokenType.tkOr, pos);
                        break;
                    }
                    else throw new Exception("Неверный символ после &");
                case '"':
                    res = GetString(pos);
                    break;
                default:
                    if (char.IsDigit(c))
                    {
                        res = GetNumber(pos);
                        break;
                    }
                    else if (IsAlpha(c))
                    {
                        res = GetIdentifier(pos);
                        break;
                    }
                    else throw new Exception($"Неизвестный символ в позиции {pos.Line},{pos.Column}");
            }
            if (res.value == null)
                res.value = code.Substring(start, cur - start);
            return res;
        }

    }
}
