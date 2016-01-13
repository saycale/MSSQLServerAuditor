<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Ссылочные пользователи баз данных" id="DatabaseUserAliases.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Ссылочные пользователи баз данных" id="DatabaseUserAliases.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<body>
				<h1 class="firstHeading">Ссылочные пользователи баз данных</h1>

				<p>Использование ссылок для пользователей объявлено устаревшим и не рекомендуется к использованию,
				хотя и данная функциональность доступна в MS SQL 2005, главным образом для сохранения
				совместимости. Для того, чтобы избежать потенциальных проблем откажитесь от данной функциональности.</p>

				<p><strong>Как исправить:</strong> Постарайтесь отказаться от использования ссылок для пользователей.</p>

				<p><strong>Уровень сложности:</strong> Средний</p>

				<p><strong>Уровень опасности:</strong> Высокий</p>

				<a href="http://www.mssqltips.com/sqlservertip/1675/security-issues-when-using-aliased-users-in-sql-server/"
				target="_blank">Проблемы безопасности при использовании ссылок для имён пользователей</a>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
