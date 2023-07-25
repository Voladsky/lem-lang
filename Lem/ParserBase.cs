using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lem
{
    internal class ParserBase
    {
        private Lexer lex;
        protected int current;
        protected Token curToken;
        public ParserBase(Lexer lex)
        {
            this.lex = lex;

        }
        public Token NextLexem()
        {
            var res = curToken;
            curToken = lex.NextToken();
            if (!IsAtEnd()) current++;
            return res;
        }
        public bool At(params TokenType[] types) => types.Any(type => PeekToken().type == type);
        public void Check(params TokenType[] types)
        {
            if (!At(types)) throw new Exception();
        }
        public bool IsMatch(params TokenType[] types)
        {
            var res = At(types);
            if (res) NextLexem();
            return res;
        }
        public Token Requires(params TokenType[] types)
        {
            if (At(types))
                return NextLexem();
            else throw new Exception();
        }
        public bool IsAtEnd() => PeekToken().type == TokenType.Eof;
        public Token PeekToken() => curToken;
    }
}
