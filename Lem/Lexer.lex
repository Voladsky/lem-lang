%using Parser;
%using QUT.Gppg;
%using System.Linq;
%using ParserHelper;

%namespace Lexer

Alpha 	[a-zA-Z_]
Digit   [0-9]
AlphaDigit {Alpha}|{Digit}
INTNUM  {Digit}+
REALNUM {INTNUM}\.{INTNUM}
ID {Alpha}{AlphaDigit}*

%%

{INTNUM} {
  yylval.iVal = int.Parse(yytext);
  return (int)Tokens.INTNUM;
}

{REALNUM} {
  yylval.dVal = double.Parse(yytext, CultureInfo.InvariantCulture);
  return (int)Tokens.REALNUM;
}

{ID}  {
  yylval.sVal = yytext;
  int res = ScannerHelper.GetIDToken(yytext);
  return res;
}

"+"  { yylval.sVal = yytext; return (int)Tokens.PLUS; }
"-"  { yylval.sVal = yytext; return (int)Tokens.MINUS; }
"*"  { yylval.sVal = yytext; return (int)Tokens.MULTIPLY; }
"/"  { yylval.sVal = yytext; return (int)Tokens.DIVIDE; }
"<"  { yylval.sVal = yytext; return (int)Tokens.LOWER; }
">"  { yylval.sVal = yytext; return (int)Tokens.GREATER; }

"{"  { return (int)Tokens.LBRACE; }
"}"  { return (int)Tokens.RBRACE; }
"["  { return (int)Tokens.LBRACKET; }
"]"  { return (int)Tokens.RBRACKET; }

"."  { return (int)Tokens.DOT; }
","  { return (int)Tokens.COMMA; }
";"  { return (int)Tokens.SEMICOLON; }
"("  { return (int)Tokens.LPAR; }
")"  { return (int)Tokens.RPAR; }
"="  { return (int)Tokens.ASSIGN; }


[^ \r\n] {
	LexError();
	return (int)Tokens.EOF;
}

%{
  yylloc = new LexLocation(tokLin, tokCol, tokELin, tokECol); // ������� ������� (������������� ��� ���������������), ������������ @1 @2 � �.�.
%}

%%

public override void yyerror(string format, params object[] args) // ��������� �������������� ������
{
  var ww = args.Skip(1).Cast<string>().ToArray();
  string errorMsg = string.Format("({0},{1}): Met {2}, expected {3}", yyline, yycol, args[0], string.Join(" ��� ", ww));
  throw new LexException(errorMsg);
}

public void LexError()
{
	string errorMsg = string.Format("({0},{1}): Unknown symbol {2}", yyline, yycol, yytext);
    throw new SyntaxException(errorMsg);
}

class ScannerHelper
{
  private static Dictionary<string,int> keywords;

  static ScannerHelper()
  {
    keywords = new Dictionary<string,int>();
    keywords.Add("for",(int)Tokens.FOR);
    keywords.Add("in",(int)Tokens.IN);
    keywords.Add("while",(int)Tokens.WHILE);
    keywords.Add("if",(int)Tokens.IF);
    keywords.Add("else",(int)Tokens.ELSE);
    keywords.Add("then", (int)Tokens.THEN);
  }
  public static int GetIDToken(string s)
  {
    if (keywords.ContainsKey(s.ToLower())) // ���� �������������� � ��������
      return keywords[s];
    else
      return (int)Tokens.ID;
  }
}
