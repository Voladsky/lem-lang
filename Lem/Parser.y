%using Lem;

%{
   public StatementListNode root;
   public Parser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
%}

%output = Parser.cs

%namespace Parser

%token FOR, IN, WHILE, IF, ELSE, THEN
%token <iVal> INTNUM
%token <dVal> REALNUM
%token LPAR, RPAR, LBRACE, RBRACE, LBRACKET, RBRACKET, DOT, COMMA, SEMICOLON
%token ASSIGN
%token <sVal> ID, PLUS, MINUS, MULTIPLY, DIVIDE
%token <sVal> LOWER, GREATER, LOWEREQUAL, GREATEREQUAL, EQUAL, NOTEQUAL

%type <id> ident
%type <ex> expr comp term factor
%type <exl> exprlist
%type <st> assign ifstatement whilestatement forstatement statement
%type <st> proccall
%type <stl> stlist block
%type <sVal> compOp addOp multOp

%union {
  public double dVal;
  public string sVal;
  public int iVal;
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

expr	: comp { $$ = $1; }
		| comp compOp comp { $$ = new BinOpNode($1, $3, $2); }
		;

exprlist	: expr { $$ = new ExprListNode(); $$.Add($1); }
			| exprlist COMMA expr { $1.Add($3); $$ = $1; }
			;

compOp	: LOWER { $$ = $1; }
		| GREATER
		| LOWEREQUAL
		| GREATEREQUAL
		| EQUAL
		| NOTEQUAL
		;

comp	: term { $$ = $1; }
		| term addOp term { $$ = new BinOpNode($1, $3, $2); }
		;

addOp	: PLUS
		| MINUS
		;

term	: factor { $$ = $1; }
		| factor multOp factor { $$ = new BinOpNode($1, $3, $2); }
		;

multOp	: MULTIPLY
		| DIVIDE
		;

factor	: INTNUM { $$ = new IntNode($1); }
		| REALNUM { $$ = new DoubleNode($1); }
		| LPAR expr RPAR { $$ = $2; }
		| ident { $$ = $1; }
		;

ifstatement	: IF expr THEN block { $$ = new IfNode($2, $4, null); }
			| IF expr THEN block ELSE block { $$ = new IfNode($2, $4, $6); }
			;
whilestatement	: WHILE expr block { $$ = new WhileNode($2, $3); }
				;

forstatement	: FOR ident IN range block
				;

range	: LBRACKET expr COMMA expr RBRACKET
		;

proccall	: ident LPAR exprlist RPAR { $$ = new ProcCallNode($1, $3); }
			;

block	: LBRACE stlist RBRACE { $$ = $2; }
		;

%%
