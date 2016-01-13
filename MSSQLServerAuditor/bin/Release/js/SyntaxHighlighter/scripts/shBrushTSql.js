/**
 * SyntaxHighlighter
 * http://alexgorbatchev.com/SyntaxHighlighter
 *
 * SyntaxHighlighter is donationware. If you are using it, please donate.
 * http://alexgorbatchev.com/SyntaxHighlighter/donate.html
 *
 * @version
 * 3.0.83 (July 02 2010)
 *
 * @copyright
 * Copyright (C) 2004-2010 Alex Gorbatchev.
 *
 * @license
 * Dual licensed under the MIT and GPL licenses.
 */
;(function()
{
	// CommonJS
	typeof(require) != 'undefined' ? SyntaxHighlighter = require('shCore').SyntaxHighlighter : null;

	function Brush()
	{
		var funcs     = '@@datefirst @@dbts @@langid @@language @@lock_timeout @@max_connections @@max_precision @@nestlevel @@options @@procid @@remserver @@servername @@servicename @@spid @@textsize @@version abs cast checksum_agg convert count count_big dense_rank getdate grouping grouping_id if isnull max min ntile object_name opendatasource openquery openrowset openxml parse rank round row_number stdev stdevp sum try_cast try_convert try_parse var varp vg';

		var keywords  = 'absolute action add admin after aggregate alias all allocate alter and any are array as asc asensitive assertion asymmetric at atomic authorization backup before begin between binary bit blob boolean both breadth break browse bulk by call called cardinality cascade cascaded case cast catalog catch char character check checkpoint class clob close clustered coalesce collate collation collect column commit completion compute condition connect connection constraint constraints constructor contains containstable continue convert corr corresponding covar_pop covar_samp create cross cube cume_dist current current_catalog current_date current_default_transform_group current_path current_role current_schema current_time current_timestamp current_transform_group_for_type current_user cursor cycle data database date datetime day dbcc deallocate dec decimal declare default deferrable deferred delete deny depth deref desc describe descriptor destroy destructor deterministic diagnostics dictionary disconnect disk distinct distributed domain double drop dump dynamic each element else end end-exec equals errlvl escape every except exception exec execute exists exit external false fetch file fillfactor filter first float for foreign found free freetext freetexttable from full fulltexttable function fusion general get global go goto grant group grouping having hold holdlock host hour identity identity_insert identitycol ignore immediate in index indicator initialize initially inner inout input insert int integer intersect intersection interval into is isolation iterate join key kill language large last lateral leading left less level like like_regex limit lineno ln load local localtime localtimestamp locator map match member merge method minute mod modifies modify module month money multiset names national natural nchar nclob new next no nocount nocheck nonclustered none normalize not null nullif numeric object occurrences_regex of off offsets old on only open opendatasource openquery openrowset openxml operation option or order ordinality out outer output over overlay pad parameter parameters partial partition path percent percent_rank percentile_cont percentile_disc pivot plan position_regex postfix precision prefix preorder prepare preserve primary print prior privileges proc procedure public raiserror range read reads readtext real reconfigure recursive ref references referencing regr_avgx regr_avgy regr_count regr_intercept regr_r2 regr_slope regr_sxx regr_sxy regr_syy relative release replication restore restrict result return returns revert revoke right role rollback rollup routine row rowcount rowguidcol rows rule save savepoint schema scope scroll search second section securityaudit select semantickeyphrasetable semanticsimilaritydetailstable semanticsimilaritytable sensitive sequence session session_user set sets setuser shutdown similar size smallint some space specific specifictype sql sqlexception sqlstate sqlwarning start state statement static statistics stddev_pop stddev_samp structure submultiset substring_regex symmetric sysname system system_user table tablesample temporary terminate textsize than then time timestamp timezone_hour timezone_minute to top trailing tran transaction translate_regex translation treat trigger true truncate try try_convert tsequal uescape under union unique unknown unnest unpivot update updatetext usage use user using value values var_pop var_samp varchar nvarchar variable varying view waitfor when whenever where while width_bucket window with within within group without work write writetext xmlagg xmlattributes xmlbinary xmlcast xmlcomment xmlconcat xmldocument xmlelement xmlexists xmlforest xmliterate xmlnamespaces xmlparse xmlpi xmlquery xmlserialize xmltable xmltext xmlvalidate year zone';

		var operators = 'all and any between cross in join like not or outer some > < = !=';

		this.regexList = [
			{ regex: /--(.*)$/gm,                                            css: 'comments'      }, // one line and multiline comments
			{ regex: SyntaxHighlighter.regexLib.multiLineCComments,          css: 'comments'      }, // multiline comments
			{ regex: SyntaxHighlighter.regexLib.multiLineDoubleQuotedString, css: 'string'        }, // double quoted strings
			{ regex: SyntaxHighlighter.regexLib.multiLineSingleQuotedString, css: 'string'        }, // single quoted strings
			{ regex: new RegExp(this.getKeywords(funcs), 'gmi'),             css: 'color2'        }, // functions
			{ regex: new RegExp(this.getKeywords(operators), 'gmi'),         css: 'color1'        }, // operators and such
			{ regex: /\b[\d\.]+\b/g,                                         css: 'value'         }, // numbers 12345
			{ regex: /(\$|@@|@)\w+/g,                                        css: 'variable bold' }, // $global, @instance, and @@class variables
			{ regex: new RegExp(this.getKeywords(keywords), 'gmi'),          css: 'keyword'       }  // keyword
			];
	};

	Brush.prototype	= new SyntaxHighlighter.Highlighter();
	Brush.aliases	= ['tsql'];

	SyntaxHighlighter.brushes.Sql = Brush;

	// CommonJS
	typeof(exports) != 'undefined' ? exports.Brush = Brush : null;
})();

