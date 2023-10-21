%using Lem;

%{
   public StatementListNode root;
   public Parser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
%}

%output = Parser.cs

%namespace Parser

%token FOR, IN, WHILE, IF, ELSE
%token <iVal> INTNUM
%token <dVal> REALNUM
%token LPAR, RPAR, LBRACE, RBRACE, LBRACKET, RBRACKET, DOT, COMMA, SEMICOLON
%token ASSIGN
%token <sVal> ID, PLUS, MINUS, MULTIPLY, DIVIDE
%token <sVal> LOWER, GREATER, LOWEREQUAL, GREATEREQUAL, EQUAL, NOTEQUAL

%left LOWER, GREATER, LOWEREQUAL, GREATEREQUAL, EQUAL, NOTEQUAL
%left PLUS MINUS
%left MULTIPLY DIVIDE

%type <id> ident
%type <ex> expr
%type <exl> exprlist
%type <st> assign ifstatement whilestatement forstatement statement proccall
%type <stl> stlist block
%type <rnd> range

%union {
  public double dVal;
  public string sVal;
  public int iVal;
  public RangeNode rnd;
  public ExprNode ex;
  public ExprListNode exl;
  public IdNode id;
  public StatementNode st;
  public StatementListNode stl;
}

%start progr

%%

progr   : stlist { root = $1; }
		;

stlist	: statement { $$ = new StatementListNode(); $$.Add($1); }
		| stlist statement { $1.Add($2); $$ = $1; }
		;

statement: assign SEMICOLON { $$ = $1; }
		| block	{ $$ = $1; }
		| ifstatement { $$ = $1; }
		| forstatement { $$ = $1; }
		| whilestatement { $$ = $1; }
		| proccall SEMICOLON { $$ = $1; }
		;

ident 	: ID { $$ = new IdNode($1); }
		;

assign 	: ident ASSIGN expr { $$ = new AssignNode($1, $3); }
		;

expr 	: expr PLUS expr { $$ = new BinOpNode($1, $3, $2); }
		| expr MULTIPLY expr { $$ = new BinOpNode($1, $3, $2); }
		| expr MINUS expr { $$ = new BinOpNode($1, $3, $2); }
  		| expr LOWER expr { $$ = new BinOpNode($1, $3, $2); }
		| expr GREATER expr { $$ = new BinOpNode($1, $3, $2); }
		| ident { $$ = $1; }
		| INTNUM { $$ = new IntNode($1); }
		| REALNUM { $$ = new DoubleNode($1); }
		| LPAR expr RPAR { $$ = $2; }
		;

exprlist	: expr { $$ = new ExprListNode(); $$.Add($1); }
			| exprlist COMMA expr { $1.Add($3); $$ = $1; }
			;

ifstatement	: IF expr statement { $$ = new IfNode($2, $3, null); }
			| IF expr block ELSE statement { $$ = new IfNode($2, $3, $5); }
			;
whilestatement	: WHILE expr statement { $$ = new WhileNode($2, $3); }
				;

forstatement	: FOR ident IN range statement { $$ = new ForNode($2, $4, $5); }
				;

range	: LBRACKET expr SEMICOLON expr RBRACKET { $$ = new RangeNode($2, $4); }
		;

proccall	: ident LPAR exprlist RPAR { $$ = new ProcCallNode($1, $3); }
			;

block	: LBRACE stlist RBRACE { $$ = $2; }
		;

%%