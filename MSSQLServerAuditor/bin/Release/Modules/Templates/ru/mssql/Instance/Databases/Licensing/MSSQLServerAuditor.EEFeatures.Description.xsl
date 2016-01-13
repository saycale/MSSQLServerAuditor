<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Использование Enterprise Edition Features" id="DatabaseEnterpriseEditionFeatures.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Использование Enterprise Edition Features" id="DatabaseEnterpriseEditionFeatures.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Использование Enterprise Edition Features</h1>

				<p>If you try to restore an Enterprise Edition database onto a Standard
				Edition server, the restore will chug right along until the very last step –
				bringing the database online. At that time, SQL Server checks to see whether
				any Enterprise Edition features are in use. If so, SQL Server just won't let
				you bring the database online. No, you can't even get it online just long
				enough to rip out those features.</p>

				<p>The script checks the <strong>sys.dm_db_persisted_sku_features</strong>
				DMV in each user database and reports back which EE features are in use –
				like compression, partitioning, or Transparent Data Encryption (TDE).</p>

				<p><strong>Как исправить:</strong> If you've got Standard Edition servers in
				your environment, be aware that you won't be able to restore these databases
				onto those Standard Edition servers. This is especially important in
				disaster recovery (DR) environments that might have been accidentally
				installed as Standard Edition.</p>

				<p><strong>Уровень сложности:</strong> Низкий</p>

				<p><strong>Уровень опасности:</strong> Низкий</p>

				<p><a href="http://www.brentozar.com/blitz/enterprise-edition-features/" target="_blank">Blitz Result: Enterprise Edition Features in Use</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

