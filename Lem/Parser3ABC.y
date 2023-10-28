%using Lem;

%{
   	public syntax_tree_node root;
	public List<Error> errors;
    public string current_file_name;
    public int max_errors = 10;
    public PT parsertools;
    public List<compiler_directive> CompilerDirectives;
   	public Parser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
%}

%using PascalABCCompiler.SyntaxTree;
%using PascalABCSavParser;
%using PascalABCCompiler.ParserTools;
%using PascalABCCompiler.Errors;
%using System.Linq;
%using SyntaxVisitors;

%output = LemParserYacc.cs
%partial
%parsertype GPPGParser

%namespace LemGPPGParserScanner

%union {
	public expression ex;
	public ident id;
    public Object ob;
    public op_type_node op;
    public syntax_tree_node stn;
    public token_info ti;
    public type_definition td;
}

%token <ti> FOR, IN, WHILE, IF, ELSE
%token <ex> INTNUM, REALNUM
%token <ti> LPAR, RPAR, LBRACE, RBRACE, LBRACKET, RBRACKET, DOT, COMMA, SEMICOLON
%token <op> ASSIGN
%token <op> PLUS, MINUS, MULTIPLY, DIVIDE
%token <id> ID
%token <op> LOWER, GREATER, LOWEREQUAL, GREATEREQUAL, EQUAL, NOTEQUAL

%left LOWER, GREATER, LOWEREQUAL, GREATEREQUAL, EQUAL, NOTEQUAL
%left PLUS MINUS
%left MULTIPLY DIVIDE

%type <id> ident
%type <ex> expr
%type <stn> exprlist
%type <stn> assign ifstatement whilestatement forstatement stmt proccall
%type <stn> stlist block

%start progr

%%

progr   : stlist { root = $1; }
		;

stlist	: stmt { $$ = new statement_list($1 as statement, @1); }
		| stlist SEMICOLON stmt { $$ = ($1 as statement_list).Add($3 as stmt, @$); }
		;

stmt	: assign { $$ = $1; }
		| block	{ $$ = $1; }
		| ifstatement { $$ = $1; }
		| forstatement { $$ = $1; }
		| whilestatement { $$ = $1; }
		;

ident 	: ID { $$ = $1; }
		;

assign 	: ident ASSIGN expr         {
        	if (!($1 is addressed_value))
        		parsertools.AddErrorFromResource("LEFT_SIDE_CANNOT_BE_ASSIGNED_TO",@$);
			$$ = new assign($1 as addressed_value, $3, $2.type, @$);
        }
		;

expr 	: expr PLUS expr { $$ = new bin_expr($1, $3, $2.type, @$) }
		| expr MULTIPLY expr { $$ = new bin_expr($1, $3, $2.type, @$) }
		| expr MINUS expr { $$ = new bin_expr($1, $3, $2.type, @$) }
  		| expr LOWER expr { $$ = new bin_expr($1, $3, $2.type, @$) }
		| expr GREATER expr { $$ = new bin_expr($1, $3, $2.type, @$) }
		| ident { $$ = $1; }
		| INTNUM { $$ = $1; }
		| REALNUM { $$ = $1; }
		| LPAR expr RPAR { $$ = $2; }
		;

exprlist	: expr { $$ = new expression_list($1, @$); }
			| exprlist COMMA expr { $$ = ($1 as expression_list).Add($3, @$);  }
			;

ifstatement	: IF expr stmt { $$ = new if_node($2, $4 as statement, null, @$) }
			| IF expr block ELSE stmt { $$ = new if_node($2, $4 as statement, $6 as statement, @$); }
			;
whilestatement	: WHILE expr stmt { $$ = NewWhileStmt($1, $2, null, $3 as statement, @$); }
				;

block	: LBRACE stlist RBRACE { $$ = $2; }
		;

%%
