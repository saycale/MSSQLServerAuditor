<?xml version="1.0" encoding="UTF-8"?>

<root>
<!--	<mssqlauditorpreprocessors name="Список хранимых процедур" id="DatabaseObjects.HTML.en" columns="100" rows="100" splitter="yes">-->
<!--		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Список хранимых процедур" id="DatabaseObjects.HTML.en">-->
<!--			<xsl:stylesheet version="1.0"-->
<!--					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"-->
<!--					xmlns:ms="urn:schemas-microsoft-com:xslt"-->
<!--					xmlns:dt="urn:schemas-microsoft-com:datatypes">-->
<!--				<xsl:template match="/">-->
<!--					<html>-->
<!--						<head>-->
<!--							<script src="$JS_FOLDER$/SyntaxHighlighter/scripts/shCore.js"></script>-->
<!--							<script src="$JS_FOLDER$/SyntaxHighlighter/scripts/shBrushSql.js"></script>-->
<!--							<script src="$JS_FOLDER$/SyntaxHighlighter/scripts/shBrushPlain.js"></script>-->
<!--							<link href="$JS_FOLDER$/SyntaxHighlighter/styles/shCore.css" rel="stylesheet" type="text/css" />-->
<!--							<link href="$JS_FOLDER$/SyntaxHighlighter/styles/shThemeDefault.css" rel="stylesheet" type="text/css" />-->
<!---->
<!--							<script type="text/javascript">SyntaxHighlighter.defaults['toolbar'] = false;</script>-->
<!--							<script type="text/javascript">SyntaxHighlighter.all()</script>-->
<!--						</head>-->
<!--						<body>-->
<!--							<style>-->
<!--								body { overflow: auto; padding:0; margin:0; }-->
<!--							</style>-->
<!---->
<!--							<table border="1" width="100%" style="font-size : 12px; font-family : Courier New">-->
<!--								<xsl:for-each select="MSSQLResults/GetDStoredProceduresCode-SQLCodeGuard-ByObject">-->
<!--									<tr>-->
<!--										<td bgcolor="#9acd32">Экземпляр</td>-->
<!--										<td bgcolor="#ccdd88">-->
<!--                                            <xsl:value-of select="Instance"/>-->
<!--										</td>-->
<!--										<td bgcolor="#9acd32">Объект</td>-->
<!--										<td bgcolor="#ccdd88">-->
<!--                                            <xsl:value-of select="SCGObject"/>-->
<!--										</td>-->
<!--									</tr>-->
<!--									<tr>-->
<!--										<td colspan="4">Код</td>-->
<!--									</tr>-->
<!--									<tr>-->
<!--										<td colspan="4">-->
<!--											<pre class="brush:sql; highlight: [{SCGErrorRows}]">-->
<!--                                                <xsl:value-of select="SCGCode"/>-->
<!--											</pre>-->
<!--										</td>-->
<!--									</tr>-->
<!--									<tr>-->
<!--										<td colspan="4">-->
<!--											Результат анализа-->
<!--										</td>-->
<!--									</tr>-->
<!--									<tr>-->
<!--										<td colspan="4">-->
<!--											<pre class="brush:plain; ruler: false; gutter: false">-->
<!--                                                <xsl:value-of select="SCGResult"/>-->
<!--											</pre>-->
<!--										</td>-->
<!--									</tr>-->
<!--								</xsl:for-each>-->
<!--							</table>-->
<!--						</body>-->
<!---->
<!--					</html>-->
<!--				</xsl:template>-->
<!--			</xsl:stylesheet>-->
<!--		</mssqlauditorpreprocessor>-->
<!--	</mssqlauditorpreprocessors>-->
    <mssqlauditorpreprocessors name="Проблемы T-SQL кода" id="DatabaseErrors.HTML.en" columns="50;50" rows="100" splitter="yes">
       <mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Проблемы T-SQL кода" id="DatabaseErrors1.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">
			<xsl:template match="/">
			<html>
				<head>
					<script src="$JS_FOLDER$/SyntaxHighlighter/scripts/shCore.js"></script>
					<script src="$JS_FOLDER$/SyntaxHighlighter/scripts/shBrushSql.js"></script>
					<script src="$JS_FOLDER$/SyntaxHighlighter/scripts/shBrushPlain.js"></script>
					<link href="$JS_FOLDER$/SyntaxHighlighter/styles/shCore.css" rel="stylesheet" type="text/css" />
					<link href="$JS_FOLDER$/SyntaxHighlighter/styles/shThemeDefault.css" rel="stylesheet" type="text/css" />

					<script type="text/javascript">SyntaxHighlighter.defaults['toolbar'] = false;</script>
					<script type="text/javascript">SyntaxHighlighter.all()</script>
				</head>
				<body>
					<style>
						body { overflow: auto; padding:0; margin:0; }
					</style>
					<table border="0" width="100%" style="font-size : 12px; font-family : Courier New">
						<xsl:for-each select="MSSQLResults/GetDStoredProceduresCode-SQLCodeGuard[@RecordSet='2']">
							<tr>
								<td>
									<pre class="brush:sql ruler: true; gutter: true; highlight: [{SCGErrorRows}]">
                                        <xsl:variable name="varSCGObject" select="SCGObject"/>
                                        <xsl:value-of select="../GetDStoredProceduresCode[ObjectName = $varSCGObject]/Procedure"/>
									</pre>
								</td>
							</tr>
						</xsl:for-each>
					</table>
				</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
       <mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Проблемы T-SQL кода" id="DatabaseErrors2.HTML.en" column="2" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
                            xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                            xmlns:ms="urn:schemas-microsoft-com:xslt"
                            xmlns:dt="urn:schemas-microsoft-com:datatypes">
                <xsl:template match="/">
					<html>
						<head>
							<script src="$JS_FOLDER$/SyntaxHighlighter/scripts/shCore.js"></script>
							<script src="$JS_FOLDER$/SyntaxHighlighter/scripts/shBrushSql.js"></script>
							<script src="$JS_FOLDER$/SyntaxHighlighter/scripts/shBrushPlain.js"></script>
							<link href="$JS_FOLDER$/SyntaxHighlighter/styles/shCore.css" rel="stylesheet" type="text/css" />
							<link href="$JS_FOLDER$/SyntaxHighlighter/styles/shThemeDefault.css" rel="stylesheet" type="text/css" />

							<script type="text/javascript">SyntaxHighlighter.defaults['toolbar'] = false;</script>
							<script type="text/javascript">SyntaxHighlighter.all()</script>
						</head>
						<body>
							<style>
								body { overflow: auto; padding:0; margin:0; }
							</style>

							<table border="1" width="100%" style="font-size : 12px; font-family : Courier New">
                                <tr bgcolor="#9acd32">
                                    <th>Строка</th>
                                    <th>Колонка</th>
                                    <th>Объект</th>
                                    <th>Код</th>
                                    <th>Сообщение</th>
                                </tr>
                                <xsl:for-each select="MSSQLResults/GetDStoredProceduresCode-SQLCodeGuard[@RecordSet='1']">
                                    <tr>
                                        <td><xsl:value-of select="SCGRow"/></td>
                                        <td><xsl:value-of select="SCGColumn"/></td>
                                        <td><xsl:value-of select="SCGObject"/></td>
                                        <td><xsl:value-of select="SCGErrorCode"/></td>
                                        <td><xsl:value-of select="SCGMessage"/></td>
                                    </tr>
                                </xsl:for-each>
                            </table>
						</body>

					</html>
				</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>

</root>

